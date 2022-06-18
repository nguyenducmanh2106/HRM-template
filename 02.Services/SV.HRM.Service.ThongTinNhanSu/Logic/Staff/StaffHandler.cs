using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NLog;
using SV.HRM.Caching.Interface;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Impl;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class StaffHandler : IStaffHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly ICached _cached;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseHandler _baseHandler;

        public StaffHandler(IMapper mapper, ICached cached, IDapperUnitOfWork dapperUnitOfWork,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IBaseHandler baseHandler
            )
        {
            _mapper = mapper;
            _cached = cached;
            _dapperUnitOfWork = dapperUnitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _baseHandler = baseHandler;
        }

        public List<OrganizationModel> GetListApplication()
        {
            IDbConnection connection = null;
            try
            {
                string connectionString = _configuration.GetValue<string>(Constant.ConnectionString.SYSTEM_CONNECTION_STRING);
                connection = new SqlConnection(connectionString);
                //connection.Open();
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();
                    var ret = dal.Query<OrganizationModel>("SELECT OrganizationID,OrganizationName,ParentOrganizationId FROM Organizations WHERE Status = @status;", new { status = Constant.StatusRecord.ACTIVE });
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

        public async Task<Response<int>> CreateStaffGeneralInfo(StaffCreateRequestModel model)
        {
            using (var trans = _dapperUnitOfWork.GetRepository().GetDbConnection().BeginTransaction())
            {
                try
                {
                    var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                    #region Check dữ liệu (Hiện đang chuyển qua validate trên form)
                    //string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "CheckDuplicate");
                    //sqlQuery = string.Format(sqlQuery, $"StaffCode = '{model.StaffCode.Trim()}'");
                    //int staffID = _dapperUnitOfWork.GetRepository().Query<int>(sqlQuery, null, trans).FirstOrDefault();
                    //if (staffID > 0)
                    //    return new Response<int>(Constant.ErrorCode.DUPPLICATE_CODE, string.Format(Constant.ErrorCode.DUPPLICATE_CODE_MESS, Constant.TableInfo.Staff.TABLE_NAME));

                    ////Sau bổ sung điểu kiện check khác...
                    #endregion

                    #region Tạo Account
                    AccountModel account = new AccountModel
                    {
                        UserName = model.StaffCode?.Trim(),
                        FullName = $"{model.FirstName} {model.MidName} {model.LastName}",
                        Password = Constant.PASSWORD_DEFAULT,
                        Email = model.PersonalEmail,
                        EthnicID = model.EthnicID ?? default,
                        Mobile = model.Mobiphone,
                        CreatedByUserId = user.UserId,
                        CreatedOnDate = DateTime.Now
                    };

                    var accountCreated = CreateAccount(account);
                    if (accountCreated is null) throw new Exception("Không thể tạo tài khoản");
                    #endregion

                    #region Lưu thông tin nhân viên & danh sách giấy tờ 
                    string licenceJson = JsonSerializer.Serialize(new { StaffLicense = model.StaffLicense });

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@UserId", accountCreated.UserId);
                    param.Add("@AvatarUrl", model.AvatarUrl);
                    param.Add("@FirstName", model.FirstName);
                    param.Add("@MidName", model.MidName);
                    param.Add("@LastName", model.LastName);
                    param.Add("@StaffCode", model.StaffCode?.Trim());
                    param.Add("@ExtraNumber1", model.ExtraNumber1);
                    param.Add("@JoiningDate", model.JoiningDate);
                    param.Add("@LeavingDate", model.LeavingDate);
                    param.Add("@FullName", model.FullName);
                    param.Add("@TenGoiKhac", model.TenGoiKhac);
                    param.Add("@BirthDay", model.Birthday);
                    param.Add("@Gender", model.Gender);
                    param.Add("@TitleID", model.TitleID);
                    param.Add("@MaritalStatus", model.MaritalStatus);
                    param.Add("@TerritoryID", model.TerritoryID);
                    param.Add("@EthnicID", model.EthnicID);
                    param.Add("@ReligionID", model.ReligionID);
                    param.Add("@Telephone", model.Telephone);
                    param.Add("@Mobiphone", model.Mobiphone);
                    param.Add("@PersonalEmail", model.PersonalEmail);
                    param.Add("@License", licenceJson);
                    param.Add("@ExtraNumber3", model.ExtraNumber3);
                    param.Add("@QualificationID", model.QualificationID);
                    param.Add("@DegreeID", model.DegreeID);
                    param.Add("@AcademicRankID", model.AcademicRankID);
                    param.Add("@ExtraNumber2", model.ExtraNumber2);
                    param.Add("@StaffGroupID", model.StaffGroupID);
                    param.Add("@SalaryStatus", model.SalaryStatus);
                    param.Add("@XuatThan", model.XuatThan);
                    param.Add("@IntroduceBy", model.IntroducedBy);
                    param.Add("@Emergency", model.Emergency);
                    param.Add("@QueQuanLocationID", model.QueQuanLocationID);
                    param.Add("@QueQuanDistrictID", model.QueQuanDistrictID);
                    param.Add("@QueQuanWardID", model.QueQuanWardID);
                    param.Add("@queQuanAddr", model.QueQuanAddr);
                    param.Add("@queQuan", model.QueQuan);
                    param.Add("@PermanentLocationID", model.PermanentLocationID);
                    param.Add("@PermanentDistrictID", model.PermanentDistrictID);
                    param.Add("@PermanentWardID", model.PermanentWardID);
                    param.Add("@HoKhauThuongTru", model.HoKhauThuongTru);
                    param.Add("@PermanentAddr", model.PermanentAddr);
                    param.Add("@ContactLocationID", model.ContactLocationID);
                    param.Add("@ContactAddrDistrictID", model.ContactAddrDistrictID);
                    param.Add("@ContactAddrWardID", model.ContactAddrWardID);
                    param.Add("@ContactAddr", model.ContactAddr);
                    param.Add("@NoiOHienNay", model.NoiOHienNay);
                    param.Add("@BirthPlaceID", model.BirthPlaceID);
                    param.Add("@BirthPlaceDistrictID", model.BirthPlaceDistrictID);
                    param.Add("@BirthPlaceWardID", model.BirthPlaceWardID);
                    param.Add("@BirthPlaceAddr", model.BirthPlaceAddr);
                    param.Add("@BirthPlace", model.BirthPlace);
                    param.Add("@OccupationID", model.OccupationID);
                    //Thêm code của tab thông tin khác
                    param.Add("@PITCode", model.PITCode);
                    param.Add("@ExtraNumber5", model.ExtraNumber5);
                    param.Add("@LabourBook", model.LabourBook);
                    param.Add("@InsuranceNo", model.InsuranceNo);
                    param.Add("@InsuranceIssuePlaceID", model.InsuranceIssuePlaceID);
                    param.Add("@InsuranceIssueDate", model.InsuranceIssueDate);
                    param.Add("@HospitalID", model.HospitalID);
                    param.Add("@NgheNghiepTruocTuyenDung", model.NgheNghiepTruocTuyenDung);
                    param.Add("@CongViecLamLauNhat", model.CongViecLamLauNhat);
                    param.Add("@DriverLicenseNo", model.DriverLicenseNo);
                    param.Add("@DriverLicenseIssuePlaceID", model.DriverLicenseIssuePlaceID);
                    param.Add("@DriverLicenseIssueDate", model.DriverLicenseIssueDate);
                    param.Add("@DanhHieuDuocPhong", model.DanhHieuDuocPhong);
                    param.Add("@Hobby", model.Hobby);
                    param.Add("@DacDiemLichSuBanThan1", model.DacDiemLichSuBanThan1);
                    param.Add("@NgayThamGiaToChucChinhTriXaHoi", model.NgayThamGiaToChucChinhTriXaHoi);
                    param.Add("@NgayVaoDoan", model.NgayVaoDoan);
                    param.Add("@GhiChu", model.GhiChu);
                    param.Add("@DeptID", model.DeptID);
                    //
                    param.Add("@HangThuongBinh", model.HangThuongBinh);
                    param.Add("@GiaDinhChinhSach", model.GiaDinhChinhSach);
                    param.Add("@Vietinbank", model.Vietinbank);
                    param.Add("@Vietcombank", model.Vietcombank);
                    param.Add("@CreatedBy", user?.UserId ?? 0);
                    param.Add("@CreationDate", DateTime.Now);
                    param.Add("@StaffID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    param.Add("@DataResult", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                    param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "sp_Staff_GeneralInfo");
                    await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, trans, CommandType.StoredProcedure);
                    int staffID = param.Get<int>("@StaffID");
                    string dataResult = param.Get<string>("@DataResult");
                    int outputResult = param.Get<int>("@OutputResult");
                    #endregion

                    #region Lưu file attach
                    List<StaffLicenseModel> staffLicenses = dataResult != null ? JsonSerializer.Deserialize<List<StaffLicenseModel>>(dataResult) : new List<StaffLicenseModel>();
                    List<HRM_AttachmentModel> licenseAttachment = new List<HRM_AttachmentModel>();

                    foreach (var item in staffLicenses)
                    {
                        switch (item.Type)
                        {
                            case Constant.Licenses.CMND:
                                model.LicenseAttachmentCMND.ForEach(file => file.TypeId = item.StaffLicenseID);
                                licenseAttachment.AddRange(model.LicenseAttachmentCMND);
                                break;
                            case Constant.Licenses.VISA:
                                model.LicenseAttachmentVISA.ForEach(file => file.TypeId = item.StaffLicenseID);
                                licenseAttachment.AddRange(model.LicenseAttachmentVISA);
                                break;
                            case Constant.Licenses.HOCHIEU:
                                model.LicenseAttachmentHC.ForEach(file => file.TypeId = item.StaffLicenseID);
                                licenseAttachment.AddRange(model.LicenseAttachmentHC);
                                break;
                            case Constant.Licenses.GPLD:
                                model.LicenseAttachmentGPLD.ForEach(file => file.TypeId = item.StaffLicenseID);
                                licenseAttachment.AddRange(model.LicenseAttachmentGPLD);
                                break;
                            case Constant.Licenses.CCHN:
                                model.LicenseAttachmentCCHN.ForEach(file => file.TypeId = item.StaffLicenseID);
                                licenseAttachment.AddRange(model.LicenseAttachmentCCHN);
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion
                    string licenseAttachmentJson = JsonSerializer.Serialize(new { HRM_Attachment = licenseAttachment });
                    param = new DynamicParameters();
                    param.Add("@LicenseAttachmentJson", licenseAttachmentJson);
                    param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "sp_Staff_GeneralInfo_AttachFile");
                    await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, trans, CommandType.StoredProcedure);
                    outputResult = param.Get<int>("@OutputResult");
                    trans.Commit();

                    if (outputResult > Constant.NODATA)
                    {
                        return new Response<int>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, staffID);
                    }
                    else
                    {
                        return new Response<int>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    logger.Error($"[ERROR]: {ex}");
                    return new Response<int>(Constant.ErrorCode.FAIL_CODE, ex.Message);
                }
            }
        }

        public async Task<Response<StaffDetailModel>> GetStaffOrtherInfoById(int id)
        {
            try
            {
                var modelDetail = new StaffDetailModel();
                var param = new { @StaffId = id };

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "sp_Staff_OrtherInfo_GetById");
                var objReturn = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, param, null, CommandType.StoredProcedure,
                        gr => modelDetail = gr.Read<StaffDetailModel>().FirstOrDefault());

                if (objReturn != null)
                    return new Response<StaffDetailModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, modelDetail);
                else
                    return new Response<StaffDetailModel>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, null);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffDetailModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<StaffDetailModel>> GetStaffGeneralInfoById(int id)
        {
            try
            {
                StaffDetailModel modelDetail;
                var listAttachment = new List<HRM_AttachmentModel>();
                var param = new { @StaffId = id };

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "sp_Staff_GeneralInfo_GetById");
                var objReturn = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, param, null, CommandType.StoredProcedure,
                        gr => gr.Read<StaffDetailModel>().FirstOrDefault(),
                        gr => gr.Read<StaffLicenseModel>().ToList(),
                        gr => gr.Read<HRM_AttachmentModel>().ToList());

                if (objReturn != null)
                {
                    modelDetail = objReturn[0] as StaffDetailModel ?? new StaffDetailModel();
                    modelDetail.StaffLicense = objReturn[1] as List<StaffLicenseModel>;
                    listAttachment = objReturn[2] as List<HRM_AttachmentModel>;

                    modelDetail.StaffLicense.ForEach(item =>
                    {
                        item.IssuePlaceName = _baseHandler.GetLocationsById(item.IssuePlaceID ?? 0)?.LocationName;
                        item.LicenseAttachment = listAttachment.Where(r => r.TypeId.Equals(item.StaffLicenseID)).ToList();
                    });

                    var listEthnic = _baseHandler.GetEthnicsById(modelDetail.EthnicID ?? 0);
                    modelDetail.EthnicName = listEthnic?.EthnicName;
                    modelDetail.TerritoryName = _baseHandler.GetCountriesById(modelDetail.TerritoryID ?? 0)?.CountryName;
                    modelDetail.ContactLocationName = _baseHandler.GetLocationsById(modelDetail.ContactLocationID ?? 0)?.LocationName;
                    modelDetail.ContactAddrDistrictName = _baseHandler.GetDistrictsById(modelDetail.ContactAddrDistrictID ?? 0)?.DistrictName;
                    modelDetail.ContactAddrWardName = _baseHandler.GetWardsById(modelDetail.ContactAddrWardID ?? 0)?.WardName;

                    modelDetail.PermanentLocationName = _baseHandler.GetLocationsById(modelDetail.PermanentLocationID ?? 0)?.LocationName;
                    modelDetail.PermanentDistrictName = _baseHandler.GetDistrictsById(modelDetail.PermanentDistrictID ?? 0)?.DistrictName;
                    modelDetail.PermanentWardName = _baseHandler.GetWardsById(modelDetail.PermanentWardID ?? 0)?.WardName;

                    modelDetail.QueQuanLocationName = _baseHandler.GetLocationsById(modelDetail.QueQuanLocationID ?? 0)?.LocationName;
                    modelDetail.QueQuanDistrictName = _baseHandler.GetDistrictsById(modelDetail.QueQuanDistrictID ?? 0)?.DistrictName;
                    modelDetail.QueQuanWardName = _baseHandler.GetWardsById(modelDetail.QueQuanWardID ?? 0)?.WardName;

                    modelDetail.BirthPlaceName = _baseHandler.GetLocationsById(modelDetail.BirthPlaceID ?? 0)?.LocationName;
                    modelDetail.BirthPlaceDistrictName = _baseHandler.GetDistrictsById(modelDetail.BirthPlaceDistrictID ?? 0)?.DistrictName;
                    modelDetail.BirthPlaceWardName = _baseHandler.GetWardsById(modelDetail.BirthPlaceWardID ?? 0)?.WardName;

                    //thông tin khác
                    modelDetail.ExtraNumber5Name = _baseHandler.GetLocationsById(Convert.ToInt32(modelDetail.ExtraNumber5))?.LocationName;
                    modelDetail.InsuranceIssuePlaceName = _baseHandler.GetLocationsById(modelDetail.InsuranceIssuePlaceID ?? 0)?.LocationName;
                    modelDetail.DriverLicenseIssuePlaceName = _baseHandler.GetLocationsById(modelDetail.DriverLicenseIssuePlaceID ?? 0)?.LocationName;

                    //lấy thông tin cơ cấu tổ chức của nhân viên
                    modelDetail.OrganizationName = _baseHandler.GetOrganization().SingleOrDefault(g => g.OrganizationId == modelDetail.DeptID)?.OrganizationName;

                    return new Response<StaffDetailModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, modelDetail);
                }
                else
                {
                    return new Response<StaffDetailModel>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffDetailModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<bool>> UpdateStaffGeneralInfo(StaffUpdateRequestModel model)
        {
            using (var trans = _dapperUnitOfWork.GetRepository().GetDbConnection().BeginTransaction())
            {
                try
                {
                    var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                    #region Lưu thông tin nhân viên & danh sách giấy tờ 
                    string licenceJson = JsonSerializer.Serialize(new { StaffLicense = model.StaffLicense });

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@StaffID", model.StaffID);
                    param.Add("@AvatarUrl", model.AvatarUrl);
                    param.Add("@FirstName", model.FirstName);
                    param.Add("@MidName", model.MidName);
                    param.Add("@LastName", model.LastName);
                    //param.Add("@StaffCode", model.StaffCode.Trim());
                    param.Add("@ExtraNumber1", model.ExtraNumber1);
                    param.Add("@JoiningDate", model.JoiningDate);
                    param.Add("@LeavingDate", model.LeavingDate);
                    param.Add("@FullName", model.FullName);
                    param.Add("@TenGoiKhac", model.TenGoiKhac);
                    param.Add("@BirthDay", model.Birthday);
                    param.Add("@Gender", model.Gender);
                    param.Add("@TitleID", model.TitleID);
                    param.Add("@MaritalStatus", model.MaritalStatus);
                    param.Add("@TerritoryID", model.TerritoryID);
                    param.Add("@EthnicID", model.EthnicID);
                    param.Add("@ReligionID", model.ReligionID);
                    param.Add("@Telephone", model.Telephone);
                    param.Add("@Mobiphone", model.Mobiphone);
                    param.Add("@PersonalEmail", model.PersonalEmail);
                    param.Add("@License", licenceJson);
                    param.Add("@ExtraNumber3", model.ExtraNumber3);
                    param.Add("@QualificationID", model.QualificationID);
                    param.Add("@DegreeID", model.DegreeID);
                    param.Add("@AcademicRankID", model.AcademicRankID);
                    param.Add("@ExtraNumber2", model.ExtraNumber2);
                    param.Add("@StaffGroupID", model.StaffGroupID);
                    param.Add("@SalaryStatus", model.SalaryStatus);
                    param.Add("@XuatThan", model.XuatThan);
                    param.Add("@IntroduceBy", model.IntroducedBy);
                    param.Add("@Emergency", model.Emergency);
                    param.Add("@QueQuanLocationID", model.QueQuanLocationID);
                    param.Add("@QueQuanDistrictID", model.QueQuanDistrictID);
                    param.Add("@QueQuanWardID", model.QueQuanWardID);
                    param.Add("@queQuanAddr", model.QueQuanAddr);
                    param.Add("@queQuan", model.QueQuan);
                    param.Add("@PermanentLocationID", model.PermanentLocationID);
                    param.Add("@PermanentDistrictID", model.PermanentDistrictID);
                    param.Add("@PermanentWardID", model.PermanentWardID);
                    param.Add("@HoKhauThuongTru", model.HoKhauThuongTru);
                    param.Add("@PermanentAddr", model.PermanentAddr);
                    param.Add("@ContactLocationID", model.ContactLocationID);
                    param.Add("@ContactAddrDistrictID", model.ContactAddrDistrictID);
                    param.Add("@ContactAddrWardID", model.ContactAddrWardID);
                    param.Add("@ContactAddr", model.ContactAddr);
                    param.Add("@NoiOHienNay", model.NoiOHienNay);
                    param.Add("@BirthPlaceID", model.BirthPlaceID);
                    param.Add("@BirthPlaceDistrictID", model.BirthPlaceDistrictID);
                    param.Add("@BirthPlaceWardID", model.BirthPlaceWardID);
                    param.Add("@BirthPlaceAddr", model.BirthPlaceAddr);
                    param.Add("@BirthPlace", model.BirthPlace);
                    param.Add("@PITCode", model.PITCode);
                    param.Add("@ExtraNumber5", model.ExtraNumber5);
                    param.Add("@LabourBook", model.LabourBook);
                    param.Add("@InsuranceNo", model.InsuranceNo);
                    param.Add("@InsuranceIssuePlaceID", model.InsuranceIssuePlaceID);
                    param.Add("@InsuranceIssueDate", model.InsuranceIssueDate);
                    param.Add("@HospitalID", model.HospitalID);
                    param.Add("@NgheNghiepTruocTuyenDung", model.NgheNghiepTruocTuyenDung);
                    param.Add("@CongViecLamLauNhat", model.CongViecLamLauNhat);
                    param.Add("@DriverLicenseNo", model.DriverLicenseNo);
                    param.Add("@DriverLicenseIssuePlaceID", model.DriverLicenseIssuePlaceID);
                    param.Add("@DriverLicenseIssueDate", model.DriverLicenseIssueDate);
                    param.Add("@DanhHieuDuocPhong", model.DanhHieuDuocPhong);
                    param.Add("@Hobby", model.Hobby);
                    param.Add("@DacDiemLichSuBanThan1", model.DacDiemLichSuBanThan1);
                    param.Add("@NgayThamGiaToChucChinhTriXaHoi", model.NgayThamGiaToChucChinhTriXaHoi);
                    param.Add("@NgayVaoDoan", model.NgayVaoDoan);
                    param.Add("@GhiChu", model.GhiChu);
                    param.Add("@OccupationID", model.OccupationID);
                    param.Add("@HangThuongBinh", model.HangThuongBinh);
                    param.Add("@GiaDinhChinhSach", model.GiaDinhChinhSach);
                    param.Add("@Vietinbank", model.Vietinbank);
                    param.Add("@Vietcombank", model.Vietcombank);
                    param.Add("@LastUpdatedBy", user?.UserId ?? 0);
                    param.Add("@LastUpdatedDate", DateTime.Now);
                    param.Add("@DataResult", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                    param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "sp_Staff_GeneralInfo_Update");
                    await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, trans, CommandType.StoredProcedure);
                    string dataResult = param.Get<string>("@DataResult");
                    int outputResult = param.Get<int>("@OutputResult");
                    #endregion

                    #region Lưu file attach
                    List<StaffLicenseModel> staffLicenses = dataResult != null ? JsonSerializer.Deserialize<List<StaffLicenseModel>>(dataResult) : new List<StaffLicenseModel>();
                    List<HRM_AttachmentModel> licenseAttachment = new List<HRM_AttachmentModel>();

                    foreach (var item in staffLicenses)
                    {
                        switch (item.Type)
                        {
                            case Constant.Licenses.CMND:
                                model.LicenseAttachmentCMND.ForEach(file => file.TypeId = item.StaffLicenseID);
                                licenseAttachment.AddRange(model.LicenseAttachmentCMND);
                                break;
                            case Constant.Licenses.VISA:
                                model.LicenseAttachmentVISA.ForEach(file => file.TypeId = item.StaffLicenseID);
                                licenseAttachment.AddRange(model.LicenseAttachmentVISA);
                                break;
                            case Constant.Licenses.HOCHIEU:
                                model.LicenseAttachmentHC.ForEach(file => file.TypeId = item.StaffLicenseID);
                                licenseAttachment.AddRange(model.LicenseAttachmentHC);
                                break;
                            case Constant.Licenses.GPLD:
                                model.LicenseAttachmentGPLD.ForEach(file => file.TypeId = item.StaffLicenseID);
                                licenseAttachment.AddRange(model.LicenseAttachmentGPLD);
                                break;
                            case Constant.Licenses.CCHN:
                                model.LicenseAttachmentCCHN.ForEach(file => file.TypeId = item.StaffLicenseID);
                                licenseAttachment.AddRange(model.LicenseAttachmentCCHN);
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion
                    string licenseAttachmentJson = JsonSerializer.Serialize(new { HRM_Attachment = licenseAttachment });
                    param = new DynamicParameters();
                    param.Add("@LicenseAttachmentJson", licenseAttachmentJson);
                    param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "sp_Staff_GeneralInfo_AttachFile");
                    await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, trans, CommandType.StoredProcedure);
                    outputResult = param.Get<int>("@OutputResult");
                    trans.Commit();

                    if (outputResult > Constant.NODATA)
                    {
                        return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                    }
                    else
                    {
                        return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    logger.Error($"[ERROR]: {ex}");
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
        }

        public async Task<Response<int>> CreateOrUpdateStaffOrtherInfo(StaffCreateRequestModel model)
        {
            try
            {
                var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                #region Check duplicate dữ liệu(Hiện tại đang check luôn trên form)
                //string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "CheckDuplicate");
                ////MST cá nhân
                //string condition = $"PITCode = '{model.PITCode?.Trim()}' AND staffID!={model.StaffID}";
                //int countRow = _dapperUnitOfWork.GetRepository().Query<int>(string.Format(sqlQuery, condition), null, null).FirstOrDefault();
                //if (countRow > 0)
                //    return new Response<int>(Constant.ErrorCode.DUPPLICATE_CODE, "Mã số thuế cá nhân đã tồn tại trên hệ thống.");

                ////Mã hộ gia đình
                //condition = $"LabourBook = '{model.LabourBook?.Trim()}' AND staffID!={model.StaffID}";
                //countRow = _dapperUnitOfWork.GetRepository().Query<int>(string.Format(sqlQuery, condition), null, null).FirstOrDefault();
                //if (countRow > 0)
                //    return new Response<int>(Constant.ErrorCode.DUPPLICATE_CODE, "Mã hộ gia đình đã tồn tại trên hệ thống.");

                ////Mã bảo hiểm xã hội
                //condition = $"InsuranceNo = '{model.InsuranceNo?.Trim()}' AND staffID!={model.StaffID}";
                //countRow = _dapperUnitOfWork.GetRepository().Query<int>(string.Format(sqlQuery, condition), null, null).FirstOrDefault();
                //if (countRow > 0)
                //    return new Response<int>(Constant.ErrorCode.DUPPLICATE_CODE, "Mã bảo hiểm xã hội đã tồn tại trên hệ thống.");

                ////Mã GPLX
                //condition = $"DriverLicenseNo = '{model.DriverLicenseNo?.Trim()}' AND staffID!={model.StaffID}";
                //countRow = _dapperUnitOfWork.GetRepository().Query<int>(string.Format(sqlQuery, condition), null, null).FirstOrDefault();
                //if (countRow > 0)
                //    return new Response<int>(Constant.ErrorCode.DUPPLICATE_CODE, "Mã giấy phép lái xe đã tồn tại trên hệ thống.");
                #endregion

                #region Lưu thông tin khác của nhân viên
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@PITCode", model.PITCode);
                param.Add("@ExtraText5", model.ExtraText5);
                param.Add("@LabourBook", model.LabourBook);
                param.Add("@InsuranceNo", model.InsuranceNo);
                param.Add("@InsuranceIssuePlaceID", model.InsuranceIssuePlaceID);
                param.Add("@InsuranceIssueDate", model.InsuranceIssueDate);
                param.Add("@HospitalID", model.HospitalID);
                param.Add("@NgheNghiepTruocTuyenDung", model.NgheNghiepTruocTuyenDung);
                param.Add("@CongViecLamLauNhat", model.CongViecLamLauNhat);
                param.Add("@DriverLicenseNo", model.DriverLicenseNo);
                param.Add("@DriverLicenseIssuePlaceID", model.DriverLicenseIssuePlaceID);
                param.Add("@DriverLicenseIssueDate", model.DriverLicenseIssueDate);
                param.Add("@DanhHieuDuocPhong", model.DanhHieuDuocPhong);
                param.Add("@Hobby", model.Hobby);
                param.Add("@DacDiemLichSuBanThan1", model.DacDiemLichSuBanThan1);
                param.Add("@NgayThamGiaToChucChinhTriXaHoi", model.NgayThamGiaToChucChinhTriXaHoi);
                param.Add("@NgayVaoDoan", model.NgayVaoDoan);
                param.Add("@GhiChu", model.GhiChu);
                param.Add("@LastUpdatedBy", user.UserId);
                param.Add("@LastUpdatedDate", DateTime.Now);
                param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "sp_Staff_OrtherInfoCreateOrUpdate");
                await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);
                #endregion

                int outputResult = param.Get<int>("@OutputResult");
                if (outputResult > Constant.NODATA)
                {
                    return new Response<int>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, model.StaffID);
                }
                else
                {
                    return new Response<int>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<int>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        /// <summary>
        /// Xóa bản ghi của StaffLiciense theo ID của Staff
        /// </summary>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyStaffLiciense(List<int> recordID)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Staff.json", nameof(Staff), "DeleteManyStaffLiciense");
                int result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, new { @recordIDs = recordID });
                if (result > 0)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        public int GetStaffIDByStaffCode(string staffCode)
        {
            using (var trans = _dapperUnitOfWork.GetRepository().GetDbConnection().BeginTransaction())
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "CheckDuplicate");
                sqlQuery = string.Format(sqlQuery, $"StaffCode = N'{staffCode}'");
                int staffID = _dapperUnitOfWork.GetRepository().Query<int>(sqlQuery, null, trans).FirstOrDefault();
                return staffID;
            }
        }

        public async Task<Response<int>> GetStaffIDByAccountID(int userID)
        {
            using (var trans = _dapperUnitOfWork.GetRepository().GetDbConnection().BeginTransaction())
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "GetStaffIDByAccount");
                sqlQuery = string.Format(sqlQuery, userID);
                int staffID = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, null, trans, CommandType.Text);
                return new Response<int>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, staffID);
            }
        }

        public Account CreateAccount(AccountModel model)
        {
            var currenUser = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
            string apiUrl = StaticVariable.QTHT_API_URL + "User/CreateAccount";
            var header = new List<KeyValuePair<string, string>>
            {
                    new KeyValuePair<string, string>(Constant.RequestHeader.AUTHORIZATION, currenUser.AccessToken)
            };

            var res = RestsharpUtils.Post<Response<Account>>(apiUrl, header, model);
            if (res != null)
            {
                if (res.Status.Equals(Constant.SUCCESS))
                    return res.Data;
                else
                    throw new Exception(res.Message);
            }

            return default;
        }

        /// <summary>
        /// Xóa tất cả bản ghi trong HRM_Attachment bao gồm cả các file thuộc các tab của nhân viên theo ID của Staff
        /// </summary>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteAll(List<int> recordID)
        {
            using (var trans = _dapperUnitOfWork.GetRepository().GetDbConnection().BeginTransaction())
            {
                try
                {
                    var objectParams = new object[recordID.Count];
                    for (var index = 0; index < recordID.Count; index++)
                    {
                        var model = recordID[index];
                        objectParams[index] = new
                        {
                            @StaffID = model
                        };
                    }

                    string sqlQueryGetStaffJustDelete = JSONObject.GetQueryFromJSON($"SqlCommand/Staff.json", nameof(Staff), "GetStaffJustDelete");
                    var resultGetStaffJustDelete = await _dapperUnitOfWork.GetRepository().QueryAsync<int>(sqlQueryGetStaffJustDelete, new { recordIDs = recordID }, trans, CommandType.Text);

                    string sqlQueryDeleteStaff = JSONObject.GetQueryFromJSON($"SqlCommand/Staff.json", nameof(Staff), "DeleteMany");
                    var resultDeleteStaff = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQueryDeleteStaff, new { recordIDs = recordID }, trans, CommandType.Text);

                    if (resultDeleteStaff > 0)
                    {
                        // xóa tài khoản bên quản trị hệ thống
                        var isDeleteSuccess = DeleteAccount(resultGetStaffJustDelete?.ToList());
                        if (isDeleteSuccess == false) throw new Exception("Xóa tài khoản thất bại");
                        else
                        {
                            trans.Commit();
                            return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                        }

                    }

                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    logger.Error($"[ERROR]: {ex}");
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
        }

        public bool DeleteAccount(List<int> recordIDs)
        {
            try
            {
                var currenUser = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
                string apiUrl = StaticVariable.QTHT_API_URL + "User/DeleteMany";
                var header = new List<KeyValuePair<string, string>>
            {
                    new KeyValuePair<string, string>(Constant.RequestHeader.AUTHORIZATION, currenUser.AccessToken)
            };

                var res = RestsharpUtils.Post<Response<bool>>(apiUrl, header, recordIDs);
                if (res != null)
                {
                    if (res.Status.Equals(Constant.SUCCESS))
                        return res.Data;
                    else
                        throw new Exception(res.Message);
                }

                return default;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return default;
            }

        }

        public async Task<Response<int>> GenerateAccount()
        {
            try
            {
                var result = 0;
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "GetAllStaffNoUserId");
                var lstStaff = (await _dapperUnitOfWork.GetRepository().QueryAsync<StaffModel>(sqlQuery, null, null, CommandType.Text)).ToList();
                if (lstStaff.Count > 0)
                {
                    foreach (var staff in lstStaff)
                    {
                        if (staff.StaffID > 0)
                        {
                            AccountModel account = new AccountModel
                            {
                                UserName = Utilities.removeVietnameseSign(staff.StaffCode?.Trim()),
                                FullName = $"{staff.FirstName} {staff.MidName} {staff.LastName}",
                                FirstName = $"{staff.FirstName} {staff.MidName}",
                                LastName = staff.LastName,
                                Password = Constant.PASSWORD_DEFAULT,
                                Email = staff.PersonalEmail,
                                EthnicID = staff.EthnicID ?? default,
                                Mobile = staff.Mobiphone,
                                CreatedByUserId = 0,
                                CreatedOnDate = DateTime.Now
                            };

                            var accountCreated = GenerateAccount(account);
                            if (accountCreated != null)
                            {
                                DynamicParameters param = new DynamicParameters();
                                param.Add("@StaffID", staff.StaffID);
                                param.Add("@UserId", accountCreated.UserId);
                                param.Add("@LastUpdatedBy", 0);
                                param.Add("@LastUpdatedDate", DateTime.Now);
                                param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                                sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Staff.json", "Staff", "sp_Staff_Update_UserId");
                                await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);
                                int outputResult = param.Get<int>("@OutputResult");
                                if (outputResult > 0)
                                {
                                    result = result + 1;
                                }
                            }
                        }
                    }
                }
                return new Response<int>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<int>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, 0);
            }
        }

        public Account GenerateAccount(AccountModel model)
        {
            string apiUrl = StaticVariable.QTHT_API_URL + "User/GenerateAccount";
            var res = RestsharpUtils.Post<Response<Account>>(apiUrl, null, model);
            if (res != null)
            {
                if (res.Status.Equals(Constant.SUCCESS))
                    return res.Data;
                else
                    throw null;
            }

            return null;
        }
    }
}
