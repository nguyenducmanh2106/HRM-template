using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class StaffSalary : StaffSalaryBaseModel
    {
        public int StaffID { get; set; }
        public int? NhomNgachLuongID { get; set; }
        public int? NgachLuongID { get; set; }
        public int? BacLuongID { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime? NgayQuyetDinh { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public float? LuongCoSo { get; set; }
        public float? HeSoLuong { get; set; }
        public string CurrencyID { get; set; }
        public string GhiChu { get; set; }
        public float? PhuCap1 { get; set; }
        public float? PhuCap2 { get; set; }
        public float? PhuCap3 { get; set; }
        public float? PhuCap4 { get; set; }
        public float? PhuCap5 { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public int? LoaiNangLuong { get; set; }
        public float? HeSoBaoLuu { get; set; }
        public float? HeSoThamNienVuotKhung { get; set; }
        public int? SoThangNangLuongTruocHan { get; set; }
        public int? SoThangKeoDaiNangLuong { get; set; }
    }
    public class StaffSalaryBaseModel
    {
        public int StaffSalaryID { get; set; }
    }

    public class StaffSalaryModel : StaffSalary
    {


        //Thuộc Tính Thêm
        public string TenBacLuong { get; set; }
        public string CurrencyName { get; set; }
        public string TenNhomNgachLuong { get; set; }
        public string TenNgachLuong { get; set; }
        public bool? IsLevelMax { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }

        public int? ThoiGianNangLuong { get; set; }

        public string TenLoaiNangLuong { get; set; }


    }

    public class StaffSalaryCreateRequestModel : StaffSalary
    {


        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffSalaryCreateRequestModel()
        {
            CreatedOnDate = DateTime.Now;
            CreatedByUserId = 0;
            LastModifiedByUserId = 0;
        }
    }

    public class StaffSalaryUpdateRequestModel : StaffSalary
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffSalaryUpdateRequestModel()
        {
            LastModifiedByUserId = 0;
            LastModifiedOnDate = DateTime.Now;
        }
    }
}
