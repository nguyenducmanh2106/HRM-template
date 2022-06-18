using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class PositionHttpService : IPositionHttpService
    {

        private IHttpHelper _httpHelper;
        public PositionHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }
        public async Task<Response<List<PositionModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<PositionModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới vị trí chức danh
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(PositionCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Position/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng position
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<PositionModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<PositionModel>>($"Position/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Position/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Position/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật vị trí chức danh
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, PositionUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Position/Update?id={id}", model);
        }
        /// <summary>
        /// check bản ghi có sử dụng không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Position/FindIdInUse", recordID);
        }
    }
}
