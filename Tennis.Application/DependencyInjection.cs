namespace Tennis.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //config.AddOpenBehavior(typeof(ValidationBehaviors<,>));
            //config.AddOpenBehavior(typeof(LoggingBehaviors<,>));
        });
        return services;
    }
}