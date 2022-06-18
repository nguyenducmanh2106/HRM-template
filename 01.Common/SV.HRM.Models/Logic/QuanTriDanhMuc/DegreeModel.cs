﻿using System;

namespace SV.HRM.Models
{
    public class DegreeBaseModel
    {
        public int DegreeID { get; set; }
        public string DegreeCode { get; set; }
    }

    public class Degree : DegreeBaseModel
    {

        public string DegreeName { get; set; }
        public int DegreeRank { get; set; }
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
        public float ExtraNumber1 { get; set; }
        public float ExtraNumber2 { get; set; }
        public float ExtraNumber3 { get; set; }
        public float ExtraNumber4 { get; set; }
        public float ExtraNumber5 { get; set; }
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
        public int DegreeLevel { get; set; }
        public string DegreeName1 { get; set; }
    }

    public class DegreeModel : Degree { }
    public class DegreeCreateModel : Degree
    {
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class DegreeUpdateModel : Degree
    {
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class DegreeComboboxModel : DegreeBaseModel
    {
        public string DegreeName { get; set; }
    }
}
