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
    public class DecisionController : ControllerBase
    {
        private readonly IDecisionHandler _decisionHandler;
        private readonly IBaseHandler _baseHandler;
        public DecisionController(IBaseHandler baseHandler,
             IDecisionHandler decisionHandler
            )
        {
            _baseHandler = baseHandler;
            _decisionHandler = decisionHandler;
        }

        [CustomAuthorize(new string[] { Role.QD_MANAGER }, Right.VIEW)]
        //[CustomAuthorize(Role.QD_MANAGER, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<DecisionModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var lstApplication = _baseHandler.GetOrganization();
                var response = await _baseHandler.GetFilter<DecisionModel>(queryFilter);
                if (response != null && response.Status == Constant.SUCCESS && lstApplication != null)
                {

                    response.Data.ForEach(item => { item.DeptName = lstApplication.SingleOrDefault(g => g.OrganizationId == item.DeptID)?.OrganizationName ?? ""; });
                    return response;
                }
                return new Response<List<DecisionModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<Response<List<DecisionModel>>> GetByStaff([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var result = await _baseHandler.GetFilter<DecisionModel>(queryFilter);
                if (result != null && result.Status == SUCCESS && result.Data.Count > 0)
                {
                    //result.Data.ForEach(g =>
                    //{
                    //    g.CountryName = _baseHandler.GetCountries().SingleOrDefault(item => item.CountryId == g.CerTerritoryID)?.CountryName ?? "";
                    //});
                }
                return result;
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
        //[CustomAuthorize(Role.QD_MANAGER, Right.CREATE)]
        [CustomAuthorize(new string[] { Role.QD_MANAGER }, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] DecisionCreateModel entity)
        {
            return await _decisionHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DecisionModel>> FindById(int recordID)
        {
            return await _decisionHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.QD_MANAGER, Right.DELETE)]
        [CustomAuthorize(new string[] { Role.QD_MANAGER }, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<Decision>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.QUYET_DINH, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.QD_MANAGER, Right.UPDATE)]
        [CustomAuthorize(new string[] { Role.QD_MANAGER }, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] DecisionUpdateModel entity)
        {
            return await _decisionHandler.Update(id, entity);
        }
        /// <summary>
        /// check ngày có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<bool>> CheckStaffDecisionInHistory(int staffId, DateTime date)
        {
            return await _decisionHandler.CheckStaffDecisionInHistory(staffId, date);
        }
    }
}
