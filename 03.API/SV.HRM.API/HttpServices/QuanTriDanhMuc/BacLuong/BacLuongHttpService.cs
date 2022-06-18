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
    public class BacLuongHttpService : IBacLuongHttpService
    {
        private IHttpHelper _httpHelper;
        public BacLuongHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<BacLuongModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<BacLuongModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới BacLuong
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(BacLuongCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"BacLuong/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng BacLuong
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<BacLuongModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<BacLuongModel>>($"BacLuong/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"BacLuong/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"BacLuong/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật nhóm ngạch lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, BacLuongUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"BacLuong/Update?id={id}", model);
        }
    }
}
