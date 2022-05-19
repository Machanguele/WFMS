using System.Net.Http;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IHttpClientHelper
    {
        Task<HttpResponseMessage>  DoUpdateSystems(object obj,string url, string httpMethod);
    }
}