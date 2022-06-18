using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class ShiftBase
    {
        public int ShiftID { get; set; }
    }
    public class Shift : ShiftBase
    {
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public string ShiftType { get; set; }
        public DateTime? CI { get; set; }
        public DateTime? CO { get; set; }
        public DateTime? RestFrom { get; set; }
        public DateTime? RestTo { get; set; }
        public string Note { get; set; }
        public int? RestTimeFirstHalf { get; set; }
        public int? RestTimeSecondHalf { get; set; }
        public bool? RestTimeIsWKT { get; set; }
        public bool? RestTimeHalfShiftIsWKT { get; set; }
        public int? WorkingTime { get; set; }
        public int? RestTime { get; set; }
        public int? RestTimeBeforeOVT { get; set; }
        public bool? CalLCIECO { get; set; }
        public int? ThresholdCheckTimeBeforeShift { get; set; }
        public int? ThresholdCheckTimeAfterShift { get; set; }
        public DateTime? InactiveDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public double? OverwriteNightShift { get; set; }
        public int? ThresholdAddition { get; set; }
        public bool? AllOVTWithNightAllowance { get; set; }
        public DateTime? RestFrom1 { get; set; }
        public DateTime? RestTo1 { get; set; }
        public DateTime? RestFrom2 { get; set; }
        public DateTime? RestTo2 { get; set; }
        public int? AdjNight { get; set; }
        public int? ShiftGroup { get; set; }
    }
    public class ShiftModel : Shift
    {
        public string ShiftGroupName { get; set; }
        public string ShiftTypeName { get; set; }
    }
    public class ShiftCreateModel : Shift
    {
        public ShiftCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class ShiftUpdateModel : Shift
    {
        public ShiftUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
