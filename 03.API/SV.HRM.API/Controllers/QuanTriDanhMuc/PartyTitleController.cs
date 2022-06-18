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
    public class PartyTitleController : ControllerBase
    {
        private readonly IPartyTitleHttpService _partyTitleHttpService;

        public PartyTitleController(IPartyTitleHttpService partyTitleHttpService)
        {
            _partyTitleHttpService = partyTitleHttpService;
        }
        /// <summary>
        /// Grid chức vụ đảng
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<PartyTitleModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _partyTitleHttpService.GetFilter(queryFilter);
        }


        /// <summary>
        /// Hàm thêm mới chức vụ đảng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(PartyTitleCreateModel model)
        {
            return await _partyTitleHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng PartyTitle
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<PartyTitleModel>> FindById(int recordID)
        {
            return await _partyTitleHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _partyTitleHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _partyTitleHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật PartyTitle
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, PartyTitleUpdateModel model)
        {
            return await _partyTitleHttpService.Update(id, model);
        }
    }
}
