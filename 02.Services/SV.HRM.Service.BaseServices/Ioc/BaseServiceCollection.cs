using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SV.HRM.Caching.Ioc;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Ioc;

namespace SV.HRM.Service.BaseServices.Ioc
{
    public static class BaseServiceCollection
    {
        public static void RegisterIoCs(this IServiceCollection services, IConfiguration configuration)
        {
            AppSettings.Instance.SetConfiguration(configuration);
            services.AddDataProcessServices();
            services.AddCachingProcessServices();
            services.AddStaffAutoMapper();
            services.AddScoped<IBaseHandler, BaseHandler>();
            services.AddScoped<ITableFieldHandler, TableFieldHandler>();
            services.AddScoped<IGroupBoxHandler, GroupBoxHandler>();
            services.AddScoped<IUserConfigHandler, UserConfigHandler>();
        }
    }
}
