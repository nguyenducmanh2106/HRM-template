using Microsoft.AspNetCore.Mvc;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Models.DashboardModel;

namespace SV.HRM.API.Controllers.Staff
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardHttpService _dashboardService;
        public DashboardController(IDashboardHttpService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<Response<List<HealthPeriodBase>>> GetListHealthPeriod()
        {
            return await _dashboardService.GetListHealthPeriod();
        }

        [HttpGet]
        public async Task<Response<string>> GetPostionIDByOccupationName(string names)
        {
            return await _dashboardService.GetPostionIDByOccupationName(names);
        }

        [HttpPost]
        public async Task<Response<BoxModel>> StatisticsAllStaff(BoxFilter filter)
        {
            return await _dashboardService.StatisticsAllStaff(filter);
        }

        [HttpPost]
        public async Task<Response<BoxModel>> StatisticsNewStaff(BoxFilter filter)
        {
            return await _dashboardService.StatisticsNewStaff(filter);
        }

        [HttpPost]
        public async Task<Response<BoxModel>> StatisticsStaffOut(BoxFilter filter)
        {
            return await _dashboardService.StatisticsStaffOut(filter);
        }

        [HttpPost]
        public async Task<Response<DonutsChartOptionsResult>> StatisticsHumanResourceStructure(BoxFilter filter)
        {
            return await _dashboardService.StatisticsHumanResourceStructure(filter);
        }

        [HttpPost]
        public async Task<Response<DonutsChartOptionsResult>> StatisticsHealthPeriod(BoxFilter filter)
        {
            return await _dashboardService.StatisticsHealthPeriod(filter);
        }

        [HttpPost]
        public async Task<Response<DonutsChartOptionsResult>> StatisticsQualification(BoxFilter filter)
        {
            return await _dashboardService.StatisticsQualification(filter);
        }

        [HttpPost]
        public async Task<Response<LineChartOptionsResult>> StatisticsPersonnelChange(BoxFilter filter)
        {
            return await _dashboardService.StatisticsPersonnelChange(filter);
        }
    }
}
