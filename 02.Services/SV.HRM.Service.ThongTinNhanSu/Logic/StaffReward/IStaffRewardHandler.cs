using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IStaffRewardHandler
    {
        /// <summary>
        /// Hàm thêm mới nhận thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffRewardCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffRewardModel>> FindById(int recordID);


        /// <summary>
        /// Hàm cập nhật nhận thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, StaffRewardUpdateModel model);

        /// <summary>
        /// Tìm khen thưởng theo số quyết định
        /// </summary>
        /// <param name="rewardType"></param>
        /// <param name="objectId"></param>
        /// <param name="decisionNo"></param>
        /// <returns></returns>
        Task<StaffRewardModel> FindByDecisionNo(int rewardType, int objectId, string decisionNo);
        /// <summary>
        /// check xem ngày cấp có trong quá trình công tác không
        /// </summary>
        /// <param name="staffID">id nhân viên</param>
        /// <param name="date">ngày cấp</param>
        /// <returns></returns>
        Task<Response<bool>> CheckStaffRewardInHistory(int staffID, DateTime date);

    }
}
