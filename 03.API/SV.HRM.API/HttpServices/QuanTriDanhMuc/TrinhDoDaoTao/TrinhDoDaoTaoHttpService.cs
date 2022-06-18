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
    public class TrinhDoDaoTaoHttpService : ITrinhDoDaoTaoHttpService
    {
        private IHttpHelper _httpHelper;
        public TrinhDoDaoTaoHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }
        public async Task<Response<List<TrinhDoDaoTaoModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<TrinhDoDaoTaoModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(TrinhDoDaoTaoCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"TrinhDoDaoTao/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng TrinhDoDaoTao
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<TrinhDoDaoTaoModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<TrinhDoDaoTaoModel>>($"TrinhDoDaoTao/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"TrinhDoDaoTao/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"TrinhDoDaoTao/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, TrinhDoDaoTaoUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"TrinhDoDaoTao/Update?id={id}", model);
        }

    }
}
