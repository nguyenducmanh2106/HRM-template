using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers.Staff
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DisciplineDetailController: ControllerBase
    {
        private readonly IDisciplineDetailHttpService _disciplineDetailHttpService;

        public DisciplineDetailController(IDisciplineDetailHttpService disciplineDetailHttpService)
        {
            _disciplineDetailHttpService = disciplineDetailHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<DisciplineDetailModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _disciplineDetailHttpService.GetFilter(queryFilter);
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<DisciplineDetailModel>>> GetByStaff(EntityGeneric queryFilter)
        {
            return await _disciplineDetailHttpService.GetByStaff(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(DisciplineDetailCreateModel model)
        {
            return await _disciplineDetailHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng DisciplineDetail
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DisciplineDetailModel>> FindById(int recordID)
        {
            return await _disciplineDetailHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _disciplineDetailHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm cập nhật kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, DisciplineDetailUpdateModel model)
        {
            return await _disciplineDetailHttpService.Update(id, model);
        }

        [HttpGet]
        public async Task<Response<bool>> CheckStaffDisciplineInHistory(int staffId, DateTime date)
        {
            return await _disciplineDetailHttpService.CheckStaffDisciplineInHistory(staffId,date);
        }
    }
}
