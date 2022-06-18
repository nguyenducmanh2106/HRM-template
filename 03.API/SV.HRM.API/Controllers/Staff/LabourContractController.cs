using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LabourContractController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ILabourContractHttpService _labourContractHttpService;

        public LabourContractController(ILogger<LabourContractController> logger,
            ILabourContractHttpService labourContractHttpService)
        {
            _labourContractHttpService = labourContractHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [Route("GetFilter")]
        [HttpPost]
        public async Task<Response<List<LabourContractModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _labourContractHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Lấy về combobox phụ lục của hợp đồng
        /// </summary>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("GetComboboxParentLabourContractID/{idRelation}")]
        [HttpGet]
        public async Task<Response<List<LabourContractModel>>> GetComboboxParentLabourContractID([FromRoute] int idRelation, string q)
        {
            return await _labourContractHttpService.GetComboboxParentLabourContractID(nameof(LabourContract), q, idRelation);
        }

        /// <summary>
        /// Hàm tạo hợp đồng lao động
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        public async Task<Response<bool>> Create(LabourContractCreateModel model)
        {
            return await _labourContractHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng LabourContract
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [Route("FindById")]
        [HttpGet]
        public async Task<Response<LabourContractModel>> FindById(int recordID)
        {
            return await _labourContractHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm cập nhật hợp đồng lao động của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] LabourContractUpdateModel model)
        {
            return await _labourContractHttpService.Update(id, model);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [Route("DeleteMany")]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _labourContractHttpService.Delete(recordID);
        }
    }
}
