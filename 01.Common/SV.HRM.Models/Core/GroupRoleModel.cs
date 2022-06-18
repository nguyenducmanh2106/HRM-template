using System;
namespace SV.HRM.Models
{
    [Serializable]
    public class GroupRoleBase
    {
        public int GroupRoleId { get; set; }
        public string GroupRoleName { get; set; }
        public string GroupRoleCode { get; set; }
    }
}
