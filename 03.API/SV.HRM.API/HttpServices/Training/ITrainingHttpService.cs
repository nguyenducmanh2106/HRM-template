using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface ITrainingHttpService
    {
        Task<Response<List<Dictionary<string, object>>>> ReportTraining(EntityGeneric TrainingQueryFilter);
        Task<Response<bool>> CreateObject(string layout, object createObject);
        Task<Response<bool>> UpdateObject(string layout, int id, object createObject);

        /// <summary>
        /// Tìm bản ghi
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<QuanLyDaoTaoModel>> FindById(int recordID);


        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);
    }
}
