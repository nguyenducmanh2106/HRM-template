using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SV.HRM.Caching.Interface
{
    public interface ICached
    {
        bool Add<T>(string key, T item, int expireInMinute = 0);
        bool Add(string key, string item, int expireInMinute = 0);

        Task<bool> AddAsync<T>(string key, T item, int expireInMinute = 0);

        Task<bool> AddAsync(string key, string item, int expireInMinute = 0);

        T Get<T>(string key, HttpContext context = null, string refreshKey = null);
        string Get(string key, HttpContext context = null, string refreshKey = null);

        Task<T> GetAsync<T>(string key, HttpContext context = null, string refreshKey = null);

        Task<string> GetAsync(string key, HttpContext context = null, string refreshKey = null);

        bool Remove(string key);
        Task<bool> RemoveAsync(string key);
    }
}
