using SV.HRM.Core;
using SV.HRM.Models;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IImportExcelHandler
    {
        /// <summary>
        /// Hàm importExcel
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<string>> ImportStaffData(ImportExcelServiceModel model);
    }
}
