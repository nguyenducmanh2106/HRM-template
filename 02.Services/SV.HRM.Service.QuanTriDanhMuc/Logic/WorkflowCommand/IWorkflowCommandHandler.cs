﻿using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public interface IWorkflowCommandHandler
    {
        /// <summary>
        /// Hàm thêm mới danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(WorkflowCommandCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<WorkflowCommandModel>> FindById(Guid recordID);


        /// <summary>
        /// Hàm cập nhật danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(Guid id, WorkflowCommandUpdateModel model);
        /// <summary>
        /// Check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckRecordInUse(List<object> obj);
    }
}
