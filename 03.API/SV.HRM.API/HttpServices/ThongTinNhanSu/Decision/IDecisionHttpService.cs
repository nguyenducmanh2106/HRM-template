using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IDecisionHttpService
    {
        Task<Response<List<DecisionModel>>> GetFilter(EntityGeneric StaffQueryFilter);

        Task<Response<List<DecisionModel>>> GetByStaff(EntityGeneric StaffQueryFilter);

        /// <summary>
        /// Hàm thêm mới quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(DecisionCreateModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng Decision
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<DecisionModel>> FindById(int recordID);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, DecisionUpdateModel model);
        /// <summary>
        /// check ngày có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckStaffDecisionInHistory(int staffId, DateTime date);
    }
}
