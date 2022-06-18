using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IStaffDiplomaHttpService
    {
        Task<Response<List<StaffDiplomaModel>>> GetFilter(EntityGeneric StaffQueryFilter);

        /// <summary>
        /// Hàm tạo bằng cấp/chứng chỉ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffDiplomaCreateRequestModel model);


        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);

        /// <summary>
        /// Tìm bản ghi trong bảng History
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffDiplomaModel>> FindById(int recordID);

        /// <summary>
        /// Hàm cập nhật bằng cấp chứng chỉ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id,StaffDiplomaUpdateRequestModel model);
    }
}
