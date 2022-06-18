using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IStaffAssessmentHttpService
    {
        /// <summary>
        /// Get filter
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        Task<Response<List<StaffAssessmentModel>>> GetFilter(EntityGeneric StaffQueryFilter);

        /// <summary>
        /// Get filter by Staff
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        Task<Response<List<StaffAssessmentModel>>> GetByStaff(EntityGeneric StaffQueryFilter);

        /// <summary>
        /// Hàm thêm mới đánh giá
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffAssessmentCreateModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng StaffAssessment
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffAssessmentModel>> FindById(int recordID);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);

        /// <summary>
        /// Hàm cập nhật đánh giá
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, StaffAssessmentUpdateModel model);
        /// <summary>
        /// check xem năm có trong quá trình công tác không
        /// </summary>
        /// <param name="staffID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckStaffAssessmentInHistory(int staffID, int year);
    }
}
