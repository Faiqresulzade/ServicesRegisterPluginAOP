using Microsoft.Extensions.DependencyInjection;
namespace ServicesRegisterPlugin.Strategies;

/// <summary>
/// Base class for registration strategies, defining the contract for registering services.
/// </summary>
public abstract class RegistrationStrategyBase
{
    /// <summary>
    /// Registers a service in the specified <see cref="IServiceCollection"/> with the given service type and interface type.
    /// </summary>
    /// <param name="services">The service collection to which the service will be registered.</param>
    /// <param name="type">The concrete type of the service to register.</param>
    /// <param name="interfaceType">The interface type to which the service is mapped; can be <c>null</c> if not applicable.</param>
    public abstract void Register(IServiceCollection services, Type type, Type interfaceType);
}
