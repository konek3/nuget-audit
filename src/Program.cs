using System;
using System.Reflection;
using System.Threading.Tasks;
using nuget_audit.Helpers;

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
                Console.WriteLine("nuget-audit -- audit-level=(Low|Medium|High|Critical) (path)");

                return;
            }

            RunAnalysis(args).GetAwaiter().GetResult();
            
            return;
        }

        private static async Task RunAnalysis(string[] args)
        {
            try
            {
                var result = await new Analyzer(args).Analyze();

                if(result)
                {
                    Environment.Exit(1);
                }
            }
            catch(Exception e)
            {
                ConsoleHelper.ColorWriteLine(ConsoleColor.Red, $"An internal error occured while running a tool: {e.Message}");
                Environment.Exit(2);
            }
        }
    }
}
