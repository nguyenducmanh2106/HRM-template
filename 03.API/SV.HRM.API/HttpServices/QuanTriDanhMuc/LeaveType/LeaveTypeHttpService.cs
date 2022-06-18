using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class LeaveTypeHttpService : ILeaveTypeHttpService
    {
        private IHttpHelper _httpHelper;
        public LeaveTypeHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }
        public async Task<Response<List<LeaveTypeModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<LeaveTypeModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới nhóm kiểu nghỉ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(LeaveTypeCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"LeaveType/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng LeaveType
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<LeaveTypeModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<LeaveTypeModel>>($"LeaveType/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"LeaveType/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"LeaveType/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật nhóm kiểu nghỉ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, LeaveTypeUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"LeaveType/Update?id={id}", model);
        }
    }
}
