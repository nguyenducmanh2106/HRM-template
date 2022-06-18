using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class PartyTitleHttpService : IPartyTitleHttpService
    {
        private IHttpHelper _httpHelper;
        public PartyTitleHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<PartyTitleModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<PartyTitleModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới chức vụ đảng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(PartyTitleCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"PartyTitle/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chức vụ đảng
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<PartyTitleModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<PartyTitleModel>>($"PartyTitle/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"PartyTitle/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"PartyTitle/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật chức vụ đảng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, PartyTitleUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"PartyTitle/Update?id={id}", model);
        }
    }
}
