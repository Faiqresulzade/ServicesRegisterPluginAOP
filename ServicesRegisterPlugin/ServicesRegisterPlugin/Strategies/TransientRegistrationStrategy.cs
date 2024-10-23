using Microsoft.Extensions.DependencyInjection;
namespace ServicesRegisterPlugin.Strategies;

internal class TransientRegistrationStrategy : RegistrationStrategyBase
{
    /// <inheritdoc/>
    public override void Register
        (IServiceCollection services, Type type, Type interfaceType) => services.Add(new ServiceDescriptor(interfaceType, type, ServiceLifetime.Transient));
}
