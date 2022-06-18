using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class StaffAssessmentBase
    {
        public int StaffAssessmentID { get; set; }
    }
    public class StaffAssessment : StaffAssessmentBase
    {
        public int StaffID { get; set; }
        public int Year { get; set; }
        public string Note { get; set; }
        public int AssessmentType { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
    public class StaffAssessmentModel : StaffAssessment
    {
        public string FullName { get; set; }
        public string DeptName { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentTypeName { get; set; }
        public string CompanyName { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }

    public class StaffAssessmentCreateModel : StaffAssessment
    {
        public StaffAssessmentCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
    public class StaffAssessmentUpdateModel : StaffAssessment
    {
        public StaffAssessmentUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
