using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace CookbookAPI.ApiClients.Interfaces
{
    public interface IApiClient
    {
        public void ChangeBaseUrl(string baseUrl);

        public Task<IRestResponse> RequestAsync(string endpoint,
            Dictionary<string, string> parameters = null, Method method = Method.GET);

        public Task<List<TDto>> DownloadAllIResources<TDto>(string resourcesUrl);

    }
}
