using Microsoft.Extensions.DependencyInjection;
namespace ServicesRegisterPlugin.Strategies;

internal class TransientRegistrationStrategy : RegistrationStrategyBase
{
    public override void Register
        (IServiceCollection services, Type type, ServiceLifetime lifetime) => services.AddTransient(type);
}
