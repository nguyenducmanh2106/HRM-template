using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.API.Controllers.QuanTriDanhMuc
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WKTimeController : ControllerBase
    {
        private readonly IWKTimeHttpService _WKTimeHttpService;

        public WKTimeController(IWKTimeHttpService WKTimeHttpService)
        {
            _WKTimeHttpService = WKTimeHttpService;
        }
        [HttpPost]
        public async Task<Response<List<WKTimeModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _WKTimeHttpService.GetFilter(queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(WKTimeRequestCreate model)
        {
            return await _WKTimeHttpService.Create(model);
        }

        [HttpPost]
        public async Task<Response<bool>> DeleteMany(string textSearch, List<int> recordIDs)
        {
            return await _WKTimeHttpService.DeleteMany(textSearch,recordIDs);
        }
    }
}
