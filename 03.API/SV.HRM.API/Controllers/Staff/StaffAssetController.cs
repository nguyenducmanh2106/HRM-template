using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers
{
    /// <summary>
    /// Bảo hộ lao động
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class StaffAssetController : ControllerBase
    {
        private readonly IStaffAssetHttpService _staffAssetHttpService;
        public StaffAssetController(IStaffAssetHttpService staffAssetHttpService)
        {
            _staffAssetHttpService = staffAssetHttpService;
        }

        [HttpGet]
        public async Task<Response<StaffAssetDetailModel>> GetById(int id)
        {
            return await _staffAssetHttpService.GetById(id);
        }

        [HttpGet]
        public async Task<Response<string>> GetStaffFullNameById(int id)
        {
            return await _staffAssetHttpService.GetStaffFullNameById(id);
        }

        [HttpPost]
        public async Task<Response<List<StaffAssetModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            return await _staffAssetHttpService.GetFilter(queryFilter);
        }

        [HttpPost]
        public async Task<Response<int>> CreateOrUpdate([FromBody] StaffAssetCreateRequestModel model)
        {
            return await _staffAssetHttpService.CreateOrUpdate(model);
        }

        [HttpPost]
        public async Task<Response<bool>> DeleteMany([FromBody] List<int> ids)
        {
            return await _staffAssetHttpService.DeleteMany(ids);
        }
    }
}
