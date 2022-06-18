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
    public class ContractTypeController : ControllerBase
    {
        private readonly IContractTypeHttpService _contractTypeHttpService;

        public ContractTypeController(IContractTypeHttpService contractTypeHttpService)
        {
            _contractTypeHttpService = contractTypeHttpService;
        }
        /// <summary>
        /// Grid loại hợp đồng
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<ContractTypeModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _contractTypeHttpService.GetFilter(queryFilter);
        }


        /// <summary>
        /// Hàm thêm mới loại hợp đồng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(ContractTypeCreateModel model)
        {
            return await _contractTypeHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng loại hợp đồng
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ContractTypeModel>> FindById(int recordID)
        {
            return await _contractTypeHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _contractTypeHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _contractTypeHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật loại hợp đồng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, ContractTypeUpdateModel model)
        {
            return await _contractTypeHttpService.Update(id, model);
        }
    }
}
