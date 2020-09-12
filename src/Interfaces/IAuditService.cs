using System.Collections.Generic;
using System.Threading.Tasks;
using nuget_audit.Models;

namespace nuget_audit.Interfaces
{
    public interface IAuditService
    {
         Task<IEnumerable<AuditResult>> GetResults(IEnumerable<IPackageInfo> packages);
    }
}