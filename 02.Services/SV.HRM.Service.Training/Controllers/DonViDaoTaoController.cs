using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.Training
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DonViDaoTaoController : ControllerBase
    {
        private readonly IDonViDaoTaoHandler _DonViDaoTaoHandler;
        private readonly IBaseHandler _baseHandler;

        public DonViDaoTaoController(IBaseHandler baseHandler,
             IDonViDaoTaoHandler DonViDaoTaoHandler
            )
        {
            _baseHandler = baseHandler;
            _DonViDaoTaoHandler = DonViDaoTaoHandler;
        }
        [HttpPost]
        public async Task<Response<List<DonViDaoTaoModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilterCategory<DonViDaoTaoModel>(queryFilter);
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
        public async Task<Response<bool>> Create([FromBody] DonViDaoTaoCreateModel entity)
        {
            return await _DonViDaoTaoHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DonViDaoTaoModel>> FindById(int recordID)
        {
            return await _DonViDaoTaoHandler.FindById(recordID);
        }
        /// <summary>
        /// Tìm bản ghi trong bản có được sử dụng không
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _DonViDaoTaoHandler.FindIdInUse(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<DonViDaoTao>(entity);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _DonViDaoTaoHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] DonViDaoTaoUpdateModel entity)
        {
            return await _DonViDaoTaoHandler.Update(id, entity);
        }
    }
}
