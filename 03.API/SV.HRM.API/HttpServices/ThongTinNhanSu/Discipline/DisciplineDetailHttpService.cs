using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class DisciplineDetailHttpService : IDisciplineDetailHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public DisciplineDetailHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<DisciplineDetailModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<DisciplineDetailModel>>>($"{StaffQueryFilter.LayoutCode}/GetFilter", StaffQueryFilter);
        }
        public async Task<Response<List<DisciplineDetailModel>>> GetByStaff(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<DisciplineDetailModel>>>($"DisciplineDetail/GetByStaff", StaffQueryFilter);
        }

        /// <summary>
        /// Hàm thêm mới kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(DisciplineDetailCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"DisciplineDetail/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng DisciplineDetail
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<DisciplineDetailModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<DisciplineDetailModel>>($"DisciplineDetail/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"DisciplineDetail/DeleteMany", recordID);
        }

        /// <summary>
        /// Hàm cập nhật thông tin gia đình của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, DisciplineDetailUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"DisciplineDetail/Update?id={id}", model);
        }

        public async Task<Response<bool>> CheckStaffDisciplineInHistory(int staffId, DateTime date)
        {
            return await _httpHelper.GetAsync<Response<bool>>($"DisciplineDetail/CheckStaffDisciplineInHistory?staffId={staffId}&date={date}");
        }

    }
}
