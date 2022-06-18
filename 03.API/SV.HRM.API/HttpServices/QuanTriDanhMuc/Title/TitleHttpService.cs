using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class TitleHttpService : ITitleHttpService
    {
        private IHttpHelper _httpHelper;
        public TitleHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<TitleModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<TitleModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(TitleCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Title/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng Title
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<TitleModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<TitleModel>>($"Title/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Title/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Title/DeleteManyUseRecord", recordID);
        }

        /// <summary>
        /// Hàm cập nhật danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, TitleUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Title/Update?id={id}", model);
        }

    }
}
