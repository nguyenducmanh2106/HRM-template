using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class CompanyBase
    {
        public int CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
    }
    public class Company : CompanyBase
    {
        public DateTime? FoundationDate { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Addr3 { get; set; }
        public int? LocationID { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int? TaxAgencyID { get; set; }
        public int? PITID { get; set; }
        public int? InsuranceAgencyID { get; set; }
        public int? InsuranceID { get; set; }
        public int? TUType { get; set; }
        public double? TUStaffPaid { get; set; }
        public double? TUCompanyPaid { get; set; }
        public string AccountNumber { get; set; }
        public int? BankID { get; set; }
        public string HomeCurrencyID { get; set; }
        public string TaxCurrencyID { get; set; }
        public double? OVTRate1 { get; set; }
        public double? OVTRate2 { get; set; }
        public double? OVTRate3 { get; set; }
        public double? OVTRate4 { get; set; }
        public double? NightAllowanceRate { get; set; }
        public double? OVTWNRate1 { get; set; }
        public double? OVTWNRate2 { get; set; }
        public double? OVTWNRate3 { get; set; }
        public double? OVTWNRate4 { get; set; }
        public double? AdvanceRate { get; set; }
        public double? DayPerMonth { get; set; }
        public double? HourPerDay { get; set; }
        public int? DirectorID { get; set; }
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
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int? TaxAgency { get; set; }
        public string CompanyName1 { get; set; }
        public string Abbreviation { get; set; }
        public double? NonTaxOVTRate1 { get; set; }
        public double? NonTaxOVTWNRate1 { get; set; }
        public double? NonTaxOVTRate2 { get; set; }
        public double? NonTaxOVTWNRate2 { get; set; }
        public double? NonTaxOVTRate3 { get; set; }
        public double? NonTaxOVTWNRate3 { get; set; }
        public double? NonTaxOVTRate4 { get; set; }
        public double? NonTaxOVTWNRate4 { get; set; }
        public double? HourPerMonth { get; set; }
        public string TaxCode { get; set; }
        public double? OVTRate5 { get; set; }
        public double? OVTWNRate5 { get; set; }
        public double? NonTaxOVTRate5 { get; set; }
        public double? NonTaxOVTWNRate5 { get; set; }
        public double? OVTRate6 { get; set; }
        public double? OVTWNRate6 { get; set; }
        public double? NonTaxOVTRate6 { get; set; }
        public double? NonTaxOVTWNRate6 { get; set; }
    }
    public class CompanyModel : Company
    {
    }
}
