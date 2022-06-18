using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.BaseServices.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TableFieldController
    {
        private readonly ITableFieldHandler _tableFieldHandler;
        public TableFieldController(
             ITableFieldHandler tableFieldHandler
            )
        {
            _tableFieldHandler = tableFieldHandler;
        }
        [HttpPost]
        public async Task<Response<List<TableFieldModel>>> GetFilter(string layoutCode, int? userID)
        {
            try
            {
                var response = await _tableFieldHandler.GetFilter(layoutCode, userID);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<Response<bool>> Update(List<TableField> models)
        {
            try
            {
                var response = await _tableFieldHandler.Update(models);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lây về cột mặc định theo từng phân hệ chức năng đê build bộ lọc
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityGeneric"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<TableField>>> GetDefaultColumn(string layoutCode)
        {
            try
            {
                var response = await _tableFieldHandler.GetDefaultColumn(layoutCode);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
