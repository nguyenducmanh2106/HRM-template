using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers.QuanTriDanhMuc
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DecisionItemController : ControllerBase
    {
        private readonly IDecisionItemHttpService _decisionItemHttpService;

        public DecisionItemController(
            IDecisionItemHttpService decisionItemHttpService)
        {
            _decisionItemHttpService = decisionItemHttpService;
        }

        [HttpPost]
        public async Task<Response<List<DecisionItemModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _decisionItemHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới DecisionItem
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(DecisionItemCreateModel model)
        {
            return await _decisionItemHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng DecisionItem
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DecisionItemModel>> FindById(int recordID)
        {
            return await _decisionItemHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _decisionItemHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _decisionItemHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật DecisionItem
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, DecisionItemUpdateModel model)
        {
            return await _decisionItemHttpService.Update(id, model);
        }
        /// <summary>
        /// check bản ghi có đang được sử dụng không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _decisionItemHttpService.FindIdInUse(recordID);
        }
    }
}
