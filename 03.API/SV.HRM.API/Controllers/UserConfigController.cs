using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using NLog;

namespace SV.HRM.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserConfigController : ControllerBase
    {
        private readonly IUserConfigHttpService _userConfigHttpService;

        public UserConfigController(
            IUserConfigHttpService userConfigHttpService)
        {
            _userConfigHttpService = userConfigHttpService;
        }

        #region

        /// <summary>
        /// Hàm lấy về combobox
        /// </summary>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<UserConfigComboboxStaff>>> GetComboboxStaff(string q, int page)
        {
            return await _userConfigHttpService.GetComboboxStaff(q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox
        /// </summary>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<UserConfigComboboxUser>>> GetComboboxUser(string q, int page)
        {
            return await _userConfigHttpService.GetComboboxUser(q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox
        /// </summary>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<UserConfigComboboxWorkflow>>> GetComboboxWorkflow(string q, int page)
        {
            return await _userConfigHttpService.GetComboboxWorkflow(q, page);
        }

        #endregion

        /// <summary>
        /// Grid view UserConfig
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<UserConfigModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _userConfigHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới ngân hàng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(UserConfigModel model)
        {
            return await _userConfigHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng UserConfig
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<UserConfigModel>> FindById(Guid recordID)
        {
            return await _userConfigHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<Guid> recordID)
        {
            return await _userConfigHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _userConfigHttpService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật ngân hàng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(Guid id, UserConfigModel model)
        {
            return await _userConfigHttpService.Update(id, model);
        }
    }
}