using System;
namespace SV.HRM.Models
{
    public class QualificationBaseModel
    {
        public int QualificationID { get; set; }
        public string QualificationCode { get; set; }
    }

    public class QualificationModel : QualificationBaseModel
    {
        public string QualificationName { get; set; }
        public string Note { get; set; }
        public DateTime? InactiveDate { get; set; }
    }

    public class QualificationCreateModel : QualificationModel
    {
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class QualificationUpdateModel : QualificationModel
    {
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class QualificationComboboxModel : QualificationBaseModel
    {
        public string QualificationName { get; set; }
    }
}
