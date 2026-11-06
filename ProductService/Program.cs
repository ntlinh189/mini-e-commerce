using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DependencyInjection;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Connect to PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("Default")
));

// Middlewares
builder.Services.AddMiddlewares();

// Controllers
builder.Services.AddControllers();

// Services
builder.Services.AddApplicationServices();

// OpenApi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
