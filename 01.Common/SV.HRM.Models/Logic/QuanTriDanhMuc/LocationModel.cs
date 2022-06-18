using System;

namespace SV.HRM.Models
{
    public class LocationBaseModel
    {
        public int LocationID { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
    }

    public class LocationModel : LocationBaseModel
    {
        public int? CountryId { get; set; }
        public int? TerritoryID { get; set; }
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
        public int? ParentID { get; set; }
    }

    public class LocationCreateRequestModel: LocationBaseModel
    {
        public int? TerritoryID { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public Guid LastModifiedByUserId { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int? ParentID { get; set; }

        public LocationCreateRequestModel()
        {
            CreatedOnDate = DateTime.Now;
            LastModifiedOnDate = DateTime.Now;
            ParentID = 0;
        }
    }

    public class LocationUpdateRequestModel : LocationBaseModel
    {
        public int? TerritoryID { get; set; }
        public Guid LastModifiedByUserId { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int? ParentID { get; set; }
        public LocationUpdateRequestModel()
        {
            LastModifiedOnDate = DateTime.Now;
            ParentID = 0;
        }
    }

    public class LocationDeleteResponseModel
    {
        public int LocationID { get; set; }
        public string Title { get; set; }
        public int Result { get; set; }
        public string Message { get; set; }
    }

    public enum LocationQueryOrder
    {
        ID_DESC,
        ID_ASC,
        ORDER_DESC,
        ORDER_ASC
    }

    public class LocationQueryFilter
    {
        public string TextSearch { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public LocationQueryOrder Order { get; set; }
        public bool? IsDeleted { get; set; }
        public LocationQueryFilter()
        {
            PageNumber = 1;
            PageSize = 10;
            TextSearch = string.Empty;
            Order = LocationQueryOrder.ORDER_ASC;
        }
    }
}
