using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    [Serializable]
    public class BaseNavigationModel
    {
        public Guid? NavigationId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string IdPath { get; set; }
        public string Path { get; set; }
        public int Level { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<bool> Status { get; set; }
    }
    [Serializable]
    public class NavigationModel : BaseNavigationModel
    {
        public Nullable<System.Guid> ParentId { get; set; }
        public System.Guid ApplicationId { get; set; }
        public bool HasChild { get; set; }
        public string UrlRewrite { get; set; }
        public string IconClass { get; set; }
        public Nullable<System.Guid> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public Nullable<System.Guid> LastModifiedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifiedOnDate { get; set; }
        //new
        public IList<NavigationModel> SubRight { get; set; }
        //public int Order { get; set; }
        public string[] RoleList { get; set; }
        public string SubUrl { get; set; }
        public bool? IsRoleChecked { get; set; }
    }
    [Serializable]
    public class NavigationCreateRequestModel
    {
        public BaseNavigationModel ParentModel { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public System.Guid ApplicationId { get; set; }
        public bool? Status { get; set; }
        public int? Order { get; set; }
        public string UrlRewrite { get; set; }
        public string IconClass { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public string[] RoleList { get; set; }
        public string SubUrl { get; set; }
        public NavigationCreateRequestModel()
        {
            ParentModel = null;
            Status = true;
            Order = 0;
            UrlRewrite = null;
            IconClass = null;
            CreatedOnDate = DateTime.Now;
        }
    }
    [Serializable]
    public class NavigationUpdateRequestModel
    {
        public BaseNavigationModel ParentModel { get; set; }
        public Guid NavigationId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> Order { get; set; }
        public string UrlRewrite { get; set; }
        public string IconClass { get; set; }
        public Nullable<System.Guid> LastModifiedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifiedOnDate { get; set; }
        public Nullable<System.Guid> FromSubNavigation { get; set; }
        public string[] RoleList { get; set; }
        public string SubUrl { get; set; }
    }
    [Serializable]
    public class NavigationQueryFilter
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string TextSearch { get; set; }
        public Guid NavigationId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> ParentId { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> Order { get; set; }
        public string UrlRewrite { get; set; }
        public string IconClass { get; set; }
        public string SubUrl { get; set; }
    }
    [Serializable]
    public class NavigationRoleUpdateModel
    {
        public Guid NavigationId { get; set; }
        public string[] RoleList { get; set; }
    }
}

