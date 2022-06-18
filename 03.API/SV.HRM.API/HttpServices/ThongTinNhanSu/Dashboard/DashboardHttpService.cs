using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static SV.HRM.Models.DashboardModel;

namespace SV.HRM.API.HttpServices
{
    public class DashboardHttpService : IDashboardHttpService
    {
        private IHttpHelper _httpHelper;
        public DashboardHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<BoxModel>> StatisticsAllStaff(BoxFilter filter)
        {
            return await _httpHelper.PostAsync<Response<BoxModel>>($"Dashboard/StatisticsAllStaff", filter);
        }

        public async Task<Response<BoxModel>> StatisticsNewStaff(BoxFilter filter)
        {
            return await _httpHelper.PostAsync<Response<BoxModel>>($"Dashboard/StatisticsNewStaff", filter);
        }

        public async Task<Response<BoxModel>> StatisticsStaffOut(BoxFilter filter)
        {
            return await _httpHelper.PostAsync<Response<BoxModel>>($"Dashboard/StatisticsStaffOut", filter);
        }
        public async Task<Response<DonutsChartOptionsResult>> StatisticsHumanResourceStructure(BoxFilter filter)
        {
            return await _httpHelper.PostAsync<Response<DonutsChartOptionsResult>>($"Dashboard/StatisticsHumanResourceStructure", filter);
        }

        public async Task<Response<DonutsChartOptionsResult>> StatisticsQualification(BoxFilter filter)
        {
            return await _httpHelper.PostAsync<Response<DonutsChartOptionsResult>>($"Dashboard/StatisticsQualification", filter);
        }

        public async Task<Response<LineChartOptionsResult>> StatisticsPersonnelChange(BoxFilter filter)
        {
            return await _httpHelper.PostAsync<Response<LineChartOptionsResult>>($"Dashboard/StatisticsPersonnelChange", filter);
        }

        public async Task<Response<DonutsChartOptionsResult>> StatisticsHealthPeriod(BoxFilter filter)
        {
            return await _httpHelper.PostAsync<Response<DonutsChartOptionsResult>>($"Dashboard/StatisticsHealthPeriod", filter);
        }

        public async Task<Response<List<HealthPeriodBase>>> GetListHealthPeriod()
        {
            return await _httpHelper.GetAsync<Response<List<HealthPeriodBase>>>($"Dashboard/GetListHealthPeriod");
        }

        public async Task<Response<string>> GetPostionIDByOccupationName(string names)
        {
            return await _httpHelper.GetAsync<Response<string>>($"Dashboard/GetPostionIDByOccupationName?names={names}");
        }
    }
}
