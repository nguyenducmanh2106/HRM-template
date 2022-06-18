using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SV.HRM.Service.BaseServices.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserConfigController : ControllerBase
    {
        private readonly IUserConfigHandler _userConfigHandler;
        private readonly IBaseHandler _baseHandler;

        public UserConfigController(IUserConfigHandler userConfigHandler, IBaseHandler baseHandler)
        {
            _userConfigHandler = userConfigHandler;
            _baseHandler = baseHandler;
        }

        [HttpPost]
        public async Task<Response<List<UserConfigModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _userConfigHandler.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm tạo mới bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] UserConfig entity)
        {
            return await _userConfigHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<UserConfigModel>> FindById(Guid recordID)
        {
            return await _userConfigHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<UserConfig>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _userConfigHandler.CheckRecordInUseGuid(entity);
        }

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(Guid id, [FromBody] UserConfig entity)
        {
            return await _userConfigHandler.Update(id, entity);
        }

        /// <summary>
        /// Hàm lấy về combobox
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<UserConfigComboboxStaff>>> GetComboboxStaff(string q, int page)
        {
            return await _userConfigHandler.GetComboboxStaff(q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<UserConfigComboboxUser>>> GetComboboxUser(string q, int page)
        {
            return await _userConfigHandler.GetComboboxUser(q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<UserConfigComboboxWorkflow>>> GetComboboxWorkflow(string q, int page)
        {
            return await _userConfigHandler.GetComboboxWorkflow(q, page);
        }
    }
}
