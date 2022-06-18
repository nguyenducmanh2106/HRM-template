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
    public class ShiftHttpService : IShiftHttpService
    {
        private IHttpHelper _httpHelper;
        public ShiftHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<ShiftModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<ShiftModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới vị trí việc làm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(ShiftCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Shift/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng Shift
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<ShiftModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<ShiftModel>>($"Shift/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Shift/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Shift/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật vị trí việc làm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, ShiftUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Shift/Update?id={id}", model);
        }
    }
}
