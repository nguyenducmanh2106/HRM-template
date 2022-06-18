using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class TACategoryBase
    {
        public int TACategoryID { get; set; }
    }
    public class TACategory : TACategoryBase
    {
        public string TACategoryCode { get; set; }
        public string TACategoryName { get; set; }
        public double? AvgHourPerDay { get; set; }
        public double? AvgDayPerMonth { get; set; }
        public double? AvgDayPerYear { get; set; }
        public double? LimitOVT { get; set; }
        public double? ThresholdOVTBeforeShift { get; set; }
        public double? ThresholdOVTAfterShift { get; set; }
        public double? ThresholdOVT { get; set; }
        public int? RoundUnitOVT { get; set; }
        public int? RoundOVTType { get; set; }
        public int? ThresholdFilterCheckIO { get; set; }
        public int? ThresholdCheckTimeBeforeShift { get; set; }
        public int? ThresholdCheckTimeAfterShift { get; set; }
        public DateTime? ThresholdDaytime { get; set; }
        public DateTime? ThresholdNightTime { get; set; }
        public int? ThresholdLCIECO { get; set; }
        public int? RoundUnitLCIECO { get; set; }
        public int? RoundLCIECOType { get; set; }
        public int? RoundUnitWKT { get; set; }
        public int? RoundWKTType { get; set; }
        public int? TypeIO { get; set; }
        public bool? IsDefault { get; set; }
        public string Note { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class TACategoryModel : TACategory
    {
        public string RoundOVTTypeName { get; set; }
        public string RoundLCIECOTypeName { get; set; }
        public string RoundWKTTypeName { get; set; }
        public string IsDefaultName { get; set; }
        public string TimeThresholdDaytime { get; set; }
        public string TimeThresholdNightTime { get; set; }
    }
    public class TACategoryCreateModel : TACategory
    {
        public TACategoryCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class TACategoryUpdateModel : TACategory
    {
        public TACategoryUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
