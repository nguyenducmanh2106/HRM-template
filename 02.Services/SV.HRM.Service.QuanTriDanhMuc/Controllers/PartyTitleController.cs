using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PartyTitleController : ControllerBase
    {
        private readonly IPartyTitleHandler _partyTitleHandler;
        private readonly IBaseHandler _baseHandler;
        public PartyTitleController(IBaseHandler baseHandler,
             IPartyTitleHandler partyTitleHandler
            )
        {
            _baseHandler = baseHandler;
            _partyTitleHandler = partyTitleHandler;
        }
        [HttpPost]
        public async Task<Response<List<PartyTitleModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<PartyTitleModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo danh hiệu khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] PartyTitleCreateModel entity)
        {
            return await _partyTitleHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<PartyTitleModel>> FindById(int recordID)
        {
            return await _partyTitleHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<PartyTitle>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _partyTitleHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật danh hiệu khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] PartyTitleUpdateModel entity)
        {
            return await _partyTitleHandler.Update(id, entity);
        }
    }
}
