using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IStaffSalaryHttpService
    {
        Task<Response<List<StaffSalaryModel>>> GetFilter(EntityGeneric StaffQueryFilter);

        /// <summary>
        /// Hàm tạo quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffSalaryCreateRequestModel model);


        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);

        /// <summary>
        /// Tìm bản ghi trong bảng StaffSalary
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffSalaryModel>> FindById(int recordID);

        /// <summary>
        /// Hàm cập nhật quá trình lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, StaffSalaryUpdateRequestModel model);

        /// <summary>
        /// Hàm cập nhật quá trình lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<object>> GetHeSoThamNien(int? staffID, int? bacLuongID, int? staffSalaryID);

        /// <summary>
        /// Tìm bản ghi quá trình lương liền kề
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffSalaryModel>> GetStaffSalary_AdjacentBefore(int staffID,int? recordID);
        /// <summary>
        /// check từ ngày đến ngày có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckStaffSalaryinHistory(int staffId, DateTime fromDate, DateTime toDate);
    }
}
