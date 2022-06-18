using SV.HRM.Models.Core;
using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    [Serializable]
    public class BaseUserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int ApplicationId { get; set; }
        public bool IsLockedOut { get; set; }
        public int Status { get; set; }
        public bool IsAdmin { get; set; }
        public int? StaffID { get; set; }
        public Guid? ID { get; set; }
    }

    [Serializable]
    public class UserModel : BaseUserModel, IEquatable<UserModel>
    {
        public string Password { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string Avatar { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public List<RoleModel> Roles { get; set; }
        public List<RightModel> Rights { get; set; }
        public Organization Organization { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string ParentOrganizationId { get; set; }
        public string ParentOrganizationName { get; set; }
        public string ParentOrganizationCode { get; set; }
        public string OrganizationCode { get; set; }
        public string ChucVu { get; set; }
        public Guid? ChucVuId { get; set; }
        public DateTime LastLoginDate { get; set; }
        //public ParentOrganizationModel ParentOrganization { get; set; }
        public bool Equals(UserModel other)
        {
            return this.UserId == other.UserId &&
               this.UserName == other.UserName &&
               this.FullName == other.FullName;
        }
        public string AccessToken { get; set; }
    }
    [Serializable]
    public class UserProfile
    {
        public UserModel User { get; set; }
        public int NumberOfDeTai { get; set; }
        public int NumberOfGoiYDeTai { get; set; }
    }
    [Serializable]
    public class UserRoleScheme
    {
        public string SchemeCode { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string RoleId { get; set; }
    }

    [Serializable]
    public class UsersPostModel
    {
        public string ApplicationId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordNew { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public List<RoleModel> Roles { get; set; }
        public string UserId { get; set; }
        public string Avatar { get; set; }
        public string Mobile { get; set; }
        public string Comment { get; set; }
        public UserInOrganizationClient UserOrganization { get; set; }

        // Extend
        public bool? IsChangePassApplicationSide { get; set; }
        public Guid ApplicationSSOId { get; set; }
        public Guid? ChucVuId { get; set; }
        public bool? Type { get; set; }

        public string ModuleId { get; set; }
        public UsersPostModel()
        {
            this.UserOrganization = new UserInOrganizationClient();
            this.Roles = new List<RoleModel>();
        }
    }
    [Serializable]
    public class UserQueryObject
    {
        public string RightCode { get; set; }
        public string RoleId { get; set; }
        public string RoleCode { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ApplicationId { get; set; }
        public string TextSearch { get; set; }
        public Guid? OrganizationId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public bool? State { get; set; }
        public bool IsIncludeRightsAndRoles { get; set; }
        public bool? IsDonVi { get; set; }
        public bool? Type { get; set; }
    }
    [Serializable]
    public class UserDeleteModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
    }
    [Serializable]
    public class RoleOfUserModel
    {
        public string UserId { get; set; }
        public string ApplicationId { get; set; }
        public Guid OrgId { get; set; }
        public List<RoleModel> Roles { get; set; }
        public RoleOfUserModel()
        {
            this.Roles = new List<RoleModel>();
        }
    }
    [Serializable]
    public class RolesOfUserFunctionModel
    {
        public string FunctionName { get; set; }
        public bool Result { get; set; }
        public string UserId { get; set; }
        public RoleModel Role { get; set; }
        public string ApplicationId { get; set; }
    }
    [Serializable]
    public class RightOfUserFunctionModel
    {
        public string FunctionName { get; set; }
        public bool Result { get; set; }
        public string UserId { get; set; }
        public RightModel Right { get; set; }
        public List<RoleModel> RoleDepens { get; set; }
    }
    [Serializable]
    public class EnableDisableRightOfRoleModel
    {
        public string UserId { get; set; }
        public string RightCode { get; set; }
        public bool Enable { get; set; }
    }
    [Serializable]
    public class CheckRoleOfUserModel
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public string ApplicationId { get; set; }
    }
    [Serializable]
    public class UserTreeView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsCheck { get; set; }
        public bool IsChild { get; set; }
        public bool IsOrganization { get; set; }
        public int Level { get; set; }
        public UserModel UserModel { get; set; }
        public List<UserTreeView> SubChild { get; set; }
    }

    public class UserInfoCacheModel : BaseUserModel
    {
        public string AccessToken { get; set; }
        public bool IsSystem { get; set; } //Phân biệt user login từ Hệ thống QT or NV
        public List<Permissions> Permissions { get; set; } = new List<Permissions>();
    }

    public class Permissions
    {
        public string RoleCode { get; set; }
        public List<string> RightCodes { get; set; }
    }

    public class Account
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int? ApplicationId { get; set; }
    }

    public class UserQueryFilter
    {
        public string TextSearch { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string ColumnOrderBy { get; set; }
        public string TypeOrderBy { get; set; }
        public string ApplicationId { get; set; }
    }
}