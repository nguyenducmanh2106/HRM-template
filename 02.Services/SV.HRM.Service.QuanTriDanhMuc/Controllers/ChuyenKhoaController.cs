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
    public class ChuyenKhoaController : ControllerBase
    {
        private readonly IChuyenKhoaHandler _chuyenKhoaHandler;
        private readonly IBaseHandler _baseHandler;

        public ChuyenKhoaController(IBaseHandler baseHandler,
             IChuyenKhoaHandler chuyenKhoaHandler
            )
        {
            _baseHandler = baseHandler;
            _chuyenKhoaHandler = chuyenKhoaHandler;
        }
        [HttpPost]
        public async Task<Response<List<ChuyenKhoaModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<ChuyenKhoaModel>(queryFilter);
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
        public async Task<Response<bool>> Create([FromBody] ChuyenKhoaCreateModel entity)
        {
            return await _chuyenKhoaHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ChuyenKhoaModel>> FindById(int recordID)
        {
            return await _chuyenKhoaHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<ChuyenKhoa>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _chuyenKhoaHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật chức vụ kiêm nhiệm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] ChuyenKhoaUpdateModel entity)
        {
            return await _chuyenKhoaHandler.Update(id, entity);
        }
    }
}
