using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SV.HRM.Caching.Interface;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class LabourContractHandler : ILabourContractHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly ICached _cached;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseHandler _baseHandler;

        public LabourContractHandler(IMapper mapper, ICached cached, IDapperUnitOfWork dapperUnitOfWork,
            IHttpContextAccessor httpContextAccessor, IBaseHandler baseHandler
            )
        {
            _mapper = mapper;
            _cached = cached;
            _dapperUnitOfWork = dapperUnitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _baseHandler = baseHandler;
        }
        /// <summary>
        /// Tạo mới hợp đồng lao động của nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<Response<bool>> Create(LabourContractCreateModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(LabourContract)}.json", nameof(LabourContract), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID ?? 0);
                param.Add("@LabourContractNo", model.LabourContractNo);
                param.Add("@ContractDate", model.ContractDate);
                param.Add("@EmployerID", model.EmployerID);
                param.Add("@ParentLabourContractID", model.ParentLabourContractID);
                param.Add("@ContractTypeID", model.ContractTypeID);
                param.Add("@ContractFromDate", model.ContractFromDate);
                param.Add("@ContractToDate", model.ContractToDate);
                param.Add("@WorkingAddr", model.WorkingAddr);
                param.Add("@BranchID", model.BranchID);
                param.Add("@EmployeePositionID", model.EmployeePositionID);
                param.Add("@SalaryRate", model.SalaryRate);
                param.Add("@BasicSalary", model.BasicSalary);
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@InsuranceSalary", model.InsuranceSalary);
                param.Add("@PayrollType", model.PayrollType);
                param.Add("@ExtraText9", model.ExtraText9);
                param.Add("@ExtraNumber2", model.ExtraNumber2);
                param.Add("@Working", model.Working);
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreationDate", DateTime.Now);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult >= Constant.SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = Constant.FileType.HOP_DONG_LAO_DONG; u.TypeId = outputResult; });
                        await _baseHandler.InsertManyFile(model.FileUpload);
                    }
                    // Write log
                    _baseHandler.CreateLog(new SystemLogModel
                    {
                        ApplicationId = userModel.ApplicationId,
                        UserId = userModel.UserId,
                        ActionByUser = LogAction.ADD,
                        Date = DateTime.Now,
                        Content = $"Tạo mới bản ghi hợp đồng lao động {model.StaffID} - {model.LabourContractNo}"
                    });
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else //trường hợp còn lại cụ thể là store proc trả ra = 0 là trường hợp exception lỗi của SQL
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<LabourContractModel>> FindById(int recordID)
        {
            LabourContractModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(LabourContract)}.json", nameof(LabourContract), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<LabourContractModel>(sqlQuery, new { LabourContractID = recordID });
                if (result != null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.HOP_DONG_LAO_DONG, recordID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<LabourContractModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<LabourContractModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<LabourContractModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<List<LabourContractModel>>> GetComboboxParentLabourContractID(string layoutCode, string keySearch, int q)
        {
            List<LabourContractModel> lstStaffModel = new List<LabourContractModel>();
            int totalCount = 0;

            try
            {
                int? nullValue = null;
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(LabourContract)}.json", nameof(LabourContract), "GetComboboxParentLabourContractID");
                string renderQuery = String.Format(sqlQuery, keySearch);
                var param = new
                {
                    @StaffID = q,
                    @ParentLabourContractID = nullValue
                };
                var objectResult = _dapperUnitOfWork.GetRepository().QueryMultiple(renderQuery, param, null, CommandType.Text,
                    gr =>
                        gr.Read<LabourContractModel>(),
                    gr =>
                        gr.Read<Int32>().FirstOrDefault()
                    );

                if (objectResult != null && objectResult[0] != null && objectResult[1] != null)
                {
                    lstStaffModel = (List<LabourContractModel>)objectResult[0];
                    totalCount = (int)objectResult[1];
                    int totalPage = (int)Math.Ceiling((decimal)totalCount / 10);
                    return new Response<List<LabourContractModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, lstStaffModel, 10, totalCount, totalPage);
                }
                return new Response<List<LabourContractModel>>(Constant.ErrorCode.ERRORSYSTEM_CODE, Constant.ErrorCode.ERRORSYSTEM_MESS);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<LabourContractModel>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<bool>> Update(int id, LabourContractUpdateModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(LabourContract)}.json", nameof(LabourContract), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@LabourContractID", id);
                param.Add("@LabourContractNo", model.LabourContractNo);
                param.Add("@ContractDate", model.ContractDate);
                param.Add("@EmployerID", model.EmployerID);
                param.Add("@ParentLabourContractID", model.ParentLabourContractID);
                param.Add("@ContractTypeID", model.ContractTypeID);
                param.Add("@ContractFromDate", model.ContractFromDate);
                param.Add("@ContractToDate", model.ContractToDate);
                param.Add("@WorkingAddr", model.WorkingAddr);
                param.Add("@BranchID", model.BranchID);
                param.Add("@EmployeePositionID", model.EmployeePositionID);
                param.Add("@SalaryRate", model.SalaryRate);
                param.Add("@BasicSalary", model.BasicSalary);
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@InsuranceSalary", model.InsuranceSalary);
                param.Add("@PayrollType", model.PayrollType);
                param.Add("@ExtraText9", model.ExtraText9);
                param.Add("@ExtraNumber2", model.ExtraNumber2);
                param.Add("@Working", model.Working);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= Constant.SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(Constant.FileType.HOP_DONG_LAO_DONG, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == Constant.SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = Constant.FileType.HOP_DONG_LAO_DONG; u.TypeId = id; });
                            var resUpdateFile = await _baseHandler.InsertManyFile(model.FileUpload);
                            if (resUpdateFile != null && resUpdateFile.Status == Constant.SUCCESS && resUpdateFile.Data)
                            {
                                // Write log
                                _baseHandler.CreateLog(new SystemLogModel
                                {
                                    ApplicationId = userModel.ApplicationId,
                                    UserId = userModel.UserId,
                                    ActionByUser = LogAction.UPDATE,
                                    Date = DateTime.Now,
                                    Content = $"Cập nhật bản ghi hợp đồng lao động {model.StaffID} - {model.LabourContractNo}"
                                });
                                return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                            }
                        }
                    }
                    // Write log
                    _baseHandler.CreateLog(new SystemLogModel
                    {
                        ApplicationId = userModel.ApplicationId,
                        UserId = userModel.UserId,
                        ActionByUser = LogAction.UPDATE,
                        Date = DateTime.Now,
                        Content = $"Cập nhật bản ghi hợp đồng lao động {model.StaffID} - {model.LabourContractNo}"
                    });
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else //trường hợp còn lại cụ thể là store proc trả ra = 0 là trường hợp exception lỗi của SQL
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        public async Task<LabourContractModel> FindByStaffAndContractInfo(int staffId, string contractNo, int contractType, DateTime? contractFromDate, DateTime? contractToDate)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(LabourContract)}.json", nameof(LabourContract), "FindByStaffAndContractInfo");
                string renderQuery = String.Format(sqlQuery, staffId, contractNo);
                var labourContract = _dapperUnitOfWork.GetRepository().QuerySingleOrDefault<LabourContractModel>(renderQuery, null, null, CommandType.Text);
                if (labourContract != null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.HOP_DONG_LAO_DONG, labourContract.LabourContractID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        labourContract.FileUpload = resFile.Data;
                    }
                }
                return labourContract;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }
    }
}
