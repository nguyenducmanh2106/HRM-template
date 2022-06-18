using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class TrinhDoDTBase
    {
        public int TrinhDoDTID { get; set; }
    }
    public class TrinhDoDT : TrinhDoDTBase
    {
        public string TrinhDoDTCode { get; set; }
        public string TrinhDoDTName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? DiplomaID { get; set; }
        public string GhiChu { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
    }
    public class TrinhDoDTModel: TrinhDoDT
    {
        public string DiplomaName { get; set; }
    }
    public class TrinhDoDTCreateModel : TrinhDoDTModel
    {
        public TrinhDoDTCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class TrinhDoDTUpdateModel : TrinhDoDT
    {
        public TrinhDoDTUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
