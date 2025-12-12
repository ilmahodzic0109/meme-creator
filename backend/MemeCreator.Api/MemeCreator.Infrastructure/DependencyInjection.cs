using MemeCreator.Application.Interfaces;
using MemeCreator.Infrastructure.Persistence;
using MemeCreator.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MemeCreator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<MemeDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IConfigRepository, ConfigRepository>();

        return services;
    }
}
