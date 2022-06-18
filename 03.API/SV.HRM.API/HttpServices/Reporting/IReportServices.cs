using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IReportServices
    {
        Task<JObject> getData(ReportEntity reportEntity);

        Task<JObject> getExcelFile(ReportFileEntity reportFileEntity);

        Task<Response<object>> getExcelFile(EntityGeneric queryFilter);
        Task<Response<List<Dictionary<string, object>>>> ReportGrid(EntityGeneric StaffQueryFilter);
        Task<Response<List<Dictionary<string, object>>>> ReportGrid_Aggregate_Manpower(EntityGeneric StaffQueryFilter);
        Task<Response<List<Dictionary<string, object>>>> ReportGrid_ListQualityStaff(EntityGeneric StaffQueryFilter);
        Task<Response<object>> getExcelFile_ListQualityStaff(EntityGeneric queryFilter);

        Task<Response<object>> Report_Attendance(object inputFilter);

    }
}
