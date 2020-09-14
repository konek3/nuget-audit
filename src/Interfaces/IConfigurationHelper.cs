using nuget_audit.Enums;

namespace nuget_audit.Interfaces
{
    public interface IConfigurationHelper
    {
        string ApiKey { get; }
        string Username { get; }
        string OssIndexUri { get; } 
        string Path { get; } 
        Severity AuditLevel { get; }

        void ValidateConfiguration();  

        void SetConfigurationFromArgs(string[] args);
    }
}