using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class EthnicBase
    {
        public int EthnicId { get; set; }
        public string EthnicCode { get; set; }
        public string EthnicName { get; set; }
    }
    public class Ethnic: EthnicBase
    {
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public int? Status { get; set; }
        public int? Order { get; set; }
    }
    public class EthnicModel: Ethnic
    {
    }
}
