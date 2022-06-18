using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IUserConfigHttpService
    {
        Task<Response<List<UserConfigComboboxStaff>>> GetComboboxStaff(string q, int page);

        Task<Response<List<UserConfigComboboxUser>>> GetComboboxUser(string q, int page);

        Task<Response<List<UserConfigComboboxWorkflow>>> GetComboboxWorkflow(string q, int page);



        Task<Response<List<UserConfigModel>>> GetFilter(EntityGeneric queryFilter);

        /// <summary>
        /// Hàm thêm mới quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(UserConfigModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng UserConfig
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<UserConfigModel>> FindById(Guid recordID);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<Guid> recordID);
        /// <summary>
        /// Hàm xóa bản ghi check có sử dụng hay không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteManyUseRecord(List<object> recordID);

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(Guid id, UserConfigModel model);
    }
}