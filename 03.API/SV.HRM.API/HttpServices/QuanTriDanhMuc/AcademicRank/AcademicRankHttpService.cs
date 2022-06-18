using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class AcademicRankHttpService : IAcademicRankHttpService
    {
        private IHttpHelper _httpHelper;
        public AcademicRankHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<AcademicRankModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<AcademicRankModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới AcademicRank
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(AcademicRankCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"AcademicRank/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng AcademicRank
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<AcademicRankModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<AcademicRankModel>>($"AcademicRank/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"AcademicRank/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"AcademicRank/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật nhóm ngạch lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, AcademicRankUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"AcademicRank/Update?id={id}", model);
        }
    }
}
