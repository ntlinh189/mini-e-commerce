using ProductService.Middlewares;

namespace ProductService.DependencyInjection;

public static class MiddlewareInjection
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();  

        return services;
    }
}