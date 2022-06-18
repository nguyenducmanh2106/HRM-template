using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class DisciplineDetailBase{
        public int DisciplineDetailID { get; set; }
    }
    public class DisciplineDetail : DisciplineDetailBase
    {
        public int? DisciplineID { get; set; }
        public int? StaffID { get; set; }

        //id của hình thức kỉ luật - map với bảng DisciplineType
        public int? DisciplineTypeID { get; set; }

        //id mức độ vi phạm - map với bảng MucDoViPhamKyLuat
        public int? PeriodID { get; set; }

        //ngày quyết định
        public DateTime? DisciplineDate { get; set; }

        //Hành vi vi phạm
        public string Note { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string DecisionNo { get; set; }

        //Ngày hiệu lực từ
        public DateTime? EffectiveFrom { get; set; }
        
        //ngày hiệu lực tới
        public DateTime? EffectiveTo { get; set; }

        //mức bồi thường
        public double? CompensationLevel { get; set; }

        //phương thức bồi thường
        public string CompensationMode { get; set; }

        public int? DeptID { get; set; }
    }
    public class DisciplineDetailModel: DisciplineDetail
    {
        public string MucDoViPhamKyLuatName { get; set; }
        public string StaffCode { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string PositionName { get; set; }
        public string DeptName { get; set; }

        public string Description1 { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }
    public class DisciplineDetailCreateModel: DisciplineDetail
    {
        public DisciplineDetailCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }
    public class DisciplineDetailUpdateModel : DisciplineDetail
    {
        public DisciplineDetailUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }
}
