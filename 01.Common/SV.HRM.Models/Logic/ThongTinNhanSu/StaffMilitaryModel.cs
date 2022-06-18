using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class StaffMilitaryBase
    {
        public int StaffMilitaryID { get; set; }
    }
    public class StaffMilitary : StaffMilitaryBase
    {
        public int? StaffID { get; set; }

        //Ngày nhập ngũ
        public DateTime? EnlistmentDate { get; set; }

        //Ngày xuất ngũ
        public DateTime? DischargeDate { get; set; }

        //Đơn vị tham gia
        public string MilitaryUnit { get; set; }

        //Cấp bậc
        public string Rank { get; set; }

        //Đối tượng - 1:con liệt sĩ,2:con thương binh bệnh binh,3:thương binh 4/4
        public int? Type { get; set; }

        //Loại
        public string Kind { get; set; }
        public bool? Is2707 { get; set; }
        public bool? Is2212 { get; set; }
        public string Note { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
    public class StaffMilitaryModel: StaffMilitary
    {
        //tên loại đối tượng
        public string TypeName { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }
    public class StaffMilitaryCreateModel: StaffMilitary
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffMilitaryCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }
    public class StaffMilitaryUpdateModel : StaffMilitary
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffMilitaryUpdateModel()
        {
            LastUpdatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }
}
