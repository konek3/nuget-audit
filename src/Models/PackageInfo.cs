using nuget_audit.Interfaces;

namespace nuget_audit.Models
{
    public class PackageInfo : IPackageInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }
}