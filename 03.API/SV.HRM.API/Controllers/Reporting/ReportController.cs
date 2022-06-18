using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using SV.HRM.Core.Utils;
using System.Net;

namespace SV.HRM.API.Controllers.Staff
{
    [Route("API/Report")]
    [ApiController]
    public partial class ReportController : ControllerBase
    {
        private readonly IReportServices _Ireport;

        public ReportController(IReportServices Ireport)
        {
            _Ireport = Ireport;
        }
        [Route("Report")]
        [HttpPost]
        public async Task<JObject> Report(ReportEntity reportObject)
        {
            return await _Ireport.getData(reportObject);
        }

        [Route("Export")]
        [HttpPost]
        public async Task<JObject> getExcelFile(ReportFileEntity reportObject)
        {
            return await _Ireport.getExcelFile(reportObject);
        }

        [Route("ReportGrid")]
        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> ReportGrid(EntityGeneric StaffQueryFilter)
        {
            return await _Ireport.ReportGrid(StaffQueryFilter);
        }
        
        [Route("ReportGrid_Aggregate_Manpower")]
        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> ReportGrid_Aggregate_Manpower(EntityGeneric StaffQueryFilter)
        {
            return await _Ireport.ReportGrid_Aggregate_Manpower(StaffQueryFilter);
        }

        
        [Route("ExportExcelGrid")]
        [HttpPost]
        public async Task<Response<object>> ExportExcelGrid([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                return await _Ireport.getExcelFile(queryFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// BÁO CÁO SỐ LƯỢNG, CHẤT LƯỢNG ĐỘI NGŨ VIÊN CHỨC BỆNH VIỆN ĐA KHOA ĐỨC GIANG
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [Route("ReportGrid_ListQualityStaff")]
        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> ReportGrid_ListQualityStaff(EntityGeneric StaffQueryFilter)
        {
            return await _Ireport.ReportGrid_ListQualityStaff(StaffQueryFilter);
        }

        /// <summary>
        /// BÁO CÁO SỐ LƯỢNG, CHẤT LƯỢNG ĐỘI NGŨ VIÊN CHỨC BỆNH VIỆN ĐA KHOA ĐỨC GIANG
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [Route("ExportExcelGrid_ListQualityStaff")]
        [HttpPost]
        public async Task<Response<object>> ExportExcelGrid_ListQualityStaff([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                return await _Ireport.getExcelFile_ListQualityStaff(queryFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [Route("Report_Attendance")]
        [HttpPost]
        public async Task<Response<object>> Report_Attendance([FromBody] object inputFilter)
        {
            try
            {
                return await _Ireport.Report_Attendance(inputFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
