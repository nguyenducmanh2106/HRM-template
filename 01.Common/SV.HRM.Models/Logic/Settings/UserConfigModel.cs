
using System;

namespace SV.HRM.Models
{
    public class UserConfig
    {
        public Guid UserConfigID { get; set; }
        public int? UserID { get; set; }
        public int? StaffID { get; set; }
        public Guid? WorkflowID { get; set; }
    }
    public class UserConfigModel : UserConfig
    {
        public string UserName { get; set; }
        public string StaffCode { get; set; }
        public string UserFullName { get; set; }
        public string StaffFullName { get; set; }
        public string WorkflowCode { get; set; }
        public string WorkflowName { get; set; }
    }

    public class UserConfigComboboxStaff
    {
        public int StaffID { get; set; }
        public string StaffFullName { get; set; }
    }

    public class UserConfigComboboxUser
    {
        public int UserID { get; set; }
        public string UserFullName { get; set; }
    }

    public class UserConfigComboboxWorkflow
    {
        public Guid WorkflowID { get; set; }
        public string WorkflowName { get; set; }
    }
}
