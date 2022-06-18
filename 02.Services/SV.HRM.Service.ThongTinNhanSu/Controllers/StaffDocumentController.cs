using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using static SV.HRM.Service.ThongTinNhanSu.CustomAuthorize;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StaffDocumentController
    {
        private readonly IStaffDocumentHandler _staffDocumentHandler;
        private readonly IBaseHandler _baseHandler;
        public StaffDocumentController(IBaseHandler baseHandler,
            IStaffDocumentHandler staffDocumentHandler
            )
        {
            _baseHandler = baseHandler;
            _staffDocumentHandler = staffDocumentHandler;
        }

        //[CustomAuthorize(Role.GTCTH_MANAGER, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<StaffLicenseModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _staffDocumentHandler.GetFilter(queryFilter);

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
        //[CustomAuthorize(Role.GTCTH_MANAGER, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] StaffLicenseCreate entity)
        {
            return await _staffDocumentHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffLicenseModel>> FindById(int recordID)
        {
            return await _staffDocumentHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sách bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.GTCTH_MANAGER, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<StaffLicense>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.VALID_DOCUMENTS, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Hàm cập nhật hồ sơ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.GTCTH_MANAGER, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] StaffLicenseUpdate entity)
        {
            return await _staffDocumentHandler.Update(id, entity);
        }
    }
}
