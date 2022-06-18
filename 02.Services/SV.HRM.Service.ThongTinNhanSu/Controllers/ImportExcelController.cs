using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ImportExcelController:ControllerBase
    {
        private readonly IImportExcelHandler _importExcelHandler;

        public ImportExcelController(IImportExcelHandler importExcelHandler)
        {
            _importExcelHandler = importExcelHandler;
        }

        [HttpPost]
        public async Task<Response<string>> ImportStaffData([FromBody]ImportExcelServiceModel model)
        {
            return await _importExcelHandler.ImportStaffData(model);
        }
    }
}
