using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public interface IWKTimeHandler
    {
        /// <summary>
        /// Thêm mới thời gian làm việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(WKTimeRequestCreate model);
        /// <summary>
        /// Xóa bản ghi chưa check có đang được sử dụng hay không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany(string textSearch,List<int> recordIDs);
    }
}
