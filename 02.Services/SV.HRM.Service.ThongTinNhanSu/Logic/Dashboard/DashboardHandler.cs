using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SV.HRM.Caching.Interface;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Impl;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static SV.HRM.Models.DashboardModel;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class DashboardHandler : IDashboardHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _configuration;
        private readonly ICached _cached;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IRemindWorkHandler _remindWorkHandler;

        public DashboardHandler(IConfiguration configuration, ICached cached, IHttpContextAccessor httpContextAccessor, IDapperUnitOfWork dapperUnitOfWork, IRemindWorkHandler remindWorkHandler)
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _cached = cached;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _remindWorkHandler = remindWorkHandler;
        }

        public async Task<Response<BoxModel>> StatisticsAllStaff(BoxFilter filter)
        {
            try
            {
                List<int> listOrgID = GetListOrgByParentID(filter.OrganizationId);

                BoxModel result = new BoxModel();
                DynamicParameters param = new DynamicParameters();
                param.Add("@OrgIds", String.Join(',', listOrgID));
                param.Add("@Date", filter.DateNow);
                param.Add("@CountVC", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@Count68", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@CountShortTime", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@TotalCountStaff", dbType: DbType.Int32, direction: ParameterDirection.Output);

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Dashboard.json", "Dashboard", "sp_Dashboard_StatisticsAllStaff");
                await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);

                result.CountVC = param.Get<int>("@CountVC");
                result.Count68 = param.Get<int>("@Count68");
                result.CountShortTime = param.Get<int>("@CountShortTime");
                result.TotalCountStaff = param.Get<int>("@TotalCountStaff");

                return new Response<BoxModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<BoxModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<BoxModel>> StatisticsNewStaff(BoxFilter filter)
        {
            try
            {
                List<int> listOrgID = GetListOrgByParentID(filter.OrganizationId);

                BoxModel result = new BoxModel();
                DynamicParameters param = new DynamicParameters();
                param.Add("@OrgIds", String.Join(',', listOrgID));
                param.Add("@StartDate", filter.StartDate);
                param.Add("@EndDate", filter.EndDate.Value.AddDays(1));
                param.Add("@CountVC", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@Count68", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@CountShortTime", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@TotalCountStaff", dbType: DbType.Int32, direction: ParameterDirection.Output);

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Dashboard.json", "Dashboard", "sp_Dashboard_StatisticsNewStaff");
                await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);

                result.CountVC = param.Get<int>("@CountVC");
                result.Count68 = param.Get<int>("@Count68");
                result.CountShortTime = param.Get<int>("@CountShortTime");
                result.TotalCountStaff = param.Get<int>("@TotalCountStaff");

                return new Response<BoxModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<BoxModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<BoxModel>> StatisticsStaffOut(BoxFilter filter)
        {
            try
            {
                List<int> listOrgID = GetListOrgByParentID(filter.OrganizationId);

                BoxModel result = new BoxModel();
                DynamicParameters param = new DynamicParameters();
                param.Add("@OrgIds", String.Join(',', listOrgID));
                param.Add("@StartDate", filter.StartDate);
                param.Add("@EndDate", filter.EndDate.Value.AddDays(1));
                param.Add("@CountVC", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@Count68", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@CountShortTime", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@TotalCountStaff", dbType: DbType.Int32, direction: ParameterDirection.Output);

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Dashboard.json", "Dashboard", "sp_Dashboard_StatisticsStaffOut");
                await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);

                result.CountVC = param.Get<int>("@CountVC");
                result.Count68 = param.Get<int>("@Count68");
                result.CountShortTime = param.Get<int>("@CountShortTime");
                result.TotalCountStaff = param.Get<int>("@TotalCountStaff");

                return new Response<BoxModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<BoxModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<DonutsChartOptionsResult>> StatisticsHumanResourceStructure(BoxFilter filter)
        {
            try
            {
                List<int> listOrgID = GetListOrgByParentID(filter.OrganizationId);

                DonutsChartOptionsResult chartResult = new DonutsChartOptionsResult();
                DynamicParameters param = new DynamicParameters();
                param.Add("@OrgIds", String.Join(',', listOrgID));
                param.Add("@Date", filter.DateNow);

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Dashboard.json", "Dashboard", "sp_Dashboard_StatisticsHumanResourceStructure");
                var execResult = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, param, null, CommandType.StoredProcedure, g => g.Read<OccupationObject>());

                if (execResult != null && execResult.First() != null)
                {
                    var listData = execResult[0] as List<OccupationObject>;
                    foreach (var item in listData)
                    {
                        chartResult.Labels.Add(item.OccupationName);
                        chartResult.Series.Add(item.Counter);
                    }
                }

                return new Response<DonutsChartOptionsResult>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, chartResult);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<DonutsChartOptionsResult>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<DonutsChartOptionsResult>> StatisticsQualification(BoxFilter filter)
        {
            try
            {
                List<int> listOrgID = GetListOrgByParentID(filter.OrganizationId);

                DonutsChartOptionsResult chartResult = new DonutsChartOptionsResult();
                DynamicParameters param = new DynamicParameters();
                param.Add("@OrgIds", String.Join(',', listOrgID));
                param.Add("@Date", filter.DateNow);

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Dashboard.json", "Dashboard", "sp_Dashboard_StatisticsQualification");
                var execResult = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, param, null, CommandType.StoredProcedure, g => g.Read<TrinhDoChuyenMonObject>());

                if (execResult != null && execResult.First() != null)
                {
                    var listData = execResult[0] as List<TrinhDoChuyenMonObject>;
                    foreach (var item in listData)
                    {
                        chartResult.ObjectIDs.Add(item.TrinhDoDaoTaoIDs);
                        chartResult.Labels.Add(item.TrinhDoDaoTaoName);
                        chartResult.Series.Add(item.Counter);
                    }
                }

                return new Response<DonutsChartOptionsResult>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, chartResult);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<DonutsChartOptionsResult>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<LineChartOptionsResult>> StatisticsPersonnelChange(BoxFilter filter)
        {
            try
            {
                List<int> listOrgID = GetListOrgByParentID(filter.OrganizationId);

                LineChartOptionsResult chartResult = new LineChartOptionsResult();
                DynamicParameters param = new DynamicParameters();
                param.Add("@OrgIds", String.Join(',', listOrgID));

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Dashboard.json", "Dashboard", "sp_Dashboard_StatisticsPersonnelChange");
                var execResult = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, param, null, CommandType.StoredProcedure, g => g.Read<History>());

                if (execResult != null && execResult.First() != null)
                {
                    var listHistory = execResult[0] as List<History>; //Danh sách nhân viên công tác tại đơn vị 
                    DateTime startDateOfMonth, endDateOfMonth;
                    DateTime startDateOfYear, endDateOfYear;
                    startDateOfYear = new DateTime(filter.Year.Value, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    endDateOfYear = startDateOfYear.AddYears(1).AddMilliseconds(-1);

                    listHistory = listHistory.Where(r => Conditions(r, startDateOfYear, endDateOfYear)).ToList();
                    LineChart obj = new LineChart { Name = "Nhân sự" };
                    //Lấy nhân viên theo từng tháng 
                    int staffCount;
                    for (int i = 0; i < 12; i++)
                    {
                        startDateOfMonth = new DateTime(filter.Year.Value, i + 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        endDateOfMonth = startDateOfMonth.AddMonths(1).AddMilliseconds(-1);
                        staffCount = listHistory.Where(r => Conditions(r, startDateOfMonth, endDateOfMonth)).Select(r => r.StaffID).Distinct().Count();
                        obj.Data[i] = staffCount;
                        chartResult.XAxis.Add($"Tháng {i + 1}");
                    }
                    chartResult.Series.Add(obj);
                }

                return new Response<LineChartOptionsResult>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, chartResult);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<LineChartOptionsResult>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public bool Conditions(History @object, DateTime startOfDate, DateTime endOfDate)
        {
            if (@object.FromDate.HasValue && @object.Todate.HasValue)
            {
                if (startOfDate > @object.Todate.Value || endOfDate < @object.FromDate.Value) return false;
                if (startOfDate <= @object.Todate.Value && @object.Todate.Value <= endOfDate) return true;
                //if (startOfDate <= @object.FromDate.Value && @object.Todate.Value <= endOfDate) return true;
                if (startOfDate <= @object.FromDate.Value && @object.FromDate.Value <= endOfDate) return true;
                if (startOfDate >= @object.FromDate.Value && endOfDate <= @object.Todate.Value) return true;
            }
            else if (@object.FromDate.HasValue && !@object.Todate.HasValue)
            {
                if (endOfDate < @object.FromDate.Value) return false;
                else return true;
            }
            else if (!@object.FromDate.HasValue && @object.Todate.HasValue)
            {
                if (startOfDate <= @object.Todate.Value && @object.Todate.Value <= endOfDate) return true;
                else return false;
            }
            else
                return false;

            return default;
        }

        public List<int> GetListOrgByParentID(int? parentID)
        {
            IDbConnection connection = null;
            List<int> orgIDs = new List<int>();
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                connection = new SqlConnection(connectionString);
                //connection.Open();
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();

                    if (parentID.HasValue)//lấy danh sách đơn vị theo id đơn vị cha
                    {
                        orgIDs = dal.Query<OrganizationModel>("sp_GetListChildOrganizationAndOrganizationId", new { OrganizationId = string.Join(',', parentID) }, null, CommandType.StoredProcedure).Select(r => r.OrganizationId).ToList();
                    }
                    else //Trường hợp parentID null lấy tất cả đơn vị user được phép truy cập
                    {
                        var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
                        orgIDs = dal.Query<OrganizationModel>("proc_recursive_organization", new { @ApplicationId = user?.ApplicationId, @UserId = user?.UserId }, null, CommandType.StoredProcedure).Select(r => r.id).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                connection.Close();
            }

            return orgIDs;
        }

        public async Task<Response<DonutsChartOptionsResult>> StatisticsHealthPeriod(BoxFilter filter)
        {
            try
            {
                DonutsChartOptionsResult chartResult = new DonutsChartOptionsResult();
                var listQLSK = Enumerable.Empty<QuanLySucKhoeModel>();
                var listHistory = Enumerable.Empty<HistoryModel>();
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Dashboard.json", "Dashboard", "sp_QLSK_GetData");
                var execResult = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, null, null, CommandType.StoredProcedure,
                    g => listHistory = g.Read<HistoryModel>(),
                    g => listQLSK = g.Read<QuanLySucKhoeModel>()
                    );

                //xác định xem kỳ khám thuộc quá trình công tác nào
                HistoryModel obj = null;
                foreach (var item in listQLSK)
                {
                    obj = listHistory.Where(r => item.StaffID.Equals(r.StaffID)
                         && (!r.Todate.HasValue || (item.NgayKham.HasValue && item.NgayKham >= r.FromDate && item.NgayKham <= r.Todate))).FirstOrDefault();

                    if (obj is null) continue;
                    item.DeptID = obj.DeptID;
                }

                //Lọc dữ liệu liên quan đến sức khỏe theo đơn vị
                List<int> listOrgID = GetListOrgByParentID(filter.OrganizationId);
                listQLSK = listQLSK.Where(r => r.DeptID.HasValue && listOrgID.Contains(r.DeptID.Value));

                if (filter.HealthPeriodID.HasValue)//Lọc theo kỳ khám
                    listQLSK = listQLSK.Where(r => filter.HealthPeriodID.Equals(r.HealthPeriodID));

                var datas = (from q in listQLSK
                             group q by q.XepLoaiSucKhoe into gr
                             orderby gr.Key ascending
                             select new
                             {
                                 key = gr.Key,
                                 count = gr.Select(r => new { r.StaffID, r.HealthPeriodID }).Distinct().Count()
                             }).ToList();

                List<string> listLoaiSK = new List<string> { "Loại 1", "Loại 2", "Loại 3", "Loại 4", "Loại 5" };
                foreach (var item in datas)
                {
                    if (!item.key.HasValue) continue;
                    chartResult.ObjectIDs.Add(item.key.Value.ToString());
                    chartResult.Labels.Add(listLoaiSK[item.key.Value - 1]);
                    chartResult.Series.Add(item.count);
                }

                return new Response<DonutsChartOptionsResult>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, chartResult);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<DonutsChartOptionsResult>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<List<HealthPeriodBase>>> GetListHealthPeriod()
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Dashboard.json", "Dashboard", "sp_HealthPeriod_GetAll");
                var datas = _dapperUnitOfWork.GetRepository().Query<HealthPeriodBase>(sqlQuery, null, null, CommandType.StoredProcedure);
                return new Response<List<HealthPeriodBase>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, datas.ToList());
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<HealthPeriodBase>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, default);
            }
        }

        public async Task<Response<string>> GetPostionIDByOccupationName(string names)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Dashboard.json", "Dashboard", "sp_Get_PositionID_ByOccupationName");
                var datas = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<string>(sqlQuery, new { @OccupationNames = names }, null, CommandType.StoredProcedure);
                return new Response<string>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, datas);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<string>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }
    }
}
