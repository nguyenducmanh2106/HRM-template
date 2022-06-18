using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SV.HRM.Core;
using SV.HRM.Models;
using SV.HRM.Service.Report.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.Report.Logic
{
    public interface IDapperReport
    {
        JObject getReport(ReportEntity reportEntity);

        JObject ExportExcel(ReportFileEntity reportEntity);
        Task<Response<object>> ExportExcel(EntityGeneric StaffQueryFilter);
        Task<Response<object>> ExportExcel_ListQualityStaff(EntityGeneric StaffQueryFilter);

        // báo cáo chấm công
        Task<Response<object>> ExportExcelAttendance(object inputFilter);
    }
}
