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
    public class MucDoViPhamKyLuatHttpSercie : IMucDoViPhamKyLuatHttpService
    {
        private IHttpHelper _httpHelper;
        public MucDoViPhamKyLuatHttpSercie(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<MucDoViPhamKyLuatModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<MucDoViPhamKyLuatModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới mucdoviphamkyluat
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(MucDoViPhamKyLuatCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"MucDoViPhamKyLuat/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng MucDoViPhamKyLuat
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<MucDoViPhamKyLuatModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<MucDoViPhamKyLuatModel>>($"MucDoViPhamKyLuat/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"MucDoViPhamKyLuat/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"MucDoViPhamKyLuat/DeleteManyUseRecord", recordID);
        }

        /// <summary>
        /// Hàm cập nhật mucdoviphamkyluat
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, MucDoViPhamKyLuatUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"MucDoViPhamKyLuat/Update?id={id}", model);
        }
    }
}
