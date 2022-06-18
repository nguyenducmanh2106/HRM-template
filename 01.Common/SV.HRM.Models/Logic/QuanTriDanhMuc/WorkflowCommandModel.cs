using System;

namespace SV.HRM.Models
{
    public class WorkflowCommandBaseModel
    {
        public Guid WorkflowCommandID { get; set; }
        public string WorkflowCommandCode { get; set; }
    }

    public class WorkflowCommand : WorkflowCommandBaseModel
    {
        public string WorkflowCommandName { get; set; }
        public int? Order { get; set; }
    }
    public class WorkflowCommandModel : WorkflowCommand
    {

    }

    public class WorkflowCommandCreateModel : WorkflowCommandModel
    {
    }

    public class WorkflowCommandUpdateModel : WorkflowCommandModel
    {
    }

    public class WorkflowCommandComboboxModel : WorkflowCommandBaseModel
    {
        public string WorkflowCommandName { get; set; }
    }
}
