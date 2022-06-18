using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class FamilyRelationShipHttpService : IFamilyRelationShipHttpService
    {
        private IHttpHelper _httpHelper;
        public FamilyRelationShipHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<FamilyRelationShipModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<FamilyRelationShipModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới mối quan hệ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(FamilyRelationShipCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"FamilyRelationShip/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng FamilyRelationShip
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<FamilyRelationShipModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<FamilyRelationShipModel>>($"FamilyRelationShip/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"FamilyRelationShip/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"FamilyRelationShip/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật mối quan hệ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, FamilyRelationShipUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"FamilyRelationShip/Update?id={id}", model);
        }

    }
}
