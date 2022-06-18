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
    public class NhomNgachLuongHttpService : INhomNgachLuongHttpService
    {
        private IHttpHelper _httpHelper;
        public NhomNgachLuongHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<NhomNgachLuongModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<NhomNgachLuongModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới NhomNgachLuong
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(NhomNgachLuongCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"NhomNgachLuong/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng NhomNgachLuong
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<NhomNgachLuongModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<NhomNgachLuongModel>>($"NhomNgachLuong/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"NhomNgachLuong/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"NhomNgachLuong/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật nhóm ngạch lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, NhomNgachLuongUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"NhomNgachLuong/Update?id={id}", model);
        }
    }
}
