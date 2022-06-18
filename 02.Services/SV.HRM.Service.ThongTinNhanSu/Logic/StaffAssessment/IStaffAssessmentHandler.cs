using SV.HRM.Core;
using SV.HRM.Models;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IStaffAssessmentHandler
    {
        /// <summary>
        /// Hàm thêm mới nhận thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffAssessmentCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffAssessmentModel>> FindById(int recordID);

        /// <summary>
        /// Hàm cập nhật nhận thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, StaffAssessmentUpdateModel model);

        /// <summary>
        /// Tìm đánh giá theo số quyết định
        /// </summary>
        /// <param name="AssessmentType"></param>
        /// <param name="objectId"></param>
        /// <param name="decisionNo"></param>
        /// <returns></returns>
        Task<StaffAssessmentModel> FindByDecisionNo(int AssessmentType, int objectId, string decisionNo);
        /// <summary>
        /// check xem năm có trong quá trình công tác không
        /// </summary>
        /// <param name="staffID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckStaffAssessmentInHistory(int staffID, int year);
    }
}
