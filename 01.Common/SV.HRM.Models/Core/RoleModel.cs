using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class RoleBase
    {
        public int NodeId { get; set; }
        public int? ParentRoleId { get; set; }
        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
    }

    [Serializable]
    public class RoleModel : IEquatable<RoleModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Code { get; set; }

        public bool Equals(RoleModel other)
        {
            //return this.RoleId == other.RoleId &&
            // this.RoleCode == other.RoleCode &&
            // this.RoleName == other.RoleName;
            return true;
        }
    }
}
