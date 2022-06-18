
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IStaffDiplomaHandler
    {
        /// <summary>
        /// Hàm tạo bằng cấp chứng chỉ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffDiplomaCreateRequestModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffDiplomaModel>> FindById(int recordID);

        /// <summary>
        /// Hàm cập nhật bằng cấp chứng chỉ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id,StaffDiplomaUpdateRequestModel model);

        /// <summary>
        /// Tìm văn bằng theo số văn bằng
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="diplomaNo"></param>
        /// <returns></returns>
        Task<StaffDiplomaModel> FindByStaffAndDiplomaNo(int staffId, string diplomaNo);
    }
}