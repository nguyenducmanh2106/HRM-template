using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class LabourContractBase
    {
        public int LabourContractID { get; set; }
    }
    public class LabourContract : LabourContractBase
    {
        //Số hợp đồng lao động
        public string LabourContractNo { get; set; }

        //id người thuê lao động-fill từ bảng Staff
        public int? EmployerID { get; set; }

        //vị trí chức danh người thuê lao động
        public int? EmployerJobTitleID { get; set; }

        //id công ty
        public int? CompanyID { get; set; }

        //Chi nhánh
        public int? BranchID { get; set; }

        //id nhân viên
        public int? StaffID { get; set; }
        public string Occupation { get; set; }
        public string Occupation1 { get; set; }

        //loại hợp đồng lao động
        public int? ContractTypeID { get; set; }

        //Ngày hiệu lực từ
        public DateTime? ContractFromDate { get; set; }

        //ngày hiệu lực đến
        public DateTime? ContractToDate { get; set; }

        //Thử việc từ
        public DateTime? ProbationFromDate { get; set; }

        //Thử việc đến
        public DateTime? ProbationToDate { get; set; }

        //Địa chỉ làm việc
        public string WorkingAddr { get; set; }

        //vị chức danh của người lao động
        public int? EmployeeJobTitleID { get; set; }

        //Chức danh của người lao động
        public int? EmployeePositionID { get; set; }

        //Mô tả công việc
        public string Working { get; set; }
        public string Working1 { get; set; }

        //Thời gian làm việc
        public string WorkingTime { get; set; }
        public string WorkingTime1 { get; set; }

        //Trang thiết bị
        public string Equipment { get; set; }
        public string Equipment1 { get; set; }

        //Phương tiện đi lại
        public string Transport { get; set; }
        public string Transport1 { get; set; }

        //Lương vùng
        public double? BasicSalary { get; set; }

        //Tiền tệ
        public string CurrencyID { get; set; }

        //Hình thức trả lương
        public string PayingMode { get; set; }
        public string PayingMode1 { get; set; }

        //Phụ cấp
        public string Allowance { get; set; }
        public string Allowance1 { get; set; }

        //Ngày trả
        public string PayDay { get; set; }
        public string PayDay1 { get; set; }

        //Thưởng
        public string Bonus { get; set; }
        public string Bonus1 { get; set; }

        //Đánh giá lương
        public string SalaryReview { get; set; }
        public string SalaryReview1 { get; set; }

        //An toàn lao động
        public string LabourSafety { get; set; }
        public string LabourSafety1 { get; set; }

        //Thời gian nghỉ
        public string AnnualLeave { get; set; }
        public string AnnualLeave1 { get; set; }

        //Bảo hiểm xã hội
        public string SocialInsurance { get; set; }
        public string SocialInsurance1 { get; set; }

        //Chính sách đào tạo
        public string TrainingPolicy { get; set; }
        public string TrainingPolicy1 { get; set; }
        public string Other { get; set; }
        public string Other1 { get; set; }

        //Đền bù
        public string Compensation { get; set; }
        public string Compensation1 { get; set; }

        //Ngày ký
        public DateTime? ContractDate { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public string Allowance2 { get; set; }
        public string Allowance3 { get; set; }
        public string Allowance4 { get; set; }
        public string Allowance5 { get; set; }
        public string PrepareAt { get; set; }
        public bool? Template { get; set; }
        public string PathFile { get; set; }
        public string ExtraText1 { get; set; }
        public string ExtraText2 { get; set; }
        public string ExtraText3 { get; set; }
        public string ExtraText4 { get; set; }
        public string ExtraText5 { get; set; }
        public string ExtraText6 { get; set; }
        public string ExtraText7 { get; set; }
        public string ExtraText8 { get; set; }

        //Ghi chú
        public string ExtraText9 { get; set; }
        public string ExtraText10 { get; set; }

        //Lương chức danh công việc
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
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string TemplateFile { get; set; }
        public int? EmployeeDeptID { get; set; }

        //Thời gian học việc từ
        public DateTime? ApprenticeFromDate { get; set; }

        //Thời gian học việc đến
        public DateTime? ApprenticeToDate { get; set; }

        //Lưu phụ lục của hợp đồng- lưu id của Hợp đồng cha
        public int? ParentLabourContractID { get; set; }
        public string WorkingAddr1 { get; set; }

        //Hệ số lương
        public double? SalaryRate { get; set; }

        //Lương bảo hiểm xã hội
        public double? InsuranceSalary { get; set; }

        //Mức/Nhóm/Bảng lương
        public string PayrollType { get; set; }
    }
    public class LabourContractModel : LabourContract
    {
        //Thuộc tính thêm

        //Tên loại hợp đồng
        public string ContractTypeName { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }

        //Tên chức danh
        public string PositionName { get; set; }

        //Tên của người sử dụng lao động
        public string FullName { get; set; }

        //Tên chi nhánh
        public string BranchName { get; set; }

        //Số phụ lục của hợp đồng
        public string ParentLabourContractName { get; set; }

        //Tên đối tượng lao động
        public string ExtraNumber2Name { get; set; }

    }
    public class LabourContractCreateModel : LabourContract
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public LabourContractCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
    public class LabourContractUpdateModel : LabourContract
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public LabourContractUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
