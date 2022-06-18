using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Models.DashboardModel;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IDashboardHandler
    {
        /// <summary>
        /// Lấy danh sách kỳ khám
        /// </summary>
        /// <returns></returns>
        Task<Response<List<HealthPeriodBase>>> GetListHealthPeriod();

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

        #region Helper
        /// <summary>
        /// Điều kiện lấy dữ liệu
        /// </summary>
        /// <param name="object"></param>
        /// <param name="startOfDate"></param>
        /// <param name="endOfDate"></param>
        /// <returns></returns>
        bool Conditions(History @object, DateTime startOfDate, DateTime endOfDate);

        /// <summary>
        /// hàm lấy về danh sách id của cơ cấu tổ chức con và chính nó
        /// </summary>
        /// <param name="organizationId">Id của cơ cấu tổ chức</param>
        /// <returns></returns>
        List<int> GetListOrgByParentID(int? parentID);
        #endregion

        /// <summary>
        /// Thống kê phân loại sức khỏe
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<Response<DonutsChartOptionsResult>> StatisticsHealthPeriod(BoxFilter filter);

        /// <summary>
        /// Lấy id vị trí chức danh từ tên
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        Task<Response<string>> GetPostionIDByOccupationName(string names);
    }
}
