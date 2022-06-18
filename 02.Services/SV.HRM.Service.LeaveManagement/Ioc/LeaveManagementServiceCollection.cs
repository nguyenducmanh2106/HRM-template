using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SV.HRM.Caching.Ioc;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Ioc;

namespace SV.HRM.Service.LeaveManagement.Ioc
{
    public static class LeaveManagementServiceCollection
    {
        public static void RegisterIoCs(this IServiceCollection services, IConfiguration configuration)
        {
            AppSettings.Instance.SetConfiguration(configuration);
            services.AddDataProcessServices();
            services.AddCachingProcessServices();
            services.AddLeaveManagementAutoMapper();
            services.AddScoped<IBaseHandler, BaseHandler>();
            services.AddScoped<ILeaveManagementHandler, LeaveManagementHandler>();
            services.AddScoped<IWorkflowHandler, WorkflowHandler>();
        }
    }
}
