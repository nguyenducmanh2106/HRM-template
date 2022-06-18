using System;
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
    public class BankController : ControllerBase
    {
        private readonly IBankHttpService _bankHttpService;

        public BankController(
            IBankHttpService bankHttpService)
        {
            _bankHttpService = bankHttpService;
        }
        /// <summary>
        /// Grid view bank
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<BankModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _bankHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới ngân hàng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(BankCreateModel model)
        {
            return await _bankHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Bank
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<BankModel>> FindById(int recordID)
        {
            return await _bankHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _bankHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _bankHttpService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật ngân hàng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, BankUpdateModel model)
        {
            return await _bankHttpService.Update(id, model);
        }
    }
}