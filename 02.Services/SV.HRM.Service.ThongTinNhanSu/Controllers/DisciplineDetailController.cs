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
    public class DisciplineDetailController : ControllerBase
    {
        private readonly IDisciplineDetailHandler _disciplineDetailHandler;
        private readonly IBaseHandler _baseHandler;
        public DisciplineDetailController(IBaseHandler baseHandler,
             IDisciplineDetailHandler disciplineDetailHandler
            )
        {
            _baseHandler = baseHandler;
            _disciplineDetailHandler = disciplineDetailHandler;
        }

        [CustomAuthorize(new string[] { Role.KL_MANAGER, Role.HSNV_KL_MANAGER }, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<DisciplineDetailModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var lstOrganization = _baseHandler.GetOrganization();
                var response = await _baseHandler.GetFilter<DisciplineDetailModel>(queryFilter);
                List<DisciplineDetailModel> models = new List<DisciplineDetailModel>();
                if (response != null && response.Status == Constant.SUCCESS && lstOrganization != null)
                {
                    response.Data.ForEach(item => { item.DeptName = lstOrganization.SingleOrDefault(g => g.OrganizationId == item.DeptID)?.OrganizationName ?? ""; });
                    return response;
                }
                return new Response<List<DisciplineDetailModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<Response<List<DisciplineDetailModel>>> GetByStaff([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var lstOrganization = _baseHandler.GetOrganization();
                var result = await _baseHandler.GetFilter<DisciplineDetailModel>(queryFilter);
                if (result != null && result.Status == Constant.SUCCESS && lstOrganization != null)
                {
                    result.Data.ForEach(item => { item.DeptName = lstOrganization.SingleOrDefault(g => g.OrganizationId == item.DeptID)?.OrganizationName ?? ""; });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.KL_MANAGER, Role.HSNV_KL_MANAGER }, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] DisciplineDetailCreateModel entity)
        {
            return await _disciplineDetailHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DisciplineDetailModel>> FindById(int recordID)
        {
            return await _disciplineDetailHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.KL_MANAGER, Role.HSNV_KL_MANAGER }, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<DisciplineDetail>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.KY_LUAT, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Hàm cập nhật kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.KL_MANAGER, Role.HSNV_KL_MANAGER }, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] DisciplineDetailUpdateModel entity)
        {
            return await _disciplineDetailHandler.Update(id, entity);
        }

        /// <summary>
        /// kiểm tra ngày có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<bool>> CheckStaffDisciplineInHistory(int staffId, DateTime date)
        {
            return await _disciplineDetailHandler.CheckStaffDisciplineInHistory(staffId, date);
        }
    }
}
