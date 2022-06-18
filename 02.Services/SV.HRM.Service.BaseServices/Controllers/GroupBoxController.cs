using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Threading.Tasks;

namespace SV.HRM.Service.BaseServices.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class GroupBoxController
    {
        private readonly IGroupBoxHandler _groupBoxHandler;
        public GroupBoxController(
             IGroupBoxHandler groupBoxHandler
            )
        {
            _groupBoxHandler = groupBoxHandler;
        }

        [HttpPost]
        public async Task<Response<object>> GetLayout(string layoutCode, int? userID)
        {
            try
            {
                var response = await _groupBoxHandler.GetLayout(layoutCode, userID);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<Response<bool>> BulkUpdate(GroupBoxFieldUpdate models)
        {
            try
            {
                var response = await _groupBoxHandler.BulkUpdate(models.GroupBoxes, models.GroupBoxFields);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
