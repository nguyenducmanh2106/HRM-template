using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProfileDocumentController: ControllerBase
    {
        private readonly IProfileDocumentHandler _profileDocumentHandler;
        private readonly IBaseHandler _baseHandler;
        public ProfileDocumentController(IBaseHandler baseHandler,
             IProfileDocumentHandler profileDocumentHandler
            )
        {
            _baseHandler = baseHandler;
            _profileDocumentHandler = profileDocumentHandler;
        }
        [HttpPost]
        public async Task<Response<List<ProfileDocumentModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _profileDocumentHandler.GetFilter(queryFilter);

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
        public async Task<Response<bool>> Create([FromBody] ProfileDocumentCreate entity)
        {
            return await _profileDocumentHandler.Create(entity);
        }
        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ProfileDocumentModel>> FindById(int recordID)
        {
            return await _profileDocumentHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sách bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<ProfileDocument>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.HO_SO, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Hàm cập nhật hồ sơ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] ProfileDocumentUpdate entity)
        {
            return await _profileDocumentHandler.Update(id, entity);
        }

    }
}
