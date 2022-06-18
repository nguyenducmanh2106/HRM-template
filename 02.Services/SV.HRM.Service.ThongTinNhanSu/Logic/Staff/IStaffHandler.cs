
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IStaffHandler
    {
        List<OrganizationModel> GetListApplication();

        /// <summary>
        /// Tạo mới thông tin chung nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<int>> CreateStaffGeneralInfo(StaffCreateRequestModel model);

        /// <summary>
        /// Cập nhập thông tin chung nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> UpdateStaffGeneralInfo(StaffUpdateRequestModel model);

        /// <summary>
        /// Lấy chi tiết thông tin chung nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<StaffDetailModel>> GetStaffGeneralInfoById(int id);

        /// <summary>
        /// Lấy chi tiết thông tin khác của nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<StaffDetailModel>> GetStaffOrtherInfoById(int id);

        /// <summary>
        ///  Tạo hoặc Sửa thông tin khác của nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<int>> CreateOrUpdateStaffOrtherInfo(StaffCreateRequestModel model);

        /// <summary>
        /// Xóa bản ghi của StaffLiciense theo ID của Staff
        /// </summary>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        Task<Response<bool>> DeleteManyStaffLiciense(List<int> recordID);

        /// <summary>
        /// Tạo tài khoản mặc định cho nhân viên
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Account CreateAccount(AccountModel model);

        int GetStaffIDByStaffCode(string staffCode);
        Task<Response<int>> GetStaffIDByAccountID(int userID);

        /// <summary>
        /// Xóa nhân viên và tất cả bản ghi trong HRM_Attachment bao gồm cả các file thuộc các tab của nhân viên theo ID của Staff
        /// </summary>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        Task<Response<bool>> DeleteAll(List<int> recordID);

        /// <summary>
        /// Tạo tài khoản người dùng từ thông tin hồ sơ nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<int>> GenerateAccount();
    }
}