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
    public class StaffAssessmentController : ControllerBase
    {
        private readonly IStaffAssessmentHandler _staffAssessmentHandler;
        private readonly IBaseHandler _baseHandler;
        public StaffAssessmentController(IBaseHandler baseHandler,
             IStaffAssessmentHandler staffAssessmentHandler
            )
        {
            _baseHandler = baseHandler;
            _staffAssessmentHandler = staffAssessmentHandler;
        }

        //[CustomAuthorize(Role.DG_MANAGER, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<StaffAssessmentModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<StaffAssessmentModel>(queryFilter);
                if (response != null && response.Status == Constant.SUCCESS)
                {
                    return response;
                }
                return new Response<List<StaffAssessmentModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<Response<List<StaffAssessmentModel>>> GetByStaff([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var result = await _baseHandler.GetFilter<StaffAssessmentModel>(queryFilter);
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
        //[CustomAuthorize(Role.DG_MANAGER, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] StaffAssessmentCreateModel entity)
        {
            return await _staffAssessmentHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffAssessmentModel>> FindById(int recordID)
        {
            return await _staffAssessmentHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.DG_MANAGER, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<StaffAssessment>(entity);
        }

        /// <summary>
        /// Hàm cập nhật khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.DG_MANAGER, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] StaffAssessmentUpdateModel entity)
        {
            return await _staffAssessmentHandler.Update(id, entity);
        }
        /// <summary>
        /// check xem năm có trong quá trình công tác không
        /// </summary>
        /// <param name="staffID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<bool>> CheckStaffAssessmentInHistory(int staffID,int year)
        {
            return await _staffAssessmentHandler.CheckStaffAssessmentInHistory(staffID,year);
        }
    }
}
