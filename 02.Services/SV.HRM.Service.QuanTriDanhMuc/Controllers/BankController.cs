using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;

namespace SV.HRM.Service.QuanTriDanhMuc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BankController:ControllerBase
    {
        private readonly IBankHandler _bankHandler;
        private readonly IBaseHandler _baseHandler;
        public BankController(IBaseHandler baseHandler,
             IBankHandler BankHandler
            )
        {
            _baseHandler = baseHandler;
            _bankHandler = BankHandler;
        }
        [HttpPost]
        public async Task<Response<List<BankModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<BankModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpPost]
        //public async Task<Response<List<BankModel>>> GetByStaff([FromBody] EntityGeneric<BankModel> queryFilter)
        //{
        //    try
        //    {
        //        var result = await _baseHandler.GetFilter<BankModel>(queryFilter);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Hàm tạo quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] BankCreateModel entity)
        {
            return await _bankHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<BankModel>> FindById(int recordID)
        {
            return await _bankHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<Bank>(entity);            
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _bankHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] BankUpdateModel entity)
        {
            return await _bankHandler.Update(id, entity);
        }
    }
}
