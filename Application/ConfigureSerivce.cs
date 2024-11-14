using System.Reflection;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureService
{
    public static IServiceCollection ConfigureServiceServices(this IServiceCollection services)
    {
        
        
        //add mediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>),typeof(RequestPreProcessorBehavior<,>));
        });
        
        return services;
    }
}