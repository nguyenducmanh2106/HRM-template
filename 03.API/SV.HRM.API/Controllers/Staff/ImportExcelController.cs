using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers.Staff
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ImportExcelController: ControllerBase
    {
        private readonly IImportExcelHttpService _importExcelHttpService;

        public ImportExcelController(IImportExcelHttpService importExcelHttpService)
        {
            _importExcelHttpService = importExcelHttpService;
        }

        
        [HttpPost]
        public async Task<Response<string>> ImportStaffData([FromForm] ImportExcelRequest formData, string dataType = "")
        {
            return await _importExcelHttpService.ImportStaffData(formData, dataType);
        }
    }
}
