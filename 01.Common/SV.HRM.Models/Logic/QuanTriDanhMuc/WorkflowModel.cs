using System;

namespace SV.HRM.Models
{
    public class WorkflowBaseModel
    {
        public Guid WorkflowID { get; set; }
        public string WorkflowCode { get; set; }
    }

    public class Workflow : WorkflowBaseModel
    {
        public string WorkflowName { get; set; }
        public bool Status { get; set; }
    }
    public class WorkflowModel : Workflow
    {

    }

    public class WorkflowCreateModel : WorkflowModel
    {
    }

    public class WorkflowUpdateModel : WorkflowModel
    {
    }

    public class WorkflowComboboxModel : WorkflowBaseModel
    {
        public string WorkflowName { get; set; }
    }
}
