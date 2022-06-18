using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Models.DashboardModel;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardHandler _dashboardHandler;
        public DashboardController(IDashboardHandler dashboardHandler)
        {
            _dashboardHandler = dashboardHandler;
        }

        [HttpGet]
        public async Task<Response<List<HealthPeriodBase>>> GetListHealthPeriod()
        {
            return await _dashboardHandler.GetListHealthPeriod();
        }

        [HttpGet]
        public async Task<Response<string>> GetPostionIDByOccupationName(string names)
        {
            return await _dashboardHandler.GetPostionIDByOccupationName(names);
        }

        [HttpPost]
        public async Task<Response<BoxModel>> StatisticsAllStaff(BoxFilter filter)
        {
            return await _dashboardHandler.StatisticsAllStaff(filter);
        }

        [HttpPost]
        public async Task<Response<BoxModel>> StatisticsNewStaff(BoxFilter filter)
        {
            return await _dashboardHandler.StatisticsNewStaff(filter);
        }

        [HttpPost]
        public async Task<Response<BoxModel>> StatisticsStaffOut(BoxFilter filter)
        {
            return await _dashboardHandler.StatisticsStaffOut(filter);
        }

        [HttpPost]
        public async Task<Response<DonutsChartOptionsResult>> StatisticsHumanResourceStructure(BoxFilter filter)
        {
            return await _dashboardHandler.StatisticsHumanResourceStructure(filter);
        }

        [HttpPost]
        public async Task<Response<DonutsChartOptionsResult>> StatisticsHealthPeriod(BoxFilter filter)
        {
            return await _dashboardHandler.StatisticsHealthPeriod(filter);
        }

        [HttpPost]
        public async Task<Response<DonutsChartOptionsResult>> StatisticsQualification(BoxFilter filter)
        {
            return await _dashboardHandler.StatisticsQualification(filter);
        }

        [HttpPost]
        public async Task<Response<LineChartOptionsResult>> StatisticsPersonnelChange(BoxFilter filter)
        {
            return await _dashboardHandler.StatisticsPersonnelChange(filter);
        }
    }
}
