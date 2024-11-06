using Autofac;
using CloudHumans.ClaudIA.Domain.Conversations.Services;
using CloudHumans.ClaudIA.Shared;

namespace CloudHumans.ClaudIA.Api.StartupInfra;

public class ApplicationModule(IConfiguration configuration) : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(CommandHandler<>).Assembly)
            .AsClosedTypesOf(typeof(IService<>))
            .InstancePerLifetimeScope();

        builder.RegisterType<HttpContextAccessor>()
               .As<IHttpContextAccessor>()
               .InstancePerLifetimeScope();
        
        builder.RegisterType<EmbeddingService>()
               .As<EmbeddingService>()
               .InstancePerLifetimeScope()
               .WithParameter("apiKey", configuration.GetValue<string>("OpenaiKey"));
        
        builder.RegisterType<VectorSearchService>()
               .As<VectorSearchService>()
               .InstancePerLifetimeScope()
               .WithParameter("apiKey", configuration.GetValue<string>("AzureAiSearchKey"));
        
        builder.RegisterType<ContextualResponseService>()
               .As<ContextualResponseService>()
               .InstancePerLifetimeScope()
               .WithParameter("apiKey", configuration.GetValue<string>("OpenaiKey"));
    }
}