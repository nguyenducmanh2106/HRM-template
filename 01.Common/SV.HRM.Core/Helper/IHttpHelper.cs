using System.Threading.Tasks;
namespace SV.HRM.Core.Helper
{
    public interface IHttpHelper
    {
        Task<T> GetAsync<T>(string url);
        Task<T> GetAsync<T>(string url, string accessToken);
        Task<T> PostAsync<T>(string url, object data = null);
        Task<T> PostAsyncCustomGrid<T>(string url, object data = null);
        Task<T> DeleteAsync<T>(string url);
    }
}
