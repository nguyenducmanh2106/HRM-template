using System;

namespace SV.HRM.Models
{
    public class ChucVuKiemNhiemBase
    {
        public int ChucVuKiemNhiemID { get; set; }
    }

    public class ChucVuKiemNhiem : ChucVuKiemNhiemBase
    {
        public string ChucVuKiemNhiemCode { get; set; }
        public string ChucVuKiemNhiemName { get; set; }
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
    }
    public class ChucVuKiemNhiemModel : ChucVuKiemNhiem
    {

    }

    public class ChucVuKiemNhiemComboboxModel : ChucVuKiemNhiemBase
    {
        public string ChucVuKiemNhiemName { get; set; }
    }
    public class ChucVuKiemNhiemCreateModel : ChucVuKiemNhiem
    {
        public ChucVuKiemNhiemCreateModel()
        {
            CreatedByUserId = 0;
            CreatedOnDate = DateTime.Now;
        }
    }

    public class ChucVuKiemNhiemUpdateModel : ChucVuKiemNhiem
    {
        public ChucVuKiemNhiemUpdateModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
        }
    }
}
