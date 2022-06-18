using System;

namespace SV.HRM.Models
{
    public class AcademicRankBaseModel
    {
        public int AcademicRankID { get; set; }
        public string AcademicRankCode { get; set; }
    }

    public class AcademicRank : AcademicRankBaseModel
    {
        public string AcademicRankName { get; set; }
        public string Note { get; set; }
        public DateTime? InactiveDate { get; set; }
    }

    public class AcademicRankModel : AcademicRank { }

    public class AcademicRankCreateModel : AcademicRank
    {
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class AcademicRankUpdateModel : AcademicRank
    {
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class AcademicRankComboboxModel : AcademicRankBaseModel
    {
        public string AcademicRankName { get; set; }
    }
}
