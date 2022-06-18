using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class TrinhDoDaoTaoBase
    {
        public int TrinhDoDaoTaoID { get; set; }
    }
    public class TrinhDoDaoTao : TrinhDoDaoTaoBase
    {
        public string TrinhDoDaoTaoCode { get; set; }
        public string TrinhDoDaoTaoName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? DiplomaID { get; set; }
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
    }
    public class TrinhDoDaoTaoModel: TrinhDoDaoTao
    {
        public string DiplomaName { get; set; }
    }
    public class TrinhDoDaoTaoCreateModel : TrinhDoDaoTaoModel
    {
        public TrinhDoDaoTaoCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class TrinhDoDaoTaoUpdateModel : TrinhDoDaoTao
    {
        public TrinhDoDaoTaoUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
