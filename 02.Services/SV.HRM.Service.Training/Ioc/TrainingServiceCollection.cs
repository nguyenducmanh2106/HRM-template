using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SV.HRM.Caching.Ioc;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Ioc;

namespace SV.HRM.Service.Training.Ioc
{
    public static class TrainingServiceCollection
    {
        public static void RegisterIoCs(this IServiceCollection services, IConfiguration configuration)
        {
            AppSettings.Instance.SetConfiguration(configuration);
            services.AddDataProcessServices();
            services.AddCachingProcessServices();
            services.AddTrainingAutoMapper();
            services.AddScoped<IBaseHandler, BaseHandler>();
            services.AddScoped<ITrainingHandler, TrainingHandler>();
            services.AddScoped<IDonViDaoTaoHandler, DonViDaoTaoHandler>();
            services.AddScoped<IChuyenNganhDaoTaoHandler, ChuyenNganhDaoTaoHandler>();
            services.AddScoped<ITrinhDoDaoTaoHandler, TrinhDoDaoTaoHandler>();
        }
    }
}
