using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models.Core
{
    [Serializable]
    public class UserInOrganizationClient
    {
        public string UserId { get; set; }
        public string OrganizationID { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public string ChucVu { get; set; }
        public string CreatedByUserID { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public string LastModifiedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public bool DeleteSuccess { get; set; }
    }
}
