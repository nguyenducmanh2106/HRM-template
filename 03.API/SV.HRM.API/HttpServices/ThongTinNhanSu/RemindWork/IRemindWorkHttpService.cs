using SV.HRM.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Models.RemindWorkModel;

namespace SV.HRM.API.HttpServices
{
    public interface IRemindWorkHttpService
    {
        /// <summary>
        /// Hợp đồng lao động hết hạn
        /// </summary>
        /// <returns></returns>
        Task<Response<List<StaffRemindWork>>> ContractExpiration(int day);

        /// <summary>
        /// Tăng lương lần tới
        /// </summary>
        /// <returns></returns>
        Task<Response<List<StaffRemindWork>>> SalaryIncrease(int day);

        /// <summary>
        /// Kết thúc đi tăng cường khoa
        /// </summary>
        /// <returns></returns>
        Task<Response<List<StaffRemindWork>>> EndStrengthenFaculty(int day);

        /// <summary>
        /// Chuyển đảng chính thức
        /// </summary>
        /// <returns></returns>
        Task<Response<List<StaffRemindWork>>> OfficialPartyChange(int day);

        /// <summary>
        /// Thời gian bổ nhiệm tiếp theo
        /// </summary>
        /// <returns></returns>
        Task<Response<List<StaffRemindWork>>> AppointNextTime(int day);

        /// <summary>
        /// Kết thúc hình thức kỷ luật
        /// </summary>
        /// <returns></returns>
        Task<Response<List<StaffRemindWork>>> Discipline(int day);

        /// <summary>
        /// Kết thúc hình thức kỷ luật
        /// </summary>
        /// <returns></returns>
        Task<Response<List<StaffRemindWork>>> BirthdayStaff(int day);

        ///// <summary>
        ///// Kêt thúc đào tạo
        ///// </summary>
        ///// <returns></returns>
        //Task<Response<object>> TrainingEnd();

        ///// <summary>
        ///// Kết thúc nghỉ thai sản
        ///// </summary>
        ///// <returns></returns>
        //Task<Response<object>> MaternityEnd();

        ///// <summary>
        ///// Kết thúc nghỉ phép
        ///// </summary>
        ///// <returns></returns>
        //Task<Response<object>> VacationEnd();

        ///// <summary>
        ///// Kết thúc nghỉ ốm
        ///// </summary>
        ///// <returns></returns>
        //Task<Response<object>> SickLeaveEnd();

        ///// <summary>
        ///// Kết thúc nghỉ không lương
        ///// </summary>
        ///// <returns></returns>
        //Task<Response<object>> UnpaidLeaveEnd();

        ///// <summary>
        ///// Thời gian được làm chứng chỉ hành nghề
        ///// </summary>
        ///// <returns></returns>
        //Task<Response<object>> IssuePracticingCertificate();

        ///// <summary>
        ///// Kỷ niệm chương
        ///// </summary>
        ///// <returns></returns>
        //Task<Response<object>> Medal();

        #region Cấu hình nhắc việc
        /// <summary>
        /// Lấy chi tiết cấu hình nhắc việc hệ thống
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<ConfigSystemRemindModel>> GetConfigSystemRemind(int staffID);

        /// <summary>
        /// Lưu cấu hình nhắc việc hệ thống
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> ConfigSystemRemind(ConfigSystemRemindModel model);

        /// <summary>
        /// Lưu cấu hình nhắc việc cá nhân
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> ConfigUserRemind(ConfigUserRemindModel model);

        /// <summary>
        /// Lấy chi tiết cấu hình nhắc việc cá nhân
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<List<ConfigUserRemindModel>>> GetPersonalReminder(int staffID);

        /// <summary>
        /// Lấy chi tiết việc cá nhân
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<ConfigUserRemindModel>> GetPersonalReminderByID(int id);


        /// <summary>
        /// Xóa nhắc việc cá nhân
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<bool>> Deleted(int id);

        /// <summary>
        /// Xóa nhiều nhắc việc cá nhân
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany(List<int> recordID);

        /// <summary>
        /// Cập nhập trạng thái hoàn thành
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<bool>> UpdateCompleted(int id);

        Task<Response<List<ConfigUserRemindModel>>> GetFilter(Models.EntityGeneric filter);

        #endregion
    }
}
