using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class OccupationHttpService : IOccupationHttpService
    {
        private IHttpHelper _httpHelper;
        public OccupationHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<OccupationModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<OccupationModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới nghề
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(OccupationCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Occupation/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng nghề
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<OccupationModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<OccupationModel>>($"Occupation/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Occupation/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Occupation/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật nghề
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, OccupationUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Occupation/Update?id={id}", model);
        }
    }
}
