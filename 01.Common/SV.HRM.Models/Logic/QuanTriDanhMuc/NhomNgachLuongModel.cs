using System;

namespace SV.HRM.Models
{
    public class NhomNgachLuongBase
    {
        public int NhomNgachLuongID { get; set; }
    }

    public class NhomNgachLuong : NhomNgachLuongBase
    {
        public string MaNhomNgachLuong { get; set; }
        public string TenNhomNgachLuong { get; set; }
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }

    }

    public class NhomNgachLuongModel : NhomNgachLuong
    {

    }

    public class NhomNgachLuongCreateModel : NhomNgachLuong
    {
        public NhomNgachLuongCreateModel()
        {
            CreatedByUserId = 0;
            CreatedOnDate = DateTime.Now;
        }
    }

    public class NhomNgachLuongUpdateModel : NhomNgachLuong
    {
        public NhomNgachLuongUpdateModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
        }
    }

}
