using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IRestService
    {
        Task<T> GetRestServiceAsync<T>(string url, string controller, string method,
            IDictionary<string, string> parameters, IDictionary<string, string> headers);

        Task<T> PostRestServiceAsync<T>(string url, string controller, string method
            , object parameters, IDictionary<string, string> headers);

        Task<T> PostRestServiceStringParametersAsync<T>(string url, string controller, string method
            , IDictionary<string, string> parameters, IDictionary<string, string> headers);
    }
}
