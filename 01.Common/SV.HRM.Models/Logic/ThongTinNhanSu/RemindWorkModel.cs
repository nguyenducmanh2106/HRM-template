using SV.HRM.Core;
using System;
namespace SV.HRM.Models
{
    public class RemindWorkModel
    {
        public class StaffHistory
        {
            #region Thông tin nhân viên
            public int StaffID { get; set; }
            public string StaffCode { get; set; }
            public string FullName { get; set; }
            #endregion

            #region Thông tin quá trình công tác
            public HistoryStatusEnum Status { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? Todate { get; set; }
            public string HistoryNo { get; set; }
            public int? DeptID { get; set; } //Đơn vị công tác
            public double? ExtraNumber3 { get; set; } //Chức vụ chính quyền
            #endregion
        }

        public class StaffLabourContract
        {
            #region Thông tin nhân viên
            public int StaffID { get; set; }
            public string StaffCode { get; set; }
            public string FullName { get; set; }
            #endregion

            #region Thông tin hợp đồng
            public DateTime? ContractFromDate { get; set; }
            public DateTime? ContractToDate { get; set; }
            public string ContractTypeName { get; set; }
            #endregion
        }

        public class StaffSalary
        {
            #region Thông tin nhân viên
            public int StaffID { get; set; }
            public string StaffCode { get; set; }
            public string FullName { get; set; }
            #endregion

            #region Thông tin quá trình lương
            public DateTime? TuNgay { get; set; }
            public DateTime? DenNgay { get; set; }
            #endregion
        }

        public class StaffDisciplineDetail
        {
            #region Thông tin nhân viên
            public int StaffID { get; set; }
            public string StaffCode { get; set; }
            public string FullName { get; set; }
            #endregion

            #region Thông tin ký luật
            public DateTime? EffectiveFrom { get; set; }
            public DateTime? EffectiveTo { get; set; }
            #endregion
        }

        public class StaffRemindWork
        {
            public int StaffID { get; set; }
            public string FullName { get; set; }
            public string StaffCode { get; set; }
            public DateTime? Birthday { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public string ContractTypeName { get; set; }
            public DateTime? ContractToDate { get; set; }
            public DateTime? EffectiveTo { get; set; }
            public DateTime? NgayVaoDangChinhThuc { get; set; }
            public int? Duration { get; set; }
        }

        public class ConfigSystemRemindModel
        {
            public int StaffID { get; set; }
            public string Value { get; set; } //Lưu chuỗi Json cấu hình
        }

        public class ConfigUserRemindModel
        {
            public string STT { get; set; }
            public int? PersonalReminderID { get; set; }
            public int StaffID { get; set; }
            public string Content { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? DueDate { get; set; }
            public string DueDateText { get; set; }
            public bool? Complete { get; set; }
            public int? NotifyTime { get; set; }
            public string NotifyText { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? CreationDate { get; set; }
            public int? LastUpdatedBy { get; set; }
            public DateTime? LastUpdatedDate { get; set; }
        }

        public class PersonalReminder
        {
            public int? PersonalReminderID { get; set; }
        }
    }
}
