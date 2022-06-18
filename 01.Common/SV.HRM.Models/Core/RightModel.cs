using System;
using System.Collections.Generic;
namespace SV.HRM.Models
{
    public class RightBase
    {
        public int RoleId { get; set; }
        public int RightId { get; set; }
        public string RightCode { get; set; }
        public string RightName { get; set; }
    }

    [Serializable]
    public class RightModel : IEquatable<RightModel>
    {
        public string RightCode { get; set; }
        public string RightName { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool Enabled { get; set; }
        public List<UserModel> Users { get; set; }
        public List<RoleModel> Roles { get; set; }

        public bool Equals(RightModel target)
        {
            return this.RightCode == target.RightCode &&
              this.RightName == target.RightName &&
              this.Order == target.Order &&
              this.Enabled == target.Enabled;
        }
    }
    // API Models

    // Single delete model
    // DELETE 	: api/system/rights/{code}
    // Model	: { RightCode, Result }
    [Serializable]
    public class RightDeleteModel
    {
        public string RightCode { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
    }
    // Single update model
    // REQ PUT  : api/system/rights/{code}
    // RES      : Response<RightModel>

    /// <summary>
    /// Class luu tru cac role cua 1 right
    /// </summary>
    /// 
    [Serializable]
    public class RightRoleModel
    {
        public RightModel Right { get; set; }
        public bool Enable { get; set; }
        public bool Inherited { get; set; }
        public string ApplicationId { get; set; }
        public List<RoleModel> InheritedFromRoles { get; set; }
        public string InheritedFromRolesJson { get; set; }

    }
    [Serializable]
    public class RightOfUserModel
    {
        public string UserId { get; set; }
        public List<RightRoleModel> ListRightInfo { get; set; }
    }
    [Serializable]
    public class RightOfRoleModel
    {
        public string RoleId { get; set; }
        public List<RightModel> ListRights { get; set; }
    }
    [Serializable]
    public class CheckRightsOfUserModel
    {
        public string UserId { get; set; }
        public string RightCode { get; set; }
        public string SiteId { get; set; }
        public bool Result { get; set; }
    }
    [Serializable]
    public class CheckRightsOfRoleModel
    {
        public string RoleId { get; set; }
        public string RightCode { get; set; }
        public bool Result { get; set; }
    }
    [Serializable]
    public class AllowedApplicationByRightAndUserModel
    {
        public string UserId { get; set; }
        public string RightCode { get; set; }
        public List<string> ListApplications { get; set; }
        public List<string> ListApplicationsOld { get; set; }
        public bool Result { get; set; }
    }
    [Serializable]
    public class AllowedApplicationByUserModel
    {
        public string UserId { get; set; }
        public List<string> ListApplications { get; set; }
    }
    [Serializable]
    public class RightQueryObject
    {
        public string RightCode { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string TextSearch { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public bool IsIncludeUsersAndRoles { get; set; }
        public string ApplicationId { get; set; }
    }
}
