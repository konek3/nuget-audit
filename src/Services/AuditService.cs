using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using nuget_audit.Interfaces;
using nuget_audit.Models;
using nuget_audit.src.Extensions;
using RestSharp;
using RestSharp.Authenticators;

namespace nuget_audit.Services
{
    public class AuditService : IAuditService
    {
        private readonly IRestClient _restClient;
        private readonly IConfigurationHelper _configurationHelper;

        public AuditService(IConfigurationHelper configurationHelper)
        {
            _configurationHelper = configurationHelper;
            _restClient = new RestClient();
            _restClient.BaseUrl = new Uri(_configurationHelper.OssIndexUri);
            _restClient.AddDefaultHeader("accept", "application/vnd.ossindex.component-report.v1+json");
            _restClient.AddDefaultHeader("Content-Type", "application/json");
            _restClient.Authenticator = new SimpleAuthenticator("username", _configurationHelper.Username, "password", _configurationHelper.ApiKey);
        }

        public async Task<IEnumerable<AuditResult>> GetResults(IEnumerable<IPackageInfo> packages)
        {
            var packagesCoordinates = packages.Select(pkg => $"pkg:nuget/{pkg.Name}@{pkg.Version}");

            var query = new AuditQuery
            {
                Coordinates = packagesCoordinates
            };

            var request = new RestRequest(Method.POST);

            request.AddJsonBody(JsonConvert.SerializeObject(query));

            var response = await _restClient.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new InvalidOperationException("Request failed with code: " + response.StatusCode);
            }

            return JsonConvert.DeserializeObject<IEnumerable<AuditResult>>(response.Content);
        }
    }
}