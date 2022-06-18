using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using static SV.HRM.Service.ThongTinNhanSu.CustomAuthorize;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    /// <summary>
    /// Bảo hộ lao động
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class StaffAssetController : ControllerBase
    {
        public IStaffAssetHandler _staffAssetHandler;
        public IBaseHandler _baseHandler;

        public StaffAssetController(IStaffAssetHandler staffAssetHandler, IBaseHandler baseHandler)
        {
            _staffAssetHandler = staffAssetHandler;
            _baseHandler = baseHandler;
        }

        [HttpGet]
        public async Task<Response<StaffAssetDetailModel>> GetById(int id)
        {
            return await _staffAssetHandler.GetById(id);
        }

        [HttpGet]
        public async Task<Response<string>> GetStaffFullNameById(int id)
        {
            return await _staffAssetHandler.GetStaffFullNameById(id);
        }

        //[CustomAuthorize(Role.TBBHLD_MANAGER, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<StaffAssetModel>>> GetFilter([FromBody] EntityGeneric filter)
        {
            return await _staffAssetHandler.GetFilter(filter);
        }

        //[CustomAuthorize(Role.TBBHLD_MANAGER, Right.CREATE)]
        [HttpPost]
        public async Task<Response<int>> CreateOrUpdate([FromBody] StaffAssetCreateRequestModel model)
        {
            return await _staffAssetHandler.CreateOrUpdate(model);
        }

        //[CustomAuthorize(Role.TBBHLD_MANAGER, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> ids)
        {
            return await _staffAssetHandler.DeleteMany(ids);
        }
    }
}
