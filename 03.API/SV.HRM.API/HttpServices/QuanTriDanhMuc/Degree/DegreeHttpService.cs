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
    public class DegreeHttpService : IDegreeHttpService
    {
        private IHttpHelper _httpHelper;
        public DegreeHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }
        public async Task<Response<List<DegreeModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<DegreeModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới học vị
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(DegreeCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Degree/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng Degree
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<DegreeModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<DegreeModel>>($"Degree/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Degree/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Degree/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật học vị
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, DegreeUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Degree/Update?id={id}", model);
        }
    }
}
