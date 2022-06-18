using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IDecisionHandler
    {
        /// <summary>
        /// Hàm thêm mới quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(DecisionCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<DecisionModel>> FindById(int recordID);

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, DecisionUpdateModel model);

        /// <summary>
        /// Tìm quyết định theo số quyết định
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="decisionNo"></param>
        /// <returns></returns>
        Task<DecisionModel> FindByStaffAndDecisionNo(int staffId, string decisionNo);
        /// <summary>
        /// check quyết định có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckStaffDecisionInHistory(int staffId, DateTime date);
    }
}
