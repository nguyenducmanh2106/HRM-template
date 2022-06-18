using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class ChuyenNganhDaoTaoBase
    {
        public int ChuyenNganhDaoTaoID { get; set; }
    }

    public class ChuyenNganhDaoTao : ChuyenNganhDaoTaoBase
    {
        public string ChuyenNganhDaoTaoCode { get; set; }
        public string ChuyenNganhDaoTaoName { get; set; }
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }

    }
    public class ChuyenNganhDaoTaoModel : ChuyenNganhDaoTao { }

    public class ChuyenNganhDaoTaoCreateModel : ChuyenNganhDaoTao
    {
        public ChuyenNganhDaoTaoCreateModel()
        {
            CreatedByUserId = 0;
            CreatedOnDate = DateTime.Now;
        }
    }

    public class ChuyenNganhDaoTaoUpdateModel : ChuyenNganhDaoTao
    {
        public ChuyenNganhDaoTaoUpdateModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
        }
    }
}
