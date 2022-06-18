using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class UniformBaseModel
    {
        public int UniformID { get; set; }
    }

    public class UniformModel : UniformBaseModel
    {
        public int StaffID { get; set; }
        public int UniformNo { get; set; }
        public int UniformYear { get; set; }
        public DateTime? IssueDate { get; set; }
        public int UniformItemID { get; set; }
        public DateTime? ReissueDate { get; set; }
        public float? Amount { get; set; }
        public string CurrencyID { get; set; }
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
        public float? ExtraNumber1 { get; set; }
        public float? ExtraNumber2 { get; set; }
        public float? ExtraNumber3 { get; set; }
        public float? ExtraNumber4 { get; set; }
        public float? ExtraNumber5 { get; set; }
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

        //Additional
        public string FullName { get; set; }
        public string StaffCode { get; set; }
        public string UniformItemName { get; set; }
    }

    public class UniformDetailModel : UniformModel
    {
        public List<HRM_AttachmentModel> ListAttachment { get; set; } = new List<HRM_AttachmentModel>();
    }

    public class UniformCreateRequestModel : UniformModel
    {
        public List<HRM_AttachmentModel> ListAttachment { get; set; } = new List<HRM_AttachmentModel>();
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class UniformUpdateRequestModel : UniformModel
    {
        public List<HRM_AttachmentModel> ListAttachment { get; set; } = new List<HRM_AttachmentModel>();
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
