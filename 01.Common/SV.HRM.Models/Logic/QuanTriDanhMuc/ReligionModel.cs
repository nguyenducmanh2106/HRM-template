using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class ReligionBase
    {
        public int ReligionID { get; set; }
    }

    public class Religion : ReligionBase
    {
        public string ReligionCode { get; set; }
        public string ReligionName { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }

    }
    public class ReligionModel : Religion { }

    public class ReligionCreateModel : Religion
    {
        public ReligionCreateModel()
        {
            CreatedByUserId = 0;
            CreatedOnDate = DateTime.Now;
        }
    }

    public class ReligionUpdateModel : Religion
    {
        public ReligionUpdateModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
        }
    }
}
