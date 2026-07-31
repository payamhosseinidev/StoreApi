using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Middleware;
using StoreApi.Repositories;
using StoreApi.Responses;
using StoreApi.Services;
using FluentValidation;
using StoreApi.Validators;
using StoreApi.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
        .Where(x => x.Value!.Errors.Count > 0)
        .ToDictionary(
             x => x.Key,
             x => x.Value!.Errors
                .Select(e => e.ErrorMessage)
                .ToArray()
        );
        var response = new ValidationErrorResponse
        {
            Status = StatusCodes.Status400BadRequest,
            Message = "Validation Failed",
            Errors = errors
        };
        return new BadRequestObjectResult(response);
    };
});

builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExeptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
