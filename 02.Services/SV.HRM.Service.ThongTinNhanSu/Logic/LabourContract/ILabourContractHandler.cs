using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface ILabourContractHandler
    {
        /// <summary>
        /// Hàm tạo mới hợp đồng lao động
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(LabourContractCreateModel model);

        /// <summary>
        /// Lấy về combobox phụ lục của hợp đồng
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <returns></returns>

        Task<Response<List<LabourContractModel>>> GetComboboxParentLabourContractID(string layoutCode, string keySearch, int q);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<LabourContractModel>> FindById(int recordID);

        /// <summary>
        /// Hàm cập nhật hợp đồng lao động của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, LabourContractUpdateModel model);

        /// <summary>
        /// Tìm hợp đồng lao động theo điều kiện
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="contractNo"></param>
        /// <param name="contractType"></param>
        /// <param name="contractFromDate"></param>
        /// <param name="contractToDate"></param>
        /// <returns></returns>
        Task<LabourContractModel> FindByStaffAndContractInfo(int staffId, string contractNo, int contractType, DateTime? contractFromDate, DateTime? contractToDate);
    }
}
