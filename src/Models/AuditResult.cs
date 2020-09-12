using System;
using Newtonsoft.Json;
using nuget_audit.Interfaces;

namespace nuget_audit.Models
{
    public class AuditResult : IAuditResult
    {
        [JsonProperty("coordinates")]
        public string Coordinates { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("reference")]
        public Uri Reference { get; set; }

        [JsonProperty("vulnerabilities")]
        public Vulnerability[] Vulnerabilities { get; set; }
    }
}