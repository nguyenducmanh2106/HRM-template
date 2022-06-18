using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class StaffDiplomaBaseModel
    {
        public int StaffDiplomaID { get; set; }
        public string DiplomaNo { get; set; }
    }
    public class StaffDiploma : StaffDiplomaBaseModel
    {
        public int? IssueYear { get; set; }
        public int? StaffID { get; set; }
        public int? DiplomaID { get; set; }
        public int? SchoolID { get; set; }
        public int? SpecialityID { get; set; }
        public string MainSubject { get; set; }
        public int? DegreeID { get; set; }
        public double? Mark { get; set; }
        public string Note { get; set; }
        public string ExtraText1 { get; set; }
        public string ExtraText2 { get; set; }
        public string ExtraText3 { get; set; }
        public string ExtraText4 { get; set; }
        public string ExtraText5 { get; set; }
        public string ExtraText6 { get; set; }
        public string ExtraText7 { get; set; }
        public string ExtraText8 { get; set; }
        public string ExtraText9 { get; set; }
        public string ExtraText10 { get; set; }



        //Chuyên ngành/Khóa đào tạo
        public double? ExtraNumber1 { get; set; }
        public double? ExtraNumber2 { get; set; }
        public double? ExtraNumber3 { get; set; }
        public double? ExtraNumber4 { get; set; }
        public double? ExtraNumber5 { get; set; }
        public DateTime? ExtraDate1 { get; set; }
        public DateTime? ExtraDate2 { get; set; }
        public DateTime? ExtraDate3 { get; set; }
        public DateTime? ExtraDate4 { get; set; }
        public DateTime? ExtraDate5 { get; set; }
        public bool? ExtraLogic1 { get; set; }
        public bool? ExtraLogic2 { get; set; }
        public bool? ExtraLogic3 { get; set; }
        public bool? ExtraLogic4 { get; set; }
        public bool? ExtraLogic5 { get; set; }
        public string SysExtraText1 { get; set; }
        public string SysExtraText2 { get; set; }
        public string SysExtraText3 { get; set; }
        public string SysExtraText4 { get; set; }
        public string SysExtraText5 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string ThoiGianHoc { get; set; }
        public string LoaiBang { get; set; }
        public int? TrinhDoDaoTaoID { get; set; }
        public int? TrinhDoChuyenMonID { get; set; }
        public int? ChuyenKhoaID { get; set; }
    }
    public class StaffDiplomaModel : StaffDiploma
    {
        //Thuộc tính thêm
        public string SchoolName { get; set; }
        public string DiplomaName { get; set; }
        public string TrinhDoDaoTaoName { get; set; }
        public string TrinhDoChuyenMonName { get; set; }
        public string SpecialityName { get; set; }
        public string TenChuyenKhoa { get; set; }
        public string ChuyenNganhName { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }

    public class StaffDiplomaCreateRequestModel : StaffDiploma
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffDiplomaCreateRequestModel()
        {
            CreationDate = DateTime.Now;
            CreatedBy = 0;
        }
    }

    public class StaffDiplomaUpdateRequestModel : StaffDiploma
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffDiplomaUpdateRequestModel()
        {
            LastUpdatedDate = DateTime.Now;
            LastUpdatedBy = 0;
        }
    }

}
