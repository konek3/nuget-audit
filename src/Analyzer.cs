using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using nuget_audit.Converters;
using nuget_audit.Enums;
using nuget_audit.Helpers;
using nuget_audit.Interfaces;
using nuget_audit.Services;
using nuget_audit.src.Extensions;

namespace nuget_audit
{
    public class Analyzer : IAnalyzer
    {
        private readonly IAuditService _auditService;
        private readonly IProjectInformationService _projectInformationService;
        private readonly IConfigurationHelper _configurationHelper;

        public Analyzer()
        {
            _configurationHelper = new ConfigurationHelper();
            _configurationHelper.ValidateConfiguration();

            _auditService = new AuditService(_configurationHelper);
            _projectInformationService = new ProjectInformationService();
        }

        public async Task Analyze(string[] args)
        {
            ValidateArgs(args);

            var packages = await _projectInformationService.GetNugetPackagesAsync(args.Last()).ToListAsync();

            Console.WriteLine($"Analyzing {packages.Count()} NuGet packages...");

            var results = await _auditService.GetResults(packages);

            foreach (var result in results)
            {
                var packageName = result.Coordinates.Split("/").Last();

                var vulnerabilitiesCount = result.Vulnerabilities.Count();

                if (vulnerabilitiesCount == 0)
                {
                    ConsoleHelper.ColorWrite(ConsoleColor.Green, "[OK] ");
                    Console.WriteLine($"No vulnerabilities found in package: {packageName}");
                    continue;
                }

                ConsoleHelper.ColorWrite(ConsoleColor.Yellow, "[WARNING] ");

                Console.WriteLine($"Vulnerabilities found: {vulnerabilitiesCount}");

                foreach (var vulnerability in result.Vulnerabilities)
                {
                    Console.WriteLine($"{vulnerability.Title}");
                    var severity = CvssScoreConverter.ConvertScoreToSeverity(vulnerability.CvssScore);

                    Console.Write($"Level: [");

                    switch (severity)
                    {
                        case Severity.Low:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case Severity.Medium:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case Severity.High:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case Severity.Crticial:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case Severity.None:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }

                    Console.Write($"{severity.ToString()}");
                    Console.ResetColor();

                    Console.WriteLine("]\n");
                }
            }
        }

        private void ValidateArgs(string[] args)
        {
            var path = args.Last();

            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Project path is not valid. Please specify a proper path.");
            }
        }
    }
}