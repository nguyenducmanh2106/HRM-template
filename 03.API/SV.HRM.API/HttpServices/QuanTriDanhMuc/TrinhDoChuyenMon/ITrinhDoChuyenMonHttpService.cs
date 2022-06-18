using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface ITrinhDoChuyenMonHttpService
    {
        Task<Response<List<TrinhDoChuyenMonModel>>> GetFilter(EntityGeneric queryFilter);

        /// <summary>
        /// Hàm thêm mới trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(TrinhDoChuyenMonCreateModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng TrinhDoChuyenMon
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<TrinhDoChuyenMonModel>> FindById(int recordID);
        /// <summary>
        /// check bản ghi trong bảng TrinhDoChuyenMon có đang được sử dụng hay không
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<bool>> FindIdInUse(List<object> recordID);
        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);
        /// <summary>
        /// Hàm xóa bản ghi check có sử dụng hay không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteManyUseRecord(List<object> recordID);

        /// <summary>
        /// Hàm cập nhật TrinhDoChuyenMon
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, TrinhDoChuyenMonUpdateModel model);
    }
}
