using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FamilyRelationShipController : ControllerBase
    {
        private readonly IFamilyRelationShipHandler _familyRelationShipHandler;
        private readonly IBaseHandler _baseHandler;

        public FamilyRelationShipController(IBaseHandler baseHandler,
             IFamilyRelationShipHandler familyRelationShipHandler
            )
        {
            _baseHandler = baseHandler;
            _familyRelationShipHandler = familyRelationShipHandler;
        }
        [HttpPost]
        public async Task<Response<List<FamilyRelationShipModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<FamilyRelationShipModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Hàm tạo mối quan hệ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] FamilyRelationShipCreateModel entity)
        {
            return await _familyRelationShipHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<FamilyRelationShipModel>> FindById(int recordID)
        {
            return await _familyRelationShipHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<FamilyRelationShip>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _familyRelationShipHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật mối quan hệ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] FamilyRelationShipUpdateModel entity)
        {
            return await _familyRelationShipHandler.Update(id, entity);
        }
    }
}
