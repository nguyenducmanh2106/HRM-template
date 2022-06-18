using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static SV.HRM.Models.RemindWorkModel;

namespace SV.HRM.API.HttpServices
{
    public class RemindWorkHttpService : IRemindWorkHttpService
    {
        private IHttpHelper _httpHelper;
        public RemindWorkHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<StaffRemindWork>>> AppointNextTime(int day)
        {
            return await _httpHelper.GetAsync<Response<List<StaffRemindWork>>>($"RemindWork/AppointNextTime?day={day}");
        }

        public async Task<Response<List<StaffRemindWork>>> ContractExpiration(int day)
        {
            return await _httpHelper.GetAsync<Response<List<StaffRemindWork>>>($"RemindWork/ContractExpiration?day={day}");
        }

        public async Task<Response<List<StaffRemindWork>>> Discipline(int day)
        {
            return await _httpHelper.GetAsync<Response<List<StaffRemindWork>>>($"RemindWork/Discipline?day={day}");
        }

        public async Task<Response<List<StaffRemindWork>>> EndStrengthenFaculty(int day)
        {
            return await _httpHelper.GetAsync<Response<List<StaffRemindWork>>>($"RemindWork/EndStrengthenFaculty?day={day}");
        }

        public async Task<Response<List<StaffRemindWork>>> OfficialPartyChange(int day)
        {
            return await _httpHelper.GetAsync<Response<List<StaffRemindWork>>>($"RemindWork/OfficialPartyChange?day={day}");
        }

        public async Task<Response<List<StaffRemindWork>>> SalaryIncrease(int day)
        {
            return await _httpHelper.GetAsync<Response<List<StaffRemindWork>>>($"RemindWork/SalaryIncrease?day={day}");
        }

        public async Task<Response<List<StaffRemindWork>>> BirthdayStaff(int day)
        {
            return await _httpHelper.GetAsync<Response<List<StaffRemindWork>>>($"RemindWork/BirthdayStaff?day={day}");
        }

        #region Cấu hình nhắc việc
        public async Task<Response<ConfigSystemRemindModel>> GetConfigSystemRemind(int staffID)
        {
            return await _httpHelper.GetAsync<Response<ConfigSystemRemindModel>>($"RemindWork/GetConfigSystemRemind?staffID={staffID}");
        }

        public async Task<Response<bool>> ConfigSystemRemind(ConfigSystemRemindModel model)
        {
            return await _httpHelper.PostAsync<Response<bool>>("RemindWork/ConfigSystemRemind", model);
        }

        public async Task<Response<bool>> ConfigUserRemind(ConfigUserRemindModel model)
        {
            return await _httpHelper.PostAsync<Response<bool>>("RemindWork/ConfigUserRemind", model);
        }

        public async Task<Response<List<ConfigUserRemindModel>>> GetPersonalReminder(int staffID)
        {
            return await _httpHelper.GetAsync<Response<List<ConfigUserRemindModel>>>($"RemindWork/GetPersonalReminder?staffID={staffID}");
        }

        public async Task<Response<ConfigUserRemindModel>> GetPersonalReminderByID(int id)
        {
            return await _httpHelper.GetAsync<Response<ConfigUserRemindModel>>($"RemindWork/GetPersonalReminderByID?id={id}");
        }

        public async Task<Response<bool>> Deleted(int id)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"RemindWork/Deleted?id={id}");
        }

        /// <summary>
        /// Hàm xóa nhiều bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"RemindWork/DeleteMany", recordID);
        }

        public async Task<Response<bool>> UpdateCompleted(int id)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"RemindWork/UpdateCompleted?id={id}");
        }

        public async Task<Response<List<ConfigUserRemindModel>>> GetFilter(EntityGeneric filter)
        {
            return await _httpHelper.PostAsync<Response<List<ConfigUserRemindModel>>>($"RemindWork/GetFilter", filter);
        }

        #endregion
    }
}
