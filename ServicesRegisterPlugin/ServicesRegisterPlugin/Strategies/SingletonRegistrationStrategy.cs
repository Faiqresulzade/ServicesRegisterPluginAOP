using Microsoft.Extensions.DependencyInjection;
namespace ServicesRegisterPlugin.Strategies;

public class SingletonRegistrationStrategy : RegistrationStrategyBase
{
    public override void Register
        (IServiceCollection services, Type type, ServiceLifetime lifetime) => services.AddSingleton(type);
}
