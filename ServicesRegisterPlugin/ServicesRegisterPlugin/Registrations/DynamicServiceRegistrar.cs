using Microsoft.Extensions.DependencyInjection;
using ServicesRegisterPlugin.Atributes;
using ServicesRegisterPlugin.Factory;
using ServicesRegisterPlugin.Helpers;
using ServicesRegisterPlugin.Options;
using System.Reflection;
namespace ServicesRegisterPlugin.Registrations;

internal static class DynamicServiceRegistrar
{
    private static List<Assembly>? cachedAssemblies;


    /// <summary>
    /// Registers services in the specified <see cref="IServiceCollection"/> based on the provided options.
    /// </summary>
    /// <param name="services">The service collection to which services will be registered.</param>
    /// <param name="options">The options to configure service registration, including assembly prefix and registration callbacks.</param>
    public static void RegisterServices(IServiceCollection services, ServiceRegistrationOptions options)
    {
        // Cache assemblies if not already cached
        cachedAssemblies ??= AssemblyHelper.GetProjectAssemblies(options.AssemblyPrefix).ToList();

        var typesToRegister = cachedAssemblies
            .SelectMany(asm => asm.GetTypes())
            .Where(type =>
                (options.TypeFilter is null || options.TypeFilter(type)) &&
                type.GetCustomAttributes().Any(attr =>
                    attr is Singleton || attr is Scoped || attr is Transient));

        if (!typesToRegister.Any()) return;

        foreach (var type in typesToRegister)
        {

            var attribute = type.GetCustomAttributes().First(attr =>
                attr is Singleton || attr is Scoped || attr is Transient);

            var specifiedInterfaceName = attribute switch
            {
                Singleton singleton => singleton.InterfaceName,
                Scoped scoped => scoped.InterfaceName,
                Transient transient => transient.InterfaceName,
                _ => null
            };

            var interfaceType = type.GetInterfaces().FirstOrDefault(x =>
                x != typeof(IDisposable) &&
                (specifiedInterfaceName is not null ? x.Name == specifiedInterfaceName :
                x.Name.EndsWith("Service") || x.Name.EndsWith("Repository")));

            // Check for existing registration
            if (interfaceType != null && !options.IgnoreRegistrationConflicts)
            {
                var existingService = services.FirstOrDefault(descriptor => descriptor.ServiceType == interfaceType);
                if (existingService is not null)
                {
                    // Handle conflict as needed (e.g., log a warning, throw an exception)
                    // For now, we simply skip the registration
                    continue;
                }
            }

            // Invoke the OnRegistering callback if provided
            options.OnRegistering?.Invoke(type, options.Lifetime);

            // Create registration strategy and register the service
            var strategy = RegistrationStrategyFactory.Create(attribute.GetType());
            strategy.Register(services, type, interfaceType ?? type);
        }
    }
}