using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IHistoryHttpService
    {
        Task<Response<List<HistoryModel>>> GetFilter(EntityGeneric StaffQueryFilter);

        /// <summary>
        /// Hàm tạo quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(HistoryCreateRequestModel model);

        /// <summary>
        /// Hàm tạo quá trình công tác trước khi vào công ty
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CreateBeforeJoiningCompany(HistoryCreateBeforeJoiningCompanyRequestModel model);

        /// <summary>
        /// Hàm lấy về giá trị hết hạn của quá trình công tác gần nhất
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<DateTime>> GetMinFromDate(int staffID);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);

        /// <summary>
        /// Tìm bản ghi trong bảng History
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<HistoryModel>> FindById(int recordID);

        /// <summary>
        /// Hàm cập nhật quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, HistoryUpdateRequestModel model);

        /// <summary>
        /// Cập nhật quá trình công tác trước khi vào công ty
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> UpdateBeforeJoiningCompany(int id, HistoryUpdateBeforeJoiningCompanyRequestModel model);

        /// <summary>
        /// Lấy quá trình công tác mới nhất có trạng thái khác tăng cường
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<HistoryModel>> GetHistoryLatest(int recordID);

    }
}
