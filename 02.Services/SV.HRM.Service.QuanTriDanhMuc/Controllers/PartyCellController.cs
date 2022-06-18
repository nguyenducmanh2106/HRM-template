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
    public class PartyCellController : ControllerBase
    {
        private readonly IPartyCellHandler _partyCellHandler;
        private readonly IBaseHandler _baseHandler;
        public PartyCellController(IBaseHandler baseHandler,
             IPartyCellHandler partyCellHandler
            )
        {
            _baseHandler = baseHandler;
            _partyCellHandler = partyCellHandler;
        }
        [HttpPost]
        public async Task<Response<List<PartyCellModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<PartyCellModel>(queryFilter);
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
        public async Task<Response<bool>> Create([FromBody] PartyCellCreateModel entity)
        {
            return await _partyCellHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<PartyCellModel>> FindById(int recordID)
        {
            return await _partyCellHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<PartyCell>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _partyCellHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật danh hiệu khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] PartyCellUpdateModel entity)
        {
            return await _partyCellHandler.Update(id, entity);
        }
    }
}
