using Microsoft.AspNetCore.Mvc;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Models.RemindWorkModel;

namespace SV.HRM.API.Controllers.Staff
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RemindWorkController : ControllerBase
    {
        private readonly IRemindWorkHttpService _httpService;
        public RemindWorkController(IRemindWorkHttpService httpService)
        {
            _httpService = httpService;
        }

        /// <summary>
        /// Hợp đồng lao động hết hạn
        /// </summary>
        /// <param name="beforeDay"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> ContractExpiration(int day)
        {
            return await _httpService.ContractExpiration(day);
        }

        /// <summary>
        /// Tăng lương lần tới
        /// </summary>
        /// <param name="beforeDay"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> SalaryIncrease(int day)
        {
            return await _httpService.SalaryIncrease(day);
        }

        /// <summary>
        /// Kết thúc đi tăng cường khoa
        /// </summary>
        /// <param name="beforeDay"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> EndStrengthenFaculty(int day)
        {
            return await _httpService.EndStrengthenFaculty(day);
        }

        /// <summary>
        /// Thời gian bổ nhiệm tiếp theo
        /// </summary>
        /// <param name="beforeDay"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> AppointNextTime(int day)
        {
            return await _httpService.AppointNextTime(day);
        }

        /// <summary>
        /// Chuyển đảng chính thức
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> OfficialPartyChange(int day)
        {
            return await _httpService.OfficialPartyChange(day);
        }

        /// <summary>
        /// Kết thúc kỷ luật
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> Discipline(int day)
        {
            return await _httpService.Discipline(day);
        }

        /// <summary>
        /// Sinh nhật nhân viên
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffRemindWork>>> BirthdayStaff(int day)
        {
            return await _httpService.BirthdayStaff(day);
        }

        /// <summary>
        /// Cấu hinh nhắc việc hệ thống
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> ConfigSystemRemind(ConfigSystemRemindModel model)
        {
            return await _httpService.ConfigSystemRemind(model);
        }

        /// <summary>
        /// Lấy chi tiết cấu hình nhắc việc hệ thống
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ConfigSystemRemindModel>> GetConfigSystemRemind(int staffID)
        {
            return await _httpService.GetConfigSystemRemind(staffID);
        }

        /// <summary>
        /// Lưu cấu hình nhắc việc cá nhân
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> ConfigUserRemind(ConfigUserRemindModel model)
        {
            return await _httpService.ConfigUserRemind(model);
        }

        /// <summary>
        /// Lấy chi tiết cấu hình nhắc việc cá nhân
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<ConfigUserRemindModel>>> GetPersonalReminder(int staffID)
        {
            return await _httpService.GetPersonalReminder(staffID);
        }

        /// <summary>
        /// Chi tiết việc cá nhân
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ConfigUserRemindModel>> GetPersonalReminderByID(int id)
        {
            return await _httpService.GetPersonalReminderByID(id);
        }

        /// <summary>
        /// Xóa nhắc việc cá nhân
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Deleted(int id)
        {
            return await _httpService.Deleted(id);
        }

        /// <summary>
        /// Cập nhập hoàn thành nhắc việc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> UpdateCompleted(int id)
        {
            return await _httpService.UpdateCompleted(id);
        }

        [HttpPost]
        public async Task<Response<List<ConfigUserRemindModel>>> GetFilter([FromBody] EntityGeneric filter)
        {
            return await _httpService.GetFilter(filter);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _httpService.DeleteMany(recordID);
        }
    }
}
