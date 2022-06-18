using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class PartyTitleBase
    {
        public int PartyTitleID { get; set; }
        public string PartyTitleCode { get; set; }
        public string PartyTitleName { get; set; }
    }
    public class PartyTitle : PartyTitleBase
    {
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
    public class PartyTitleModel: PartyTitle
    {
    }
    public class PartyTitleCreateModel : PartyTitle
    {
        public PartyTitleCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class PartyTitleUpdateModel : PartyTitle
    {
        public PartyTitleUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
