using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class ChuyenNganhBase
    {
        public int ChuyenNganhID { get; set; }
    }

    public class ChuyenNganh : ChuyenNganhBase
    {
        public string ChuyenNganhCode { get; set; }
        public string ChuyenNganhName { get; set; }
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }

    }
    public class ChuyenNganhModel : ChuyenNganh { }

    public class ChuyenNganhCreateModel : ChuyenNganh
    {
        public ChuyenNganhCreateModel()
        {
            CreatedByUserId = 0;
            CreatedOnDate = DateTime.Now;
        }
    }

    public class ChuyenNganhUpdateModel : ChuyenNganh
    {
        public ChuyenNganhUpdateModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
        }
    }
}
