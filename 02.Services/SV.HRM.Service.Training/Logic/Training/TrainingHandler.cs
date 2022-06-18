

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SV.HRM.Caching.Interface;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Impl;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using NLog;

namespace SV.HRM.Service.Training
{
    public class TrainingHandler : ITrainingHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ICached _cached;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IBaseHandler _baseHandler;

        public TrainingHandler(ICached cached, IHttpContextAccessor httpContextAccessor, IDapperUnitOfWork dapperUnitOfWork, IConfiguration configuration,
            IBaseHandler baseHandler
            )
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _cached = cached;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _baseHandler = baseHandler;
        }
        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<QuanLyDaoTaoModel>> FindById(int recordID)
        {
            QuanLyDaoTaoModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Report_Training.json", "Report_Training", "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<QuanLyDaoTaoModel>(sqlQuery, new { QuanLyDaoTaoID = recordID });
                if (result != null)
                {
                    //lấy thông tin file
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.QUAN_LY_DAO_TAO, recordID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }

                    return new Response<QuanLyDaoTaoModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<QuanLyDaoTaoModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                throw ex;
                //return new Response<T>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);
            }
        }

        public List<OrganizationModel> GetListOrganization()
        {
            IDbConnection connection = null;
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                connection = new SqlConnection(connectionString);
                //connection.Open();
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();
                    var ret = dal.Query<OrganizationModel>("SELECT OrganizationID,OrganizationName,ParentOrganizationId FROM Organizations WHERE Status = @status;", new { status = StatusRecord.ACTIVE });
                    return ret.ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                connection.Close();
                return null;
            }
        }
    }
}
