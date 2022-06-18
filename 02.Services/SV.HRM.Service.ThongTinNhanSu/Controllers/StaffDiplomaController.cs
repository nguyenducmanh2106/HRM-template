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
    public class StaffDiplomaController : ControllerBase
    {
        private readonly IStaffHandler _StaffHandler;
        private readonly IBaseHandler _baseHandler;
        private readonly IStaffDiplomaHandler _staffDiplomaHandler;
        public StaffDiplomaController(IStaffHandler dbStaffHandler, IBaseHandler baseHandler, IStaffDiplomaHandler staffDiplomaHandler)
        {
            _StaffHandler = dbStaffHandler;
            _baseHandler = baseHandler;
            _staffDiplomaHandler = staffDiplomaHandler;
        }

        //[CustomAuthorize(Role.BCCC_MANAGER, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<StaffDiplomaModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                return await _baseHandler.GetFilter<StaffDiplomaModel>(queryFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.BCCC_MANAGER, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] StaffDiplomaCreateRequestModel entity)
        {
            return await _staffDiplomaHandler.Create(entity);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.BCCC_MANAGER, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<StaffDiploma>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.QUA_TRINH_LUONG, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffDiplomaModel>> FindById(int recordID)
        {
            return await _staffDiplomaHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm cập nhật bằng cấp chứng chỉ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.BCCC_MANAGER, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] StaffDiplomaUpdateRequestModel entity)
        {
            return await _staffDiplomaHandler.Update(id, entity);
        }
    }
}
