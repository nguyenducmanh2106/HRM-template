using System;

namespace SV.HRM.Models
{
    public class LeaveManagementBaseModel
    {
        public Guid LeaveManagementID { get; set; }
        public string Description { get; set; }
    }
    public class LeaveManagement : LeaveManagementBaseModel
    {
        public int? LeaveGroup { get; set; }
        public int? LeaveType { get; set; }
        public int? LeaveID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public double? CountHour { get; set; }
        public double? CountDay { get; set; }
        public Guid? UserID { get; set; }
        public Guid? CurrentUserID { get; set; }
        public Guid? PreUserID { get; set; }
        public bool? IsDraft { get; set; }
        public bool? IsInProcess { get; set; }
        public bool? IsFinished { get; set; }
        public int? TransitionClassifier { get; set; }
        public string AllowIdentityIds { get; set; }
        public Guid? CreatedByUserID { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public Guid? LastModifiedByUserID { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
    }
    public class LeaveManagementModel : LeaveManagement
    {
        public int STT { get; set; }
        public string CreatedUserName { get; set; }
        public string CurrentUserName { get; set; }
        public string PreUserName { get; set; }
        public string LeaveName { get; set; }
        public string WorkflowName { get; set; }
        public string WorkflowStateName { get; set; }
        public string LeaveDate { get; set; }
    }

    public class RemainDayOffModel
    {
        public int UserID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? WkDate { get; set; }
        public double? WkDay { get; set; }
    }

    public class LeaveManagementCreateModel : LeaveManagement
    {
        public LeaveManagementCreateModel()
        {
            CreatedOnDate = DateTime.Now;
        }
    }

    public class LeaveManagementUpdateModel : LeaveManagement
    {
        public LeaveManagementUpdateModel()
        {
            LastModifiedOnDate = DateTime.Now;
        }
    }

    public class LeaveManagementQueryFilter
    {
        public Guid LeaveManagementID { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string TextSearch { get; set; }
        public string WorkflowScreen { get; set; }
        public LeaveManagementQueryFilter() 
        {
            PageNumber = 1;
            TextSearch = string.Empty;
        }
    }
}
