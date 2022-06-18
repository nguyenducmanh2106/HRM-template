using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class ChucVuChinhQuyenHttpService : IChucVuChinhQuyenHttpService
    {
        private IHttpHelper _httpHelper;

        public ChucVuChinhQuyenHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }
        public async Task<Response<List<ChucVuChinhQuyenModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<ChucVuChinhQuyenModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới chức vụ chính quyền
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(ChucVuChinhQuyenCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ChucVuChinhQuyen/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng ChucVuChinhQuyen
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<ChucVuChinhQuyenModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<ChucVuChinhQuyenModel>>($"ChucVuChinhQuyen/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"ChucVuChinhQuyen/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"ChucVuChinhQuyen/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật ChucVuChinhQuyen
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, ChucVuChinhQuyenUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ChucVuChinhQuyen/Update?id={id}", model);
        }
    }
}
