using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class LeaveTypeBase
    {
        public int LeaveTypeID { get; set; }
        public string LeaveTypeCode { get; set; }
        public string LeaveTypeName { get; set; }
    }

    public class LeaveType : LeaveTypeBase
    {
        public DateTime? InactiveDate { get; set; }
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
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime? CreatDate { get; set; }
        public int? CreatByStaffID { get; set; }
        public int? LastUpdatedByStaffID { get; set; }
        public string LeaveTypeName1 { get; set; }
        public bool? Status { get; set; }
    }

    public class LeaveTypeModel : LeaveType
    {
    }
    public class LeaveTypeCreateModel : LeaveType
    {
        public LeaveTypeCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class LeaveTypeUpdateModel : LeaveType
    {
        public LeaveTypeUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
