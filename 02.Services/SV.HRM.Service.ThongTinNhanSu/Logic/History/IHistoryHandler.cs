
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IHistoryHandler
    {
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
        Task<Response<bool>> CreateBeforeJoiningCompany(HistoryCreateBeforeJoiningCompanyRequestModel entity);

        /// <summary>
        /// Hàm lấy về giá trị hết hạn của quá trình công tác gần nhất
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<DateTime>> GetMinFromDate(int staffID);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<HistoryModel>> FindById(int recordID);


        /// <summary>
        /// Hàm cập nhật quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id,HistoryUpdateRequestModel model);

        /// <summary>
        /// Cập nhật quá trình công tác trước khi vào công ty
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> UpdateBeforeJoiningCompany(int id, HistoryUpdateBeforeJoiningCompanyRequestModel model);

        /// <summary>
        /// Tìm quá trình công tác có trạng thái là điều động tăng cường và có ngày trùng ngày quá trình công tác hiện tại
        /// phục vụ nghiệp vụ BV Đức Giang lấy
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<HistoryModel>> FindByBelongHistoryNow(int recordID, DateTime? FromDate, DateTime? Todate);

        /// <summary>
        /// Tìm QTCT theo Staff
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="status"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        Task<HistoryModel> FindByStaffAndDate(int staffId, int status, DateTime? fromDate, DateTime? toDate);

        /// <summary>
        /// Lấy quá trình công tác mới nhất có trạng thái khác tăng cường
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<HistoryModel>> GetHistoryLatest(int recordID);
    }
}