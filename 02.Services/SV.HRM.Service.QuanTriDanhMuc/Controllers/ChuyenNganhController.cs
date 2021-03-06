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
    public class ChuyenNganhController : ControllerBase
    {
        private readonly IChuyenNganhHandler _chuyenNganhHandler;
        private readonly IBaseHandler _baseHandler;

        public ChuyenNganhController(IBaseHandler baseHandler,
             IChuyenNganhHandler chuyenNganhHandler
            )
        {
            _baseHandler = baseHandler;
            _chuyenNganhHandler = chuyenNganhHandler;
        }
        [HttpPost]
        public async Task<Response<List<ChuyenNganhModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<ChuyenNganhModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo chức vụ kiêm nhiệm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] ChuyenNganhCreateModel entity)
        {
            return await _chuyenNganhHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ChuyenNganhModel>> FindById(int recordID)
        {
            return await _chuyenNganhHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<ChuyenNganh>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _chuyenNganhHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật chức vụ kiêm nhiệm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] ChuyenNganhUpdateModel entity)
        {
            return await _chuyenNganhHandler.Update(id, entity);
        }
    }
}
