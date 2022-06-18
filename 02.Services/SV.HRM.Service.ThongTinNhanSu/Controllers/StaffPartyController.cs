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
    public class StaffPartyController : ControllerBase
    {
        private readonly IStaffPartyHandler _staffPartyHandler;
        private readonly IBaseHandler _baseHandler;
        public StaffPartyController(IBaseHandler baseHandler,
             IStaffPartyHandler staffPartyHandler
            )
        {
            _baseHandler = baseHandler;
            _staffPartyHandler = staffPartyHandler;
        }

        //[CustomAuthorize(Role.TTD_MANAGER, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<StaffPartyModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                return await _baseHandler.GetFilter<StaffPartyModel>(queryFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo thông tin đảng của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.TTD_MANAGER, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] StaffPartyCreateModel entity)
        {
            return await _staffPartyHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffPartyModel>> FindById(int recordID)
        {
            return await _staffPartyHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.TTD_MANAGER, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<StaffParty>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.THONG_TIN_DANG, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Hàm cập nhật thông tin đảng của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.TTD_MANAGER, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] StaffPartyUpdateModel entity)
        {
            return await _staffPartyHandler.Update(id, entity);
        }

        /// <summary>
        /// Tìm và kiểm tra bản ghi đầu tiên để fill giá trị cho các bản ghi tạo sau đó theo nghiệp vụ
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffPartyModel>> FirstOrDefaultByStaffID(int recordID)
        {
            return await _staffPartyHandler.FirstOrDefaultByStaffID(recordID);
        }
    }
}
