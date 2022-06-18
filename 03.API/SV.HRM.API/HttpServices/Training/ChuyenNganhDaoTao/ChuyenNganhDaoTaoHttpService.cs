using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class ChuyenNganhDaoTaoHttpService : IChuyenNganhDaoTaoHttpService
    {
        private IHttpHelper _httpHelper;
        public ChuyenNganhDaoTaoHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<ChuyenNganhDaoTaoModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<ChuyenNganhDaoTaoModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới chuyên khoa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(ChuyenNganhDaoTaoCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ChuyenNganhDaoTao/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng Chuyên khoa
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<ChuyenNganhDaoTaoModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<ChuyenNganhDaoTaoModel>>($"ChuyenNganhDaoTao/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"ChuyenNganhDaoTao/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"ChuyenNganhDaoTao/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật chuyên khoa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, ChuyenNganhDaoTaoUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ChuyenNganhDaoTao/Update?id={id}", model);
        }
    }
}
