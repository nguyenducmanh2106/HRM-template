using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SV.HRM.Service.BaseServices
{
    public interface IBaseHandler
    {

        /// <summary>
        /// Grid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityGeneric"></param>
        /// <returns></returns>
        Task<Response<List<T>>> GetFilter<T>(EntityGeneric entityGeneric);

        /// <summary>
        /// Hàm chung lấy về combobox sử dụng lazyload phân trang
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode">Tên bảng</param>
        /// <param name="keySearch">Tên trường dùng để tìm trên combobox</param>
        /// <param name="q">Giá trị của keySearch</param>
        /// <param name="page">Trang thứ mấy</param>
        /// <param name="hasStatusColumn">đối với bảng có cột trạng thái</param>
        /// <returns></returns>
        Task<Response<List<T>>> GetCombobox<T>(string layoutCode, string keySearch, string q, int page, CommandType? commandType = null, bool? hasStatusColumn = false);


        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layout"></param>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete<T>(string layout, int recordID);

        /// <summary>
        /// Lấy combobox theo quan trường nào đó sử dụng eager load
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <param name="hasStatusColumn">đối với bảng có cột trạng thái</param>
        /// <returns></returns>
        Task<Response<List<T>>> GetComboboxByField<T>(string layoutCode, string keySearch, int q, bool? hasStatusColumn = false);

        /// <summary>
        /// Check trùng theo trường nào đó
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckDuplicate<T>(string keySearch, int q);
        Task<Response<bool>> CheckDuplicate(BaseCheckDuplicate model);

        /// <summary>
        /// Lấy danh sách từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        Task<Response<List<T>>> GetComboboxFromQTHT<T>(string layoutCode, string keySearch, int q, bool? hasStatusColumn = false);

        /// <summary>
        /// Lấy danh sách từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        Task<Response<T>> GetNameLocation<T>(string layoutCode, string keySearch, int q);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        Task<List<UserModel>> GetAllUsers();

        /// <summary>
        /// Lấy danh sách quyền của user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        Task<Response<List<Permissions>>> GetPermissionByUser(int userID, int? appID);

        /// <summary>
        /// Lấy thông tin user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<Response<UserInfoCacheModel>> GetUserInfo(string userName);


        /// <summary>
        /// Hàm chung lấy về combobox nhân viên sử dụng lazyload phân trang
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode">Tên bảng</param>
        /// <param name="keySearch">Tên trường dùng để tìm trên combobox</param>
        /// <param name="q">Giá trị của keySearch</param>
        /// <param name="page">Trang thứ mấy</param>
        /// <param name="commandTyp
        Task<Response<List<StaffComboboxModel>>> GetComboboxStaff(string layoutCode, string keySearch, string q, int page, CommandType? commandType = null);

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn check bản ghi có đang được sử dụng không
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        Task<Response<bool>> DeleteManyCheckUseRecord<T>(List<object> objectDelete);

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn check bản ghi có đang được sử dụng không
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        Task<Response<bool>> DeleteManyCheckUseRecordGuid<T>(List<object> objectDelete);

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany<T>(List<int> recordID);

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany<T>(List<Guid> recordID);

        Task<List<BaseUserModel>> GetListUser();
    }
}
