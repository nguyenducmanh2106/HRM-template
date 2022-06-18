using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IWKTimeHttpService
    {
        Task<Response<List<WKTimeModel>>> GetFilter(EntityGeneric queryFilter);
        /// <summary>
        /// Hàm thêm mới danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(WKTimeRequestCreate model);
        /// <summary>
        /// xóa bản ghi theo textSearch và rownum
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="recordIDs"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany(string textSearch, List<int> recordIDs);
    }
}
