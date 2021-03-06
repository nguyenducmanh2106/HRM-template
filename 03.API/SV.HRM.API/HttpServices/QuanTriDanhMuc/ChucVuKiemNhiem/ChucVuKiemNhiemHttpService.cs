using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class ChucVuKiemNhiemHttpService : IChucVuKiemNhiemHttpService
    {
        private IHttpHelper _httpHelper;

        public ChucVuKiemNhiemHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }
        public async Task<Response<List<ChucVuKiemNhiemModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<ChucVuKiemNhiemModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới chức vụ kiêm nhiệm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(ChucVuKiemNhiemCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ChucVuKiemNhiem/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chức vụ kiêm nhiệm
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<ChucVuKiemNhiemModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<ChucVuKiemNhiemModel>>($"ChucVuKiemNhiem/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"ChucVuKiemNhiem/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"ChucVuKiemNhiem/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật chức vụ kiêm nhiệm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, ChucVuKiemNhiemUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ChucVuKiemNhiem/Update?id={id}", model);
        }
    }
}
