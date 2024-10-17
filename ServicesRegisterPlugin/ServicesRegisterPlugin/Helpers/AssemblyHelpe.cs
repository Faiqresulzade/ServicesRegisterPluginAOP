using System.Reflection;
namespace ServicesRegisterPlugin.Helpers;

public static class AssemblyHelper
{
    public static Assembly[] GetProjectAssemblies(string prefix = "")
        => AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(assembly =>
                        string.IsNullOrEmpty(prefix) ||
                        assembly.FullName.StartsWith(prefix))
                    .ToArray();
}
