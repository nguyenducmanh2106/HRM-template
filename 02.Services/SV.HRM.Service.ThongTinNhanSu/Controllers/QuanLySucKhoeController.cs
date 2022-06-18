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
    public class QuanLySucKhoeController : ControllerBase
    {
        private readonly IQuanLySucKhoeHandler _quanLySucKhoeHandler;
        private readonly IBaseHandler _baseHandler;
        public QuanLySucKhoeController(IBaseHandler baseHandler,
             IQuanLySucKhoeHandler quanLySucKhoeHandler
            )
        {
            _baseHandler = baseHandler;
            _quanLySucKhoeHandler = quanLySucKhoeHandler;
        }

        [CustomAuthorize(new string[] { Role.SK_MANAGER, Role.HSNV_SK_MANAGER }, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<QuanLySucKhoeModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var lstApplication = _baseHandler.GetOrganization();
                var response = await _baseHandler.GetFilter<QuanLySucKhoeModel>(queryFilter);
                if (response != null && response.Status == Constant.SUCCESS && lstApplication != null)
                {

                    response.Data.ForEach(item => { item.DeptName = lstApplication.SingleOrDefault(g => g.OrganizationId == item.DeptID)?.OrganizationName ?? ""; });
                    return response;
                }
                return new Response<List<QuanLySucKhoeModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo quản lý sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.SK_MANAGER, Role.HSNV_SK_MANAGER }, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] QuanLySucKhoeCreateModel entity)
        {
            return await _quanLySucKhoeHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<QuanLySucKhoeModel>> FindById(int recordID)
        {
            return await _quanLySucKhoeHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.SK_MANAGER, Role.HSNV_SK_MANAGER }, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<QuanLySucKhoe>(entity);
            if (res != null && res.Status == Constant.SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(Constant.FileType.QUAN_LY_SUC_KHOE, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Hàm cập nhật quản lý quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.SK_MANAGER, Role.HSNV_SK_MANAGER }, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] QuanLySucKhoeUpdateModel entity)
        {
            return await _quanLySucKhoeHandler.Update(id, entity);
        }

        /// <summary>
        /// Hàm check ngày khám có trong kỳ khám không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<bool>> CheckDateBetween(int staffId, int healthPeriod, DateTime date)
        {
            return await _quanLySucKhoeHandler.CheckDateBetween(staffId, healthPeriod, date);
        }

        /// <summary>
        /// Hàm check ngày khám có trong quá trình công tác không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<bool>> CheckHealthPeriodAndHistory(int staffId, int healthPeriod)
        {
            return await _quanLySucKhoeHandler.CheckHealthPeriodAndHistory(staffId, healthPeriod);
        }
    }
}
