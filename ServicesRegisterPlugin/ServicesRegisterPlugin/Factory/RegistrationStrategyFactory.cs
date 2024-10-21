using ServicesRegisterPlugin.Atributes;
using ServicesRegisterPlugin.Strategies;
namespace ServicesRegisterPlugin.Factory;

public class RegistrationStrategyFactory
{
    public static RegistrationStrategyBase Create(Type attributeType)
      => attributeType switch
      {
          _ when attributeType == typeof(Singleton) => new SingletonRegistrationStrategy(),
          _ when attributeType == typeof(Scoped) => new ScopedRegistrationStrategy(),
          _ when attributeType == typeof(Transient) => new TransientRegistrationStrategy(),
          _ => throw new NotSupportedException($"No strategy found for {attributeType.Name}")
      };
}
