using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SV.HRM.Models
{
    public class HistoryBaseModel
    {
        public int HistoryID { get; set; }
    }

    public class History : HistoryBaseModel
    {
        public int StaffID { get; set; }
        public int? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? Todate { get; set; }
        public string HistoryNo { get; set; }
        public DateTime? DecisionDate { get; set; }
        public DateTime? ApplyFromDate { get; set; }
        public DateTime? ApplyToDate { get; set; }
        public int? LinkID { get; set; }
        public string SalaryType { get; set; }
        public string CurrencyID { get; set; }
        public double? SalaryRate { get; set; }
        public double? BasicSalary { get; set; }
        public double? InsuranceSalary { get; set; }
        public double? AttractionRate { get; set; }
        public double? AbilityRate { get; set; }
        public int? DeptID { get; set; }
        public int? PositionID { get; set; }
        public int? PositionGradeID { get; set; }
        public int? CategoryID { get; set; }
        public int? CostcenterID { get; set; }
        public int? LocationID { get; set; }
        public int? InsuranceID { get; set; }
        public int? JobTitleGradeID { get; set; }
        public int? JobTitleID { get; set; }
        public int? PeriodBalanceID { get; set; }
        public int? WorkGroupID { get; set; }
        public string Note { get; set; }
        //Tên công ty
        public string ExtraText1 { get; set; }
        //Đơn vị
        public string ExtraText2 { get; set; }
        //Chức danh
        public string ExtraText3 { get; set; }
        //Vị trí làm việc
        public string ExtraText4 { get; set; }
        //Chức vụ đoàn thể | chức vụ đảng
        public string ExtraText5 { get; set; }
        //Chức vụ chính quyền
        public string ExtraText6 { get; set; }
        //Số hợp đồng thử việc
        public string ExtraText7 { get; set; }
        //Kết quả thử việc
        public string ExtraText8 { get; set; }

        public string ExtraText9 { get; set; }
        public string ExtraText10 { get; set; }
        //Mức lương
        public float? ExtraNumber1 { get; set; }
        //Hệ số lương
        public float? ExtraNumber2 { get; set; }
        //Chức vụ chính quyền
        public double? ExtraNumber3 { get; set; }
        //Chức vụ kiêm nhiệm
        public double? ExtraNumber4 { get; set; }
        public double? ExtraNumber5 { get; set; }
        //Ngày bắt đầu nhiệm kỳ
        public DateTime? ExtraDate1 { get; set; }
        //Ngày kết thúc nhiệm kỳ
        public DateTime? ExtraDate2 { get; set; }
        public DateTime? ExtraDate3 { get; set; }
        public DateTime? ExtraDate4 { get; set; }
        public DateTime? ExtraDate5 { get; set; }

        //dùng phân biệt quá trình trước vào cty hay đang công tác tại công ty
        public bool? ExtraLogic1 { get; set; }

        //Công tác trong ngành ý tế
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
        public double? AllowanceRate1 { get; set; }
        public double? AllowanceRate2 { get; set; }
        public double? AllowanceRate3 { get; set; }
        public double? AllowanceRate4 { get; set; }
        public double? AllowanceRate5 { get; set; }
        public bool? BaseOnSkill { get; set; }
        public string PayrollType { get; set; }
        public int? KyDongBaoHiem { get; set; }
        public int? TrinhDoChuyenMonID { get; set; }
        public string TrinhDoChuyenMonName { get; set; }
        public string ChucVuChinhQuyenName { get; set; }
        public string ChucVuKiemNhiemName { get; set; }
    }

    public class HistoryModel : History
    {
        //Thuộc tính thêm
        public string OrganizationName { get; set; }
        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public string StatusText { get; set; }

        public string CategoryName { get; set; }
        public string WorkGroupName { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }

    }

    public class HistoryCreateRequestModel : History
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }

        public HistoryCreateRequestModel()
        {
            CreatedBy = 0;
            LastUpdatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedDate = DateTime.Now;
        }
    }


    //Quá trình công tác trước khi vào công ty
    public class HistoryCreateBeforeJoiningCompanyRequestModel : History
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }

        public HistoryCreateBeforeJoiningCompanyRequestModel()
        {
            CreatedBy = 0;
            LastUpdatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedDate = DateTime.Now;
        }
    }

    public class HistoryUpdateBeforeJoiningCompanyRequestModel : History
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }

        public HistoryUpdateBeforeJoiningCompanyRequestModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }

    public class HistoryUpdateRequestModel : History
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }

        public HistoryUpdateRequestModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }

    public enum HistoryQueryOrder
    {
        ID_DESC,
        ID_ASC,
        ORDER_DESC,
        ORDER_ASC
    }

    public class HistoryQueryFilter
    {
        public string TextSearch { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public HistoryQueryOrder Order { get; set; }
        public bool? IsDeleted { get; set; }
        public HistoryQueryFilter()
        {
            //PageNumber = 1;
            //PageSize = 10;
            TextSearch = string.Empty;
            Order = HistoryQueryOrder.ORDER_ASC;
        }
    }
}
