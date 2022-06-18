using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Number { get; set; }
        public string Comment { get; set; }
        public Guid AuthorId { get; set; }
        public Guid? ManagerId { get; set; }
        public decimal Sum { get; set; }
        public string State { get; set; }
        public string StateName { get; set; }
        public string SchemeName { get; set; }
    }

    public class InputCommand
    {
        public Guid DocumentId { get; set; }
        public string UserId { get; set; }
        public string NextUserId { get; set; }
        public string CommandName { get; set; }
        public string StateNameToSet { get; set; }
        public string Comment { get; set; }
        public List<FileAttach> DataFileAttach { get; set; }
        public List<Guid> AllowIdentityIds { get; set; }
    }

    public class FileAttach
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

    public class DocumentHistoryViewModel
    {
        public string NguoiXuLy { get; set; }
        public Guid? UserIdNguoiXuLy { get; set; }
        public string NguoiXuLyTiepTheo { get; set; }
        public Guid? UserIdNguoiXuLyTiepTheo { get; set; }
        public string HanhDong { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public string GhiChu { get; set; }
    }

    public class UserInfo
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public string OrgName { get; set; }
    }

    public class DocumentStateModel
    {
        public string StateName { get; set; }
        public bool? IsFinished { get; set; }
    }

    public class ExecuteCommandModel
    {
        public Guid LeaveManagementID { get; set; }
        public Guid WorkflowCommandID { get; set; }
        public Guid? NextUserID { get; set; }
        public string Comment { get; set; }
    }
}
