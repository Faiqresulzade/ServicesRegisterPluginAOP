using Microsoft.Extensions.DependencyInjection;
using ServicesRegisterPlugin.Atributes;
using ServicesRegisterPlugin.Helpers;
using System.Reflection;

namespace ServicesRegisterPlugin.Registrations;

public static class DynamicServiceRegistrar
{
    /// <summary>
    /// Registers services dynamically based on custom attributes.
    /// </summary>
    public static void RegisterServices(this IServiceCollection services, string assemblyPrefix = "")
    {
        var assemblies = AssemblyHelper.GetProjectAssemblies(assemblyPrefix);
        foreach (var assembly in assemblies)
        {
            var typesToRegister = assembly.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface); // Only consider non-abstract, non-interface types

            foreach (var type in typesToRegister)
            {
                var lifetime = GetServiceLifetime(type);
                if (lifetime is not null)
                {
                    RegisterType(services, type, lifetime.Value);
                }
            }
        }
    }

    /// <summary>
    /// Determines the service lifetime based on the custom attributes.
    /// </summary>
    private static ServiceLifetime? GetServiceLifetime(Type type)
    {
        if (type.GetCustomAttribute<Scoped>() != null)
            return ServiceLifetime.Scoped;
        if (type.GetCustomAttribute<Transient>() != null)
            return ServiceLifetime.Transient;
        if (type.GetCustomAttribute<Singleton>() != null)
            return ServiceLifetime.Singleton;

        return default; // If no attribute is found, return null
    }

    /// <summary>
    /// Registers a specific type in the service collection based on the service lifetime.
    /// </summary>
    private static void RegisterType(IServiceCollection services, Type type, ServiceLifetime lifetime)
    {
        var interfaceTypes = type.GetInterfaces()
                                 .Where(x => x != typeof(IDisposable)); // Avoid registering IDisposable

        foreach (var interfaceType in interfaceTypes)
        {
            services.Add(new ServiceDescriptor(interfaceType, type, lifetime));
        }

        if (!interfaceTypes.Any())
        {
            services.Add(new ServiceDescriptor(type, type, lifetime));
        }
    }
}
