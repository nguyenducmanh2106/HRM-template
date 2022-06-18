using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;

namespace SV.HRM.Service.Training
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingHandler _trainingHandler;
        private readonly IBaseHandler _baseHandler;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TrainingController(ITrainingHandler trainingHandler, IBaseHandler baseHandler)
        {
            _trainingHandler = trainingHandler;
            _baseHandler = baseHandler;
        }
        [HttpPost]
        public async Task<Response<bool>> CreateObject(string layout, [FromBody] object createObject)
        {
            var result = await _baseHandler.CreateObject(layout, createObject);
            if (result == null && result.Data == null)
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, String.Format(Constant.ErrorCode.CUSTOM_ERROR_MESSAGE, "Thời gian bắt đầu đi học đang không thuộc quá trình công tác nào"), false);
            }
            IDictionary<string, object> dic = (IDictionary<string, object>)result.Data;
            if (result != null && dic.ContainsKey("ErrorState") && Convert.ToInt32(dic["ErrorState"]) == 1)
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, String.Format(Constant.ErrorCode.CUSTOM_ERROR_MESSAGE, "Thời gian bắt đầu đi học đang không thuộc quá trình công tác nào"), false);
            }

            return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
        }

        [HttpPost]
        public async Task<Response<bool>> UpdateObject(string layout, int id, [FromBody] object createObject)
        {
            var result = await _baseHandler.UpdateObject(layout, id, createObject);
            if (result == null && result.Data == null)
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, String.Format(Constant.ErrorCode.CUSTOM_ERROR_MESSAGE, "Thời gian bắt đầu đi học đang không thuộc quá trình công tác nào"), false);
            }
            IDictionary<string, object> dic = (IDictionary<string, object>)result.Data;
            if (result != null && dic.ContainsKey("ErrorState") && Convert.ToInt32(dic["ErrorState"]) == 1)
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, String.Format(Constant.ErrorCode.CUSTOM_ERROR_MESSAGE, "Thời gian bắt đầu đi học đang không thuộc quá trình công tác nào"), false);
            }

            return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
        }

        /// <summary>
        /// Tìm bản ghi
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<QuanLyDaoTaoModel>> FindById(int recordID)
        {
            return await _trainingHandler.FindById(recordID);
        }

        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> ReportTraining(EntityGeneric queryFilter)
        {
            try
            {
                var lstApplication = _trainingHandler.GetListOrganization();
                var ethnics = _baseHandler.GetEthnics();
                var territorys = _baseHandler.GetCountries();
                var cities = _baseHandler.GetLocations();
                var responseStaff = await _baseHandler.GetFilter<object>(queryFilter);
                List<object> staffs = new List<object>();
                if (responseStaff != null && responseStaff.Status == Constant.SUCCESS && lstApplication != null)
                {
                    staffs = responseStaff.Data;
                    var json = JsonConvert.SerializeObject(staffs);
                    var dictionarys = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                    //return new Response<List<object>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, staffs, responseStaff.DataCount, responseStaff.TotalCount, responseStaff.TotalPage, responseStaff.PageNumber, responseStaff.PageSize);
                    foreach (Dictionary<string, object> dictionary in dictionarys)
                    {
                        if (dictionary.ContainsKey("DeptID"))
                        {
                            var organization = lstApplication.SingleOrDefault(g => dictionary["DeptID"] != null && (g.OrganizationId == (Int64)dictionary["DeptID"]));
                            dictionary["OrganizationName"] = organization?.OrganizationName ?? null;

                            var organizationParentName = lstApplication.SingleOrDefault(g => organization != null && organization.ParentOrganizationId != null && (g.OrganizationId == organization.ParentOrganizationId))?.OrganizationName ?? null;
                            dictionary["Khoi"] = organizationParentName;
                        }
                        if (dictionary.ContainsKey("ChungChiQuanLyBenhVien"))
                        {
                            dictionary["ChungChiQuanLyBenhVien"] = dictionary["ChungChiQuanLyBenhVien"] != null ? true : false;
                        }
                        if (dictionary.ContainsKey("ChungChiQuanLyDieuDuong"))
                        {
                            dictionary["ChungChiQuanLyDieuDuong"] = dictionary["ChungChiQuanLyDieuDuong"] != null ? true : false;
                        }
                    }
                    if (dictionarys != null)
                    {
                        return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, dictionarys, responseStaff.DataCount, responseStaff.TotalCount, responseStaff.TotalPage, responseStaff.PageNumber, responseStaff.PageSize);
                    }
                    else return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
                }

                return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                throw ex;
            }
        }

        /// <summary>
        /// Hàm xoá
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(new string[] { Role.HSNV_QTCT_MANAGER }, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<QuanLyDaoTao>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.QUAN_LY_DAO_TAO, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

    }
}
