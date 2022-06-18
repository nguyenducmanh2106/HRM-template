using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WKTimeController : ControllerBase
    {
        private readonly IWKTimeHandler _WKTimeHandler;
        private readonly IBaseHandler _baseHandler;

        public WKTimeController(IBaseHandler baseHandler,
             IWKTimeHandler WKTimeHandler
            )
        {
            _baseHandler = baseHandler;
            _WKTimeHandler = WKTimeHandler;
        }
        [HttpPost]
        public async Task<Response<List<WKTimeModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<WKTimeModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Hàm tạo thời gian làm việc
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] WKTimeRequestCreate entity)
        {
            return await _WKTimeHandler.Create(entity);
        }
        /// <summary>
        /// xóa bản ghi theo rownum và textSearch
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="recordIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(string textSearch, List<int> recordIDs)
        {
            return await _WKTimeHandler.DeleteMany(textSearch,recordIDs);
        }
    }
}
