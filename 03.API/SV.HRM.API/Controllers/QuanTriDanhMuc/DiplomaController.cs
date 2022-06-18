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
    public class DiplomaController : ControllerBase
    {
        private readonly IDiplomaHttpService _diplomaHttpService;

        public DiplomaController(IDiplomaHttpService diplomaHttpService)
        {
            _diplomaHttpService = diplomaHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<DiplomaModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _diplomaHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới loại bằng cấp
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(DiplomaCreateModel model)
        {
            return await _diplomaHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng loại bằng cấp
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DiplomaModel>> FindById(int recordID)
        {
            return await _diplomaHttpService.FindById(recordID);
        }
        /// <summary>
        /// check bản ghi có đang được sử dụng không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _diplomaHttpService.FindIdInUse(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _diplomaHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _diplomaHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật loại bằng cấp
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, DiplomaUpdateModel model)
        {
            return await _diplomaHttpService.Update(id, model);
        }
    }
}
