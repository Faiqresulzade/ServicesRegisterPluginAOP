namespace ServicesRegisterPlugin.Atributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class Scoped : Attribute
{
    public string? InterfaceName { get; init; }
    public Scoped(string? interfaceName = null) => InterfaceName = interfaceName;
}
