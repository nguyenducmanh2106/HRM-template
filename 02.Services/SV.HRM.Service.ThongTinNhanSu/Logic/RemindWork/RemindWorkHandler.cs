using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SV.HRM.Caching.Interface;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using static SV.HRM.Core.Utils.Constant;
using static SV.HRM.Models.RemindWorkModel;
using StaffSalary = SV.HRM.Models.RemindWorkModel.StaffSalary;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class RemindWorkHandler : IRemindWorkHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBaseHandler _baseHandler;
        private readonly ICached _cached;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;

        public RemindWorkHandler(IBaseHandler baseHandler, IDapperUnitOfWork dapperUnitOfWork, ICached cached, IHttpContextAccessor httpContextAccessor)
        {
            _baseHandler = baseHandler;
            _cached = cached;
            _httpContextAccessor = httpContextAccessor;
            _dapperUnitOfWork = dapperUnitOfWork;
        }

        public async Task<Response<List<StaffRemindWork>>> ContractExpiration(int day)
        {
            try
            {
                var result = new List<StaffRemindWork>();
                int duration;
                StaffRemindWork contract;
                var listData = GetCurrentContract().Where(r => r.ContractToDate.HasValue);//Lấy Hợp đồng xác định được thời hạn kết thúc
                var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                foreach (var item in listData)
                {
                    duration = item.ContractToDate.HasValue ? item.ContractToDate.Value.Add(-item.ContractToDate.Value.TimeOfDay).Subtract(now).Days : default;
                    if (duration <= day) //Nhắc đến khi có hợp đòng mới đối với hợp đồng quá hạn
                    {
                        contract = new StaffRemindWork
                        {
                            StaffID = item.StaffID,
                            FullName = $"{item.FullName}({item.StaffCode})",
                            ContractTypeName = item.ContractTypeName,
                            ContractToDate = item.ContractToDate,
                            Duration = duration
                        };
                        result.Add(contract);
                    }
                }

                int totalCount = result.Count;
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result, totalCount, totalCount);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<List<StaffRemindWork>>> SalaryIncrease(int day)
        {
            try
            {
                var result = new List<StaffRemindWork>();
                int duration;
                StaffRemindWork staff;
                var listData = GetCurrentSalary().Where(r => r.DenNgay.HasValue); //Lấy quá trình lương xác định được ngày kết thúc
                var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                foreach (var item in listData)
                {
                    duration = item.DenNgay.HasValue ? item.DenNgay.Value.Add(-item.DenNgay.Value.TimeOfDay).Subtract(now).Days : default;
                    if (duration <= day) //nhắc đến khi có quá trình lương mới
                    {
                        staff = new StaffRemindWork
                        {
                            StaffID = item.StaffID,
                            FullName = $"{item.FullName}({item.StaffCode})",
                            ToDate = item.DenNgay,
                            Duration = duration
                        };
                        result.Add(staff);
                    }
                }

                int totalCount = result.Count;
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result, totalCount, totalCount);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<List<StaffRemindWork>>> EndStrengthenFaculty(int day)
        {
            try
            {
                var result = new List<StaffRemindWork>();
                var listData = GetCurrentStrengthenHistory().Where(r => r.Todate.HasValue);//Lấy nv có QT Tăng cường xác định được ToDate
                StaffRemindWork staff = new StaffRemindWork();
                int duration;

                var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                foreach (var history in listData)
                {
                    duration = history.Todate.HasValue ? history.Todate.Value.Subtract(now).Days : default;
                    if (0 <= duration && duration <= day) //Chỉ nhắc trước ngày hết hạn
                    {
                        staff = new StaffRemindWork
                        {
                            StaffID = history.StaffID,
                            FullName = $"{history.FullName}({history.StaffCode})",
                            ToDate = history.Todate,
                            Duration = duration,
                        };
                        result.Add(staff);
                    }
                }
                int totalCount = result.Count;
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result, totalCount, totalCount);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<List<StaffRemindWork>>> AppointNextTime(int day)
        {
            try
            {
                var result = new List<StaffRemindWork>();
                int duration;
                StaffRemindWork staff;
                var listData = GetCurrentHistory();
                foreach (var history in listData)
                {
                    duration = history.FromDate.HasValue ? history.FromDate.Value.Subtract(DateTime.Now).Days : default;
                    if (0 <= duration && duration <= day) //Chỉ nhắc trước ngày hết hạn
                    {
                        staff = new StaffRemindWork
                        {
                            FullName = $"{history.FullName}({history.StaffCode})",
                            FromDate = history.FromDate.HasValue ? history.FromDate.Value.AddYears(5) : default, //Nhiệm kỳ kế tiếp sau 5 năm
                            Duration = duration//Thời hạn đến nhiệm kỳ tiếp
                        };
                        result.Add(staff);
                    }
                }
                int totalCount = result.Count;
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result, totalCount, totalCount);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<List<StaffRemindWork>>> OfficialPartyChange(int day)
        {
            try
            {
                List<StaffRemindWork> result = new List<StaffRemindWork>();
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_OfficialPartyChange");
                _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, new { @Day = day }, null, CommandType.StoredProcedure,
                   g => result = g.Read<StaffRemindWork>().ToList());

                int totalCount = result.Count;
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result, totalCount, totalCount);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<List<StaffRemindWork>>> Discipline(int day)
        {
            try
            {
                List<StaffRemindWork> result = new List<StaffRemindWork>();
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_Discipline");
                _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, new { @Day = day }, null, CommandType.StoredProcedure,
                   g => result = g.Read<StaffRemindWork>().ToList());

                int totalCount = result.Count;
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result, totalCount, totalCount);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<List<StaffRemindWork>>> BirthdayStaff(int day)
        {
            try
            {
                List<StaffRemindWork> result = new List<StaffRemindWork>();
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_BirthdayStaff");
                _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, new { @Day = day }, null, CommandType.StoredProcedure,
                   g => result = g.Read<StaffRemindWork>().ToList());

                int totalCount = result.Count;
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result, totalCount, totalCount);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<StaffRemindWork>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        #region Helper
        public List<StaffHistory> GetCurrentHistory()
        {
            try
            {
                IEnumerable<StaffHistory> histories = Enumerable.Empty<StaffHistory>(); //Danh sách QTCT đã sắp xếp theo Todate(DESC)
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_GetCurrentHistory");
                _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, new { @Status = HistoryStatusEnum.DieuDongTangCuong }, null, CommandType.StoredProcedure,
                   g => histories = g.Read<StaffHistory>());

                var groupStaff = from h in histories
                                 group h by h.StaffID into gr
                                 select gr;

                var datas = new List<StaffHistory>();
                StaffHistory history;
                foreach (var staff in groupStaff)
                {
                    //Lấy quá trình công tác hiện tại
                    history = staff.FirstOrDefault(r => !r.Todate.HasValue) ?? staff.FirstOrDefault();
                    datas.Add(history);
                }

                return datas;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return default;
            }
        }

        public List<StaffHistory> GetCurrentStrengthenHistory()
        {
            try
            {
                IEnumerable<StaffHistory> histories = Enumerable.Empty<StaffHistory>(); //Danh sách QT TĂNG CƯỜNG đã sắp xếp theo Todate(DESC)
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_GetCurrentStrengthenHistory");
                _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, new { @Status = HistoryStatusEnum.DieuDongTangCuong }, null, CommandType.StoredProcedure,
                   g => histories = g.Read<StaffHistory>());

                var groupStaff = from h in histories
                                 group h by h.StaffID into gr
                                 select gr;

                StaffHistory history;
                var datas = new List<StaffHistory>();
                foreach (var staff in groupStaff)
                {
                    //Lấy quá trình Tăng cuòng hiện tại
                    history = staff.FirstOrDefault(r => !r.Todate.HasValue) ?? staff.FirstOrDefault();
                    datas.Add(history);
                }

                return datas;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return default;
            }
        }

        public List<StaffLabourContract> GetCurrentContract()
        {
            try
            {
                IEnumerable<StaffLabourContract> histories = Enumerable.Empty<StaffLabourContract>(); //Danh sách HĐ đã sắp xếp theo ContractToDate(DESC)
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_GetCurrentContract");
                _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, null, null, CommandType.StoredProcedure,
                   g => histories = g.Read<StaffLabourContract>());

                var groupStaff = from h in histories
                                 group h by h.StaffID into gr
                                 select gr;

                StaffLabourContract contract;
                var datas = new List<StaffLabourContract>();
                foreach (var staff in groupStaff)
                {
                    //Lấy Hợp đồng hiện tại
                    contract = staff.FirstOrDefault(r => !r.ContractToDate.HasValue) ?? staff.FirstOrDefault();
                    datas.Add(contract);
                }
                return datas;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return default;
            }
        }

        public List<StaffSalary> GetCurrentSalary()
        {
            try
            {
                IEnumerable<StaffSalary> salaries = Enumerable.Empty<StaffSalary>(); //Danh sách QT Lương đã sắp xếp theo Denngay(DESC)
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_GetCurrentSalary");
                _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, null, null, CommandType.StoredProcedure,
                   g => salaries = g.Read<StaffSalary>());

                var groupStaff = from h in salaries
                                 group h by h.StaffID into gr
                                 select gr;

                StaffSalary salary;
                var datas = new List<StaffSalary>();
                foreach (var staff in groupStaff)
                {
                    //Lấy QT Lương hiện tại
                    salary = staff.FirstOrDefault(r => !r.DenNgay.HasValue) ?? staff.FirstOrDefault();
                    datas.Add(salary);
                }
                return datas;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return default;
            }
        }
        #endregion


        #region Cấu hình nhắc việc
        public async Task<Response<ConfigSystemRemindModel>> GetConfigSystemRemind(int staffID)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_GetConfigSystemRemindByStaffID");
                var data = await _dapperUnitOfWork.GetRepository().QueryAsync<ConfigSystemRemindModel>(sqlQuery, new { @StaffID = staffID }, null, CommandType.StoredProcedure);
                if (data is null)
                    return new Response<ConfigSystemRemindModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);

                return new Response<ConfigSystemRemindModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, data.FirstOrDefault());
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<ConfigSystemRemindModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<bool>> ConfigSystemRemind(ConfigSystemRemindModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_ConfigSystemRemind");
                int rowAffected = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, new { @StaffID = model.StaffID, @Value = model.Value }, null, CommandType.StoredProcedure);
                if (rowAffected > Constant.NODATA)
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);

                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        public async Task<Response<bool>> ConfigUserRemind(ConfigUserRemindModel model)
        {
            try
            {
                var param = new
                {
                    @PersonalReminderID = model.PersonalReminderID,
                    @StaffID = model.StaffID,
                    @Content = model.Content,
                    @DueDate = model.DueDate,
                    @NotifyTime = model.NotifyTime,
                    @Complete = model.Complete
                };

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_ConfigUserRemind");
                int rowAffected = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);
                if (rowAffected > Constant.NODATA)
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);

                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        public async Task<Response<List<ConfigUserRemindModel>>> GetPersonalReminder(int staffID)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_GetPersonalReminderByStaffID");
                var data = await _dapperUnitOfWork.GetRepository().QueryAsync<ConfigUserRemindModel>(sqlQuery, new { @StaffID = staffID }, null, CommandType.StoredProcedure);
                if (data is null)
                    return new Response<List<ConfigUserRemindModel>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);

                return new Response<List<ConfigUserRemindModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, data.ToList());
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<ConfigUserRemindModel>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<ConfigUserRemindModel>> GetPersonalReminderByID(int id)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_GetPersonalReminderByID");
                var data = await _dapperUnitOfWork.GetRepository().QueryAsync<ConfigUserRemindModel>(sqlQuery, new { @PersonalReminderID = id }, null, CommandType.StoredProcedure);
                if (data is null)
                    return new Response<ConfigUserRemindModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);

                return new Response<ConfigUserRemindModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, data.FirstOrDefault());
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<ConfigUserRemindModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<bool>> Deleted(int id)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_PersonalReminder_Del");
                int rowAffect = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, new { @PersonalReminderID = id }, null, CommandType.StoredProcedure);
                if (rowAffect > Constant.NODATA)
                {
                    // Write log
                    _baseHandler.CreateLog(new SystemLogModel
                    {
                        ApplicationId = userModel.ApplicationId,
                        UserId = userModel.UserId,
                        ActionByUser = LogAction.ADD,
                        Date = DateTime.Now,
                        Content = $"Xóa bản ghi nhắc việc cá nhân"
                    });
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<bool>> DeleteMany(List<int> recordIds)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                var res = await _baseHandler.DeleteMany<PersonalReminder>(recordIds);

                if (res != null && res.Status == Constant.SUCCESS && res.Data)
                {
                    // Write log
                    _baseHandler.CreateLog(new SystemLogModel
                    {
                        ApplicationId = userModel.ApplicationId,
                        UserId = userModel.UserId,
                        ActionByUser = LogAction.ADD,
                        Date = DateTime.Now,
                        Content = $"Xóa bản ghi nhắc việc cá nhân"
                    });
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<bool>> UpdateCompleted(int id)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/RemindWork.json", "RemindWork", "sp_RemindWork_PersonalReminder_Completed");
                int rowAffect = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, new { @PersonalReminderID = id }, null, CommandType.StoredProcedure);
                if (rowAffect > Constant.NODATA)
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<List<ConfigUserRemindModel>>> GetFilter(EntityGeneric filter)
        {
            try
            {
                var result = (await _baseHandler.GetFilter<ConfigUserRemindModel>(filter));
                return result;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<ConfigUserRemindModel>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }
        #endregion
    }
}
