using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class WardBase
    {
        public int WardId { get; set; }
        public string WardCode { get; set; }
        public string WardName { get; set; }
    }
    public class Ward : WardBase
    {
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public int? Status { get; set; }
        public int? Order { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
    }
    public class WardModel:Ward
    {
    }
}
