using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.Training
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TrinhDoDTController : ControllerBase
    {
        private readonly ITrinhDoDaoTaoHandler _trinhDoDaoTaoHandler;
        private readonly IBaseHandler _baseHandler;

        public TrinhDoDTController(IBaseHandler baseHandler,
             ITrinhDoDaoTaoHandler trinhDoDaoTaoHandler
            )
        {
            _baseHandler = baseHandler;
            _trinhDoDaoTaoHandler = trinhDoDaoTaoHandler;
        }
        [HttpPost]
        public async Task<Response<List<TrinhDoDTModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilterCategory<TrinhDoDTModel>(queryFilter);
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
        public async Task<Response<bool>> Create([FromBody] TrinhDoDTCreateModel entity)
        {
            return await _trinhDoDaoTaoHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<TrinhDoDTModel>> FindById(int recordID)
        {
            return await _trinhDoDaoTaoHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<TrinhDoDT>(entity);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _trinhDoDaoTaoHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] TrinhDoDTUpdateModel entity)
        {
            return await _trinhDoDaoTaoHandler.Update(id, entity);
        }
    }
}
