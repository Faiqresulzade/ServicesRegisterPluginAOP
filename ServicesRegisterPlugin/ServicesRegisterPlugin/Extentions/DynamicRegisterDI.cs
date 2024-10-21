using Microsoft.Extensions.DependencyInjection;
using ServicesRegisterPlugin.Options;
using ServicesRegisterPlugin.Registrations;
namespace ServicesRegisterPlugin.Extensions;

public static class DynamicRegisterDI
{
    /// <summary>
    /// Registers services dynamically based on custom attributes.
    /// </summary>
    /// <param name="services">The service collection to register services to.</param>
    /// <param name="configureOptions">An action to configure service registration options.</param>
    public static void RegisterServices
        (this IServiceCollection services, Action<ServiceRegistrationOptions> configureOptions)
    {
        var options = new ServiceRegistrationOptions();
        configureOptions?.Invoke(options);

        DynamicServiceRegistrar.RegisterServices(services, options);
    }
}
