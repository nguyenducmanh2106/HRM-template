using Microsoft.Extensions.Logging;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SV.HRM.Service.BaseServices
{
    public class TableFieldHandler : ITableFieldHandler
    {

        private readonly ILogger<TableFieldHandler> _logger;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;

        public TableFieldHandler(ILogger<TableFieldHandler> logger, IDapperUnitOfWork dapperUnitOfWork
            )
        {
            _logger = logger;
            _dapperUnitOfWork = dapperUnitOfWork;
        }
        public async Task<Response<bool>> BulkCreate(List<TableFieldModel> models, int? userID)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(TableField), "Insert_TableField");
                var objectParams = new object[models.Count];
                for (var index = 0; index < models.Count; index++)
                {
                    var model = models[index];
                    objectParams[index] = new
                    {
                        @ID = Guid.NewGuid(),
                        @LayoutCode = model.LayoutCode,
                        @FilterParams = model.filterParams,
                        @Type = model.type,
                        @Hide = model.hide,
                        @Width = model.width,
                        @Field = model.field,
                        @FieldFilter = model.fieldFilter,
                        @HeaderName = model.headerName,
                        @SortOrder = model.sortOrder,
                        @UserID = userID,
                        @SuppressMenu = model.suppressMenu,
                        @ColumnKey = model.columnKey,
                        @ParentColumnKey = model.parentColumnKey
                    };
                }
                var result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, objectParams);
                return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<Response<TableField>> FindById(int recordID)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Lây về cột mặc định theo từng phân hệ chức năng đê build bộ lọc
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityGeneric"></param>
        /// <returns></returns>
        public async Task<Response<List<TableField>>> GetDefaultColumn(string layoutCode)
        {
            try
            {
                string sqlQueryTableFieldDefault = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(TableField), "List_TableField_Default");
                var resultTableFieldDefault = await _dapperUnitOfWork.GetRepository().QueryAsync<TableField>(sqlQueryTableFieldDefault, new { @LayoutCode = layoutCode });
                if (resultTableFieldDefault != null && resultTableFieldDefault.Count() > 0)
                {
                    return new Response<List<TableField>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, resultTableFieldDefault.ToList());
                }
                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Response<List<TableFieldModel>>> GetFilter(string layoutCode, int? userID = null)
        {
            try
            {
                string sqlQueryTableField_ByUserID = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(TableField), "List_TableField_ByUserID");
                var resultTableField_ByUserID = await _dapperUnitOfWork.GetRepository().QueryAsync<TableFieldModel>(sqlQueryTableField_ByUserID, new { @LayoutCode = layoutCode, @UserID = userID });
                string sqlQueryTableFieldDefault = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(TableField), "List_TableField_Default");
                var resultTableFieldDefault = await _dapperUnitOfWork.GetRepository().QueryAsync<TableFieldModel>(sqlQueryTableFieldDefault, new { @LayoutCode = layoutCode });
                //Nếu có layout cột mặc định rồi
                if (resultTableField_ByUserID != null && resultTableField_ByUserID.Count() > 0)
                {
                    //if (layoutCode == "Report_Staff")
                    //{
                    //    resultTableField_ByUserID = RecursiveColumn(null, resultTableField_ByUserID);
                    //}
                    return new Response<List<TableFieldModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, resultTableField_ByUserID.ToList());
                }

                if (resultTableFieldDefault != null && resultTableFieldDefault.Count() > 0)
                {
                    var resultCreate = await BulkCreate(resultTableFieldDefault.ToList(), userID);
                    if (resultCreate != null && resultCreate.Status == Constant.SUCCESS)
                    {
                        resultTableField_ByUserID = await _dapperUnitOfWork.GetRepository().QueryAsync<TableFieldModel>(sqlQueryTableField_ByUserID, new { @LayoutCode = layoutCode, @UserID = userID });
                        //if (layoutCode == "Report_Staff")
                        //{
                        //    resultTableField_ByUserID = RecursiveColumn(null, resultTableField_ByUserID);
                        //}
                        return new Response<List<TableFieldModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, resultTableField_ByUserID.ToList());
                    }

                }
                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Response<bool>> Update(List<TableField> models)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(TableField), "Update_TableField");
                var objectParams = new object[models.Count];
                for (var index = 0; index < models.Count; index++)
                {
                    var model = models[index];
                    objectParams[index] = new
                    {
                        @SortOrder = model.sortOrder,
                        @Hide = model.hide,
                        @Width = model.width,
                        @ID = model.ID
                    };
                }
                var result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, objectParams);
                return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
