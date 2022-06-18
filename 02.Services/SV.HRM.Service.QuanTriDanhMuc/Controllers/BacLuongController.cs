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
    public class BacLuongController : ControllerBase
    {
        private readonly IBacLuongHandler _bacLuongHandler;
        private readonly IBaseHandler _baseHandler;

        public BacLuongController(IBaseHandler baseHandler,
             IBacLuongHandler bacLuongHandler
            )
        {
            _baseHandler = baseHandler;
            _bacLuongHandler = bacLuongHandler;
        }
        [HttpPost]
        public async Task<Response<List<BacLuongModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<BacLuongModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo bậc lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] BacLuongCreateModel entity)
        {
            return await _bacLuongHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<BacLuongModel>> FindById(int recordID)
        {
            return await _bacLuongHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<BacLuong>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _bacLuongHandler.CheckRecordInUse(entity);
        }
        /// <summary>
        /// Hàm cập nhật bậc lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] BacLuongUpdateModel entity)
        {
            return await _bacLuongHandler.Update(id, entity);
        }
    }
}
