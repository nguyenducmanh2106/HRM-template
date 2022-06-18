using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class StaffLicenseBaseModel
    {
        public int StaffLicenseID { get; set; }
    }
    public class StaffLicenseModel : StaffLicense
    {
        public string TypeName { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }

    public class StaffLicense : StaffLicenseBaseModel
    {
        public int StaffID { get; set; }
        public int Type { get; set; }
        public string LicenseNumber { get; set; }
        public int? IssuePlaceID { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiredIssueDate { get; set; }
        public string PracticingScope { get; set; }
        public List<HRM_AttachmentModel> LicenseAttachment { get; set; } = new List<HRM_AttachmentModel>();
        public string IssuePlaceName { get; set; }
    }

    public class StaffLicenseCreate : StaffLicense
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }
    public class StaffLicenseUpdate : StaffLicense
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }
}
