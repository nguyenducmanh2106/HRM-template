using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SV.HRM.API.HttpServices;
using SV.HRM.Caching.Ioc;
using SV.HRM.Core.Utils;
using System;
using System.Net.Http.Headers;

namespace SV.HRM.API.Ioc
{
    public static class ApiServiceCollection
    {
        public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            AppSettings.Instance.SetConfiguration(configuration);
            services.AddCachingProcessServices();
            services.AddScoped<IAuthenHttpService, AuthenHttpService>();
            services.AddHttpClient<IBaseHttpService, BaseHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.BASE_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IUserConfigHttpService, UserConfigHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.BASE_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IStaffHttpService, StaffHttpService>(client =>
             {
                 client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
             });
            services.AddHttpClient<IHistoryHttpService, HistoryHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IStaffDiplomaHttpService, StaffDiplomaHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IStaffSalaryHttpService, StaffSalaryHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<ILabourContractHttpService, LabourContractHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IStaffFamilyHttpService, StaffFamilyHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IStaffPartyHttpService, StaffPartyHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IStaffRefHttpService, StaffRefHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IStaffMilitaryHttpService, StaffMilitaryHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IDisciplineDetailHttpService, DisciplineDetailHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IDecisionHttpService, DecisionHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IQuanLySucKhoeHttpService, QuanLySucKhoeHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IStaffRewardHttpService, StaffRewardHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IStaffAssetHttpService, StaffAssetHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IStaffAssessmentHttpService, StaffAssessmentHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IUniformHttpService, UniformHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IProfileDocumentHttpService, ProfileDocumentHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IImportExcelHttpService, ImportExcelHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IDashboardHttpService, DashboardHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IStaffDocumentHttpService, StaffDocumentHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IRemindWorkHttpService, RemindWorkHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.THONG_TIN_NHAN_SU_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            //Quan tri danh muc
            services.AddHttpClient<IBankHttpService, BankHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IHospitalHttpService, HospitalHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IPositionHttpService, PositionHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IJobTitleHttpService, JobTitleHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<ISchoolHttpService, SchoolHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IChucVuChinhQuyenHttpService, ChucVuChinhQuyenHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IChucVuKiemNhiemHttpService, ChucVuKiemNhiemHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<INhomNgachLuongHttpService, NhomNgachLuongHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IOccupationHttpService, OccupationHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<ITitleHttpService, TitleHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IFamilyRelationShipHttpService, FamilyRelationShipHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IDiplomaHttpService, DiplomaHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IChuyenKhoaHttpService, ChuyenKhoaHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<ISpecialityHttpService, SpecialityHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IDecisionItemHttpService, DecisionItemHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IContractTypeHttpService, ContractTypeHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
			services.AddHttpClient<IHealthPeriodHttpService, HealthPeriodHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IRewardHttpService, RewardHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IDisciplineTypeHttpService, DisciplineTypeHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IMucDoViPhamKyLuatHttpService, MucDoViPhamKyLuatHttpSercie>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IChuyenNganhHttpService, ChuyenNganhHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString("QuanTriDanhMucHttpServiceUrl"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IPartyCellHttpService, PartyCellHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IPartyTitleHttpService, PartyTitleHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<ILeaveTypeHttpService, LeaveTypeHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<ITrinhDoChuyenMonHttpService, TrinhDoChuyenMonHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<ITrinhDoDaoTaoHttpService, TrinhDoDaoTaoHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<INgachLuongHttpService, NgachLuongHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IBacLuongHttpService, BacLuongHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IDegreeHttpService, DegreeHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IAcademicRankHttpService, AcademicRankHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<ILeaveHttpService, LeaveHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IShiftHttpService, ShiftHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IReligionHttpService, ReligionHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IWorkflowHttpService, WorkflowHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IWorkflowCommandHttpService, WorkflowCommandHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IWorkflowStateHttpService, WorkflowStateHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IHolidayHttpService, HolidayHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<ITACategoryHttpService, TACategoryHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IWKTimeHttpService, WKTimeHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.QUAN_TRI_DANH_MUC_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            // http service  report
            services.AddHttpClient<IReportServices, ReportService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.REPORT_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            // Training
            services.AddHttpClient<ITrainingHttpService, TrainingHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.TRAINING_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<ITrinhDoDTHttpService, TrinhDoDTHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.TRAINING_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IChuyenNganhDaoTaoHttpService, ChuyenNganhDaoTaoHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.TRAINING_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IDonViDaoTaoHttpService, DonViDaoTaoHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.TRAINING_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            // Attendance
            services.AddHttpClient<IAttendanceHttpService, AttendanceHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.ATTENDANCE_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            //Leave management
            services.AddHttpClient<ILeaveManagementHttpService, LeaveManagementHttpService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.Instance.GetString(Constant.HttpServiceUrl.LEAVE_MANAGEMENT_HTTP_SERVICE_URL));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }
    }
}
