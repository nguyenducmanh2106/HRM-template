using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class QuanLySucKhoeBase
    {
        public int QuanLySucKhoeID { get; set; }
    }
    public class QuanLySucKhoe : QuanLySucKhoeBase
    {
        public int? StaffID { get; set; }

        //Kỳ khám - map với bảng HealthPeriod
        public int? HealthPeriodID { get; set; }
        public string BenhLy { get; set; }

        //Xếp loại fix cứng code 1->5
        public int? XepLoaiSucKhoe { get; set; }

        public string CanhBaoBenhTat { get; set; }
        public string TuVanHuongDan { get; set; }
        public string GhiChu { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int? DeptID { get; set; }
        public int? CategoryID { get; set; }
        public int? JobTitleID { get; set; }
        public int? PositionID { get; set; }

        public DateTime? NgayKham { get; set; }
        public string PhamViKham { get; set; }
        public double? ChieuCao { get; set; }
        public double? CanNang { get; set; }
        public string NhomMau { get; set; }
    }

    public class QuanLySucKhoeModel : QuanLySucKhoe
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public string StaffCode { get; set; }
        public string HealthPeriodCode { get; set; }
        public string FullName { get; set; }
        public string DeptName { get; set; }
        public string XepLoaiSucKhoeName { get; set; }
    }
    public class QuanLySucKhoeCreateModel : QuanLySucKhoe
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public QuanLySucKhoeCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
    public class QuanLySucKhoeUpdateModel : QuanLySucKhoe
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public QuanLySucKhoeUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
