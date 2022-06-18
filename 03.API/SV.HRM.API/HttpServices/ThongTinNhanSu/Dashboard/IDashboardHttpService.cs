using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Models.DashboardModel;

namespace SV.HRM.API.HttpServices
{
    public interface IDashboardHttpService
    {
        /// <summary>
        /// Lấy danh sách kỳ khám
        /// </summary>
        /// <returns></returns>
        Task<Response<List<HealthPeriodBase>>> GetListHealthPeriod();

        /// <summary>
        /// Lấy vị trí chức danh theo nghề
        /// </summary>
        /// <returns></returns>
        Task<Response<string>> GetPostionIDByOccupationName(string names);

        /// <summary>
        /// Thống kê nhân lực bệnh viện
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<Response<BoxModel>> StatisticsAllStaff(BoxFilter filter);

        /// <summary>
        /// Thống kê nhân viên mới
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<Response<BoxModel>> StatisticsNewStaff(BoxFilter filter);

        /// <summary>
        /// Thống kê nhân viên nghỉ việc
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<Response<BoxModel>> StatisticsStaffOut(BoxFilter filter);

        /// <summary>
        /// Thống kê cơ cấu nhân lực
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<Response<DonutsChartOptionsResult>> StatisticsHumanResourceStructure(BoxFilter filter);

        /// <summary>
        /// Thống kê trình độ chuyên môn
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<Response<DonutsChartOptionsResult>> StatisticsQualification(BoxFilter filter);

        /// <summary>
        /// Thống kê biến động nhân sự
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<Response<LineChartOptionsResult>> StatisticsPersonnelChange(BoxFilter filter);

        /// <summary>
        /// Thống kê phân loại sức khỏe
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<Response<DonutsChartOptionsResult>> StatisticsHealthPeriod(BoxFilter filter);
    }
}
