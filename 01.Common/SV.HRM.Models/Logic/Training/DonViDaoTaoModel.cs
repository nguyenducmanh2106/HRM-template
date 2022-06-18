using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class DonViDaoTaoBase
    {
        public int DonViDaoTaoID { get; set; }

    }
    public class DonViDaoTao : DonViDaoTaoBase
    {
        public string DonViDaoTaoCode { get; set; }
        public string DonViDaoTaoName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate{get;set;}
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }

    }
    public class DonViDaoTaoModel: DonViDaoTao
    {
        public string DiplomaName { get; set; }
    }
    public class DonViDaoTaoCreateModel : DonViDaoTaoModel
    {
        public DonViDaoTaoCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class DonViDaoTaoUpdateModel : DonViDaoTao
    {
        public DonViDaoTaoUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
