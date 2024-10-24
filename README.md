\# AutoDIRegister

\`AutoDIRegister\` is a dynamic service registration plugin for .NET, enabling automatic service registration based on custom attributes. It supports different service lifetimes, such as \`Singleton\`, \`Scoped\`, and \`Transient\`, and provides configurable options for flexible dependency injection.

\## Features

\- Automatically registers services marked with custom attributes: \`\[Singleton\]\`, \`\[Scoped\]\`, and \`\[Transient\]\`.
\- Configurable options to customize service registration behavior.
\- Supports open generics and custom type filters.
\- Handles registration conflicts gracefully based on configuration.
\- Dynamic assembly scanning for automatic service registration.

\## Getting Started

\### Installation

You can install the package via NuGet:

\`\`\`bash
dotnet add package AutoDIRegister
\`\`\`

Alternatively, you can use the NuGet Package Manager in Visual Studio to search for and install the \`AutoDIRegister\` package.

\### Basic Usage

To start using \`AutoDIRegister\`, follow these steps:

1. \*\*Add the Plugin to Your Project:\*\*

   Make sure the package is installed and added to your project.

2. \*\*Mark Services with Attributes:\*\*

   Mark your classes with \`\[Singleton\]\`, \`\[Scoped\]\`, or \`\[Transient\]\` attributes, depending on the desired service lifetime.

   \`\`\`csharp
   using ServicesRegisterPlugin.Atributes;

   \[Singleton\]
   public class MySingletonService : IMyService
   {
       // Implementation
   }

   \[Scoped\]
   public class MyScopedService : IMyService
   {
       // Implementation
   }

   \[Transient\]
   public class MyTransientService : IMyService
   {
       // Implementation
   }
   \`\`\`

3. \*\*Register Services Using the Plugin:\*\*

   Call the \`RegisterServices\` extension method in your \`Program.cs\` or \`Startup.cs\` file.

   \`\`\`csharp
   using ServicesRegisterPlugin.Registrations;

   var builder = WebApplication.CreateBuilder(args);

   builder.Services.RegisterServices(configure =>
   {
       configure.AssemblyPrefix = "MyApp"; // Optionally filter assemblies by prefix
   });

   var app = builder.Build();

   // Continue application setup
   \`\`\`

\## Configuration Options

You can customize the service registration by providing additional options to the \`RegisterServices\` method.

\### ServiceRegistrationOptions

\- \*\*AssemblyPrefix:\*\* Filter assemblies to be scanned by specifying a prefix. Only assemblies with names starting with this prefix will be scanned.
\- \*\*TypeFilter:\*\* A custom filter to determine which types are eligible for registration.
\- \*\*IgnoreRegistrationConflicts:\*\* When set to \`true\`, skips services that are already registered. If \`false\`, it throws an exception in case of conflicts.
\- \*\*OnRegistering:\*\* A callback that gets invoked before a service is registered.

\### Example with Options

\`\`\`csharp
builder.Services.RegisterServices(configure =>
{
    configure.AssemblyPrefix = "MyProject"; // Only scan assemblies starting with "MyProject"
    configure.TypeFilter = type => type.Name.EndsWith("Service"); // Register only types ending with "Service"
    configure.IgnoreRegistrationConflicts = true; // Ignore conflicts if a service is already registered
    configure.OnRegistering = (type, lifetime) =>
    {
        Console.WriteLine($"Registering {type.Name} with {lifetime} lifetime.");
    };
});
\`\`\`

\## Attribute Configuration

\### Singleton, Scoped, and Transient Attributes

The \`\[Singleton\]\`, \`\[Scoped\]\`, and \`\[Transient\]\` attributes can be used to specify the lifetime of a service. These attributes support an optional constructor parameter \`InterfaceName\` to specify which interface the class should be registered as.

\### InterfaceName Requirement

If the service class's name does not end with "Service" or "Repository," you must explicitly specify the interface name in the attribute constructor.

\`\`\`csharp
\[Singleton("IMyCustomService")\]
public class MyCustomClass : IMyCustomService
{
    // Implementation
}
\`\`\`

In this example, since the class name does not end with "Service" or "Repository," the interface name is provided explicitly. If the name does match one of these suffixes, the registration will automatically infer the interface.

\## Handling Open Generics

The plugin supports registering open generic types. To register an open generic type, simply mark the class with the appropriate attribute, and the plugin will handle the rest.

\`\`\`csharp
\[Scoped\]
public class GenericRepository<T> : IRepository<T>
{
    // Implementation
}
\`\`\`

\## Custom Type Filters

You can define custom type filters to control which types should be registered. For example, you can filter types based on their name, namespace, or any other condition.

\`\`\`csharp
configure.TypeFilter = type => type.Namespace.StartsWith("MyApp.Services") && type.Name.Contains("Handler");
\`\`\`

\## Registration Conflict Handling

The plugin allows you to configure how conflicts are handled when a service is already registered:

\- \*\*Ignore Registration Conflicts:\*\* If set to \`true\`, the plugin will skip registering services that are already registered.
\- \*\*Throw Exception on Conflicts:\*\* If set to \`false\`, the plugin will throw an exception if a service is already registered.

\## Advanced Usage

\### Dynamic Service Registration with Multiple Assemblies

You can configure the plugin to scan multiple assemblies by specifying a common prefix or configuring your assemblies manually.

\`\`\`csharp
builder.Services.RegisterServices(configure =>
{
    configure.AssemblyPrefix = "MySolution"; // Scans all assemblies starting with "MySolution"
});
\`\`\`

\### Using the OnRegistering Callback

The \`OnRegistering\` callback allows you to perform custom logic before a service is registered. For instance, you can log registrations or modify service registration options dynamically.

\`\`\`csharp
configure.OnRegistering = (type, lifetime) =>
{
    Console.WriteLine($"Preparing to register {type.Name} with {lifetime} lifetime.");
};
\`\`\`

\## Examples

\### Basic Registration

Registering services without any configuration:

\`\`\`csharp
builder.Services.RegisterServices();
\`\`\`

\### Filtering by Type Name

Only register types whose names end with "Service":

\`\`\`csharp
builder.Services.RegisterServices(configure =>
{
    configure.TypeFilter = type => type.Name.EndsWith("Service");
});
\`\`\`

\### Explicit Interface Registration

Register a class with a specific interface name:

\`\`\`csharp
\[Singleton("ICustomService")\]
public class CustomService : ICustomService
{
    // Implementation
}
\`\`\`

In this case, you need to specify \`"ICustomService"\` because the class name does not follow the default convention.

\## Troubleshooting

\### Common Issues

1. \*\*No Services Registered:\*\* Ensure that the target assembly name matches the specified prefix in \`AssemblyPrefix\` if you are filtering by assembly.
2. \*\*Attribute Not Applied:\*\* Make sure you have applied \`\[Singleton\]\`, \`\[Scoped\]\`, or \`\[Transient\]\` to your service class.
3. \*\*Conflicting Registrations:\*\* Use the \`IgnoreRegistrationConflicts\` option to avoid exceptions due to duplicate registrations.

\### Debugging Tips

\- Use the \`OnRegistering\` callback to log registrations and track which services are being registered.
\- Check the configuration of \`TypeFilter\` if some expected services are not being registered.
