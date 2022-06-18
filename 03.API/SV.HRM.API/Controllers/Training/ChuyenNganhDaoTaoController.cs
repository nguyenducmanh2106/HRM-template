using Microsoft.AspNetCore.Mvc;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ChuyenNganhDaoTaoController : ControllerBase
    {
        private readonly IChuyenNganhDaoTaoHttpService _ChuyenNganhDaoTaoHttpService;

        public ChuyenNganhDaoTaoController(IChuyenNganhDaoTaoHttpService ChuyenNganhDaoTaoHttpService)
        {
            _ChuyenNganhDaoTaoHttpService = ChuyenNganhDaoTaoHttpService;
        }
        /// <summary>
        /// Grid chuyên nganh
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<ChuyenNganhDaoTaoModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _ChuyenNganhDaoTaoHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới chuyên nganh
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(ChuyenNganhDaoTaoCreateModel model)
        {
            return await _ChuyenNganhDaoTaoHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng ChuyenNganhDaoTao
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ChuyenNganhDaoTaoModel>> FindById(int recordID)
        {
            return await _ChuyenNganhDaoTaoHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _ChuyenNganhDaoTaoHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _ChuyenNganhDaoTaoHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật ChuyenNganhDaoTao
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, ChuyenNganhDaoTaoUpdateModel model)
        {
            return await _ChuyenNganhDaoTaoHttpService.Update(id, model);
        }
    }
}
