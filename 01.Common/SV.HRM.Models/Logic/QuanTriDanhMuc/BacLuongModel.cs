using System;

namespace SV.HRM.Models
{
    public class BacLuongBase
    {
        public int BacLuongID { get; set; }
    }

    public class BacLuong : BacLuongBase
    {
        public string MaBacLuong { get; set; }
        public string TenBacLuong { get; set; }
        public float? HeSoLuong { get; set; }
        public int? NgachLuongID { get; set; }
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }

        public bool? IsLevelMax { get; set; }

    }
    public class BacLuongModel : BacLuong
    {
        public string TenNgachLuong { get; set; }
    }
    public class BacLuongCreateModel : BacLuong
    {

        public BacLuongCreateModel()
        {
            CreatedByUserId = 0;
            CreatedOnDate = DateTime.Now;
        }
    }

    public class BacLuongUpdateModel : BacLuong
    {
        public BacLuongUpdateModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
        }
    }
}
