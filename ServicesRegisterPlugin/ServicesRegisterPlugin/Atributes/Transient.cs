namespace ServicesRegisterPlugin.Atributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class Transient : Attribute
{
    public string? InterfaceName { get; init; }
    public Transient(string? interfaceName = default) => InterfaceName = interfaceName;
}
