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
    public class JobTitleController : ControllerBase
    {
        private readonly IJobTitleHandler _jobTitleHandler;
        private readonly IBaseHandler _baseHandler;

        public JobTitleController(IBaseHandler baseHandler,
             IJobTitleHandler jobTitleHandler
            )
        {
            _baseHandler = baseHandler;
            _jobTitleHandler = jobTitleHandler;
        }
        [HttpPost]
        public async Task<Response<List<JobTitleModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<JobTitleModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo vị trí việc làm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] JobTitleCreateModel entity)
        {
            return await _jobTitleHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<JobTitleModel>> FindById(int recordID)
        {
            return await _jobTitleHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<JobTitle>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _jobTitleHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật vị trí việc làm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] JobTitleUpdateModel entity)
        {
            return await _jobTitleHandler.Update(id, entity);
        }
    }
}
