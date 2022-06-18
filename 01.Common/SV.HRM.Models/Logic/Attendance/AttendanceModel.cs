using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class AttendanceBase
    {
        public int AttendanceID { get; set; }
    }
    public class Attendance : AttendanceBase
    {
        public int StaffID { get; set; }
        public int? ShiftID { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public DateTime? FirstCI { get; set; }
        public DateTime? LastCO { get; set; }
        public DateTime? RestFrom { get; set; }
        public DateTime? RestTo { get; set; }
        public DateTime? RestFrom1 { get; set; }
        public DateTime? RestTo1 { get; set; }
        public DateTime? RestFrom2 { get; set; }
        public DateTime? RestTo2 { get; set; }
        public double? OVT1 { get; set; }
        public double? OVTWN1 { get; set; }
        public double? OVT2 { get; set; }
        public double? OVTWN2 { get; set; }
        public double? OVT3 { get; set; }
        public double? OVTWN3 { get; set; }
        public double? OVT4 { get; set; }
        public double? OVTWN4 { get; set; }
        public double? OVTAdd { get; set; }
        public double? OVTAddWN { get; set; }
        public int? OVTAddType { get; set; }
        public bool? OVTApprove { get; set; }
        public double? DayShift { get; set; }
        public double? NightShift { get; set; }
        public double? LateFirstCI { get; set; }
        public double? LateRest { get; set; }
        public double? EarlyRest { get; set; }
        public double? EarlyLastCO { get; set; }
        public double? ScheduleWorkingTime { get; set; }
        public double? ScheduleRestTime { get; set; }
        public double? RealWorkingTime { get; set; }
        public double? RealRestTime { get; set; }
        public bool? IOError { get; set; }
        public bool? Lock { get; set; }
        public bool? Post { get; set; }
        public int? PeriodID { get; set; }
        public string Note { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public double? LateFirstCIActual { get; set; }
        public double? EarlyLastCOActual { get; set; }
        public double? LateRestActual { get; set; }
        public double? EarlyRestActual { get; set; }
        public string CardID { get; set; }
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
        public bool? ShiftManager { get; set; }
        public bool? EatAtCompany { get; set; }
        public double? OVTBeforeShift { get; set; }
        public double? OVTAfterShift { get; set; }
        public bool? WatchKeeping { get; set; }
        public double? OVT1Theory { get; set; }
        public double? OVTWN1Theory { get; set; }
        public double? OVT2Theory { get; set; }
        public double? OVTWN2Theory { get; set; }
        public double? OVT3Theory { get; set; }
        public double? OVTWN3Theory { get; set; }
        public double? OVT5 { get; set; }
        public double? OVT6 { get; set; }
        public double? OVTWN5 { get; set; }
        public double? OVTWN6 { get; set; }
        public double? AmtOVT1 { get; set; }
        public double? AmtOVTWN1 { get; set; }
        public double? AmtOVT2 { get; set; }
        public double? AmtOVTWN2 { get; set; }
        public double? AmtOVT3 { get; set; }
        public double? AmtOVTWN3 { get; set; }
        public double? AmtOVT4 { get; set; }
        public double? AmtOVTWN4 { get; set; }
        public double? AmtOVT5 { get; set; }
        public double? AmtOVTWN5 { get; set; }
        public double? AmtOVT6 { get; set; }
        public double? AmtOVTWN6 { get; set; }
        public double? AmtSalary { get; set; }
        public string CurrencyID { get; set; }
        public string HCcyID { get; set; }
        public string BCcyID { get; set; }
        public string RCcyID { get; set; }
        public double? BasicSalary { get; set; }
        //public double? BasicSalarypublic { get; set; }
        public double? BasicSalaryActual { get; set; }
        public double? ApprenticeSalary { get; set; }
        public double? ProbationSalary { get; set; }
        public double? ExchRateH { get; set; }
        public double? ExchRateB { get; set; }
        public double? ExchRateR { get; set; }
        public double? ExchRateInsurance { get; set; }
        public double? LCIECO { get; set; }
        public double? WkdayStandard { get; set; }
        public double? WkdayActual { get; set; }
        public double? WkdaySchedule { get; set; }
        public double? WkDayFullPaid { get; set; }
        public double? AmtNight { get; set; }
        public double? AmtLCIECO { get; set; }
        public double? AmtNonTaxOVT1 { get; set; }
        public double? AmtNonTaxOVTWN1 { get; set; }
        public double? AmtNonTaxOVT2 { get; set; }
        public double? AmtNonTaxOVTWN2 { get; set; }
        public double? AmtNonTaxOVT3 { get; set; }
        public double? AmtNonTaxOVTWN3 { get; set; }
        public double? AmtNonTaxOVT4 { get; set; }
        public double? AmtNonTaxOVTWN4 { get; set; }
        public double? NightHour { get; set; }
        public string LeaveTypeCode { get; set; }
        public string LeaveCode { get; set; }
        public double? LeaveUsed { get; set; }
        public int? LeaveID { get; set; }
        public int? LeaveType { get; set; }
        public string LichSuXuLy { get; set; }
        public string GiaiTrinh { get; set; }
        public string CanhBao { get; set; }
        public string ShiftLeave { get; set; }
        public int? DeptID { get; set; }
        public int? JobTitleID { get; set; }
        public int? PositionID { get; set; }
    }

    public class AttendanceModel : Attendance
    {
        public string ShiftLeaveName { get; set; }
    }
    public class AttendanceCreateModel : Attendance
    {
        public AttendanceCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }

    public class AttendanceUpdateModel : Attendance
    {
        public AttendanceUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
