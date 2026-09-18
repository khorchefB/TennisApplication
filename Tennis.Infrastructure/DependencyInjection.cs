using Microsoft.Extensions.DependencyInjection;
using Tennis.Infrastructure.Repository;

namespace Tennis.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<TennisPlayerRepository>();
        services.AddSingleton<ITennisPlayerRepository>(serviceProvider =>
            serviceProvider.GetRequiredService<TennisPlayerRepository>());
        services.AddSingleton<IHostedService>(serviceProvider =>
            serviceProvider.GetRequiredService<TennisPlayerRepository>());

        return services;
    }
}
