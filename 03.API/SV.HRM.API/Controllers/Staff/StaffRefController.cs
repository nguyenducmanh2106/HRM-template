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
    /// Tham chiếu nhân viên
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class StaffRefController : ControllerBase
    {
        private IStaffRefHttpService _staffRefService;

        public StaffRefController(IStaffRefHttpService staffRefService)
        {
            _staffRefService = staffRefService;
        }

        /// <summary>
        /// Lọc dữ liệu tham chiếu
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffRefModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            return await _staffRefService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Tạo or sửa tham chiếu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<int>> CreateOrUpdate([FromBody] StaffRefCreateRequestModel model)
        {
            return await _staffRefService.CreateOrUpdate(model);
        }

        /// <summary>
        /// Chi tiết tham chiếu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffRefDetailModel>> GetById(int id)
        {
            return await _staffRefService.GetById(id);
        }

        /// <summary>
        /// Xóa tham chiếu
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> ids)
        {
            return await _staffRefService.DeleteMany(ids);
        }
    }
}
