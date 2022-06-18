using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class TrinhDoChuyenMonBase
    {
        public int TrinhDoChuyenMonID { get; set; }

    }
    public class TrinhDoChuyenMon : TrinhDoChuyenMonBase
    {
        public string TrinhDoChuyenMonCode { get; set; }
        public string TrinhDoChuyenMonName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate{get;set;}
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
        public int? DiplomaID { get; set; }
        public DateTime? InactiveDate { get; set; }

    }
    public class TrinhDoChuyenMonModel: TrinhDoChuyenMon
    {
        public string DiplomaName { get; set; }
    }
    public class TrinhDoChuyenMonCreateModel : TrinhDoChuyenMonModel
    {
        public TrinhDoChuyenMonCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class TrinhDoChuyenMonUpdateModel : TrinhDoChuyenMon
    {
        public TrinhDoChuyenMonUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
