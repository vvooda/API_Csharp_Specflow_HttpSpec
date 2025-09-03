using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FluentAssertions;
using ApiTestingFramework.Interfaces;

namespace ApiTestingFramework.Implementations
{
    public class ResponseHandler : IResponseHandler
    {
        public async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<string> GetResponseContentAsync(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        public void VerifyStatusCode(HttpResponseMessage response, int expectedStatusCode)
        {
            response.StatusCode.Should().Be((System.Net.HttpStatusCode)expectedStatusCode);
        }

        public async Task<T[]> DeserializeArrayResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T[]>(content);
        }
    }
}