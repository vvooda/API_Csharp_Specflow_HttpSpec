using System.Net.Http;
using System.Threading.Tasks;

namespace ApiTestingFramework.Interfaces
{
    public interface IResponseHandler
    {
        Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response);
        Task<string> GetResponseContentAsync(HttpResponseMessage response);
        void VerifyStatusCode(HttpResponseMessage response, int expectedStatusCode);
        Task<T[]> DeserializeArrayResponseAsync<T>(HttpResponseMessage response);
    }
}