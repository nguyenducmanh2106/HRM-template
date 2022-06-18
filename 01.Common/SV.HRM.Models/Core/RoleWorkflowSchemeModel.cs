using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models.Core
{
    public class RoleWorkflowSchemeModel
    {
        public string Id { get; set; }
        public string ApplicationId { get; set; }
        public string WorkflowSchemeCode { get; set; }
        public string RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
    }
    public enum RolesSchemeQueryOrder
    {
        NGAY_TAO_DESC,
        NGAY_TAO_ASC,
        ID_DESC,
        ID_ASC
    }
    public class WorkflowPermissionQueryFilter
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string TextSearch { get; set; }
        public string SchemeCode { get; set; }
        public string RoleId { get; set; }
        public string ApplicationId { get; set; }
        public RolesSchemeQueryOrder Order { get; set; }

        public WorkflowPermissionQueryFilter()
        {
            PageSize = 0;
            PageNumber = 1;
            TextSearch = string.Empty;
            SchemeCode = "";
            Order = RolesSchemeQueryOrder.NGAY_TAO_DESC;
        }
    }
    public class DeleteRolesSchemePostModel
    {
        public List<RoleWorkflowSchemeModel> ListItemDelete { get; set; }
    }

}

