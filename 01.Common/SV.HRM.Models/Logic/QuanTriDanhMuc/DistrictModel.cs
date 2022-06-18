using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class DistrictBase
    {
        public int DistrictId { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
    }
    public class District : DistrictBase
    {
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public int? Status { get; set; }
        public int? Order { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
    }
    public class DistrictModel: District
    {
    }
}
