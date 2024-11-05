using CloudHumans.ClaudIA.Shared;

namespace CloudHumans.ClaudIA.Api.StartupInfra;

internal static class ServicesExtensions
{
    public static IServiceCollection AddHttpGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<HttpGlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
    
    public static IServiceCollection AddOpenApiSpecs(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApiDocument();
        return services;
    }
}