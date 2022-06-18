using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SV.HRM.Caching.Common;
using SV.HRM.Caching.Impl;
using SV.HRM.Caching.Interface;
using SV.HRM.Core.Utils;

namespace SV.HRM.Caching.Ioc
{
    public static class RedisCachedServiceCollection
    {
        public static IServiceCollection RegisterIoCs(ref IServiceCollection services)
        {
            var cacheQueueConfig = AppSettings.Instance.Get<CachingConfigModel>("Cache:Redis:Queue");
            var cacheDataConfig = AppSettings.Instance.Get<CachingConfigModel>("Cache:Redis:Data");
            if (cacheDataConfig.Allow)
            {
                services.AddSingleton<ICached>(sv => { return new RedisCached(cacheDataConfig); });
            }
            else
            {
                services.AddSingleton<ICached>(sv => { return new NoCached(); });
            }

            if (cacheQueueConfig.Allow)
            {
                services.AddSingleton<IQueueAndListCached>(sv => { return new RedisCached(cacheQueueConfig); });
            }
            else
            {
                services.AddSingleton<IQueueAndListCached>(sv => { return new NoCacheQueue(); });
            }

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
