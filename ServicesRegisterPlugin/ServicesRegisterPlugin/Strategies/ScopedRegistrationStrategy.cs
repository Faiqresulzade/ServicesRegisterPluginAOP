using Microsoft.Extensions.DependencyInjection;
namespace ServicesRegisterPlugin.Strategies;

public class ScopedRegistrationStrategy : RegistrationStrategyBase
{
    public override void Register
        (IServiceCollection services, Type type, ServiceLifetime lifetime) => services.AddScoped(type);
}
