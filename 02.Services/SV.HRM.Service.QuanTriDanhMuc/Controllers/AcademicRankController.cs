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
    public class AcademicRankController : ControllerBase
    {
        private readonly IAcademicRankHandler _academicRankHandler;
        private readonly IBaseHandler _baseHandler;

        public AcademicRankController(IBaseHandler baseHandler,
             IAcademicRankHandler academicRankHandler
            )
        {
            _baseHandler = baseHandler;
            _academicRankHandler = academicRankHandler;
        }
        [HttpPost]
        public async Task<Response<List<AcademicRankModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<AcademicRankModel>(queryFilter);
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
        public async Task<Response<bool>> Create([FromBody] AcademicRankCreateModel entity)
        {
            return await _academicRankHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<AcademicRankModel>> FindById(int recordID)
        {
            return await _academicRankHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<AcademicRank>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _academicRankHandler.CheckRecordInUse(entity);
        }
        /// <summary>
        /// Hàm cập nhật bậc lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] AcademicRankUpdateModel entity)
        {
            return await _academicRankHandler.Update(id, entity);
        }
    }
}
