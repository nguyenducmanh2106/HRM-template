using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Models.RemindWorkModel;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RemindWorkController : ControllerBase
    {
        private readonly IRemindWorkHandler _remindWorkHandler;

        public RemindWorkController(IRemindWorkHandler remindWorkHandler)
        {
            _remindWorkHandler = remindWorkHandler;
        }

        /// <summary>
        /// Hợp đồng lao động hết hạn
        /// </summary>
        /// <param name="beforeDay"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> ContractExpiration(int day)
        {
            return await _remindWorkHandler.ContractExpiration(day);
        }

        /// <summary>
        /// Tăng lương lần tới
        /// </summary>
        /// <param name="beforeDay"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> SalaryIncrease(int day)
        {
            return await _remindWorkHandler.SalaryIncrease(day);
        }

        /// <summary>
        /// Kết thúc đi tăng cường khoa
        /// </summary>
        /// <param name="beforeDay"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> EndStrengthenFaculty(int day)
        {
            return await _remindWorkHandler.EndStrengthenFaculty(day);
        }

        /// <summary>
        /// Thời gian bổ nhiệm tiếp theo
        /// </summary>
        /// <param name="beforeDay"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> AppointNextTime(int day)
        {
            return await _remindWorkHandler.AppointNextTime(day);
        }

        /// <summary>
        /// Chuyển đảng chính thức
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> OfficialPartyChange(int day)
        {
            return await _remindWorkHandler.OfficialPartyChange(day);
        }

        /// <summary>
        /// Kết thúc kỷ luật
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> Discipline(int day)
        {
            return await _remindWorkHandler.Discipline(day);
        }

        /// <summary>
        /// Sinh nhật nhân viên
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> BirthdayStaff(int day)
        {
            return await _remindWorkHandler.BirthdayStaff(day);
        }

        /// <summary>
        /// Lấy chi tiết cấu hình nhắc việc hệ thống
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ConfigSystemRemindModel>> GetConfigSystemRemind(int staffID)
        {
            return await _remindWorkHandler.GetConfigSystemRemind(staffID);
        }

        /// <summary>
        /// Lấy chi tiết cấu hình nhắc việc cá nhân
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<ConfigUserRemindModel>>> GetPersonalReminder(int staffID)
        {
            return await _remindWorkHandler.GetPersonalReminder(staffID);
        }

        /// <summary>
        /// Lấy chi tiết việc cá nhân
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ConfigUserRemindModel>> GetPersonalReminderByID(int id)
        {
            return await _remindWorkHandler.GetPersonalReminderByID(id);
        }

        /// <summary>
        /// Lưu cấu hình nhắc việc hệ thống
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> ConfigSystemRemind(ConfigSystemRemindModel model)
        {
            return await _remindWorkHandler.ConfigSystemRemind(model);
        }

        /// <summary>
        /// Lưu cấu hình nhắc việc cá nhân
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> ConfigUserRemind(ConfigUserRemindModel model)
        {
            return await _remindWorkHandler.ConfigUserRemind(model);
        }

        /// <summary>
        /// Xóa nhắc việc cá nhân
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Deleted(int id)
        {
            return await _remindWorkHandler.Deleted(id);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="recordIds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordIds)
        {
            return await _remindWorkHandler.DeleteMany(recordIds);
        }

        /// <summary>
        /// Cập nhập hoàn thành nhắc việc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> UpdateCompleted(int id)
        {
            return await _remindWorkHandler.UpdateCompleted(id);
        }

        [HttpPost]
        public async Task<Response<List<ConfigUserRemindModel>>> GetFilter([FromBody] EntityGeneric filter)
        {
            return await _remindWorkHandler.GetFilter(filter);
        }
    }
}
