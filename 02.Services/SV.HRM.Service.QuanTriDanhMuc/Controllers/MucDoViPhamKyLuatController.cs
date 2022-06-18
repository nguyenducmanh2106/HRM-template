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
    public class MucDoViPhamKyLuatController : ControllerBase
    {
        private readonly IMucDoViPhamKyLuatHandler _mucDoViPhamKyLuatHandler;
        private readonly IBaseHandler _baseHandler;

        public MucDoViPhamKyLuatController(IBaseHandler baseHandler,
             IMucDoViPhamKyLuatHandler mucDoViPhamKyLuatHandler
            )
        {
            _baseHandler = baseHandler;
            _mucDoViPhamKyLuatHandler = mucDoViPhamKyLuatHandler;
        }
        [HttpPost]
        public async Task<Response<List<MucDoViPhamKyLuatModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<MucDoViPhamKyLuatModel>(queryFilter);
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
        public async Task<Response<bool>> Create([FromBody] MucDoViPhamKyLuatCreateModel entity)
        {
            return await _mucDoViPhamKyLuatHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<MucDoViPhamKyLuatModel>> FindById(int recordID)
        {
            return await _mucDoViPhamKyLuatHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<MucDoViPhamKyLuat>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _mucDoViPhamKyLuatHandler.CheckRecordInUse(entity);
        }
        /// <summary>
        /// Hàm cập nhật mức độ vi phạm kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] MucDoViPhamKyLuatUpdateModel entity)
        {
            return await _mucDoViPhamKyLuatHandler.Update(id, entity);
        }
    }
}
