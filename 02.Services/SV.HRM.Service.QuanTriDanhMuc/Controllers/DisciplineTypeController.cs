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
    public class DisciplineTypeController : ControllerBase
    {
        private readonly IDisciplineTypeHandler _disciplineTypeHandler;
        private readonly IBaseHandler _baseHandler;
        public DisciplineTypeController(IBaseHandler baseHandler,
             IDisciplineTypeHandler disciplineTypeHandler
            )
        {
            _baseHandler = baseHandler;
            _disciplineTypeHandler = disciplineTypeHandler;
        }

        [HttpPost]
        public async Task<Response<List<DisciplineTypeModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<DisciplineTypeModel>(queryFilter);
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
        public async Task<Response<bool>> Create([FromBody] DisciplineTypeCreateModel entity)
        {
            return await _disciplineTypeHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DisciplineTypeModel>> FindById(int recordID)
        {
            return await _disciplineTypeHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<DisciplineType>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _disciplineTypeHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] DisciplineTypeUpdateModel entity)
        {
            return await _disciplineTypeHandler.Update(id, entity);
        }
    }
}
