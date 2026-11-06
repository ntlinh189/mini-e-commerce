using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Services;
using FluentValidation;
using ProductService.Validators;
using ProductService.Filters;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("Default")
));

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.AddService<ValidationFilter>();
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();

builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddScoped<ProductManagementService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
