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
    public class DiplomaController : ControllerBase
    {
        private readonly IDiplomaHandler _diplomaHandler;
        private readonly IBaseHandler _baseHandler;

        public DiplomaController(IBaseHandler baseHandler,
             IDiplomaHandler diplomaHandler
            )
        {
            _baseHandler = baseHandler;
            _diplomaHandler = diplomaHandler;
        }
        [HttpPost]
        public async Task<Response<List<DiplomaModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<DiplomaModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Hàm tạo loại bằng cấp
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] DiplomaCreateModel entity)
        {
            return await _diplomaHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DiplomaModel>> FindById(int recordID)
        {
            return await _diplomaHandler.FindById(recordID);
        }
        /// <summary>
        /// Tìm bản ghi trong bản có được sử dụng không
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _diplomaHandler.FindIdInUse(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<Diploma>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _diplomaHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật loại bằng cấp
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] DiplomaUpdateModel entity)
        {
            return await _diplomaHandler.Update(id, entity);
        }
    }
}
