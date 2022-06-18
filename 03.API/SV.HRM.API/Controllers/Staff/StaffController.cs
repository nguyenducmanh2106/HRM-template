using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using NLog;

namespace SV.HRM.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public partial class StaffController : ControllerBase
    {
        private readonly IStaffHttpService _StaffService;
        private readonly IBaseHttpService _baseService;

        public StaffController(IStaffHttpService StaffService, IBaseHttpService baseService)
        {
            _StaffService = StaffService;
            _baseService = baseService;
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _StaffService.GetFilter(StaffQueryFilter);
        }

        [HttpGet]
        public async Task<Response<List<StaffModel>>> GetAll(string q, int page)
        {
            return await _StaffService.GetAll(q, page);
        }

        [HttpGet]
        public async Task<Response<StaffModel>> GetById(int StaffId)
        {
            return await _StaffService.GetById(StaffId);
        }

        [HttpGet]
        public async Task<Response<StaffDetailModel>> GetStaffGeneralInfoById(int id)
        {
            return await _StaffService.GetStaffGeneralInfoById(id);
        }

        [HttpGet]
        public async Task<Response<StaffDetailModel>> GetStaffOrtherInfoById(int id)
        {
            return await _StaffService.GetStaffOrtherInfoById(id);
        }

        [HttpGet]
        public async Task<Response<int>> GetStaffIDByAccountID(int userID)
        {
            return await _StaffService.GetStaffIDByAccountID(userID);
        }

        [HttpPost]
        public async Task<Response<int>> CreateStaffGeneralInfo(StaffCreateRequestModel model)
        {
            return await _StaffService.CreateStaffGeneralInfo(model);
        }

        [HttpPost]
        public async Task<Response<bool>> UpdateStaffGeneralInfo([FromBody] StaffUpdateRequestModel model)
        {
            return await _StaffService.UpdateStaffGeneralInfo(model);
        }

        [HttpPost]
        public async Task<Response<int>> CreateOrUpdateStaffOrtherInfo([FromBody] StaffCreateRequestModel model)
        {
            return await _StaffService.CreateOrUpdateStaffOrtherInfo(model);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _StaffService.Delete(recordID);
        }

        [HttpPost]
        public async Task<Response<bool>> DeleteList(List<int> id)
        {
            return await _StaffService.DeleteList(id);
        }

        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> ReportStaff(EntityGeneric StaffQueryFilter)
        {
            return await _StaffService.ReportStaff(StaffQueryFilter);
        }

        [HttpPost]
        public async Task<Response<int>> GenerateAccount()
        {
            return await _StaffService.GenerateAccount();
        }
    }
}
