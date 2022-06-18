using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class StaffAssetBaseModel
    {
        public int StaffAssetID { get; set; }
    }

    public class StaffAssetModel : StaffAssetBaseModel
    {
        public int StaffID { get; set; }
        public string StaffCode { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string PositionName { get; set; }
        public string Ref { get; set; }
        public float? Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
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

        //Additional Infomation
        public string FullName { get; set; }
        public string OrganizationName { get; set; }
    }

    public class StaffAssetDetailModel : StaffAssetModel
    {
        public List<HRM_AttachmentModel> ListAttachment { get; set; } = new List<HRM_AttachmentModel>();
    }

    public class StaffAssetCreateRequestModel : StaffAssetModel
    {
        public List<HRM_AttachmentModel> ListAttachment { get; set; } = new List<HRM_AttachmentModel>();
        public int CreatedBy { get; set; }
        public string CreationDate { get; set; }
    }

    public class StaffAssetUpdateRequestModel : StaffAssetModel
    {
        public List<HRM_AttachmentModel> ListAttachment { get; set; } = new List<HRM_AttachmentModel>();
        public int LastUpdatedBy { get; set; }
        public string LastUpdatedDate { get; set; }
    }
}
