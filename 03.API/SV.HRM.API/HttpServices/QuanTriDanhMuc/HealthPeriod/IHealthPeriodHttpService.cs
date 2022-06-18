﻿using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IHealthPeriodHttpService
    {
        Task<Response<List<HealthPeriodModel>>> GetFilter(EntityGeneric queryFilter);

        /// <summary>
        /// Hàm thêm mới mối quan hệ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(HealthPeriodCreateModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng HealthPeriod
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<HealthPeriodModel>> FindById(int recordID);
        /// <summary>
        /// check bản ghi trong bảng HealthPeriod có đang được sử dụng hay không
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
        /// Hàm cập nhật mối quan hệ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, HealthPeriodUpdateModel model);

        /// <summary>
        /// Hàm check Date trong năm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckDateInPeriod(HealthPeriod model);
    }
}
