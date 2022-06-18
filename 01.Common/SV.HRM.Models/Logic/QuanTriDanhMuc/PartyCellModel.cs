using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class PartyCellBase
    {
        public int PartyCellID { get; set; }
        public string PartyCellCode { get; set; }
        public string PartyCellName { get; set; }
    }
    public class PartyCell : PartyCellBase
    {
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
    public class PartyCellModel : PartyCell
    {
    }
    public class PartyCellCreateModel : PartyCell
    {
        public PartyCellCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class PartyCellUpdateModel : PartyCell
    {
        public PartyCellUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
