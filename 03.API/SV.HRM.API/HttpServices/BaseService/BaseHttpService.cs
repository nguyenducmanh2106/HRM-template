using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class BaseHttpService : IBaseHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public BaseHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        /// <summary>
        /// Check trùng theo trường nào đó
        /// </summary>
        /// <returns></returns>
        public async Task<Response<bool>> CheckDuplicate(BaseCheckDuplicate model)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Base/CheckDuplicate", model);
        }


        /// <summary>
        /// Hàm lấy về combobox sử dụng load More
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<Response<List<T>>> GetCombobox<T>(string layoutCode, string keySearch, string q, int page)
        {
            return await _httpHelper.GetAsync<Response<List<T>>>($"Base/GetCombobox{layoutCode}?layoutCode={layoutCode}&keySearch={keySearch}&q={q}&page={page}");
        }

        /// <summary>
        /// Hàm lấy về combobox không sử dụng load more
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<Response<List<T>>> GetComboboxByField<T>(string layoutCode, string keySearch, int q)
        {
            return await _httpHelper.PostAsync<Response<List<T>>>($"Base/GetCombobox{layoutCode}?layoutCode={layoutCode}&keySearch={keySearch}&q={q}");
        }

        public async Task<Response<List<TableFieldModel>>> RenderDynamicColumn(string layoutCode, int? userID = null)
        {
            return await _httpHelper.PostAsync<Response<List<TableFieldModel>>>($"TableField/GetFilter?layoutCode={layoutCode}&userID={userID}");
        }

        public async Task<Response<bool>> UpdateTableField(List<TableField> models)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"TableField/Update", models);
        }

        public async Task<Response<List<TableField>>> GetDefaultColumn(string layoutCode)
        {
            return await _httpHelper.PostAsync<Response<List<TableField>>>($"TableField/GetDefaultColumn?layoutCode={layoutCode}");
        }

        public async Task<Response<List<T>>> GetComboboxFromQTHT<T>(string layoutCode, string keySearch, int q)
        {
            return await _httpHelper.PostAsync<Response<List<T>>>($"Base/GetCombobox{layoutCode}?layoutCode={layoutCode}&keySearch={keySearch}&q={q}");
        }

        public async Task<Response<object>> GetLayout(string layoutCode, int? userID = null)
        {
            return await _httpHelper.PostAsync<Response<object>>($"GroupBox/GetLayout?layoutCode={layoutCode}&userID={userID}");
        }

        public async Task<Response<bool>> BulkUpdateGroupBoxField(GroupBoxFieldUpdate models)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"GroupBox/BulkUpdate", models);
        }

        public async Task<Response<T>> GetNameLocation<T>(string layoutCode, string keySearch, int q)
        {
            return await _httpHelper.PostAsync<Response<T>>($"Base/GetNameLocation?layoutCode={layoutCode}&keySearch={keySearch}&q={q}");
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _httpHelper.GetAsync<List<UserModel>>($"Base/GetAllUsers");
        }

        public async Task<Response<UserInfoCacheModel>> GetUserInfo(string userName)
        {
            return await _httpHelper.GetAsync<Response<UserInfoCacheModel>>($"Base/GetUserInfo?userName={userName}");
        }

        public async Task<Response<List<Permissions>>> GetPermissionByUser(int userID, int? appID)
        {
            return await _httpHelper.GetAsync<Response<List<Permissions>>>($"Base/GetPermissionByUser?userID={userID}&appID={appID}");
        }
    }
}
