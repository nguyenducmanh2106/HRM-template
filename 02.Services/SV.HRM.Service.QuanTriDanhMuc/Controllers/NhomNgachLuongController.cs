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
    public class NhomNgachLuongController : ControllerBase
    {

        private readonly INhomNgachLuongHandler _nhomNgachLuongHandler;
        private readonly IBaseHandler _baseHandler;

        public NhomNgachLuongController(IBaseHandler baseHandler,
             INhomNgachLuongHandler nhomNgachLuongHandler
            )
        {
            _baseHandler = baseHandler;
            _nhomNgachLuongHandler = nhomNgachLuongHandler;
        }

        [HttpPost]
        public async Task<Response<List<NhomNgachLuongModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<NhomNgachLuongModel>(queryFilter);
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
        public async Task<Response<bool>> Create([FromBody] NhomNgachLuongCreateModel entity)
        {
            return await _nhomNgachLuongHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<NhomNgachLuongModel>> FindById(int recordID)
        {
            return await _nhomNgachLuongHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<NhomNgachLuong>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _nhomNgachLuongHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật bệnh viện
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] NhomNgachLuongUpdateModel entity)
        {
            return await _nhomNgachLuongHandler.Update(id, entity);
        }
    }
}
