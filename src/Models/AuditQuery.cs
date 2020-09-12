using System.Collections.Generic;
using Newtonsoft.Json;

namespace nuget_audit.Models
{
    public class AuditQuery
    {
        [JsonProperty("coordinates")]
        public IEnumerable<string> Coordinates {get; set; }
    }
}