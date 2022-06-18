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
    [ApiController]
    [Route("[controller]/[action]")]
    public class RewardController : ControllerBase
    {
        private readonly IRewardHttpService _rewardHttpService;

        public RewardController(IRewardHttpService rewardHttpService)
        {
            _rewardHttpService = rewardHttpService;
        }

        /// <summary>
        /// Grid view Reward
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<RewardModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _rewardHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới danh hiệu, khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(RewardCreateModel model)
        {
            return await _rewardHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Reward
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<RewardModel>> FindById(int recordID)
        {
            return await _rewardHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _rewardHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _rewardHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật danh hiệu khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, RewardUpdateModel model)
        {
            return await _rewardHttpService.Update(id, model);
        }
    }
}
