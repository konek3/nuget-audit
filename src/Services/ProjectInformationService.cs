using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using nuget_audit.Interfaces;
using nuget_audit.Models;

namespace nuget_audit.Services
{
    public class ProjectInformationService : IProjectInformationService
    {
        public const string PackageReferenceXmlKey = "PackageReference";
        public const string NuGetNameXmlKey = "Include";
        public const string NuGetVersionXmlKey = "Version";

        public async IAsyncEnumerable<IPackageInfo> GetNugetPackagesAsync(string path)
        {
            if (Directory.Exists(path))
            {
                var projects = ProcessDirectory(path);

                foreach (var project in projects)
                {
                    var xmlContent = await File.ReadAllTextAsync(project);

                    if (xmlContent != string.Empty)
                    {
                        var doc = XDocument.Parse(xmlContent);

                        var nugets = doc.Descendants(PackageReferenceXmlKey);

                        foreach(var nuget in nugets)
                        {
                            yield return new PackageInfo
                            {
                                Name = nuget.Attribute(NuGetNameXmlKey).Value,
                                Version = nuget.Attribute(NuGetVersionXmlKey).Value
                            };
                        }
                    }
                }
            }
        }

        private IEnumerable<string> ProcessDirectory(string targetDirectory)
        {
            var fileEntries = Directory.GetFiles(targetDirectory);

            foreach (string fileName in fileEntries)
            {
                var result = ProcessFile(fileName);

                if (result != null)
                {
                    yield return result;
                }
            }

            var subdirectoryEntries = Directory.GetDirectories(targetDirectory);

            foreach (string subdirectory in subdirectoryEntries)
            {
                var projects = ProcessDirectory(subdirectory);

                foreach (var project in projects)
                {
                    yield return project;
                }
            }
        }

        private string ProcessFile(string path)
        {
            if (IsFileDotnetProjectFile(path))
            {
                return path;
            }

            return null;
        }

        private bool IsFileDotnetProjectFile(string path)
        {
            return path.EndsWith(".csproj");
        }
    }
}