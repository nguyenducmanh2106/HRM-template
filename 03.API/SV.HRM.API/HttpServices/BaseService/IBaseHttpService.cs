using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IBaseHttpService
    {
        /// <summary>
        /// Hàm lấy về combobox sử dụng load More
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<Response<List<T>>> GetCombobox<T>(string layoutCode, string keySearch, string q, int page);

        /// <summary>
        /// Hàm lấy về combobox không sử dụng load more
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<Response<List<T>>> GetComboboxByField<T>(string layoutCode, string keySearch, int q);

        /// <summary>
        /// Check trùng theo trường nào đó
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckDuplicate(BaseCheckDuplicate model);

        Task<Response<List<TableFieldModel>>> RenderDynamicColumn(string layoutCode, int? userID = null);
        Task<Response<List<TableField>>> GetDefaultColumn(string layoutCode);
        Task<Response<bool>> UpdateTableField(List<TableField> models);

        /// <summary>
        /// Hàm lấy về combobox không sử dụng load more
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<Response<List<T>>> GetComboboxFromQTHT<T>(string layoutCode, string keySearch, int q);

        /// <summary>
        /// Lấy dữ liệu layout theo user
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<Response<object>> GetLayout(string layoutCode, int? userID = null);

        /// <summary>
        /// Update lại layout của thông tin chung
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        Task<Response<bool>> BulkUpdateGroupBoxField(GroupBoxFieldUpdate models);

        /// <summary>
        /// Hàm lấy về combobox không sử dụng load more
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<Response<T>> GetNameLocation<T>(string layoutCode, string keySearch, int q);

        /// <summary>
        /// Get danh muc nguoi dung
        /// </summary>
        /// <returns></returns>
        Task<List<UserModel>> GetAllUsers();

        /// <summary>
        /// Lấy thông tin user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<Response<UserInfoCacheModel>> GetUserInfo(string userName);

        /// <summary>
        /// Lấy danh sách quyền của user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        Task<Response<List<Permissions>>> GetPermissionByUser(int userID, int? appID);
    }
}
