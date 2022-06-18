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
    public class TACategoryHttpService : ITACategoryHttpService
    {
        private IHttpHelper _httpHelper;

        public TACategoryHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<TACategoryModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<TACategoryModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới hình thức kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(TACategoryCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"TACategory/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng hình thức kỷ luật
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<TACategoryModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<TACategoryModel>>($"TACategory/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"TACategory/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"TACategory/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật hình thức kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, TACategoryUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"TACategory/Update?id={id}", model);
        }
    }
}
