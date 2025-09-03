using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiTestingFramework.Interfaces;

namespace ApiTestingFramework.Implementations
{
    public class HttpClientService : IApiClient
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl;

        public HttpClientService()
        {
            _httpClient = new HttpClient();
        }

        public void SetBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public void AddHeader(string name, string value)
        {
            _httpClient.DefaultRequestHeaders.Add(name, value);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var fullUrl = GetFullUrl(url);
            return await _httpClient.GetAsync(fullUrl);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            var fullUrl = GetFullUrl(url);
            return await _httpClient.PostAsync(fullUrl, content);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, string jsonContent)
        {
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await PostAsync(url, content);
        }

        private string GetFullUrl(string url)
        {
            return _baseUrl != null ? new Uri(new Uri(_baseUrl), url).ToString() : url;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}