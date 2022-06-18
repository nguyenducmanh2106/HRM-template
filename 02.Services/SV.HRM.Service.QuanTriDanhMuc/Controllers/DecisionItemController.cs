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
    public class DecisionItemController : ControllerBase
    {
        private readonly IDecisionItemHandler _decisionItemHandler;
        private readonly IBaseHandler _baseHandler;

        public DecisionItemController(IBaseHandler baseHandler,
             IDecisionItemHandler decisionItemHandler
            )
        {
            _baseHandler = baseHandler;
            _decisionItemHandler = decisionItemHandler;
        }

        /// <summary>
        /// lấy danh sách hệ đào tạo
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<List<DecisionItemModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<DecisionItemModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo hệ đào tạo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] DecisionItemCreateModel entity)
        {
            return await _decisionItemHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DecisionItemModel>> FindById(int recordID)
        {
            return await _decisionItemHandler.FindById(recordID);
        }

        /// <summary>
        /// Tìm bản ghi trong bản có được sử dụng không
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _decisionItemHandler.FindIdInUse(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<DecisionItem>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _decisionItemHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật hệ đào tạo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] DecisionItemUpdateModel entity)
        {
            return await _decisionItemHandler.Update(id, entity);
        }
    }
}
