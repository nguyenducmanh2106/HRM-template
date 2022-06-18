using System;

namespace SV.HRM.Models
{
    public class WorkflowStateBaseModel
    {
        public Guid WorkflowStateID { get; set; }
        public string WorkflowStateCode { get; set; }
    }

    public class WorkflowState : WorkflowStateBaseModel
    {
        public string WorkflowStateName { get; set; }
    }

    public class WorkflowStateModel : WorkflowState
    {

    }

    public class WorkflowStateCreateModel : WorkflowStateModel
    {
    }

    public class WorkflowStateUpdateModel : WorkflowStateModel
    {
    }

    public class WorkflowStateComboboxModel : WorkflowStateBaseModel
    {
        public string WorkflowStateName { get; set; }
    }
}
