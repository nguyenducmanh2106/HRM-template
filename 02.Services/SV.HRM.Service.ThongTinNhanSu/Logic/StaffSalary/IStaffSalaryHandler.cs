
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IStaffSalaryHandler
    {
        /// <summary>
        /// Hàm tạo quá trình lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffSalaryCreateRequestModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffSalaryModel>> FindById(int recordID);

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany(List<int> recordID);

        /// <summary>
        /// Hàm cập nhật quá trình lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, StaffSalaryUpdateRequestModel model);


        /// <summary>
        /// Hàm lấy hệ số thâm niên
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<object>> GetHeSoThamNien(int? staffID, int? bacLuongID, int? staffSalaryID);

        /// <summary>
        /// Tìm quá trình lương theo số quyết định
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="decisionNo"></param>
        /// <param name="decisionDate"></param>
        /// <returns></returns>
        Task<StaffSalaryModel> FindByStaffAndDecisionNo(int staffId, string decisionNo, DateTime? decisionDate);

        /// <summary>
        /// lấy quá trình lương liền kề trước đó
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        Task<Response<StaffSalaryModel>> GetStaffSalary_AdjacentBefore(int staffId,int? recordId);
        /// <summary>
        /// check từ ngày đến ngày có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckStaffSalaryinHistory(int staffId, DateTime fromDate,DateTime toDate);
    }
}