using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class CountryBase
    {
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }

    public class Country : CountryBase
    {
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public int? Status { get; set; }
        public int? Order { get; set; }
    }

    public class CountryModel: Country
    {
    }
}
