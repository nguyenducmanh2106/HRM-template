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
    public class PartyCellController : ControllerBase
    {
        private readonly IPartyCellHttpService _partyCellHttpService;

        public PartyCellController(IPartyCellHttpService partyCellHttpService)
        {
            _partyCellHttpService = partyCellHttpService;
        }
        /// <summary>
        /// Grid chức vụ chính quyền
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<PartyCellModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _partyCellHttpService.GetFilter(queryFilter);
        }


        /// <summary>
        /// Hàm thêm mới chức vụ chính quyền
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(PartyCellCreateModel model)
        {
            return await _partyCellHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng PartyCell
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<PartyCellModel>> FindById(int recordID)
        {
            return await _partyCellHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _partyCellHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _partyCellHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật PartyCell
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, PartyCellUpdateModel model)
        {
            return await _partyCellHttpService.Update(id, model);
        }
    }
}
