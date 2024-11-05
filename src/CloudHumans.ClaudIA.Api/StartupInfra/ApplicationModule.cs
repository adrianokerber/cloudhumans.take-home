using Autofac;
using CloudHumans.ClaudIA.Shared;

namespace CloudHumans.ClaudIA.Api.StartupInfra;

public class ApplicationModule : Autofac.Module
{
    public ApplicationModule(){}
    
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(CloudHumans.ClaudIA.Environment).Assembly)
            .AsClosedTypesOf(typeof(IService<>))
            .InstancePerLifetimeScope();

        builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
    }
}