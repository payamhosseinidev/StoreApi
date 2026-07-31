using FluentValidation;
using StoreApi.Responses;

namespace StoreApi.Middleware
{
    public class ExeptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExeptionMiddleware> _logger;

        public ExeptionMiddleware(RequestDelegate next, ILogger<ExeptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex,ex.Message);

                context.Response.ContentType = "application/json";

                //FluentValidation
                if(ex is ValidationException validationException)
                {
                    var errors = validationException.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                                   g => g.Key,
                                   g => g.Select(x => x.ErrorMessage).ToArray()
                        );
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    var response = new ValidationErrorResponse
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Validation Failed",
                        Errors = errors
                    };
                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }

                // Other Errors
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response500 = new ErrorResponse
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred."
                };

                await context.Response.WriteAsJsonAsync(response500);
            }
        }
    }
}
