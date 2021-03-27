using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CookbookAPI.ApiClients.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace CookbookAPI.ApiClients
{
    public class ApiClient : IApiClient
    {
        private readonly IRestClient _client;
        private string _baseUrl;

        public ApiClient(IRestClient client, string baseUrl)
        {
            _client = client;
            _baseUrl = baseUrl;
            _client.BaseUrl = new Uri(_baseUrl);
        }

        public ApiClient(IRestClient client)
        {
            _client = client;
        }

        public void ChangeBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client.BaseUrl = new Uri(_baseUrl);
        }

        public async Task<IRestResponse> RequestAsync(string endpoint,
            Dictionary<string, string> parameters = null, Method method = Method.GET)
        {
            if (string.IsNullOrEmpty(_baseUrl))
                throw new Exception("You have to set base url");

            var request = new RestRequest(endpoint, method);

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    request.AddParameter(p.Key, p.Value);
                }
            }

            var response = await _client.ExecuteAsync(request);
            return response;
        }

        public async Task<List<TDto>> DownloadAllIResources<TDto>(string resourcesUrl)
        {
            var response = await RequestAsync(resourcesUrl);

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var dtos = JsonConvert.DeserializeObject<List<TDto>>(response.Content, serializerSettings);

            return dtos;
        }
    }
}
