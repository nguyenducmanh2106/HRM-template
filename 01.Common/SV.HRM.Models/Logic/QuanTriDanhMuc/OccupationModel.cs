using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class OccupationBase
    {
        public int OccupationID { get; set; }
        public string OccupationName { get; set;}
        public string OccupationCode { get; set; }

    }
    public class Occupation: OccupationBase
    {
        public string Note { get; set; }
        public int? Status { get; set; }
        public int? Order { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
    }
    public class OccupationModel: Occupation { }
    public class OccupationCreateModel : Occupation
    {
        public OccupationCreateModel()
        {
            CreatedByUserId = 0;
            CreatedOnDate = DateTime.Now;
            Status = 1;
        }
    }

    public class OccupationUpdateModel : Occupation
    {
        public OccupationUpdateModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
            Status = 1;
        }
    }
}
