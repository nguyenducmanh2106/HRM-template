using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using SV.HRM.Service.Report.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.Report.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        private readonly IDapperReport iDapper;
        private readonly IBaseReportHandler _baseReportHandler;

        public ReportController(IDapperReport _IDapper, IBaseReportHandler baseReportHandler)
        {

            iDapper = _IDapper;
            _baseReportHandler = baseReportHandler;
        }

        [HttpPost]
        public JObject Report(ReportEntity reportObject)
        {
            return iDapper.getReport(reportObject);
        }

        [HttpPost]
        public JObject getExcel(ReportFileEntity reportObject)
        {
            return iDapper.ExportExcel(reportObject);
        }

        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> ReportGrid([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var lstApplication = _baseReportHandler.GetOrganization();
                var responseStaff = await _baseReportHandler.GetFilter<object>(queryFilter);
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
                            dictionary["OrganizationCode"] = organization?.OrganizationCode ?? null;
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
                throw ex;
            }
        }

        [HttpPost]
        public async Task<Response<object>> getExcelGrid([FromBody] EntityGeneric queryFilter)
        {
            var result = await iDapper.ExportExcel(queryFilter);
            return result;
        }

        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> ReportGrid_ListQualityStaff([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var lstApplication = _baseReportHandler.GetOrganization();
                var ethnics = _baseReportHandler.GetEthnics();

                // lấy id của dân tộc kinh
                var ethnicID = ethnics.SingleOrDefault(g => g.EthnicName.ToLower().Contains("kinh"))?.EthnicId;
                queryFilter.CustomPagingData["EthnicID"] = ethnicID;

                var responseStaff = await _baseReportHandler.GetFilter<object>(queryFilter, true);
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
                            dictionary["OrganizationCode"] = organization?.OrganizationCode ?? null;
                        }
                    }

                    if (dictionarys != null)
                    {

                        return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, dictionarys, responseStaff.DataCount, responseStaff.TotalCount, responseStaff.TotalPage, responseStaff.PageNumber, responseStaff.PageSize, null, responseStaff?.SummaryData);
                    }
                    else return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
                }

                return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<Response<object>> getExcelGrid_ListQualityStaff([FromBody] EntityGeneric queryFilter)
        {
            var result = await iDapper.ExportExcel_ListQualityStaff(queryFilter);
            return result;
        }
        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> ReportGrid_Aggregate_Manpower([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var lstApplication = _baseReportHandler.GetOrganization();

                var responseStaff = await _baseReportHandler.GetFilter<object>(queryFilter, true);
                List<object> staffs = new List<object>();
                if (responseStaff != null && responseStaff.Status == Constant.SUCCESS && lstApplication != null)
                {
                    staffs = responseStaff.Data;
                    var json = JsonConvert.SerializeObject(staffs);
                    var dictionarys = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                   
                    foreach (Dictionary<string, object> dictionary in dictionarys)
                    {
                        if (dictionary.ContainsKey("DeptID"))
                        {
                            var organization = lstApplication.SingleOrDefault(g => dictionary["DeptID"] != null && (g.OrganizationId == (Int64)dictionary["DeptID"]));
                            dictionary["OrganizationName"] = organization?.OrganizationName ?? null;
                            dictionary["OrganizationCode"] = organization?.OrganizationCode ?? null;
                        }
                    }

                    if (dictionarys != null)
                    {

                        return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, dictionarys, responseStaff.DataCount, responseStaff.TotalCount, responseStaff.TotalPage, responseStaff.PageNumber, responseStaff.PageSize, null, responseStaff?.SummaryData);
                    }
                    else return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
                }

                return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<Response<object>> Report_Attendance([FromBody] object inputFilter)
        {
            var result = await iDapper.ExportExcelAttendance(inputFilter);
            return result;
        }
    }
}
