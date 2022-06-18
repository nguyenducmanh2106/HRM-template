using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IStaffRewardHttpService
    {
        Task<Response<List<StaffRewardModel>>> GetFilter(EntityGeneric StaffQueryFilter);
        Task<Response<List<StaffRewardModel>>> GetByStaff(EntityGeneric StaffQueryFilter);
        /// <summary>
        /// Hàm thêm mới khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffRewardCreateModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng StaffReward
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffRewardModel>> FindById(int recordID);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);

        /// <summary>
        /// Hàm cập nhật khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, StaffRewardUpdateModel model);
        /// <summary>
        /// check xem ngày cấp có trong quá trình công tác
        /// </summary>
        /// <param name="staffID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckStaffRewardInHistory(int staffID, DateTime date);
    }
}
