using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IDisciplineDetailHttpService
    {
        Task<Response<List<DisciplineDetailModel>>> GetFilter(EntityGeneric StaffQueryFilter);
        Task<Response<List<DisciplineDetailModel>>> GetByStaff(EntityGeneric StaffQueryFilter);
        /// <summary>
        /// Hàm thêm mới kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(DisciplineDetailCreateModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng DisciplineDetail
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<DisciplineDetailModel>> FindById(int recordID);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);

        /// <summary>
        /// Hàm cập nhật kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, DisciplineDetailUpdateModel model);
        /// <summary>
        /// check ngày có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckStaffDisciplineInHistory(int staffId, DateTime date);
    }
}
