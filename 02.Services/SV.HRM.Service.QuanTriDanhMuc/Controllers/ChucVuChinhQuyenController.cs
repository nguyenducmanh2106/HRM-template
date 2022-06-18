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
    public class ChucVuChinhQuyenController : ControllerBase
    {
        private readonly IChucVuChinhQuyenHandler _chuVuChinhQuyenHandler;
        private readonly IBaseHandler _baseHandler;

        public ChucVuChinhQuyenController(IBaseHandler baseHandler,
             IChucVuChinhQuyenHandler chuVuChinhQuyenHandler
            )
        {
            _baseHandler = baseHandler;
            _chuVuChinhQuyenHandler = chuVuChinhQuyenHandler;
        }

        [HttpPost]
        public async Task<Response<List<ChucVuChinhQuyenModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<ChucVuChinhQuyenModel>(queryFilter);
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
        public async Task<Response<bool>> Create([FromBody] ChucVuChinhQuyenCreateModel entity)
        {
            return await _chuVuChinhQuyenHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ChucVuChinhQuyenModel>> FindById(int recordID)
        {
            return await _chuVuChinhQuyenHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<ChucVuChinhQuyen>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _chuVuChinhQuyenHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật chức vụ chính quyền
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] ChucVuChinhQuyenUpdateModel entity)
        {
            return await _chuVuChinhQuyenHandler.Update(id, entity);
        }
    }
}
