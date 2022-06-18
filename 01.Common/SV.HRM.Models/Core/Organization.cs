using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models.Core
{
    [Serializable]
    public class Organization
    {
        [JsonProperty("application_id")]
        public System.Guid ApplicationId { get; set; }
        [JsonProperty("organization_id")]
        public System.Guid OrganizationId { get; set; }
        [JsonProperty("ParentUnitId")]
        public System.Guid? ParentUnitId { get; set; }
        [JsonProperty("parent_organization_id")]
        public System.Guid ParentId { get; set; }
        [JsonProperty("organization_code")]
        public string OrganizationCode { get; set; }
        [JsonProperty("organization_name")]
        public string Name { get; set; }
        [JsonProperty("organization_type")]
        public string OrganizationType { get; set; }
        [JsonProperty("created_by_user_id")]
        public System.Guid CreatedByUserId { get; set; }
        [JsonProperty("so_luong_phong")]
        public Nullable<int> SoLuongPhong { get; set; }
        [JsonProperty("created_on_date")]
        public System.DateTime CreatedOnDate { get; set; }
        [JsonProperty("last_modified_by_user_id")]
        public System.Guid LastModifiedByUserId { get; set; }
        [JsonProperty("last_modified_on_date")]
        public System.DateTime LastModifiedOnDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("sub_organization")]
        public List<Organization> subOrganization { get; set; }
        [JsonProperty("so_luong_can_bo")]
        public int SoLuongCanBo { get; set; }
        [JsonProperty("so_thu_tu")]
        public int? STT { get; set; }
        [JsonProperty("hien_thi_tren_so_do")]
        public int HienThiTrenBieuDo { get; set; }
        [JsonProperty("dia_chi")]
        public string DiaChi { get; set; }
        [JsonProperty("so_dien_thoai")]
        public string SoDienThoai { get; set; }
        [JsonProperty("so_fax")]
        public string SoFax { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        public int? Loai { get; set; }
        public IList<Organization> LstDonVi { get; set; }
    }
}
