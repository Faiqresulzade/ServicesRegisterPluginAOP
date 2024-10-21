using Microsoft.Extensions.DependencyInjection;
namespace ServicesRegisterPlugin.Strategies;

public abstract class RegistrationStrategyBase
{
    public abstract void Register(IServiceCollection services, Type type, ServiceLifetime lifetime);
}
