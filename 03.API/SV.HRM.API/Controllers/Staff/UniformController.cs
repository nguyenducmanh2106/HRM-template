using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UniformController : ControllerBase
    {
        private readonly IUniformHttpService _uniformHttpService;
        public UniformController(IUniformHttpService uniformHttpService)
        {
            _uniformHttpService = uniformHttpService;
        }

        [HttpGet]
        public async Task<Response<UniformDetailModel>> GetById(int id)
        {
            return await _uniformHttpService.GetById(id);
        }

        [HttpGet]
        public async Task<Response<string>> GetStaffFullNameById(int id)
        {
            return await _uniformHttpService.GetStaffFullNameById(id);
        }

        [HttpPost]
        public async Task<Response<List<UniformModel>>> GetFilter([FromBody] EntityGeneric filter)
        {
            return await _uniformHttpService.GetFilter(filter);
        }

        [HttpPost]
        public async Task<Response<int>> CreateOrUpdate([FromBody] UniformCreateRequestModel model)
        {
            return await _uniformHttpService.CreateOrUpdate(model);
        }

        [HttpPost]
        public async Task<Response<bool>> DeleteMany([FromBody] List<int> ids)
        {
            return await _uniformHttpService.DeleteMany(ids);
        }
    }
}
