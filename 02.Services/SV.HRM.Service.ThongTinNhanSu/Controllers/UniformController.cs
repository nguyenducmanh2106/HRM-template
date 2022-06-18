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
    /// Đồng phục nhân viên
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class UniformController : ControllerBase
    {
        public IUniformHandler _uniformHandler;
        public IBaseHandler _baseHandler;

        public UniformController(IUniformHandler uniformHandler, IBaseHandler baseHandler)
        {
            _uniformHandler = uniformHandler;
            _baseHandler = baseHandler;
        }

        [HttpGet]
        public async Task<Response<UniformDetailModel>> GetById(int id)
        {
            return await _uniformHandler.GetById(id);
        }

        [HttpGet]
        public async Task<Response<string>> GetStaffFullNameById(int id)
        {
            return await _uniformHandler.GetStaffFullNameById(id);
        }

        //[CustomAuthorize(Role.DPNV_MANAGER, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<UniformModel>>> GetFilter([FromBody] EntityGeneric filter)
        {
            return await _uniformHandler.GetFilter(filter);
        }

        //[CustomAuthorize(Role.DPNV_MANAGER, Right.CREATE)]
        [HttpPost]
        public async Task<Response<int>> CreateOrUpdate([FromBody] UniformCreateRequestModel model)
        {
            return await _uniformHandler.CreateOrUpdate(model);
        }

        //[CustomAuthorize(Role.DPNV_MANAGER, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> ids)
        {
            return await _uniformHandler.DeleteMany(ids);
        }
    }
}
