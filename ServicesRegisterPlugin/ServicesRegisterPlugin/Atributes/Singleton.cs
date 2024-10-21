namespace ServicesRegisterPlugin.Atributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class Singleton : Attribute
{
    public string? InterfaceName { get; init; }
    public Singleton(string? interfaceName = null) => InterfaceName = interfaceName;
}
