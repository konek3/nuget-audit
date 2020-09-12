using System.Collections.Generic;

namespace nuget_audit.Interfaces
{
    public interface IProjectInformationService
    {
         IAsyncEnumerable<IPackageInfo> GetNugetPackagesAsync(string path);
    }
}