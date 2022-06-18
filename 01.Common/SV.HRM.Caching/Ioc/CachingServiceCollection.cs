﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SV.HRM.Caching.Impl;
using SV.HRM.Caching.Interface;
using SV.HRM.Core.Utils;

namespace SV.HRM.Caching.Ioc
{
    public static class CachingServiceCollection
    {
        public static void AddCachingProcessServices(this IServiceCollection services)
        {
            switch (StaticVariable.CacheType)
            {
                case 1:
                    RedisCachedServiceCollection.RegisterIoCs(ref services);
                    break;

                default:
                    services.AddSingleton<ICached>(cd => { return new NoCached(); });
                    break;
            }
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICacheAppService, CacheAppService>();
        }
    }
}
