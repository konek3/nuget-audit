namespace nuget_audit.Interfaces
{
    public interface IConfigurationHelper
    {
        string ApiKey { get; }
        string Username { get; }
        string OssIndexUri { get; }  

        void ValidateConfiguration();      
    }
}