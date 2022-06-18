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
    public class ContractTypeController : ControllerBase
    {
        private readonly IContractTypeHandler _contractTypeHandler;
        private readonly IBaseHandler _baseHandler;

        public ContractTypeController(IBaseHandler baseHandler,
             IContractTypeHandler contractTypeHandler
            )
        {
            _baseHandler = baseHandler;
            _contractTypeHandler = contractTypeHandler;
        }

        /// <summary>
        /// lấy danh sách hệ đào tạo
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<List<ContractTypeModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<ContractTypeModel>(queryFilter);
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
        public async Task<Response<bool>> Create([FromBody] ContractTypeCreateModel entity)
        {
            return await _contractTypeHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ContractTypeModel>> FindById(int recordID)
        {
            return await _contractTypeHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<ContractType>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _contractTypeHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật hệ đào tạo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] ContractTypeUpdateModel entity)
        {
            return await _contractTypeHandler.Update(id, entity);
        }
    }
}
