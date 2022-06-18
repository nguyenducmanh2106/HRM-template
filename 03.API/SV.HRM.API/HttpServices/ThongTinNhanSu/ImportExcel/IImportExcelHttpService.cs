using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IImportExcelHttpService
    {
        Task<Response<string>> ImportStaffData(ImportExcelRequest formData, string dataType = "");
    }
}
