using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SV.HRM.Caching.Ioc;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Ioc;
using SV.HRM.Service.Report.Logic;

namespace SV.HRM.Service.Report.Ioc
{
    public static class ReportServiceCollection
    {
        public static void RegisterIoCs(this IServiceCollection services, IConfiguration configuration)
        {
            AppSettings.Instance.SetConfiguration(configuration);
            services.AddDataProcessServices();
            services.AddCachingProcessServices();
            services.AddReportAutoMapper();
            services.AddScoped<IDapperReport, DapperReport>();
            services.AddScoped<IBaseReportHandler, BaseReportHandler>();

        }
    }
}
