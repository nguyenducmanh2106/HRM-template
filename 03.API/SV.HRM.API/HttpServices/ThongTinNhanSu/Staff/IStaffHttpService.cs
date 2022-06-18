using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IStaffHttpService
    {
        Task<Response<List<StaffModel>>> GetFilter(EntityGeneric StaffQueryFilter);
        Task<Response<List<StaffModel>>> GetAll(string q, int page);
        Task<Response<StaffModel>> GetById(int StaffId);
        Task<Response<StaffDetailModel>> GetStaffGeneralInfoById(int id);
        Task<Response<StaffDetailModel>> GetStaffOrtherInfoById(int id);
        Task<Response<int>> GetStaffIDByAccountID(int userID);
        Task<Response<int>> CreateStaffGeneralInfo(StaffCreateRequestModel model);
        Task<Response<bool>> UpdateStaffGeneralInfo(StaffUpdateRequestModel model);
        Task<Response<int>> CreateOrUpdateStaffOrtherInfo(StaffCreateRequestModel model);
        Task<Response<bool>> Delete(List<int> recordID);
        Task<Response<bool>> DeleteList(List<int> id);

        Task<Response<List<Dictionary<string, object>>>> ReportStaff(EntityGeneric StaffQueryFilter);
        Task<Response<int>> GenerateAccount();
    }
}
