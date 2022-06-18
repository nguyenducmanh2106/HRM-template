using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IQuanLySucKhoeHttpService
    {
        Task<Response<List<QuanLySucKhoeModel>>> GetFilter(EntityGeneric StaffQueryFilter);
        /// <summary>
        /// Hàm thêm mới quản lý sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(QuanLySucKhoeCreateModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng QuanLySucKhoe
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<QuanLySucKhoeModel>> FindById(int recordID);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);

        /// <summary>
        /// Hàm cập nhật quản lý sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, QuanLySucKhoeUpdateModel model);
        /// <summary>
        /// Hàm check date có trong kỳ khám không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckDateBetween(int staffId, int healthPeriod, DateTime date);
        /// <summary>
        /// Hàm check kỳ khám có trong quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckHealthPeriodAndHistory(int staffId, int healthPeriod);
    }
}
