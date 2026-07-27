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

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    Status = 500,
                    Message = "An unexpected error occurred."
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
