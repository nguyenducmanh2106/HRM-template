using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SV.HRM.Core.Utils;
using static SV.HRM.Core.Utils.Constant;
using static SV.HRM.Service.ThongTinNhanSu.CustomAuthorize;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IStaffHandler _StaffHandler;
        private readonly IBaseHandler _baseHandler;
        private readonly IHistoryHandler _historyHandler;
        public HistoryController(IStaffHandler dbStaffHandler, IBaseHandler baseHandler, IHistoryHandler historyHandler)
        {
            _StaffHandler = dbStaffHandler;
            _baseHandler = baseHandler;
            _historyHandler = historyHandler;
        }

        [CustomAuthorize(new string[] { Role.HSNV_QTCT_MANAGER }, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<HistoryModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var lstApplication = _StaffHandler.GetListApplication();
                var responseStaff = await _baseHandler.GetFilter<HistoryModel>(queryFilter);
                List<HistoryModel> staffs = new List<HistoryModel>();
                if (responseStaff != null && responseStaff.Status == Constant.SUCCESS && lstApplication != null)
                {
                    staffs = responseStaff.Data;
                    var staff = (from _staff in staffs
                                 join _app in lstApplication on Convert.ToInt32(_staff.DeptID) equals _app.OrganizationId into tableDefault
                                 from _result in tableDefault.DefaultIfEmpty()
                                 select new HistoryModel()
                                 {
                                     HistoryID = _staff.HistoryID,
                                     FromDate = _staff.FromDate,
                                     Todate = _staff.Todate,
                                     Status = _staff.Status,
                                     StatusText = _staff.Status == HistoryStatusConstant.NUM_THU_VIEC ? HistoryStatusConstant.THU_VIEC : _staff.Status == HistoryStatusConstant.NUM_DAO_TAO_TAP_NGHE ? HistoryStatusConstant.DAO_TAO_TAP_NGHE : _staff.Status == HistoryStatusConstant.NUM_CHINH_THUC ? HistoryStatusConstant.CHINH_THUC : _staff.Status == HistoryStatusConstant.NUM_DIEU_DONG ? HistoryStatusConstant.DIEU_DONG : null,
                                     OrganizationName = _result?.OrganizationName ?? _staff.ExtraText2,
                                     JobTitleName = _staff.JobTitleName,
                                     PositionName = _staff.PositionName ?? _staff.ExtraText3,
                                 }
                             ).ToList();
                    if (staff != null)
                    {
                        return new Response<List<HistoryModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, staff, responseStaff.DataCount, responseStaff.TotalCount, responseStaff.TotalPage, responseStaff.PageNumber, responseStaff.PageSize);
                    }
                    else return new Response<List<HistoryModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
                }
                return new Response<List<HistoryModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
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
        [CustomAuthorize(new string[] { Role.HSNV_QTCT_MANAGER }, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] HistoryCreateRequestModel entity)
        {
            return await _historyHandler.Create(entity);
        }

        /// <summary>
        /// Hàm tạo quá trình công tác trước khi vào công ty
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.HSNV_QTCT_MANAGER }, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> CreateBeforeJoiningCompany(HistoryCreateBeforeJoiningCompanyRequestModel entity)
        {
            return await _historyHandler.CreateBeforeJoiningCompany(entity);
        }

        /// <summary>
        /// Hàm lấy về giá trị hết hạn của quá trình công tác gần nhất
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DateTime>> GetMinFromDate(int staffID)
        {
            return await _historyHandler.GetMinFromDate(staffID);
        }

        /// <summary>
        /// Hàm tạo quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.HSNV_QTCT_MANAGER }, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<History>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.QUA_TRINH_CONG_TAC, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<HistoryModel>> FindById(int recordID)
        {
            return await _historyHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm tạo quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.HSNV_QTCT_MANAGER }, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] HistoryUpdateRequestModel entity)
        {
            return await _historyHandler.Update(id, entity);
        }

        /// <summary>
        /// Cập nhật quá trình công tác trước khi vào công ty
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.HSNV_QTCT_MANAGER }, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> UpdateBeforeJoiningCompany(int id, [FromBody] HistoryUpdateBeforeJoiningCompanyRequestModel model)
        {
            return await _historyHandler.UpdateBeforeJoiningCompany(id, model);
        }

        /// <summary>
        /// Lấy quá trình công tác mới nhất có trạng thái khác tăng cường
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<HistoryModel>> GetHistoryLatest(int recordID)
        {
            return await _historyHandler.GetHistoryLatest(recordID);
        }
    }
}
