using Microsoft.Extensions.DependencyInjection;
using ServicesRegisterPlugin.Atributes;
using ServicesRegisterPlugin.Factory;
using ServicesRegisterPlugin.Helpers;
using ServicesRegisterPlugin.Options;
using System.Reflection;
namespace ServicesRegisterPlugin.Registrations;

internal static class DynamicServiceRegistrar
{
    public static void RegisterServices(IServiceCollection services, ServiceRegistrationOptions options)
    {
        var typesToRegister = AssemblyHelper.GetProjectAssemblies(options.AssemblyPrefix)
                                            .SelectMany(asm => asm.GetTypes())
                                            .Where(type => type.GetCustomAttributes()
                                            .Any(attr => attr is Singleton ||attr is Scoped || attr is Transient))
                                            .ToList();

        foreach (var type in typesToRegister)
        {
            var attribute = type.GetCustomAttributes()
                .Where(attr => attr is Singleton || attr is Scoped || attr is Transient)
                .First();

            var strategy = RegistrationStrategyFactory.Create(attribute.GetType());
            strategy.Register(services, type, options.Lifetime);
        }
    }
}
