using Autofac;
using Autofac.Extensions.DependencyInjection;
using CloudHumans.ClaudIA.Api.StartupInfra;
using FastEndpoints;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Host
    .ConfigureContainer<ContainerBuilder>(cb =>
    {
        cb.RegisterModule(new ApplicationModule(builder.Configuration));
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services
    .AddHttpGlobalExceptionHandler()
    .AddFastEndpoints()
    .AddOpenApiSpecs();

var app = builder.Build();

app.UseFastEndpoints();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.UseOpenApi();

    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    app.UseSwaggerUi(); // UseSwaggerUI Protected by if (env.IsDevelopment())
}

app.Run();