using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SV.HRM.Core.Helper;
using System;
using System.Net.Http;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SV.HRM.API.HttpServices
{

    public class ReportService : IReportServices
    {
        private readonly IHttpHelper _httpHelper;

        public ReportService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<JObject> getData(ReportEntity reportEntity)
        {
            JObject json = new JObject() {
                {"store",reportEntity.store },
                {"reportName",reportEntity.reportName },
                {"tuNgay",reportEntity.tuNgay },
                {"denNgay",reportEntity.denNgay },
                {"ExtraText",reportEntity.ExtraText }

            };
            return await _httpHelper.PostAsync<JObject>($"Report/Report", json);
        }
       
        public async Task<JObject> getExcelFile(ReportFileEntity reportFileEntity)
        {
            JObject json = new JObject() {
                {"store",reportFileEntity.store },
                {"reportName",reportFileEntity.reportName },
                {"tuNgay",reportFileEntity.tuNgay },
                {"denNgay",reportFileEntity.denNgay }

            };
            return await _httpHelper.PostAsync<JObject>($"Report/getExcel", json); ;
        }

        public async Task<Response<List<Dictionary<string, object>>>> ReportGrid(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<Dictionary<string, object>>>>($"Report/ReportGrid", StaffQueryFilter);
        }
        
        public async Task<Response<List<Dictionary<string, object>>>> ReportGrid_Aggregate_Manpower(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<Dictionary<string, object>>>>($"Report/ReportGrid_Aggregate_Manpower", StaffQueryFilter);
        }

        public async Task<Response<object>> getExcelFile(EntityGeneric StaffQueryFilter)
        {
            var result =  await _httpHelper.PostAsyncCustomGrid<Response<object>>($"Report/getExcelGrid", StaffQueryFilter); ;
            return result;
        }

        public async Task<Response<object>> getExcelFile_ListQualityStaff(EntityGeneric StaffQueryFilter)
        {
            var result = await _httpHelper.PostAsyncCustomGrid<Response<object>>($"Report/getExcelGrid_ListQualityStaff", StaffQueryFilter); ;
            return result;
        }

        public async Task<Response<List<Dictionary<string, object>>>> ReportGrid_ListQualityStaff(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<Dictionary<string, object>>>>($"Report/ReportGrid_ListQualityStaff", StaffQueryFilter);
        }

        public async Task<Response<object>> Report_Attendance(object inputFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<object>>($"Report/Report_Attendance", inputFilter);
        }
    }
}
