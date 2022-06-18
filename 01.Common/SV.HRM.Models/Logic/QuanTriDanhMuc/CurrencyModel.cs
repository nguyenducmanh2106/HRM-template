using System;
namespace SV.HRM.Models
{
    public class CurrencyBaseModel
    {
        public string CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class CurrencyModel : CurrencyBaseModel
    {
        public string CurrencyName { get; set; }
        public string Description { get; set; }
        public int? TerritoryID { get; set; }
        public string Symbol { get; set; }
        public int? Precision { get; set; }
        public int? NetSalaryPrecision { get; set; }
        public int? ItemSalaryPrecision { get; set; }
        public int? RoundType { get; set; }
        public bool? InactiveDate { get; set; }
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
        public string Note { get; set; }
        public bool Active { get; set; }
    }

    public class CurrencyComboboxModel : CurrencyBaseModel
    {
        public string CurrencyName { get; set; }
    }
}
