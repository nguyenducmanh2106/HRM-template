using SV.HRM.Core.Utils;
using System;

namespace SV.HRM.Models
{
    public class SystemLogBase
    {
        public int LogId { get; set; }
        public DateTime Date { get; set; }
        public string ActionByUser { get; set; }
        public int? UserId { get; set; }
        public int? ApplicationId { get; set; }
    }

    public class SystemLogModel : SystemLogBase
    {
        public string Exception { get; set; }
        public string Content { get; set; }
        public string IpAddress { get; set; }
        public string AppCodeName { get; set; }
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string UserAgent { get; set; }
        public string Language { get; set; }
        public string DeviceMemory { get; set; }
        public string Platform { get; set; }
        public string Vendor { get; set; }
        public string VendorSub { get; set; }
        public string Product { get; set; }
        public string ProductSub { get; set; }
        public bool? CookieEnabled { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ResponseCode { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationCode { get; set; }
    }

    public class OperatingSystemModel
    {
        public string AppCodeName { get; set; }
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string UserAgent { get; set; }
        public string Language { get; set; }
        public string DeviceMemory { get; set; }
        public string Platform { get; set; }
        public string Vendor { get; set; }
        public string VendorSub { get; set; }
        public string Product { get; set; }
        public string ProductSub { get; set; }
        public bool? CookieEnabled { get; set; }
    }

    public class GeoLocationModel
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class LogDeleteModel
    {
        public int LogId { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
    }

    public class SystemLogQueryObject
    {
        public string TextSearch { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? ApplicationId { get; set; }
        public string ColumnOrderBy { get; set; }
        public string TypeOrderBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Action { get; set; }
        SystemLogQueryObject()
        {
            TextSearch = "";
            PageNumber = 1;
            PageSize = 10;
            ColumnOrderBy = "Date";
            TypeOrderBy = Constant.OrderBy.DESCENDING;
        }
    }
}
