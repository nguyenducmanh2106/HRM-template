using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TrinhDoDTController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ITrinhDoDTHttpService _trinhDoDTHttpService;

        public TrinhDoDTController(ITrinhDoDTHttpService TrinhDoDTHttpService)
        {
            _trinhDoDTHttpService = TrinhDoDTHttpService;
        }
        [HttpPost]
        public async Task<Response<List<TrinhDoDTModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _trinhDoDTHttpService.GetFilter(queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(TrinhDoDTCreateModel model)
        {
            return await _trinhDoDTHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng TrinhDoDT
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<TrinhDoDTModel>> FindById(int recordID)
        {
            return await _trinhDoDTHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _trinhDoDTHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _trinhDoDTHttpService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, TrinhDoDTUpdateModel model)
        {
            return await _trinhDoDTHttpService.Update(id, model);
        }
    }
}
