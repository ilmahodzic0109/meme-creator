using MemeCreator.Application.Interfaces;
using MemeCreator.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MemeCreator.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IConfigService, ConfigService>();
        return services;
    }
}
