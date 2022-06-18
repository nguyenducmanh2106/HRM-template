using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class DiplomaHttpService : IDiplomaHttpService
    {
        private IHttpHelper _httpHelper;

        public DiplomaHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }
        public async Task<Response<List<DiplomaModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<DiplomaModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới loại bằng cấp
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(DiplomaCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Diploma/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng loai bằng cấp
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<DiplomaModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<DiplomaModel>>($"Diploma/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Diploma/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Diploma/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật loại bằng cấp
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, DiplomaUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Diploma/Update?id={id}", model);
        }
        /// <summary>
        /// check bản ghi có sử dụng không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Diploma/FindIdInUse", recordID);
        }
    }
}
