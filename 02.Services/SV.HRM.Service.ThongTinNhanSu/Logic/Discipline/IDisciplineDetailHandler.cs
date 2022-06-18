using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IDisciplineDetailHandler
    {
        /// <summary>
        /// Hàm thêm mới kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(DisciplineDetailCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<DisciplineDetailModel>> FindById(int recordID);


        /// <summary>
        /// Hàm cập nhật kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, DisciplineDetailUpdateModel model);

        /// <summary>
        /// Tìm kỷ luật theo số quyết định
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="decisionNo"></param>
        /// <returns></returns>
        Task<DisciplineDetailModel> FindByStaffAndDecisionNo(int staffId, string decisionNo);

        /// <summary>
        /// check từ ngày đến ngày có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckStaffDisciplineInHistory(int staffId, DateTime date);
    }
}
