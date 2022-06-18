using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public partial class HistoryController : ControllerBase
    {
        private readonly IHistoryHttpService _historyService;

        public HistoryController(IHistoryHttpService historyService)
        {
            _historyService = historyService;
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<HistoryModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _historyService.GetFilter(StaffQueryFilter);
        }

        /// <summary>
        /// Hàm tạo quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(HistoryCreateRequestModel model)
        {
            return await _historyService.Create(model);
        }

        /// <summary>
        /// Hàm tạo quá trình công tác trước khi vào công ty
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> CreateBeforeJoiningCompany(HistoryCreateBeforeJoiningCompanyRequestModel model)
        {
            return await _historyService.CreateBeforeJoiningCompany(model);
        }


        /// <summary>
        /// Hàm lấy về giá trị hết hạn của quá trình công tác gần nhất
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DateTime>> GetMinFromDate(int staffID)
        {
            return await _historyService.GetMinFromDate(staffID);
        }


        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _historyService.Delete(recordID);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng History
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<HistoryModel>> FindById(int recordID)
        {
            return await _historyService.FindById(recordID);
        }

        /// <summary>
        /// Hàm cập nhật quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id,HistoryUpdateRequestModel model)
        {
            return await _historyService.Update(id,model);
        }

        /// <summary>
        /// Cập nhật quá trình công tác trước khi vào công ty
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> UpdateBeforeJoiningCompany(int id, HistoryUpdateBeforeJoiningCompanyRequestModel model)
        {
            return await _historyService.UpdateBeforeJoiningCompany(id, model);
        }

        /// <summary>
        /// Lấy quá trình công tác mới nhất có trạng thái khác tăng cường
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<HistoryModel>> GetHistoryLatest(int recordID)
        {
            return await _historyService.GetHistoryLatest(recordID);
        }

    }
}
