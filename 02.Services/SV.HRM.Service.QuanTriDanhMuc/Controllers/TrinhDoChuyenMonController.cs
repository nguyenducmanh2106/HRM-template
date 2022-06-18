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
    public class TrinhDoChuyenMonController : ControllerBase
    {
        private readonly ITrinhDoChuyenMonHandler _trinhDoChuyenMonHandler;
        private readonly IBaseHandler _baseHandler;

        public TrinhDoChuyenMonController(IBaseHandler baseHandler,
             ITrinhDoChuyenMonHandler trinhDoChuyenMonHandler
            )
        {
            _baseHandler = baseHandler;
            _trinhDoChuyenMonHandler = trinhDoChuyenMonHandler;
        }
        [HttpPost]
        public async Task<Response<List<TrinhDoChuyenMonModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<TrinhDoChuyenMonModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Hàm tạo quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] TrinhDoChuyenMonCreateModel entity)
        {
            return await _trinhDoChuyenMonHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<TrinhDoChuyenMonModel>> FindById(int recordID)
        {
            return await _trinhDoChuyenMonHandler.FindById(recordID);
        }
        /// <summary>
        /// Tìm bản ghi trong bản có được sử dụng không
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _trinhDoChuyenMonHandler.FindIdInUse(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<TrinhDoChuyenMon>(entity);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _trinhDoChuyenMonHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] TrinhDoChuyenMonUpdateModel entity)
        {
            return await _trinhDoChuyenMonHandler.Update(id, entity);
        }
    }
}
