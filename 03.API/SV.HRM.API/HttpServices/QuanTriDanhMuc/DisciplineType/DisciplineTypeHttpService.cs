using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class DisciplineTypeHttpService : IDisciplineTypeHttpService
    {
        private IHttpHelper _httpHelper;

        public DisciplineTypeHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client,  httpContextAccessor);
        }

        public async Task<Response<List<DisciplineTypeModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<DisciplineTypeModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới hình thức kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(DisciplineTypeCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"DisciplineType/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng hình thức kỷ luật
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<DisciplineTypeModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<DisciplineTypeModel>>($"DisciplineType/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"DisciplineType/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"DisciplineType/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật hình thức kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, DisciplineTypeUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"DisciplineType/Update?id={id}", model);
        }
    }
}
