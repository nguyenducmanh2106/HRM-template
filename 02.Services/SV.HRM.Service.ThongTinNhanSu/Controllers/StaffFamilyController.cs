using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using System.Linq;
using static SV.HRM.Service.ThongTinNhanSu.CustomAuthorize;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StaffFamilyController : ControllerBase
    {
        private readonly IStaffFamilyHandler _staffFamilyHandler;
        private readonly IBaseHandler _baseHandler;
        public StaffFamilyController(IBaseHandler baseHandler
            , IStaffFamilyHandler staffFamilyHandler
            )
        {
            _baseHandler = baseHandler;
            _staffFamilyHandler = staffFamilyHandler;
        }

        //[CustomAuthorize(Role.GD_MANAGER, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<StaffFamilyModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var result = await _baseHandler.GetFilter<StaffFamilyModel>(queryFilter);
                if (result != null && result.Status == SUCCESS && result.Data.Count > 0)
                {
                    result.Data.ForEach(g =>
                    {
                        g.CountryName = _baseHandler.GetCountries().SingleOrDefault(item => item.CountryId == g.CerTerritoryID)?.CountryName ?? "";
                    });
                }
                return result;
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
        //[CustomAuthorize(Role.GD_MANAGER, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] StaffFamilyCreateModel entity)
        {
            return await _staffFamilyHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffFamilyModel>> FindById(int recordID)
        {
            return await _staffFamilyHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.GD_MANAGER, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<StaffFamily>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.GIA_DINH, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

        /// <summary>
        /// Hàm cập nhật thông tin gia đình của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.GD_MANAGER, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] StaffFamilyUpdateModel entity)
        {
            return await _staffFamilyHandler.Update(id, entity);
        }
    }
}
