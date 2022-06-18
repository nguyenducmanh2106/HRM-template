using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class StaffAssessmentHttpService : IStaffAssessmentHttpService
    {
        private IHttpHelper _httpHelper;
        public StaffAssessmentHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<StaffAssessmentModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<StaffAssessmentModel>>>($"StaffAssessment/GetFilter", StaffQueryFilter);
        }
        public async Task<Response<List<StaffAssessmentModel>>> GetByStaff(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<StaffAssessmentModel>>>($"{StaffQueryFilter.LayoutCode}/GetByStaff", StaffQueryFilter);
        }

        /// <summary>
        /// Hàm thêm mới đánh giá
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffAssessmentCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffAssessment/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng StaffAssessment
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffAssessmentModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<StaffAssessmentModel>>($"StaffAssessment/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"StaffAssessment/DeleteMany", recordID);
        }

        /// <summary>
        /// Hàm cập nhật đánh giá
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffAssessmentUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffAssessment/Update?id={id}", model);
        }
        public async Task<Response<bool>> CheckStaffAssessmentInHistory(int staffID, int year)
        {
            return await _httpHelper.GetAsync<Response<bool>>($"StaffAssessment/CheckStaffAssessmentInHistory?staffID={staffID}&year={year}");
        }
    }
}
