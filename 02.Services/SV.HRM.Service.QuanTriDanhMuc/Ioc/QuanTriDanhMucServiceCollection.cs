using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SV.HRM.Caching.Ioc;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Ioc;

namespace SV.HRM.Service.QuanTriDanhMuc.Ioc
{
    public static class QuanTriDanhMucServiceCollection
    {
        public static void RegisterIoCs(this IServiceCollection services, IConfiguration configuration)
        {
            AppSettings.Instance.SetConfiguration(configuration);
            services.AddDataProcessServices();
            services.AddCachingProcessServices();
            services.AddQuanTriDanhMucAutoMapper();
            services.AddScoped<IBaseHandler, BaseHandler>();
            services.AddScoped<IBankHandler, BankHandler>();
            services.AddScoped<IHospitalHandler, HospitalHandler>();
            services.AddScoped<IPositionHandler, PositionHandler>();
            services.AddScoped<IJobTitleHandler, JobTitleHandler>();
            services.AddScoped<ISchoolHandler, SchoolHandler>();
            services.AddScoped<IChucVuChinhQuyenHandler, ChucVuChinhQuyenHandler>();
            services.AddScoped<IChucVuKiemNhiemHandler, ChucVuKiemNhiemHandler>();
            services.AddScoped<INhomNgachLuongHandler, NhomNgachLuongHandler>();
            services.AddScoped<IOccupationHandler, OccupationHandler>();
            services.AddScoped<ITitleHandler, TitleHandler>();
            services.AddScoped<IFamilyRelationShipHandler, FamilyRelationShipHandler>();
            services.AddScoped<IDiplomaHandler, DiplomaHandler>();
            services.AddScoped<IChuyenKhoaHandler, ChuyenKhoaHandler>();
            services.AddScoped<ISpecialityHandler, SpecialityHandler>();
            services.AddScoped<IDecisionItemHandler, DecisionItemHandler>();
            services.AddScoped<IContractTypeHandler, ContractTypeHandler>();
            services.AddScoped<IHealthPeriodHandler, HealthPeriodHandler>();
            services.AddScoped<IRewardHandler, RewardHandler>();
            services.AddScoped<IDisciplineTypeHandler, DisciplineTypeHandler>();
            services.AddScoped<IMucDoViPhamKyLuatHandler, MucDoViPhamKyLuatHandler>();
            services.AddScoped<IChuyenNganhHandler, ChuyenNganhHandler>();
            services.AddScoped<IPartyCellHandler, PartyCellHandler>();
            services.AddScoped<IPartyTitleHandler, PartyTitleHandler>();
            services.AddScoped<ILeaveTypeHandler, LeaveTypeHandler>();
            services.AddScoped<ITrinhDoChuyenMonHandler, TrinhDoChuyenMonHandler>();
            services.AddScoped<ITrinhDoDaoTaoHandler, TrinhDoDaoTaoHandler>();
            services.AddScoped<INgachLuongHandler, NgachLuongHandler>();
            services.AddScoped<IBacLuongHandler, BacLuongHandler>();
            services.AddScoped<IDegreeHandler, DegreeHandler>();
            services.AddScoped<IAcademicRankHandler, AcademicRankHandler>();
            services.AddScoped<ILeaveHandler, LeaveHandler>();
            services.AddScoped<IShiftHandler, ShiftHandler>();
            services.AddScoped<IReligionHandler, ReligionHandler>();
            services.AddScoped<IWorkflowHandler, WorkflowHandler>();
            services.AddScoped<IWorkflowCommandHandler, WorkflowCommandHandler>();
            services.AddScoped<IWorkflowStateHandler, WorkflowStateHandler>();
            services.AddScoped<IHolidayHandler, HolidayHandler>();
            services.AddScoped<ITACategoryHandler, TACategoryHandler>();
            services.AddScoped<IWKTimeHandler, WKTimeHandler>();
        }
    }
}
