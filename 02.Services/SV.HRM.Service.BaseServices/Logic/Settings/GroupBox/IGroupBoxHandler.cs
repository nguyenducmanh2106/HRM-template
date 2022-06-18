using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.BaseServices
{
    public interface IGroupBoxHandler
    {
        /// <summary>
        /// Hàm thêm mới quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> BulkCreate(List<GroupBoxModel> groupBoxes, List<GroupBoxField> groupBoxFields, int? userID);



        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> BulkUpdate(List<GroupBox> groupBoxes, List<GroupBoxField> groupBoxFields);

        /// <summary>
        /// Lấy về layout theo user
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityGeneric"></param>
        /// <returns></returns>
        Task<Response<object>> GetLayout(string layoutCode, int? userID = null);

    }
}
