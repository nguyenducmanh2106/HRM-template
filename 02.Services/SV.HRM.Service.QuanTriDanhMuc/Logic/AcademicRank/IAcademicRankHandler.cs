using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public interface IAcademicRankHandler
    {
        /// <summary>
        /// Hàm thêm mới trường học
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(AcademicRankCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<AcademicRankModel>> FindById(int recordID);


        /// <summary>
        /// Hàm cập nhật trường học
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, AcademicRankUpdateModel model);
        /// <summary>
        /// Check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckRecordInUse(List<object> obj);
    }
}
