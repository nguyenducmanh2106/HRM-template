
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.BaseServices
{
    public interface IUserConfigHandler
    {
        Task<Response<List<UserConfigModel>>> GetFilter(EntityGeneric queryFilter);

        /// <summary>
        /// Hàm thêm mới bank
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(UserConfig model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<UserConfigModel>> FindById(Guid recordID);


        /// <summary>
        /// Hàm cập nhật bank
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(Guid id, UserConfig model);

        /// <summary>
        /// Check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckRecordInUse(List<object> obj);

        /// <summary>
        /// Check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckRecordInUseGuid(List<object> obj);

        Task<Response<List<UserConfigComboboxStaff>>> GetComboboxStaff(string q, int page);
        Task<Response<List<UserConfigComboboxUser>>> GetComboboxUser(string q, int page);
        Task<Response<List<UserConfigComboboxWorkflow>>> GetComboboxWorkflow(string q, int page);
    }
}
