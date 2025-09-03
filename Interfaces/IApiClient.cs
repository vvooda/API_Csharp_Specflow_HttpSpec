using System.Net.Http;
using System.Threading.Tasks;

namespace ApiTestingFramework.Interfaces
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        Task<HttpResponseMessage> PostAsync(string url, string jsonContent);
        void SetBaseUrl(string baseUrl);
        void AddHeader(string name, string value);
    }
}