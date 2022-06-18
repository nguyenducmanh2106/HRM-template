using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    [Serializable]
    public class OrganizationBase
    {
        public int OrganizationId { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationName { get; set; }
        public int ApplicationId { get; set; }
    }

    [Serializable]
    public class OrganizationModel : OrganizationBase
    {
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public int? ParentOrganizationId { get; set; }
        public string ParentOrganizationName { get; set; }
        public int? Status { get; set; }
        public int? Order { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public int? Type { get; set; }
        public int? Level { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int? CountryId { get; set; }
        public int? LocationId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public string Website { get; set; }

        public int id { get; set; }
        public string name { get; set; }
    }
}
