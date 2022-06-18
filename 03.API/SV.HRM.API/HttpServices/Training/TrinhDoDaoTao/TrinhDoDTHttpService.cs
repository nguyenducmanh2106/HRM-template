using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class TrinhDoDTHttpService : ITrinhDoDTHttpService
    {
        private IHttpHelper _httpHelper;
        public TrinhDoDTHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }
        public async Task<Response<List<TrinhDoDTModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<TrinhDoDTModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(TrinhDoDTCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"TrinhDoDT/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng TrinhDoDT
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<TrinhDoDTModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<TrinhDoDTModel>>($"TrinhDoDT/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"TrinhDoDT/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"TrinhDoDT/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, TrinhDoDTUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"TrinhDoDT/Update?id={id}", model);
        }

    }
}
