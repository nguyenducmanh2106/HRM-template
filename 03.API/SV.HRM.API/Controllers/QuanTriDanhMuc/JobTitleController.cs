using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers.QuanTriDanhMuc
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class JobTitleController : ControllerBase
    {
        private readonly IJobTitleHttpService _jobTitleHttpService;

        public JobTitleController(IJobTitleHttpService jobTitleHttpService)
        {
            _jobTitleHttpService = jobTitleHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<JobTitleModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _jobTitleHttpService.GetFilter(queryFilter);
        }


        /// <summary>
        /// Hàm thêm mới vị trí việc làm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(JobTitleCreateModel model)
        {
            return await _jobTitleHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng JobTitle
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<JobTitleModel>> FindById(int recordID)
        {
            return await _jobTitleHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _jobTitleHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _jobTitleHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật vị trí việc làm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, JobTitleUpdateModel model)
        {
            return await _jobTitleHttpService.Update(id, model);
        }
    }
}
