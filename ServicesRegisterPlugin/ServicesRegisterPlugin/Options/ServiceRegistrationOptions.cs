using Microsoft.Extensions.DependencyInjection;
namespace ServicesRegisterPlugin.Options;

/// <summary>
/// Options for configuring dynamic service registration.
/// </summary>
public class ServiceRegistrationOptions
{
    /// <summary>
    /// Assembly prefix used to filter assemblies for service registration.
    /// </summary>
    public string AssemblyPrefix { get; set; } = string.Empty;

    /// <summary>
    /// Optional filter for selecting types to register.
    /// </summary>
    public Func<Type, bool>? TypeFilter { get; set; }

    /// <summary>
    /// Specifies the default service lifetime if not overridden by attributes.
    /// </summary>
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;

    /// <summary>
    /// Optional callback for handling registration events.
    /// </summary>
    public Action<Type, ServiceLifetime>? OnRegistering { get; set; }

    /// <summary>
    /// If true, ignores registration conflicts (e.g., duplicate services).
    /// </summary>
    public bool IgnoreRegistrationConflicts { get; set; } = false;
}
