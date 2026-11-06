using ProductService.Services;

namespace ProductService.DependencyInjection;

public static class ServiceInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ProductManagementService>();
        
        return services;
    }
}