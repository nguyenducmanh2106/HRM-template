using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class Staff : StaffBaseModel
    {
        public int? ApplicantID { get; set; }
        public byte[] StaffImg { get; set; }
        public string AvatarUrl { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int? Gender { get; set; }
        public int? MaritalStatus { get; set; }
        public DateTime? Birthday { get; set; }
        public int? BirthPlaceID { get; set; }
        public int? TitleID { get; set; }
        public int? EthnicID { get; set; }
        public string Religion { get; set; }
        public int? ReligionID { get; set; }
        public int? TerritoryID { get; set; }
        public string PermanentAddr { get; set; }
        public int? PermanentLocationID { get; set; }
        public string ContactAddr { get; set; }
        public int? ContactLocationID { get; set; }
        public string IDCardNo { get; set; }
        public int? IssuePlaceID { get; set; }
        public DateTime? IssueDate { get; set; }
        public string PassportNo { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public DateTime? PassportExpireDate { get; set; }
        public string Telephone { get; set; }
        public string Mobiphone { get; set; }
        public string PersonalEmail { get; set; }
        public string Emergency { get; set; }
        public string IntroducedBy { get; set; }
        public int? AcademicTitleID { get; set; }
        public int? AcademicRankID { get; set; }
        public int? DegreeID { get; set; }
        public bool? PartyMember { get; set; }
        public DateTime? LeavingDate { get; set; }
        public DateTime? JoiningDate { get; set; }
        public double? ProbationSalaryRate { get; set; }
        public DateTime? ProbationFromDate { get; set; }
        public DateTime? ProbationToDate { get; set; }
        public DateTime? ProbationScheduleToDate { get; set; }
        public string ProbationResult { get; set; }
        public DateTime? ApprenticeFromDate { get; set; }
        public DateTime? ApprenticeToDate { get; set; }
        public DateTime? ApprenticeScheduleToDate { get; set; }
        public string ApprenticeResult { get; set; }
        public double? ApprenticeSalaryRate { get; set; }
        public double? BasicSalary { get; set; }
        public double? InsuranceSalary { get; set; }
        public string SalaryType { get; set; }
        public string CurrencyID { get; set; }
        public int? BankID { get; set; }
        public string AccountNo { get; set; }
        public double? DayPerMonth { get; set; }
        public double? HourPerDay { get; set; }
        public double? StartAnnualLeave { get; set; }
        public double? CurrentAnnualLeave { get; set; }
        public bool? Expat { get; set; }
        public int? DeptID { get; set; }
        public int? PositionID { get; set; }
        public int? PositionGradeID { get; set; }
        public int? CostCenterID { get; set; }
        public int? CategoryID { get; set; }
        public int? JobTitleID { get; set; }
        public int? JobTitleGradeID { get; set; }
        public int? LocationWorkingID { get; set; }
        public int? WorkGroupID { get; set; }
        public string FaxOffice { get; set; }
        public string TelOffice { get; set; }
        public string CompanyEmail { get; set; }
        public int? PITTypeID { get; set; }
        public string PITCode { get; set; }
        public int? TaxAgencyID { get; set; }
        public string LabourBook { get; set; }
        public string InsuranceNo { get; set; }
        public int? InsuranceID { get; set; }
        public string SICode { get; set; }
        public string HICode { get; set; }
        public int? HospitalID { get; set; }
        public int? InsuranceAgencyID { get; set; }
        public string JobDes { get; set; }
        public string Hobby { get; set; }
        public string CVDes { get; set; }
        public string Note { get; set; }
        public int? ManagerID { get; set; }
        //public byte[] StaffImg { get; set; }
        public string ExtraText1 { get; set; }
        public string ExtraText2 { get; set; }
        public string ExtraText3 { get; set; }
        public string ExtraText4 { get; set; }
        public string ExtraText5 { get; set; }
        public string ExtraText6 { get; set; }
        public string ExtraText7 { get; set; }
        public string ExtraText8 { get; set; }
        public string ExtraText9 { get; set; }
        public string ExtraText10 { get; set; }
        public double? ExtraNumber1 { get; set; }
        public double? ExtraNumber2 { get; set; }
        public double? ExtraNumber3 { get; set; }
        public double? ExtraNumber4 { get; set; }
        public double? ExtraNumber5 { get; set; }
        public DateTime? ExtraDate1 { get; set; }
        public DateTime? ExtraDate2 { get; set; }
        public DateTime? ExtraDate3 { get; set; }
        public DateTime? ExtraDate4 { get; set; }
        public DateTime? ExtraDate5 { get; set; }
        public bool? ExtraLogic1 { get; set; }
        public bool? ExtraLogic2 { get; set; }
        public bool? ExtraLogic3 { get; set; }
        public bool? ExtraLogic4 { get; set; }
        public bool? ExtraLogic5 { get; set; }
        public string SysExtraText1 { get; set; }
        public string SysExtraText2 { get; set; }
        public string SysExtraText3 { get; set; }
        public string SysExtraText4 { get; set; }
        public string SysExtraText5 { get; set; }
        public string PassWord { get; set; }
        public DateTime? LabourBookIssueDate { get; set; }
        public int? LabourBookIssuePlaceID { get; set; }
        public DateTime? OfficialDate { get; set; }
        public int? SICodeAgencyID { get; set; }
        public DateTime? SICodeIssueDate { get; set; }
        public int? Status { get; set; }
        public string BankCurrencyID { get; set; }
        public DateTime? InsuranceIssueDate { get; set; }
        public int? InsuranceIssuePlaceID { get; set; }
        public string DriverLicenseNo { get; set; }
        public DateTime? DriverLicenseIssueDate { get; set; }
        public int? DriverLicenseIssuePlaceID { get; set; }
        public int? MilitaryObligation { get; set; }
        public bool? TUMember { get; set; }
        public int? SalaryStatus { get; set; }
        public double? SalaryRate { get; set; }
        public DateTime? PetitionDate { get; set; }
        public DateTime? HIDate { get; set; }
        public int? LateMonthInsurance { get; set; }
        public string School { get; set; }
        public string Diplomacy { get; set; }
        public string StaffAlias { get; set; }
        public int? ApprenticeGroup { get; set; }
        public int? SIFromMonth { get; set; }
        public int? SIFromYear { get; set; }
        public int? UIFromMonth { get; set; }
        public int? UIFromYear { get; set; }
        public string SINote { get; set; }
        public int? TUFromMonth { get; set; }
        public int? TUFromYear { get; set; }
        public string StatusFile { get; set; }
        public string TenGoiKhac { get; set; }
        public string GiaoDucPhoThong { get; set; }
        public string HoKhauThuongTru { get; set; }
        public int? PermanentDistrictID { get; set; }
        public int? PermanentWardID { get; set; }
        public string NoiOHienNay { get; set; }
        public int? ContactAddrDistrictID { get; set; }
        public int? ContactAddrWardID { get; set; }
        public string QueQuan { get; set; }
        public string QueQuanAddr { get; set; }
        public int? QueQuanLocationID { get; set; }
        public int? QueQuanDistrictID { get; set; }
        public int? QueQuanWardID { get; set; }
        public string CapUyHienTai { get; set; }
        public string CapUyKiem { get; set; }
        public string ChucVuDangDoanThe { get; set; }
        public string XuatThan { get; set; }
        public string NgheNghiepTruocTuyenDung { get; set; }
        public DateTime? NgayThamGiaCachMang { get; set; }
        public DateTime? NgayThamGiaToChucChinhTriXaHoi { get; set; }
        public string LyLuanChinhTri { get; set; }
        public string NgoaiNgu { get; set; }
        public string DanhHieuDuocPhong { get; set; }
        public string KhenThuong { get; set; }
        public string KyLuat { get; set; }
        public string DacDiemLichSuBanThan1 { get; set; }
        public string DacDiemLichSuBanThan2 { get; set; }
        public string GhiChu { get; set; }
        public int? BirthPlaceDistrictID { get; set; }
        public int? BirthPlaceWardID { get; set; }
        public string BirthPlace { get; set; }
        public string BirthPlaceAddr { get; set; }
        public string PhuCapChucVu { get; set; }
        public string ChucVuHienTai { get; set; }
        public DateTime? NgayVaoDoan { get; set; }
        public string CongViecLamLauNhat { get; set; }

        //ID cột nghề- map với bảng Occupation
        public int? OccupationID { get; set; }
        public int? HangThuongBinh { get; set; }
        public int? GiaDinhChinhSach { get; set; }
        public string Vietinbank { get; set; }
        public string Vietcombank { get; set; }
    }
    public class StaffBaseModel
    {
        public int StaffID { get; set; }
        public string StaffCode { get; set; }
    }

    public class StaffModel : Staff
    {
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }

        //Tên khoa phòng gốc
        public string OrganizationName { get; set; }

        //Tên khoa phòng tăng cường- nghiệp vụ đức giang

        public string OrganizationName1 { get; set; }
        public int? QualificationID { get; set; }
        public int? StaffGroupID { get; set; }

        //Từ ngày điều động tăng cường
        public DateTime? FromDateEnhancedManeuvering { get; set; }

        //Đến ngày điều động tăng cường
        public DateTime? TodateEnhancedManeuvering { get; set; }

        // id đơn vị/khoa phòng của điều động tăng cường
        public int? DeptIDEnhancedManeuvering { get; set; }
        // tên đơn vị/khoa phòng của điều động tăng cường
        public string DeptNameEnhancedManeuvering { get; set; }

        // id đơn vị/khoa phòng của khoa phòng hiện tại
        public int? DeptIDCurrent { get; set; }
        // tên đơn vị/khoa phòng của khoa phòng hiện tại
        public string OrganizationCurrent { get; set; }

        //Tên trình độ chuyên môn 
        public string TrinhDoChuyenMonName { get; set; }

        //Tên của trường nghề
        public string OccupationName { get; set; }

        //Tên đối tượng lao động
        public string LabourSubjectName { get; set; }

        //Tên hạng thương binh
        public string HangThuongBinhName { get; set; }

        //Tên con gia đình chính sách
        public string GiaDinhChinhSachName { get; set; }

        public string ReligionName { get; set; }
    }

    public class StaffCreateRequestModel : StaffModel
    {
        public List<StaffLicenseModel> StaffLicense { get; set; } = new List<StaffLicenseModel>();
        public List<HRM_AttachmentModel> LicenseAttachmentCMND { get; set; } = new List<HRM_AttachmentModel>();
        public List<HRM_AttachmentModel> LicenseAttachmentVISA { get; set; } = new List<HRM_AttachmentModel>();
        public List<HRM_AttachmentModel> LicenseAttachmentHC { get; set; } = new List<HRM_AttachmentModel>();
        public List<HRM_AttachmentModel> LicenseAttachmentGPLD { get; set; } = new List<HRM_AttachmentModel>();
        public List<HRM_AttachmentModel> LicenseAttachmentCCHN { get; set; } = new List<HRM_AttachmentModel>();
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string Nationality { get; set; }
        public bool? Active { get; set; }

        public StaffCreateRequestModel()
        {
            CreationDate = DateTime.Now;
        }
    }

    public class StaffUpdateRequestModel : StaffModel
    {
        public List<StaffLicenseModel> StaffLicense { get; set; } = new List<StaffLicenseModel>();
        public List<HRM_AttachmentModel> LicenseAttachmentCMND { get; set; } = new List<HRM_AttachmentModel>();
        public List<HRM_AttachmentModel> LicenseAttachmentVISA { get; set; } = new List<HRM_AttachmentModel>();
        public List<HRM_AttachmentModel> LicenseAttachmentHC { get; set; } = new List<HRM_AttachmentModel>();
        public List<HRM_AttachmentModel> LicenseAttachmentGPLD { get; set; } = new List<HRM_AttachmentModel>();
        public List<HRM_AttachmentModel> LicenseAttachmentCCHN { get; set; } = new List<HRM_AttachmentModel>();
        public int? LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool? Active { get; set; }
        public StaffUpdateRequestModel()
        {
            LastUpdatedDate = DateTime.Now;
        }
    }

    public class StaffDetailModel : StaffModel
    {
        public string TitleName { get; set; }
        public string EthnicName { get; set; }
        public string DegreeName { get; set; }
        public string AcademicRankName { get; set; }
        public string StaffGroupName { get; set; }
        public string TerritoryName { get; set; }
        public string ContactLocationName { get; set; }
        public string ContactAddrDistrictName { get; set; }
        public string ContactAddrWardName { get; set; }
        public string PermanentLocationName { get; set; }
        public string PermanentDistrictName { get; set; }
        public string PermanentWardName { get; set; }
        public string QueQuanLocationName { get; set; }
        public string QueQuanDistrictName { get; set; }
        public string QueQuanWardName { get; set; }
        public string BirthPlaceName { get; set; }
        public string BirthPlaceDistrictName { get; set; }
        public string BirthPlaceWardName { get; set; }


        public string ExtraNumber5Name { get; set; }
        public string InsuranceIssuePlaceName { get; set; }
        public string HospitalName { get; set; }
        public string DriverLicenseIssuePlaceName { get; set; }

        public List<StaffLicenseModel> StaffLicense { get; set; } = new List<StaffLicenseModel>();

        //Ngày vào đảng chính thức
        public DateTime? NgayVaoDangChinhThuc { get; set; }

        //Tên chức vụ chính quyền
        public string ChucVuChinhQuyenName { get; set; }

        //Tên chức vụ chính quyền
        public string ChucVuKiemNghiemName { get; set; }
    }

    public class StaffComboboxModel : StaffBaseModel
    {
        public string FullName { get; set; }
        public string JobTitleName { get; set; }
    }

    public class StaffDeleteResponseModel
    {
        public int StaffID { get; set; }
        public string Title { get; set; }
        public int Result { get; set; }
        public string Message { get; set; }
    }

    public enum StaffQueryOrder
    {
        ID_DESC,
        ID_ASC,
        ORDER_DESC,
        ORDER_ASC
    }

    public class StaffQueryFilter
    {
        public string TextSearch { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public StaffQueryOrder Order { get; set; }
        public bool? IsDeleted { get; set; }
        public StaffQueryFilter()
        {
            //PageNumber = 1;
            //PageSize = 10;
            TextSearch = string.Empty;
            Order = StaffQueryOrder.ORDER_ASC;
        }
    }

    public class AccountModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int EthnicID { get; set; }
        public int CreatedByUserId { get; set; } //Người tạo hồ sơ
        public DateTime? CreatedOnDate { get; set; } = DateTime.Now;
    }
}
