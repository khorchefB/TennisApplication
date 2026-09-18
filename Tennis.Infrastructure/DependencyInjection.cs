using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tennis.Infrastructure.Repository;

namespace Tennis.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITennisPlayerRepository, TennisPlayerRepository>();
        return services;
    }
}