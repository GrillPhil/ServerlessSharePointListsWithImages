﻿using MyListApp.Auth;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyListApp.Graph
{
    public class SharePointListService : IListService
    {
        private readonly IAuthService _authService;
        private const string _graphEndpoint = "https://graph.microsoft.com/beta/";

        public SharePointListService(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task CreateItemAsync(string siteId, string listId, ListItem item)
        {
            var httpClient = await CreateAuthenticatedHttpClient();
            var requestUrl = $"{_graphEndpoint}sites/{siteId}/lists/{listId}/items";
            var requestContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(requestUrl, requestContent);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
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

