using ServicesRegisterPlugin.Atributes;
using ServicesRegisterPlugin.Exceptions;
using ServicesRegisterPlugin.Strategies;
namespace ServicesRegisterPlugin.Factory;

public class RegistrationStrategyFactory
{
    /// <summary>
    /// Creates a registration strategy based on the provided attribute type.
    /// </summary>
    /// <param name="attributeType">The type of the attribute for which to create a registration strategy.</param>
    /// <returns>A <see cref="RegistrationStrategyBase"/> implementation corresponding to the specified attribute type.</returns>
    /// <exception cref="NotFoundTypeException">Thrown when the attribute type is not supported.</exception>
    public static RegistrationStrategyBase Create(Type attributeType)
      => attributeType switch
      {
          _ when attributeType == typeof(Singleton) => new SingletonRegistrationStrategy(),
          _ when attributeType == typeof(Scoped) => new ScopedRegistrationStrategy(),
          _ when attributeType == typeof(Transient) => new TransientRegistrationStrategy(),
          _ => throw new NotFoundTypeException($"No strategy found for {attributeType.Name}")
      };
}
