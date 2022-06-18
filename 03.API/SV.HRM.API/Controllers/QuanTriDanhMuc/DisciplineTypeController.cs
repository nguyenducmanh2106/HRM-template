using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers.QuanTriDanhMuc
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DisciplineTypeController : ControllerBase
    {
        private readonly IDisciplineTypeHttpService _disciplineTypeHttpService;

        public DisciplineTypeController(IDisciplineTypeHttpService disciplineTypeHttpService)
        {
            _disciplineTypeHttpService = disciplineTypeHttpService;
        }
        /// <summary>
        /// Grid view DisciplineType
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<DisciplineTypeModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _disciplineTypeHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới hình thức kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(DisciplineTypeCreateModel model)
        {
            return await _disciplineTypeHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng DisciplineType
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DisciplineTypeModel>> FindById(int recordID)
        {
            return await _disciplineTypeHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _disciplineTypeHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _disciplineTypeHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật hình thức kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, DisciplineTypeUpdateModel model)
        {
            return await _disciplineTypeHttpService.Update(id, model);
        }
    }
}
