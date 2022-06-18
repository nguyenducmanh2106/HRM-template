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
    public class UserConfigHttpService : IUserConfigHttpService
    {
        private IHttpHelper _httpHelper;

        public UserConfigHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }
        public async Task<Response<List<UserConfigModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<UserConfigModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(UserConfigModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"UserConfig/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng UserConfig
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<UserConfigModel>> FindById(Guid recordID)
        {
            return await _httpHelper.GetAsync<Response<UserConfigModel>>($"UserConfig/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<Guid> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"UserConfig/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"UserConfig/DeleteManyUseRecord", recordID);
        }

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(Guid id, UserConfigModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"UserConfig/Update?id={id}", model);
        }

        public async Task<Response<List<UserConfigComboboxStaff>>> GetComboboxStaff(string q, int page)
        {
            return await _httpHelper.GetAsync<Response<List<UserConfigComboboxStaff>>>($"UserConfig/GetComboboxStaff?q={q}&page={page}");
        }

        public async Task<Response<List<UserConfigComboboxUser>>> GetComboboxUser(string q, int page)
        {
            return await _httpHelper.GetAsync<Response<List<UserConfigComboboxUser>>>($"UserConfig/GetComboboxUser?q={q}&page={page}");
        }

        public async Task<Response<List<UserConfigComboboxWorkflow>>> GetComboboxWorkflow(string q, int page)
        {
            return await _httpHelper.GetAsync<Response<List<UserConfigComboboxWorkflow>>>($"UserConfig/GetComboboxWorkflow?q={q}&page={page}");
        }
    }
}