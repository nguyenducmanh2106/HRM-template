using System;

namespace SV.HRM.Models
{
    public class NgachLuongBase
    {
        public int NgachLuongID { get; set; }
    }

    public class NgachLuong : NgachLuongBase
    {
        public string MaNgachLuong { get; set; }
        public string TenNgachLuong { get; set; }
        public int? NhomNgachLuongID { get; set; }
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public int? ThoiGianNangLuong { get; set; }
    }
    public class NgachLuongModel : NgachLuong {
        public string TenNhomNgachLuong { get; set; }
    }
    public class NgachLuongCreateModel : NgachLuong
    {

        public NgachLuongCreateModel()
        {
            CreatedByUserId = 0;
            CreatedOnDate = DateTime.Now;
        }
    }

    public class NgachLuongUpdateModel : NgachLuong
    {
        public NgachLuongUpdateModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
        }
    }
}
