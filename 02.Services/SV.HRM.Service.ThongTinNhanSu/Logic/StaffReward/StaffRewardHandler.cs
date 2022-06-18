using Dapper;
using Microsoft.Extensions.Logging;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class StaffRewardHandler : IStaffRewardHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;

        public StaffRewardHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler
            )
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
        }
        /// <summary>
        /// Tạo mới nhận thưởng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffRewardCreateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffReward)}.json", nameof(StaffReward), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@RewardType", model.RewardType);
                param.Add("@CompanyID", model.CompanyID);
                param.Add("@DeptID", model.DeptID);
                param.Add("@BranchID", model.BranchID);
                param.Add("@StaffID", model.StaffID);
                param.Add("@RewardDecisionNo", model.RewardDecisionNo);
                param.Add("@RewardDate", model.RewardDate);
                param.Add("@FromYear", model.FromYear);
                param.Add("@ToYear", model.ToYear);
                param.Add("@RewardID", model.RewardID);
                param.Add("@Note", model.Note);

                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreationDate", DateTime.Now);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= Constant.SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = Constant.FileType.KHEN_THUONG; u.TypeId = outputResult; });
                        await _baseHandler.InsertManyFile(model.FileUpload);
                    }
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
        public async Task<Response<StaffRewardModel>> FindById(int recordID)
        {
            StaffRewardModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffReward)}.json", nameof(StaffReward), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffRewardModel>(sqlQuery, new { StaffRewardID = recordID });
                if (result != null)
                {
                    //Lấy thông tin đơn vị/phòng ban
                    var lstApplication = _baseHandler.GetOrganization();
                    if(lstApplication != null)
                    {
                        result.DeptName = lstApplication.SingleOrDefault(g => g.OrganizationId == result.DeptID)?.OrganizationName ?? "";
                    }
                    //Lấy thông tin file đính kèm
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.KHEN_THUONG, recordID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<StaffRewardModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<StaffRewardModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffRewardModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Hàm cập nhật khen thưởng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffRewardUpdateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffReward)}.json", nameof(StaffReward), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffRewardID", id);
                param.Add("@RewardType", model.RewardType);
                param.Add("@CompanyID", model.CompanyID);
                param.Add("@DeptID", model.DeptID);
                param.Add("@BranchID", model.BranchID);
                param.Add("@StaffID", model.StaffID);
                param.Add("@RewardDecisionNo", model.RewardDecisionNo);
                param.Add("@RewardDate", model.RewardDate);
                param.Add("@FromYear", model.FromYear);
                param.Add("@ToYear", model.ToYear);
                param.Add("@RewardID", model.RewardID);
                param.Add("@Note", model.Note);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult == Constant.SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(Constant.FileType.KHEN_THUONG, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == Constant.SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = Constant.FileType.KHEN_THUONG; u.TypeId = id; });
                            var resUpdateFile = await _baseHandler.InsertManyFile(model.FileUpload);
                            if (resUpdateFile != null && resUpdateFile.Status == Constant.SUCCESS && resUpdateFile.Data)
                            {
                                return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                            }
                        }
                    }
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

        public async Task<StaffRewardModel> FindByDecisionNo(int rewardType, int objectId, string decisionNo)
        {
            try
            {
                var sqlQuery = string.Empty;
                if (rewardType.Equals(Constant.LoaiKhenThuong.NUM_KHEN_THUONG_CA_NHAN))
                {
                    sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffReward)}.json", nameof(StaffReward), "FindByStaffAndDecisionNo");
                }
                if (rewardType.Equals(Constant.LoaiKhenThuong.NUM_KHEN_THUONG_TAP_THE))
                {
                    sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffReward)}.json", nameof(StaffReward), "FindByDeptAndDecisionNo");
                }

                string renderQuery = String.Format(sqlQuery, objectId, decisionNo);
                var reward = _dapperUnitOfWork.GetRepository().QuerySingleOrDefault<StaffRewardModel>(renderQuery, null, null, CommandType.Text);
                if (reward != null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.KHEN_THUONG, reward.StaffRewardID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        reward.FileUpload = resFile.Data;
                    }
                }
                return reward;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }
        // diennv check date có trong quá trình công tác không
        public async Task<Response<bool>> CheckStaffRewardInHistory(int staffID,DateTime date)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffReward)}.json", nameof(StaffReward), "Check_StaffRewardInHistory");
                string renderQuery = String.Format(sqlQuery, staffID, date.ToString("yyyy-MM-dd"));
                var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<History>(renderQuery, null, null, CommandType.Text);
                if (result != null)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS,true);
                }else
                {
                    return new Response<bool>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, false);
                }
            }
            catch (Exception)
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }
    }
}
