using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class DecisionItemHttpService : IDecisionItemHttpService
    {
        private IHttpHelper _httpHelper;
        public DecisionItemHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<DecisionItemModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<DecisionItemModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới hệ đào tạo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(DecisionItemCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"DecisionItem/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng DecisionItem
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<DecisionItemModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<DecisionItemModel>>($"DecisionItem/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"DecisionItem/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"DecisionItem/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật hệ đào tạo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, DecisionItemUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"DecisionItem/Update?id={id}", model);
        }
        /// <summary>
        /// check bản ghi có sử dụng không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"DecisionItem/FindIdInUse", recordID);
        }
    }
}
