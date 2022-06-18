using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IStaffPartyHandler
    {
        /// <summary>
        /// Hàm tạo thông tin gia đình nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffPartyCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffPartyModel>> FindById(int recordID);


        /// <summary>
        /// Hàm cập nhật thông tin gia đình của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, StaffPartyUpdateModel model);


        /// <summary>
        /// Tìm và kiểm tra bản ghi đầu tiên để fill giá trị cho các bản ghi tạo sau đó theo nghiệp vụ
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffPartyModel>> FirstOrDefaultByStaffID(int recordID);

        /// <summary>
        /// Tìm thông tin Đảng
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="diplomaNo"></param>
        /// <returns></returns>
        Task<StaffPartyModel> FindByStaffAndTime(int staffId, DateTime? fromDate, DateTime? toDate);
    }
}
