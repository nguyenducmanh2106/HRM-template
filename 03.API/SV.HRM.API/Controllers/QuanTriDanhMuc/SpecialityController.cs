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
    [Route("[controller]/[action]")]
    [ApiController]
    public class SpecialityController : ControllerBase
    {
        private readonly ISpecialityHttpService _specialityHttpService;

        public SpecialityController(ISpecialityHttpService specialityHttpService)
        {
            _specialityHttpService = specialityHttpService;
        }

        [HttpPost]
        public async Task<Response<List<SpecialityModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _specialityHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới trường học
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(SpecialityCreateModel model)
        {
            return await _specialityHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Speciality
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<SpecialityModel>> FindById(int recordID)
        {
            return await _specialityHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _specialityHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _specialityHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật trường học
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, SpecialityUpdateModel model)
        {
            return await _specialityHttpService.Update(id, model);
        }
        /// <summary>
        /// check bản ghi có đang được sử dụng không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _specialityHttpService.FindIdInUse(recordID);
        }
    }
}
