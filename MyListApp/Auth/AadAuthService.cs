using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyListApp.Auth
{
    public class AadAuthService : IAuthService
    {
        private const string GRANT_TYPE = "client_credentials";
        private const string AUTH_URL = "https://login.microsoftonline.com/";
        private readonly string _tenant, _clientId, _clientSecret, _resource, _scope;
        
        public AadAuthService(IOptions<AuthOptions> options)
        {
            _tenant = options.Value.TenantId;
            _clientId = options.Value.ClientId;
            _clientSecret = options.Value.ClientSecret;
            _resource = options.Value.Resource;
            _scope = options.Value.Scope;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("scope", _scope),
                new KeyValuePair<string, string>("grant_type", GRANT_TYPE),
                new KeyValuePair<string, string>("resource",_resource)
            };
            var client = new HttpClient();
            var requestUrl = $"{AUTH_URL}{_tenant}/oauth2/token";
            var requestContent = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(requestUrl, requestContent);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic tokenResponse = JsonConvert.DeserializeObject(responseBody);
            return tokenResponse?.access_token;
        }
    }
}
