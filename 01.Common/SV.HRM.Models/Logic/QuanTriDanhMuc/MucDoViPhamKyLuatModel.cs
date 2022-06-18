using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class MucDoViPhamKyLuatBase
    {
        public int MucDoViPhamKyLuatID { get; set; }
        public string MucDoViPhamKyLuatName { get; set; }
    }
    public class MucDoViPhamKyLuat : MucDoViPhamKyLuatBase
    {
        public string MucDoViPhamKyLuatCode { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
    }
    public class MucDoViPhamKyLuatModel: MucDoViPhamKyLuat
    {
    }
    public class MucDoViPhamKyLuatCreateModel : MucDoViPhamKyLuat
    {

        public MucDoViPhamKyLuatCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class MucDoViPhamKyLuatUpdateModel : MucDoViPhamKyLuat
    {
        public MucDoViPhamKyLuatUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
