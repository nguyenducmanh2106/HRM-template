using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
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
using static SV.HRM.Core.Utils.Constant;

namespace SV.HRM.Service.Attendance
{
    public class AttendanceHandler : IAttendanceHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly ICached _cached;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseHandler _baseHandler;

        public AttendanceHandler(IMapper mapper, ICached cached, IDapperUnitOfWork dapperUnitOfWork,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IBaseHandler baseHandler
            )
        {
            _mapper = mapper;
            _cached = cached;
            _dapperUnitOfWork = dapperUnitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _baseHandler = baseHandler;
        }

        public List<OrganizationModel> GetListOrganization()
        {
            IDbConnection connection = null;
            try
            {
                string connectionString = _configuration.GetValue<string>(Constant.ConnectionString.SYSTEM_CONNECTION_STRING);
                connection = new SqlConnection(connectionString);
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();
                    var ret = dal.Query<OrganizationModel>("SELECT OrganizationID,OrganizationName,ParentOrganizationId FROM Organizations WHERE Status = @status;", new { status = StatusRecord.ACTIVE });
                    return ret.ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                connection.Close();
                return null;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// hàm lấy về danh sách id của cơ cấu tổ chức mà user có quyền truy cập
        /// </summary>
        /// <param name="organizationId">Id của cơ cấu tổ chức</param>
        /// <returns></returns>
        public IEnumerable<OrganizationModel> GetPermissionAccessOrganizationId()
        {
            var userInfo = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
            IDbConnection connection = null;
            try
            {
                string connectionString = _configuration.GetValue<string>(Constant.ConnectionString.SYSTEM_CONNECTION_STRING);
                connection = new SqlConnection(connectionString);
                //connection.Open();
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();
                    var ret = dal.Query<OrganizationModel>("proc_recursive_organization", new { @ApplicationId = userInfo.ApplicationId, @UserId = userInfo.UserId }, null, CommandType.StoredProcedure);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                connection.Close();
                return null;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// hàm lấy về danh sách id của cơ cấu tổ chức con và chính nó
        /// </summary>
        /// <param name="organizationId">Id của cơ cấu tổ chức</param>
        /// <returns></returns>
        public IEnumerable<OrganizationModel> GetListChildOrganizationAndOrganizationId(string organizationIds)
        {
            IDbConnection connection = null;
            try
            {
                string connectionString = _configuration.GetValue<string>(Constant.ConnectionString.SYSTEM_CONNECTION_STRING);
                connection = new SqlConnection(connectionString);
                //connection.Open();
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();
                    var ret = dal.Query<OrganizationModel>("sp_GetListChildOrganizationAndOrganizationId", new { OrganizationId = organizationIds }, null, CommandType.StoredProcedure);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                connection.Close();
                return null;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// đếm số nhân viên theo chỉ tiêu
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<Response<List<object>>> CountStaffAttendance(string deptID, DateTime? date)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Attendance)}.json", nameof(Attendance), "Count_Staff_Attendance");
                List<string> valueDeptID = new List<string>();
                if (deptID == null || deptID.Trim().Length == 0)
                {
                    var orgs = GetPermissionAccessOrganizationId();
                    foreach (var item in orgs)
                    {
                        valueDeptID.Add(item.id.ToString());
                    }
                }
                else
                {
                    valueDeptID.Add(deptID.ToString());
                }

                var organization = GetListChildOrganizationAndOrganizationId(String.Join(',', valueDeptID)).Select(g => g.OrganizationId);

                deptID = string.Join(',', organization);
                var dyParameters = new DynamicParameters();
                dyParameters.Add("@DeptID",deptID);
                dyParameters.Add("@Date", date);
                var result = await _dapperUnitOfWork.GetRepository().QueryAsync<object>(sqlQuery, dyParameters);
                if(result != null)
                {
                    return new Response<List<object>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result.ToList());
                }
                return new Response<List<object>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<object>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }
        /// <summary>
        /// lấy về combobox staff
        /// </summary>
        /// <param name="deptID"> phòng ban</param>
        /// <param name="searchText">tìm kiếm</param>
        /// <param name="date">ngày tạo chấm công</param>
        /// <param name="actions">chỉ ra là store Update hay store Create</param>
        /// <returns></returns>
        public async Task<Response<List<StaffComboboxModel>>> GetComboboxStaff(string deptID, string searchText, DateTime? date, string actions)
        {
            try
            {
                List<StaffComboboxModel> lstStaffModel = new List<StaffComboboxModel>();
                int totalCount = 0;
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Attendance)}.json", nameof(Attendance), "Get_Combobox_Staff");
                string queryFormat = String.Format(sqlQuery, actions);
                List<string> valueDeptID = new List<string>();
                if (deptID == null || deptID.Trim().Length == 0)
                {
                    var orgs = GetPermissionAccessOrganizationId();
                    foreach (var item in orgs)
                    {
                        valueDeptID.Add(item.id.ToString());
                    }
                }
                else
                {
                    valueDeptID.Add(deptID.ToString());
                }
                var organization = GetListChildOrganizationAndOrganizationId(String.Join(',', valueDeptID)).Select(g => g.OrganizationId);

                deptID = string.Join(',', organization);
                var dyParameters = new DynamicParameters();
                dyParameters.Add("@searchText", searchText);
                dyParameters.Add("@DeptID", deptID);
                dyParameters.Add("@Date", date);
                var objectResult = _dapperUnitOfWork.GetRepository().QueryMultiple(queryFormat, dyParameters,null,CommandType.StoredProcedure,
                    gr =>
                        gr.Read<StaffComboboxModel>(),
                    gr =>
                        gr.Read<Int32>().FirstOrDefault()
                    );
                if (objectResult != null && objectResult[0] != null && objectResult[1] != null)
                {
                    lstStaffModel = (List<StaffComboboxModel>)objectResult[0];
                    totalCount = (int)objectResult[1];
                    int totalPage = (int)Math.Ceiling((decimal)totalCount / 10);
                    return new Response<List<StaffComboboxModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, lstStaffModel, 10, totalCount, totalPage);
                }
                return new Response<List<StaffComboboxModel>>(Constant.ErrorCode.ERRORSYSTEM_CODE, Constant.ErrorCode.ERRORSYSTEM_MESS);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<StaffComboboxModel>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }
        /// <summary>
        /// thêm mới chấm công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(object model)
        {
            try
            {
                var data = TransformToAttendanceCreate(model);
                bool isCreateSuccess = true;
                foreach (var item in data)
                {
                    var paramters = ParametersAttendanceCreate(item);
                    string sql = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Attendance)}.json", nameof(Attendance), "Proc_Create");
                    if (paramters!= null)
                    {
                        var result = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sql,paramters,null,CommandType.StoredProcedure);
                        if(result <= 0)
                        {
                            isCreateSuccess = false;
                        }
                    }

                }
                if (isCreateSuccess)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }else
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        public async Task<Response<bool>> Update(int id,AttendanceUpdateModel model)
        {
            try
            {
                var userInfo = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
                var shiftLeave = model.ShiftLeave != null ? model.ShiftLeave : "";
                var shiftID = 0;
                var leaveID = 0;
                var lichSuXuLy = userInfo.FullName + " cập nhật thông tin lần cuối lúc " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                if (shiftLeave.Trim().Length > 0)
                {
                    var arrShiftLeave = shiftLeave.Split("_");
                    if (int.Parse(arrShiftLeave[0]) == 1)
                    {
                        shiftID = int.Parse(arrShiftLeave[1]);
                    }
                    if (int.Parse(arrShiftLeave[0]) == 2)
                    {
                        leaveID = int.Parse(arrShiftLeave[1]);
                    }
                }
                string sql = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Attendance)}.json", nameof(Attendance), "Proc_Update_Column");
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AttendanceID",id);
                parameters.Add("@ShiftID", shiftID);
                parameters.Add("@LeaveID", leaveID);
                parameters.Add("@LichSuXuLy", lichSuXuLy);
                parameters.Add("@GiaiTrinh", model.GiaiTrinh);
                parameters.Add("@ShiftLeave", model.ShiftLeave);
                var result = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sql, parameters, null, CommandType.StoredProcedure);
                if (result > 0)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }
        /// <summary>
        /// đổi object về AttendanceCreateModel dùng cho store create
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<AttendanceCreateModel> TransformToAttendanceCreate(object model)
        {
            // type = 1 create, type 2 update
            var userInfo = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
            var listAttendance = new List<AttendanceCreateModel>();
            if (model != null)
            {
                var dictionaries = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.ToString());
                if (dictionaries != null)
                {
                    var deptID = dictionaries["DeptID"] != null ? int.Parse(dictionaries["DeptID"].ToString()) : 0;
                    var shiftLeave = dictionaries["ShiftLeave"].ToString().Split("_");
                    var shiftID = 0;
                    var leaveID = 0;
                    var attendanceDate = dictionaries["AttendanceDate"] != null ? dictionaries["AttendanceDate"].ToString() : "";
                    var giaiTrinh = dictionaries["GiaiTrinh"] != null ? dictionaries["GiaiTrinh"].ToString() : "";
                    var lichSuXuLy = userInfo.FullName + " cập nhật thông tin lần cuối lúc " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    if (int.Parse(shiftLeave[0]) == 1)
                    {
                        shiftID = int.Parse(shiftLeave[1]);
                    }
                    if (int.Parse(shiftLeave[0]) == 2)
                    {
                        leaveID = int.Parse(shiftLeave[1]);
                    }
                    var listStaffID = JsonConvert.DeserializeObject<List<int>>(dictionaries["StaffID"].ToString());
                    if (listStaffID.Count() > 0)
                    {
                        foreach (var item in listStaffID)
                        {
                            var attendance = new AttendanceCreateModel
                            {
                                StaffID = item,
                                AttendanceDate = DateTime.Parse(attendanceDate),
                                ShiftID = shiftID,
                                LeaveID = leaveID,
                                ShiftLeave = dictionaries["ShiftLeave"].ToString(),
                                GiaiTrinh = giaiTrinh,
                                LichSuXuLy = lichSuXuLy,
                                IOError = false,
                                Lock = false,
                                Post = false,
                                DeptID = deptID,
                            };
                            listAttendance.Add(attendance);
                        }
                    }
                }
            }
            return listAttendance;
        }
        /// <summary>
        /// đổi object về AttendanceUpdateModel dùng cho store update
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<AttendanceUpdateModel> TransformToAttendanceUpdate(object model)
        {
            // type = 1 create, type 2 update
            var userInfo = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
            var listAttendance = new List<AttendanceUpdateModel>();
            if (model != null)
            {
                var dictionaries = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.ToString());
                if (dictionaries != null)
                {
                    var shiftLeave = dictionaries["ShiftLeave"].ToString().Split("_");
                    var shiftID = 0;
                    var leaveID = 0;
                    var attendanceDate = dictionaries["AttendanceDate"] != null ? dictionaries["AttendanceDate"].ToString() : "";
                    var giaiTrinh = dictionaries["GiaiTrinh"] != null ? dictionaries["GiaiTrinh"].ToString() : "";
                    var lichSuXuLy = userInfo.FullName + " cập nhật thông tin lần cuối lúc " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    if (int.Parse(shiftLeave[0]) == 1)
                    {
                        shiftID = int.Parse(shiftLeave[1]);
                    }
                    if (int.Parse(shiftLeave[0]) == 2)
                    {
                        leaveID = int.Parse(shiftLeave[1]);
                    }
                    var listStaffID = JsonConvert.DeserializeObject<List<int>>(dictionaries["StaffID"].ToString());
                    if (listStaffID.Count() > 0)
                    {
                        foreach (var item in listStaffID)
                        {
                            var attendance = new AttendanceUpdateModel
                            {
                                StaffID = item,
                                AttendanceDate = DateTime.Parse(attendanceDate),
                                ShiftID = shiftID,
                                LeaveID = leaveID,
                                ShiftLeave = dictionaries["ShiftLeave"].ToString(),
                                GiaiTrinh = giaiTrinh,
                                LichSuXuLy = lichSuXuLy,
                                IOError = false,
                                Lock = false,
                                Post = false
                            };
                            listAttendance.Add(attendance);
                        }
                    }
                }
            }
            return listAttendance;
        }
        /// <summary>
        /// dynamic params lấy theo model
        /// thay đổi trường trong model phải sửa store và đúng thứ tự các param
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private DynamicParameters ParametersAttendanceCreate(AttendanceCreateModel model)
        {
            DynamicParameters parameters = new DynamicParameters();
            var properties = model.GetType().GetProperties();
            foreach (var item in properties)
            {
                if (!item.Name.Equals("AttendanceID"))
                {
                    parameters.Add($"@{item.Name}", item.GetValue(model, null));
                }
            }
            return parameters;
        }
        /// <summary>
        /// dynamic params lấy theo model
        /// thay đổi trường trong model phải sửa store và đúng thứ tự các param
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private DynamicParameters ParametersAttendanceUpdate(AttendanceUpdateModel model)
        {
            DynamicParameters parameters = new DynamicParameters();
            var properties = model.GetType().GetProperties();
            foreach (var item in properties)
            {
                if (!item.Name.Equals("CreatedBy") && !item.Name.Equals("CreationDate"))
                {
                    parameters.Add($"@{item.Name}", item.GetValue(model, null));
                }
            }
            return parameters;
        }
        /// <summary>
        /// chuyển thành sql với ít column
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string TransformToSQLUpdate(object model)
        {
            var userInfo = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
            string sql = "";
            var dictionaries = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.ToString());
            var shiftLeave = dictionaries["ShiftLeave"] != null ? dictionaries["ShiftLeave"].ToString() : "";
            var shiftID = 0;
            var leaveID = 0;
            //var attendanceDate = dictionaries["AttendanceDate"] != null ? dictionaries["AttendanceDate"].ToString() : "";
            var giaiTrinh = dictionaries["GiaiTrinh"] != null ? dictionaries["GiaiTrinh"].ToString() : "";
            var lichSuXuLy = userInfo.FullName + " cập nhật thông tin lần cuối lúc " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            if (shiftLeave.Trim().Length > 0)
            {
                var arrShiftLeave = shiftLeave.Split("_");
                if (int.Parse(arrShiftLeave[0]) == 1)
                {
                    shiftID = int.Parse(arrShiftLeave[1]);
                }
                if (int.Parse(arrShiftLeave[0]) == 2)
                {
                    leaveID = int.Parse(arrShiftLeave[1]);
                }
            }
            
            if (dictionaries.Count() > 0)
            {
                sql += "UPDATE Attendance SET ";
            }
            //if (attendanceDate.Trim().Length > 0)
            //{
            //    sql += " AttendanceDate = " +"'"+ DateTime.Parse(attendanceDate).ToString("yyyy-MM-dd") +"'";
            //}
            if (shiftLeave.Trim().Length > 0)
            {
                sql += " ShiftLeave = " + "'"+ shiftLeave+ "'";
            }
            if (shiftID > 0)
            {
                sql += " ShiftID = " + shiftID;
            }
            if (leaveID > 0)
            {
                sql += " LeaveID = " + leaveID;
            }
            if (giaiTrinh.Trim().Length > 0)
            {
                sql += " ShiftLeave = " + "'" + shiftLeave + "'";
            }
            if(sql.Trim().Length > 0)
            {
                sql += " LichSuXuLy = " + "'" + lichSuXuLy + "'";
                sql += " where AttendanceID IN @AttendanceID";
            }
            return sql;
        }
        /// <summary>
        /// trả về list id chấm công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<int> TransformToListID(object model)
        {
            List<int> listAttendanceID = new List<int>();
            var dictionaries = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.ToString());
            //listAttendanceID = JsonConvert.DeserializeObject<List<int>>(dictionaries["AttendanceID"].ToString());
            var lstID = dictionaries["AttendanceID"] != null ? dictionaries["AttendanceID"].ToString() : "";
            if (lstID.Trim().Length > 0)
            {
                listAttendanceID = JsonConvert.DeserializeObject<List<int>>(lstID);
            }
            return listAttendanceID;
        }
        /// <summary>
        /// tìm kiếm theo id
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<AttendanceModel>> FindById(int recordID)
        {
            AttendanceModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Attendance)}.json", nameof(Attendance), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<AttendanceModel>(sqlQuery, new { @AttendanceID = recordID });
                if (result != null)
                {
                    return new Response<AttendanceModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<AttendanceModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<AttendanceModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }
        /// <summary>
        /// duyệt chấm công
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> ConfirmAttendance(List<int> recordID)
        {
            try
            {
                var userInfo = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
                var lichSuXuLy = userInfo.FullName + " cập nhật thông tin lần cuối lúc " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Attendance)}.json", $"{nameof(Attendance)}", "Confirm_Attendance");
                int result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, new { @LichSuXuLy = lichSuXuLy, @recordIDs = recordID });
                if (result > 0)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }
        /// <summary>
        /// lấy id phòng ban theo userID 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<Response<int>> GetDeptIDByUserID(int userID)
        {
            using (var trans = _dapperUnitOfWork.GetRepository().GetDbConnection().BeginTransaction())
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Attendance.json", "Attendance", "GetDeptIDByUserID");
                sqlQuery = string.Format(sqlQuery, userID);
                int deptID = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, null, trans, CommandType.Text);
                return new Response<int>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, deptID);
            }
        }

        public async Task<Response<List<object>>> GetNoteLabour()
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Attendance)}.json", nameof(Attendance), "Get_Note_Labour");
                var result = await _dapperUnitOfWork.GetRepository().QueryAsync<object>(sqlQuery, null);
                if (result != null)
                {
                    return new Response<List<object>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result.ToList());
                }
                return new Response<List<object>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<object>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<int>> CheckPostAttendance(int recordID)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Attendance)}.json", nameof(Attendance), "Check_Post_Attendance");
                var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<int>(sqlQuery, new { @AttendanceID = recordID });
                return new Response<int>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<int>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, 0);
            }
        }
    }
}
