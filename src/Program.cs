using System;
using System.Reflection;

namespace nuget_audit
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var versionString = 
                    Assembly.GetEntryAssembly()
                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                        .InformationalVersion.ToString();

                Console.WriteLine($"NugetAudit v{versionString}");
                Console.WriteLine("-------------");
                Console.WriteLine("\nUsage:");
                Console.WriteLine("nuget-audit --audit-level=(low|moderate|high|critical)");
                return;
            }
        }
    }
}
