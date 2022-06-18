using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers.Staff
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DecisionController: ControllerBase
    {
        private readonly IDecisionHttpService _decisionHttpService;

        public DecisionController(IDecisionHttpService decisionHttpService)
        {
            _decisionHttpService = decisionHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<DecisionModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _decisionHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<DecisionModel>>> GetByStaff(EntityGeneric queryFilter)
        {
            return await _decisionHttpService.GetByStaff(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(DecisionCreateModel model)
        {
            return await _decisionHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Decision
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DecisionModel>> FindById(int recordID)
        {
            return await _decisionHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _decisionHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, DecisionUpdateModel model)
        {
            return await _decisionHttpService.Update(id, model);
        }
        /// <summary>
        /// check ngày khám có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<bool>> CheckStaffDecisionInHistory(int staffId, DateTime date)
        {
            return await _decisionHttpService.CheckStaffDecisionInHistory(staffId, date);
        }
    }
}
