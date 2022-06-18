using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class LeaveBaseModel {
        public int LeaveID { get; set; }
        public string LeaveCode { get; set; }
    }
    public class Leave : LeaveBaseModel
    {
        public int? LeaveTypeID { get; set; }
        public string LeaveName { get; set; }
        public string LeaveName1 { get; set; }
        public bool? IsSystem { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsAnnualLeave { get; set; }
        public bool? WomanOnly { get; set; }
        public bool? ExpatOnly { get; set; }
        public double? RatePaidByCompany { get; set; }
        public double? RatePaidBySIAgency { get; set; }
        public bool? PaidSI { get; set; }
        public bool? PaidHI { get; set; }
        public int? AvgNoMonth { get; set; }
        public string Note { get; set; }
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
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public bool? NonTaxable { get; set; }
        public int? RegisterByDate { get; set; }
    }
    public class LeaveModel : Leave
    {
        public string LeaveTypeName { get; set; }
    }
    public class LeaveCreateModel : Leave
    {
        public LeaveCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }
    public class LeaveUpdateModel : Leave
    {
        public LeaveUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
