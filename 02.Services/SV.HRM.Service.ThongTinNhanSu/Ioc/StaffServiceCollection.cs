using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SV.HRM.Caching.Ioc;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Ioc;

namespace SV.HRM.Service.ThongTinNhanSu.Ioc
{
    public static class StaffServiceCollection
    {
        public static void RegisterIoCs(this IServiceCollection services, IConfiguration configuration)
        {
            AppSettings.Instance.SetConfiguration(configuration);
            services.AddDataProcessServices();
            services.AddCachingProcessServices();
            services.AddStaffAutoMapper();
            services.AddScoped<IStaffHandler, StaffHandler>();
            services.AddScoped<IHistoryHandler, HistoryHandler>();
            services.AddScoped<IBaseHandler, BaseHandler>();
            services.AddScoped<IStaffDiplomaHandler, StaffDiplomaHandler>();
            services.AddScoped<IStaffSalaryHandler, StaffSalaryHandler>();
            services.AddScoped<ILabourContractHandler, LabourContractHandler>();
            services.AddScoped<IStaffFamilyHandler, StaffFamilyHandler>();
            services.AddScoped<IStaffPartyHandler, StaffPartyHandler>();
            services.AddScoped<IStaffRefHandler, StaffRefHandler>();
            services.AddScoped<IStaffAssetHandler, StaffAssetHandler>();
            services.AddScoped<IStaffMilitaryHandler, StaffMilitaryHandler>();
            services.AddScoped<IDisciplineDetailHandler, DisciplineDetailHandler>();
            services.AddScoped<IDecisionHandler, DecisionHandler>();
            services.AddScoped<IQuanLySucKhoeHandler, QuanLySucKhoeHandler>();
            services.AddScoped<IStaffRewardHandler, StaffRewardHandler>();
            services.AddScoped<IStaffAssessmentHandler, StaffAssessmentHandler>();
            services.AddScoped<IUniformHandler, UniformHandler>();
            services.AddScoped<IProfileDocumentHandler, ProfileDocumentHandler>();
            services.AddScoped<IImportExcelHandler, ImportExcelHandler>();
            services.AddScoped<IDashboardHandler, DashboardHandler>();
            services.AddScoped<IRemindWorkHandler, RemindWorkHandler>();
            services.AddScoped<IStaffDocumentHandler, StaffDocumentHandler>();
        }
    }
}
