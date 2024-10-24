using Microsoft.Extensions.DependencyInjection;
using ServicesRegisterPlugin.Atributes;
using ServicesRegisterPlugin.Factory;
using ServicesRegisterPlugin.Helpers;
using ServicesRegisterPlugin.Options;
using System.Reflection;

namespace ServicesRegisterPlugin.Registrations
{
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
                RegisterType(services, type, options);
            }
        }

        /// <summary>
        /// Registers a specific type in the <see cref="IServiceCollection"/> based on its attributes and options.
        /// </summary>
        /// <param name="services">The service collection to register the type in.</param>
        /// <param name="type">The type to be registered.</param>
        /// <param name="options">The options used for registration.</param>
        private static void RegisterType(IServiceCollection services, Type type, ServiceRegistrationOptions options)
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

            var interfaceType = GetInterfaceType(type, specifiedInterfaceName);

            // Check for existing registration
            if (interfaceType != null && !options.IgnoreRegistrationConflicts)
            {
                var existingService = services.FirstOrDefault(descriptor => descriptor.ServiceType == interfaceType);
                if (existingService is not null)
                {
                    Console.WriteLine("this service already added ioc");
                    return;
                }
            }

            if (interfaceType is not null && interfaceType.IsGenericType)
            {
                try
                {
                    Type closedGenericType = interfaceType.MakeGenericType(type.GetGenericArguments());
                    interfaceType = closedGenericType;
                }
                catch (Exception)
                { }
            }

            // Invoke the OnRegistering callback if provided
            options.OnRegistering?.Invoke(type, options.Lifetime);

            // Create registration strategy and register the service
            var strategy = RegistrationStrategyFactory.Create(attribute.GetType());
            strategy.Register(services, type, interfaceType ?? type);
        }

        /// <summary>
        /// Determines the appropriate interface type for the given type based on the specified interface name.
        /// </summary>
        /// <param name="type">The type to find the interface for.</param>
        /// <param name="specifiedInterfaceName">The name of the specified interface, if any.</param>
        /// <returns>The matching interface type, or null if not found.</returns>
        private static Type? GetInterfaceType(Type type, string? specifiedInterfaceName)
        {
            return type.GetInterfaces()
                .FirstOrDefault(interfaceType =>
                    interfaceType != typeof(IDisposable) &&
                    (
                        specifiedInterfaceName is not null
                            ? interfaceType.Name == specifiedInterfaceName
                            : interfaceType.Name.EndsWith("Service") || interfaceType.Name.EndsWith("Repository")
                    ));
        }
    }
}