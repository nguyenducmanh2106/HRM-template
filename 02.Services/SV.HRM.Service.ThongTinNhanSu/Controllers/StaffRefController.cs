using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StaffRefController : ControllerBase
    {
        public IStaffRefHandler _staffRefHandler;
        public IBaseHandler _baseHandler;

        public StaffRefController(IStaffRefHandler staffRefHandler, IBaseHandler baseHandler)
        {
            _staffRefHandler = staffRefHandler;
            _baseHandler = baseHandler;
        }

        [HttpGet]
        public async Task<Response<StaffRefDetailModel>> GetById(int id)
        {
            return await _staffRefHandler.GetById(id);
        }

        [HttpPost]
        public async Task<Response<List<StaffRefModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            return await _baseHandler.GetFilter<StaffRefModel>(queryFilter);
        }

        [HttpPost]
        public async Task<Response<int>> CreateOrUpdate([FromBody] StaffRefCreateRequestModel model)
        {
            return await _staffRefHandler.CreateOrUpdate(model);
        }

        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> ids)
        {
            return await _staffRefHandler.DeleteMany(ids);
        }
    }
}
