using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class JobTitleBaseModel
    {
        public int JobTitleID { get; set; }
        public string JobTitleCode { get; set; }
    }

    public class JobTitle : JobTitleBaseModel
    {
        public string JobTitleName { get; set; }
        public int? JobTitleRank { get; set; }
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
        public string JobTitleName1 { get; set; }
        public string JobTitleName_Vi { get; set; }
        public int JobTitleGroup { get; set; }
        public int SoThuTu { get; set; }
        public string HangCDNN { get; set; }
    }

    public class JobTitleModel : JobTitle
    {
        public string JobTitleGroupName { get; set; }
    }
    public class JobTitleComboboxModel : JobTitleBaseModel
    {
        public string JobTitleName { get; set; }
    }

    public class JobTitleCreateModel : JobTitle
    {
        
        public JobTitleCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class JobTitleUpdateModel : JobTitle
    {
        public JobTitleUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
