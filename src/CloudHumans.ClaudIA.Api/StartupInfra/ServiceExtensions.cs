﻿using CloudHumans.ClaudIA.Infrastructure;

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
        services.AddOpenApiDocument(doc => { doc.Title = "CloudHumans.ClaudIA.Api"; });
        return services;
    }
}