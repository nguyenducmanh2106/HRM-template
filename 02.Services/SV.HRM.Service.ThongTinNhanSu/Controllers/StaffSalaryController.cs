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
    public class StaffSalaryController : ControllerBase
    {
        private readonly IBaseHandler _baseHandler;
        private readonly IStaffSalaryHandler _staffSalaryHandler;
        public StaffSalaryController(IBaseHandler baseHandler, IStaffSalaryHandler staffSalaryHandler)
        {
            _baseHandler = baseHandler;
            _staffSalaryHandler = staffSalaryHandler;
        }

        //[CustomAuthorize(Role.QTL_MANAGER, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<StaffSalaryModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                return await _baseHandler.GetFilter<StaffSalaryModel>(queryFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo quá trình lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.QTL_MANAGER, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] StaffSalaryCreateRequestModel entity)
        {
            return await _staffSalaryHandler.Create(entity);
        }

        /// <summary>
        /// Hàm xóa quá trình lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.QTL_MANAGER, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            //return await _staffSalaryHandler.DeleteMany(entity);
            var res = await _baseHandler.DeleteMany<StaffSalary>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.BANG_CAP_CHUNG_CHI, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffSalaryModel>> FindById(int recordID)
        {
            return await _staffSalaryHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm tạo quá trình lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.QTL_MANAGER, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] StaffSalaryUpdateRequestModel entity)
        {
            return await _staffSalaryHandler.Update(id, entity);
        }

        /// <summary>
        /// Hàm lấy hệ số thâm niên
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<object>> GetHeSoThamNien(int? staffID, int? bacLuongID, int? staffSalaryID)
        {
            return await _staffSalaryHandler.GetHeSoThamNien(staffID, bacLuongID, staffSalaryID);
        }


        /// <summary>
        /// lấy quá trình lương liền kề trước đó
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<StaffSalaryModel>> GetStaffSalary_AdjacentBefore(int staffID, int? recordId)
        {
            return await _staffSalaryHandler.GetStaffSalary_AdjacentBefore(staffID, recordId);
        }
        /// <summary>
        /// kiểm tra từ ngày đến ngày có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<bool>> CheckStaffSalaryinHistory(int staffId, DateTime fromDate, DateTime toDate)
        {
            return await _staffSalaryHandler.CheckStaffSalaryinHistory(staffId, fromDate, toDate);
        }
    }
}
