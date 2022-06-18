using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models.Core
{
    public class WorkflowSchemeModel
    {
        public string Code { get; set; }
        public string Scheme { get; set; }
        public bool IsDelete { get; set; }
    }
    public class WorkflowCreateDocumentModel
    {
        public string DocumentId { get; set; }
        public string AuthorId { get; set; }
        public string DocumentName { get; set; }
        public string WorkflowCode { get; set; }
    }
    public class WorkflowDocumentModel
    {
        public string DocumentId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string State { get; set; }
        public string StateName { get; set; }
        public string WorkflowCode { get; set; }
    }
    public class WorkflowDocumentTransitionHistoryModel
    {
        public string DocumentId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string InitialState { get; set; }
        public string DestinationState { get; set; }
        public string AllowedEmployee { get; set; }
        public string Comment { get; set; }
        public DateTime? TransitionTime { get; set; }
        public string Command { get; set; }
        public UserModel Author { get; set; }
    }
    public class WorkflowCreateDocumentResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string DocumentId { get; set; }
    }

    public class WorkflowProcessDocumentModel
    {
        public string DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string Command { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public string ActorId { get; set; }
        public UserModel Actor { get; set; }
    }

    public class WorkflowProcessDocumentResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string DocumentId { get; set; }
        public string WorkflowState { get; set; }
        public string WorkflowCode { get; set; }
        public string DocumentName { get; set; }
        public string AllowedToEmployeeNames { get; set; }
        public UserModel Actor { get; set; }
    }

    public enum WorkflowQueryOrder
    {
        NGAY_TAO_DESC,
        NGAY_TAO_ASC,
        ID_DESC,
        ID_ASC
    }

    public class WorkflowQueryFilter
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string TextSearch { get; set; }
        public string UserId { get; set; }
        public WorkflowQueryOrder Order { get; set; }

        public WorkflowQueryFilter()
        {
            PageSize = 0;
            PageNumber = 1;
            TextSearch = string.Empty;
            Order = WorkflowQueryOrder.NGAY_TAO_DESC;
        }
    }

}
