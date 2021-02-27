using MyListApp.Auth;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyListApp.Graph
{
    public class SharePointFileService : IFileService
    {
        private readonly IAuthService _authService;
        private const string _graphEndpoint = "https://graph.microsoft.com/beta/";

        public SharePointFileService(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<DriveItem> UploadAsync(string path, byte[] content, string contentType)
        {
            var httpClient = await CreateAuthenticatedHttpClient();
            var requestUrl = $"{_graphEndpoint}{path}:/content";

            var requestContent = new ByteArrayContent(content);

            var response = await httpClient.PutAsync(requestUrl, requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<DriveItem>(responseContent);
            }
            else
            {
                throw new Exception("Call to Graph failed with status code " + response.StatusCode);
            }
        }

        private async Task<HttpClient> CreateAuthenticatedHttpClient()
        {
            var token = await _authService.GetAccessTokenAsync();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            return httpClient;
        }
    }
}
