using Dapper;
using Microsoft.Extensions.Logging;
using NLog;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public class WKTimeHandler : IWKTimeHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;
        public WKTimeHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler)
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
        }
        public async Task<Response<bool>> Create(WKTimeRequestCreate model)
        {

            try
            {
                //List<DateTime> dateCreate = null;
                if (model.FromDate != null && model.ToDate != null)
                {
                    var dateBetween = GetDaysBetween(model.FromDate, model.ToDate);
                    //dateCreate = dateBetween.ToList();
                    List<DateTime> ldt = new List<DateTime>();
                    string[] exceptDayOfWeek = null; // biến lưu các ngày ngoại trừ
                    var checkBeforeCreate = true; // biến check ngày hôm đấy đã có nhóm đấy chưa
                    var sql_Get_Date_Holiday = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(WKTime)}.json", nameof(WKTime), "Get_Date_Holiday");
                    if (!String.IsNullOrEmpty(model.ExceptDayOfWeek))
                    {
                        exceptDayOfWeek = model.ExceptDayOfWeek.Split(",");
                    }
                    if (exceptDayOfWeek != null && exceptDayOfWeek.Length > 0)
                    {
                        foreach (var item in exceptDayOfWeek)
                        {
                            switch (item)
                            {
                                case "T2":
                                case "t2":
                                    dateBetween = dateBetween.Where(d => d.DayOfWeek != DayOfWeek.Monday);
                                    break;
                                case "T3":
                                case "t3":
                                    dateBetween = dateBetween.Where(d => d.DayOfWeek != DayOfWeek.Tuesday);
                                    break;
                                case "T4":
                                case "t4":
                                    dateBetween = dateBetween.Where(d => d.DayOfWeek != DayOfWeek.Wednesday);
                                    break;
                                case "T5":
                                case "t5":
                                    dateBetween = dateBetween.Where(d => d.DayOfWeek != DayOfWeek.Thursday);
                                    break;
                                case "T6":
                                case "t6":
                                    dateBetween = dateBetween.Where(d => d.DayOfWeek != DayOfWeek.Friday);
                                    break;
                                case "T7":
                                case "t7":
                                    dateBetween = dateBetween.Where(d => d.DayOfWeek != DayOfWeek.Saturday);
                                    break;
                                case "CN":
                                case "cn":
                                    dateBetween = dateBetween.Where(d => d.DayOfWeek != DayOfWeek.Sunday);
                                    break;
                                case "NL":
                                case "nl":
                                    var holidayDate = await _dapperUnitOfWork.GetRepository().QueryAsync<DateTime>(sql_Get_Date_Holiday,null,null,CommandType.Text);
                                    if (holidayDate != null)
                                    {
                                        dateBetween = dateBetween.Where(d => holidayDate?.Where(x => x.Date == d.Date).Count() <= 0);
                                        var data = dateBetween.ToList();
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                    List<WKTimeCreate> wKTimeCreates = new List<WKTimeCreate>();
                    var sql_Check_Before_Insert = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(WKTime)}.json", nameof(WKTime), "Check_Before_Insert");
                    if (dateBetween != null && dateBetween.Count() > 0)
                    {
                        foreach (var item in dateBetween)
                        {
                            var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<WKTime>(sql_Check_Before_Insert,new { @TACategoryID = model.TACategoryID, @Date = item },null);
                            if (result != null)
                            {
                                checkBeforeCreate = false;
                                break;
                            }
                            else
                            {
                                if (item != null)
                                {
                                    var wkTimeCreate = new WKTimeCreate
                                    {
                                        TACategoryID = model.TACategoryID,
                                        WKDate = item,
                                        WKDay = model.WKDay,
                                        WKHour = model.WKHour,
                                        Note = model.Note
                                    };
                                    wKTimeCreates.Add(wkTimeCreate);
                                }
                            }
                        }
                    }
                    if (checkBeforeCreate)
                    {
                        bool isCreateSuccess = true;
                        if (wKTimeCreates != null && wKTimeCreates.Count() > 0)
                        {
                            foreach (var item in wKTimeCreates)
                            {
                                var sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(WKTime)}.json", nameof(WKTime), "Proc_Create");
                                DynamicParameters param = new DynamicParameters();
                                param.Add("@TACategoryID", item.TACategoryID);
                                param.Add("@WKDate", item.WKDate);
                                param.Add("@WKDay", item.WKDay);
                                param.Add("@WKHour", item.WKHour);
                                param.Add("@Note", item.@Note);
                                int result = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);
                                if (result <= 0)
                                {
                                    isCreateSuccess = false;
                                }
                            }
                        }
                        else
                        {
                            isCreateSuccess = false;
                        }
                        if (isCreateSuccess)
                        {
                            return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                        }
                        else
                        {
                            return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                        }
                    }
                    else
                    {
                        return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                    }
                }
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, ex.Message, false);
            }
        }

        public async Task<Response<bool>> DeleteMany(string textSearch, List<int> recordIDs)
        {
            try
            {
                var checkDelete = true;
                if (recordIDs != null && recordIDs.Count() > 0)
                {
                    string sql_FindByRowNum = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(WKTime)}.json", nameof(WKTime), "FindByRowNum");
                    string sql_Delete = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(WKTime)}.json", nameof(WKTime), "Delete");
                    var result = await _dapperUnitOfWork.GetRepository().QueryAsync<WKTimeModel>(sql_FindByRowNum, new { @TextSearch = textSearch, @recordIDs = recordIDs }, null);
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            var wktm = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sql_Delete, new { @TACategoryID = item.TACategoryID, @WKDate = item.WKDate });
                            if (wktm <= 0)
                            {
                                checkDelete = false;
                            }
                        }
                    }
                    if (checkDelete)
                    {
                        return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                    }
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, ex.Message, false);
            }
        }

        // hàm trả về danh sách các ngày từ 2 date
        private IEnumerable<DateTime> GetDaysBetween(DateTime start, DateTime end)
        {
            if (start.Date > end.Date)
            {
                var tmp = start;
                start = end;
                end = tmp;
            }
            if (start.Date <= end.Date)
            {
                for (DateTime i = start; i <= end; i = i.AddDays(1))
                {
                    yield return i;
                }
            }
        }

    }
}
