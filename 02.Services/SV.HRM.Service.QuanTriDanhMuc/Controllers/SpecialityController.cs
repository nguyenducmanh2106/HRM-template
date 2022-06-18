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
    public class SpecialityController : ControllerBase
    {
        private readonly ISpecialityHandler _specialityHandler;
        private readonly IBaseHandler _baseHandler;

        public SpecialityController(IBaseHandler baseHandler,
             ISpecialityHandler specialityHandler
            )
        {
            _baseHandler = baseHandler;
            _specialityHandler = specialityHandler;
        }

        /// <summary>
        /// lấy danh sách hệ đào tạo
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<List<SpecialityModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<SpecialityModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo hệ đào tạo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] SpecialityCreateModel entity)
        {
            return await _specialityHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<SpecialityModel>> FindById(int recordID)
        {
            return await _specialityHandler.FindById(recordID);
        }
        /// <summary>
        /// Tìm bản ghi trong bản có được sử dụng không
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _specialityHandler.FindIdInUse(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<Speciality>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _specialityHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật hệ đào tạo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] SpecialityUpdateModel entity)
        {
            return await _specialityHandler.Update(id, entity);
        }
    }
}
