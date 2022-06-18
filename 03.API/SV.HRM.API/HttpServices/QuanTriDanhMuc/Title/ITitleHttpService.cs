﻿using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface ITitleHttpService
    {
        Task<Response<List<TitleModel>>> GetFilter(EntityGeneric queryFilter);

        /// <summary>
        /// Hàm thêm mới danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(TitleCreateModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng Title
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<TitleModel>> FindById(int recordID);

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
        /// Hàm cập nhật danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, TitleUpdateModel model);
    }
}
