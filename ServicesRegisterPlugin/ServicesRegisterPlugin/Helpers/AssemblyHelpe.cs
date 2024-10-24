using System.Reflection;
namespace ServicesRegisterPlugin.Helpers;

public static class AssemblyHelper
{
    public static Assembly[] GetProjectAssemblies(string prefix = "")
    {
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

        var referencedAssemblyNames = loadedAssemblies
            .SelectMany(assembly => assembly.GetReferencedAssemblies())
            .Distinct()
            .Where(assemblyName => string.IsNullOrEmpty(prefix) || assemblyName.FullName.StartsWith(prefix))
            .ToList();

        foreach (var assemblyName in referencedAssemblyNames)
        {
            if (!loadedAssemblies.Any(a => a.FullName == assemblyName.FullName))
            {
                try
                {
                    var assembly = Assembly.Load(assemblyName);
                    loadedAssemblies.Add(assembly);
                }
                catch
                {
                    Console.WriteLine("failed to load assembly");
                }
            }
        }

       return loadedAssemblies
            .Where(assembly => string.IsNullOrEmpty(prefix) || assembly.FullName.StartsWith(prefix))
            .ToArray();
    }
}