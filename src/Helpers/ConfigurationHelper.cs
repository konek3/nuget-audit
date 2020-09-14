using System;
using nuget_audit.Interfaces;
using nuget_audit.Enums;
using System.Linq;
using System.IO;

namespace nuget_audit.Helpers
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public string ApiKey { get; } = Environment.GetEnvironmentVariable("NUGET_AUDIT_API_KEY");
        public string Username { get; } = Environment.GetEnvironmentVariable("NUGET_AUDIT_USERNAME");
        public string OssIndexUri { get; } = "https://ossindex.sonatype.org/api/v3/component-report";
        public Severity AuditLevel { get; private set; }
        public string Path { get; private set; }

        public static readonly string[] ErrorLevels = new [] { "" };

        public void ValidateConfiguration()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                throw new ArgumentNullException(nameof(ApiKey), "NUGET_AUDIT_API_KEY environment variable is not set.");
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                throw new ArgumentNullException(nameof(Username), "NUGET_AUDIT_USERNAME environment variable is not set.");
            }
        }

        public void SetConfigurationFromArgs(string[] args)
        {
            ValidateArgs(args);

            var errorLevel = args.FirstOrDefault(arg => arg.Contains("audit-level"));

            if (errorLevel != null)
            {
                AuditLevel = (Severity)Enum.Parse(typeof(Severity), errorLevel.Split("=").Last());
            }

            Path = args.LastOrDefault();
        }

        private void ValidateArgs(string[] args)
        {
            if (args.Length > 2)
            {
                throw new ArgumentException("Number of arguments is not valid.");
            }

            var path = args.Last();

            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Project path is not valid. Please specify a proper path.");
            }
        }
    }
}