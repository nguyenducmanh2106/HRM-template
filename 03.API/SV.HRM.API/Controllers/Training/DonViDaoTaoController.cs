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
    public class DonViDaoTaoController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDonViDaoTaoHttpService _DonViDaoTaoHttpService;

        public DonViDaoTaoController(IDonViDaoTaoHttpService DonViDaoTaoHttpService)
        {
            _DonViDaoTaoHttpService = DonViDaoTaoHttpService;
        }
        [HttpPost]
        public async Task<Response<List<DonViDaoTaoModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _DonViDaoTaoHttpService.GetFilter(queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(DonViDaoTaoCreateModel model)
        {
            return await _DonViDaoTaoHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng DonViDaoTao
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DonViDaoTaoModel>> FindById(int recordID)
        {
            return await _DonViDaoTaoHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _DonViDaoTaoHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _DonViDaoTaoHttpService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, DonViDaoTaoUpdateModel model)
        {
            return await _DonViDaoTaoHttpService.Update(id, model);
        }
        /// <summary>
        /// check bản ghi có đang được sử dụng không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _DonViDaoTaoHttpService.FindIdInUse(recordID);
        }
    }
}
