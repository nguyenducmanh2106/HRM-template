using System;

namespace SV.HRM.Models
{
    public class TerritoryBaseModel
    {
        public int TerritoryID { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
    }

    public class TerritoryModel : TerritoryBaseModel
    {
        public string Nationality { get; set; }
        public string Description0 { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Description4 { get; set; }
        public string Description5 { get; set; }
        public bool? Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class TerritoryCreateRequestModel: TerritoryBaseModel
    {
       
        public Guid CreatedByUserId { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public Guid LastModifiedByUserId { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public string Nationality { get; set; }
        public bool? Active { get; set; }

        public TerritoryCreateRequestModel()
        {
            CreatedOnDate = DateTime.Now;
            LastModifiedOnDate = DateTime.Now;
        }
    }

    public class TerritoryUpdateRequestModel : TerritoryBaseModel
    {
        public Guid LastModifiedByUserId { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public bool? Active { get; set; }
        public TerritoryUpdateRequestModel()
        {
            LastModifiedOnDate = DateTime.Now;
        }
    }

    public class TerritoryDeleteResponseModel
    {
        public int TerritoryID { get; set; }
        public string Title { get; set; }
        public int Result { get; set; }
        public string Message { get; set; }
    }

    public enum TerritoryQueryOrder
    {
        ID_DESC,
        ID_ASC,
        ORDER_DESC,
        ORDER_ASC
    }

    public class TerritoryQueryFilter
    {
        public string TextSearch { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public TerritoryQueryOrder Order { get; set; }
        public bool? IsDeleted { get; set; }
        public TerritoryQueryFilter()
        {
            //PageNumber = 1;
            //PageSize = 10;
            TextSearch = string.Empty;
            Order = TerritoryQueryOrder.ORDER_ASC;
        }
    }
}
