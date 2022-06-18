using System;

namespace SV.HRM.Models
{
    public class ChuyenKhoaBase
    {
        public int ChuyenKhoaID { get; set; }
    }

    public class ChuyenKhoa : ChuyenKhoaBase
    {
        public string MaChuyenKhoa { get; set; }
        public string TenChuyenKhoa { get; set; }
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }

    }
    public class ChuyenKhoaModel : ChuyenKhoa { }

    public class ChuyenKhoaCreateModel : ChuyenKhoa
    {
        public ChuyenKhoaCreateModel()
        {
            CreatedByUserId = 0;
            CreatedOnDate = DateTime.Now;
        }
    }

    public class ChuyenKhoaUpdateModel : ChuyenKhoa
    {
        public ChuyenKhoaUpdateModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
        }
    }
}
