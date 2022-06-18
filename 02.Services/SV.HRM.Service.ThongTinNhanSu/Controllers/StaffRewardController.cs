using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using static SV.HRM.Service.ThongTinNhanSu.CustomAuthorize;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StaffRewardController : ControllerBase
    {
        private readonly IStaffRewardHandler _staffRewardHandler;
        private readonly IBaseHandler _baseHandler;
        public StaffRewardController(IBaseHandler baseHandler,
             IStaffRewardHandler staffRewardHandler
            )
        {
            _baseHandler = baseHandler;
            _staffRewardHandler = staffRewardHandler;
        }

        [CustomAuthorize(new string[] { Role.KT_MANAGER, Role.HSNV_KT_MANAGER }, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<StaffRewardModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var lstApplication = _baseHandler.GetOrganization();
                var response = await _baseHandler.GetFilter<StaffRewardModel>(queryFilter);
                if (response != null && response.Status == Constant.SUCCESS && lstApplication != null)
                {

                    response.Data.ForEach(item => { item.DeptName = lstApplication.SingleOrDefault(g => g.OrganizationId == item.DeptID)?.OrganizationName ?? ""; });
                    return response;
                }
                return new Response<List<StaffRewardModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<Response<List<StaffRewardModel>>> GetByStaff([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var result = await _baseHandler.GetFilter<StaffRewardModel>(queryFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo nhận thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.KT_MANAGER, Role.HSNV_KT_MANAGER }, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] StaffRewardCreateModel entity)
        {
            return await _staffRewardHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffRewardModel>> FindById(int recordID)
        {
            return await _staffRewardHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.KT_MANAGER, Role.HSNV_KT_MANAGER }, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<StaffReward>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.KHEN_THUONG, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Hàm cập nhật khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.KT_MANAGER, Role.HSNV_KT_MANAGER }, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] StaffRewardUpdateModel entity)
        {
            return await _staffRewardHandler.Update(id, entity);
        }

        [HttpGet]
        public async Task<Response<bool>> CheckStaffRewardInHistory(int staffID, DateTime date)
        {
            return await _staffRewardHandler.CheckStaffRewardInHistory(staffID, date);
        }
    }
}
