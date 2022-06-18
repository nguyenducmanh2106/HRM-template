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
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolHandler _schoolHandler;
        private readonly IBaseHandler _baseHandler;

        public SchoolController(IBaseHandler baseHandler,
             ISchoolHandler schoolHandler
            )
        {
            _baseHandler = baseHandler;
            _schoolHandler = schoolHandler;
        }
        /// <summary>
        /// lấy danh sách trường học
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<List<SchoolModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<SchoolModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo trường học
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] SchoolCreateModel entity)
        {
            return await _schoolHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<SchoolModel>> FindById(int recordID)
        {
            return await _schoolHandler.FindById(recordID);
        }
        /// <summary>
        /// Tìm bản ghi trong bản có được sử dụng không
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _schoolHandler.FindIdInUse(recordID);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<School>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _schoolHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật trường học
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] SchoolUpdateModel entity)
        {
            return await _schoolHandler.Update(id, entity);
        }
    }
}
