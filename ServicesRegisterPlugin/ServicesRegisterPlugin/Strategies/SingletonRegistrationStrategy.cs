using Microsoft.Extensions.DependencyInjection;
namespace ServicesRegisterPlugin.Strategies;

public class SingletonRegistrationStrategy : RegistrationStrategyBase
{
    /// <inheritdoc/>
    public override void Register
        (IServiceCollection services, Type type, Type interfaceType) => services.Add(new ServiceDescriptor(interfaceType, type, ServiceLifetime.Singleton));
}
