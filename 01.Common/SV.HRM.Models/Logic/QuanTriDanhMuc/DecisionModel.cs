using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class DecisionBase
    {
        public int DecisionID { get; set; }

        //quyết định số
        public string DecisionNo { get; set; }

        //id loại quyết định - map với bảng DecisionItem
        public int? DecisionItemID { get; set; }
    }
    public class Decision : DecisionBase
    {
        public int? StaffID { get; set; }
        public int? WorkingRequestID { get; set; }

        //ngày quyết định
        public DateTime? DecisionDate { get; set; }

        //Vị trí chức danh
        public int? JobTitleID { get; set; }
        public int? JobTitleGradeID { get; set; }

        //chức danh
        public int? PositionID { get; set; }
        public int? PositionGradeID { get; set; }

        //Phòng ban/đơn vị
        public int? DeptID { get; set; }

        //Lương vùng
        public double? BasicSalary { get; set; }

        //Đơn vị tính
        public string CurrencyID { get; set; }

        //Lương bảo hiểm xã hội
        public double? InsuranceSalary { get; set; }
        public double? AttractionRate { get; set; }
        public double? AbilityRate { get; set; }

        //Phụ cấp 1
        public double? AllowanceRate1 { get; set; }

        //Phụ cấp 2
        public double? AllowanceRate2 { get; set; }

        //Phụ cấp 3
        public double? AllowanceRate3 { get; set; }

        //Phụ cấp 4
        public double? AllowanceRate4 { get; set; }

        //Phụ cấp 5
        public double? AllowanceRate5 { get; set; }

        //ngày hiệu lực từ
        public DateTime? EffectiveFromDate { get; set; }

        //ngày hiệu lực đến
        public DateTime? EffectiveToDate { get; set; }

        //ghi chú
        public string Note { get; set; }

        //Người yêu cầu
        public int? StaffRequestID { get; set; }

        //Người thực hiện
        public int? StaffPerformID { get; set; }

        //người bàn giao
        public int? StaffHandoverID { get; set; }

        //người chuyển giao
        public int? StaffTransferID { get; set; }

        //ngày chấp thuận
        public DateTime? ApproveDate { get; set; }

        //người chấp thuận
        public int? StaffApproveID { get; set; }

        //Mức/nhóm/Bảng lương
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

        //Hệ số lương
        public double? ExtraNumber1 { get; set; }

        //Lương chức danh công việc
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
        public bool? ChangeBasicSalary { get; set; }
        public bool? ChangeInsuranceSalary { get; set; }
        public bool? ChangeAttractionRate { get; set; }
        public bool? ChangeAbilityRate { get; set; }
        public bool? ChangeAllowanceRate1 { get; set; }
        public bool? ChangeAllowanceRate2 { get; set; }
        public bool? ChangeAllowanceRate3 { get; set; }
        public bool? ChangeAllowanceRate4 { get; set; }
        public bool? ChangeAllowanceRate5 { get; set; }
    }
    public class DecisionModel : Decision
    {
        //Tên loại quyết định
        public string DecisionItemName { get; set; }

        //Tên chức danh
        public string PositionName { get; set; }

        //Tên loại chức danh
        public string JobTitleName { get; set; }

        //Tên đơn vị tính
        public string CurrencyName { get; set; }

        //Mã nhân viên
        public string StaffCode { get; set; }
    
        //Tên nhân viên
        public string FullName { get; set; }
        //Tên phòng ban
        public string DeptName { get; set; }

        //Người yêu cầu
        public string StaffRequestName { get; set; }

        //Người thực hiện
        public string StaffPerformName { get; set; }

        //người bàn giao
        public string StaffHandoverName { get; set; }

        //người chuyển giao
        public string StaffTransferName { get; set; }

        //người chấp thuận
        public string StaffApproveName { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }
    public class DecisionCreateModel : Decision
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public DecisionCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
    public class DecisionUpdateModel : Decision
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public DecisionUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
