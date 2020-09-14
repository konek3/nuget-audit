using System;
using System.Linq;
using System.Threading.Tasks;
using nuget_audit.Converters;
using nuget_audit.Enums;
using nuget_audit.Helpers;
using nuget_audit.Interfaces;
using nuget_audit.Services;
using nuget_audit.Extensions;

namespace nuget_audit
{
    public class Analyzer : IAnalyzer
    {
        private readonly IAuditService _auditService;
        private readonly IProjectInformationService _projectInformationService;
        private readonly IConfigurationHelper _configurationHelper;
        private delegate void _configurationHelperBootstrap();

        public Analyzer(string[] args)
        {
            _configurationHelper = new ConfigurationHelper();

            _configurationHelper.ValidateConfiguration();
            _configurationHelper.SetConfigurationFromArgs(args);

            _auditService = new AuditService(_configurationHelper);
            _projectInformationService = new ProjectInformationService();
        }

        public async Task<bool> Analyze()
        {
            var shouldFail = false;
            var packages = await _projectInformationService.GetNugetPackagesAsync(_configurationHelper.Path).ToListAsync();

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

                ConsoleHelper.ColorWrite(ConsoleColor.DarkYellow, "[WARNING] ");

                Console.WriteLine($"Found {vulnerabilitiesCount} vulnerabilities in package: {packageName}\n");

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
                        case Severity.Critical:
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

            var flatResults = results.SelectMany(result => result.Vulnerabilities).ToList();
            var totalCount = flatResults.Count;

            Console.WriteLine($"\nTotal vulnerabilities found: {totalCount}");

            var vulnerabilitiesLevel = flatResults.Select(vul => CvssScoreConverter.ConvertScoreToSeverity(vul.CvssScore));

            if (vulnerabilitiesLevel.Any(level => level > _configurationHelper.AuditLevel))
            {
                shouldFail = true;
            }

            return shouldFail;
        }
    }
}