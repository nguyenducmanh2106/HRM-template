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
    public class StaffMilitaryController
    {
        private readonly IStaffMilitaryHandler _staffMilitaryHandler;
        private readonly IBaseHandler _baseHandler;
        public StaffMilitaryController(IBaseHandler baseHandler,
             IStaffMilitaryHandler staffMilitaryHandler
            )
        {
            _baseHandler = baseHandler;
            _staffMilitaryHandler = staffMilitaryHandler;
        }

        [HttpPost]
        public async Task<Response<List<StaffMilitaryModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                return await _baseHandler.GetFilter<StaffMilitaryModel>(queryFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Hàm tạo thông tin quân ngũ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] StaffMilitaryCreateModel entity)
        {
            return await _staffMilitaryHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffMilitaryModel>> FindById(int recordID)
        {
            return await _staffMilitaryHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<StaffMilitary>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.THONG_TIN_QUAN_NGU, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Hàm cập nhật thông tin quân ngũ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] StaffMilitaryUpdateModel entity)
        {
            return await _staffMilitaryHandler.Update(id, entity);
        }
    }
}
