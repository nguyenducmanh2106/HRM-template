using System;

namespace SV.HRM.Models
{
    public class StaffGroupBaseModel
    {
        public int StaffGroupID { get; set; }
        public string StaffGroupCode { get; set; }
    }

    public class StaffGroupModel : StaffGroupBaseModel
    {
        public string StaffGroupName { get; set; }
        public string Note { get; set; }
        public DateTime? InactiveDate { get; set; }
    }

    public class StaffGroupCreateModel : StaffGroupModel
    {
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class StaffGroupUpdateModel : StaffGroupModel
    {
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class StaffGroupComboboxModel : StaffGroupBaseModel
    {
        public string StaffGroupName { get; set; }
    }
}

