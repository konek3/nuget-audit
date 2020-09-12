namespace nuget_audit.Interfaces
{
    public interface IPackageInfo
    {
         string Name { get; set; }

         string Version { get; set; }
    }
}