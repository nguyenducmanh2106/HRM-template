using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class ChuyenNganhHttpService : IChuyenNganhHttpService
    {
        private IHttpHelper _httpHelper;
        public ChuyenNganhHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<ChuyenNganhModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<ChuyenNganhModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới chuyên khoa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(ChuyenNganhCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ChuyenNganh/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng Chuyên khoa
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<ChuyenNganhModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<ChuyenNganhModel>>($"ChuyenNganh/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"ChuyenNganh/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"ChuyenNganh/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật chuyên khoa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, ChuyenNganhUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ChuyenNganh/Update?id={id}", model);
        }
    }
}
