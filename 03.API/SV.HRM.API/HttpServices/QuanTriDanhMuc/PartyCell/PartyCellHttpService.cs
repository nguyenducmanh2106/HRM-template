using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class PartyCellHttpService : IPartyCellHttpService
    {
        private IHttpHelper _httpHelper;
        public PartyCellHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<PartyCellModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<PartyCellModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới chi bộ đảng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(PartyCellCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"PartyCell/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chi bộ đảng
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<PartyCellModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<PartyCellModel>>($"PartyCell/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"PartyCell/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"PartyCell/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật chi bộ đảng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, PartyCellUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"PartyCell/Update?id={id}", model);
        }
    }
}
