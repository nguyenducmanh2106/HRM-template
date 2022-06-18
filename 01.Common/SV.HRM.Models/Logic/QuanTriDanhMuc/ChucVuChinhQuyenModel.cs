using System;

namespace SV.HRM.Models
{
    public class ChucVuChinhQuyenBase
    {
        public int ChucVuChinhQuyenID { get; set; }
    }

    public class ChucVuChinhQuyen : ChucVuChinhQuyenBase
    {
        public string ChucVuChinhQuyenCode { get; set; }
        public string ChucVuChinhQuyenName { get; set; }
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
    }

    public class ChucVuChinhQuyenModel : ChucVuChinhQuyen
    {

    }

    public class ChucVuChinhQuyenComboboxModel : ChucVuChinhQuyenBase
    {
        public string ChucVuChinhQuyenName { get; set; }
    }

    public class ChucVuChinhQuyenCreateModel : ChucVuChinhQuyen
    {
        public ChucVuChinhQuyenCreateModel()
        {
            CreatedByUserId = 0;
            CreatedOnDate = DateTime.Now;
        }
    }

    public class ChucVuChinhQuyenUpdateModel : ChucVuChinhQuyen
    {
        public ChucVuChinhQuyenUpdateModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
        }
    }
}
