using System;
using nuget_audit.Interfaces;

namespace nuget_audit.Helpers
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public string ApiKey { get; } = Environment.GetEnvironmentVariable("NUGET_AUDIT_API_KEY");
        public string Username { get; } = Environment.GetEnvironmentVariable("NUGET_AUDIT_USERNAME");
        public string OssIndexUri { get; } = "https://ossindex.sonatype.org/api/v3/component-report";

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
    }
}