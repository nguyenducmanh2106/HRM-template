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
    public class AcademicRankController : ControllerBase
    {
        private readonly IAcademicRankHttpService _academicRankHttpService;

        public AcademicRankController(IAcademicRankHttpService academicRankHttpService)
        {
            _academicRankHttpService = academicRankHttpService;
        }
        /// <summary>
        /// Grid view AcademicRank
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<AcademicRankModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _academicRankHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới bậc lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(AcademicRankCreateModel model)
        {
            return await _academicRankHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng AcademicRank
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<AcademicRankModel>> FindById(int recordID)
        {
            return await _academicRankHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _academicRankHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _academicRankHttpService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật AcademicRank
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, AcademicRankUpdateModel model)
        {
            return await _academicRankHttpService.Update(id, model);
        }
    }
}
