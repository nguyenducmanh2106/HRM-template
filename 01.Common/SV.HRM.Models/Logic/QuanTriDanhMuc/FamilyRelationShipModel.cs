using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class FamilyRelationShipBase
    {
        public int FamilyRelationshipID { get; set; }
    }
    public class FamilyRelationShip: FamilyRelationShipBase
    {
        public string FamilyRelationshipCode { get; set; }
        public string FamilyRelationshipName { get; set; }
        public int? Status { get; set; }
        public int? SoThuTu { get; set; }
        public int? CreatedByUserId { get;set; }
        public DateTime? CreatedOnDate { get; set; }
        public int LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
    }
    public class FamilyRelationShipModel: FamilyRelationShip
    {
    }
    public class FamilyRelationShipCreateModel : FamilyRelationShip
    {
        public FamilyRelationShipCreateModel()
        {
            CreatedByUserId = 0;
            CreatedOnDate = DateTime.Now;
        }
    }

    public class FamilyRelationShipUpdateModel : FamilyRelationShip
    {
        public FamilyRelationShipUpdateModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
        }
    }
}
