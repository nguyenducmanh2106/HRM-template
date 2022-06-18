using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class StaffFamilyBase
    {
        public int StaffFamilyID { get; set; }
    }
    public class StaffFamily: StaffFamilyBase
    {
        public int StaffID { get; set; }

        //quan hệ gia đình
        public int? Relationship { get; set; }

        //Ngày sinh
        public DateTime? Birthday { get; set; }

        //họ tên
        public string FullName { get; set; }

        //địa chỉ/quê quán
        public string Addr { get; set; }

        //số điện thoại
        public string Telephone { get; set; }
        public string Education { get; set; }

        //Nghệ nghiệp hiện hay
        public string Job { get; set; }

        //quốc tịch
        public int? TerritoryID { get; set; }
        public int? LinkStaffID { get; set; }
        public string Note { get; set; }

        //Mã số BHXH
        public string ExtraText1 { get; set; }

        //Mã hộ gia đình
        public string ExtraText2 { get; set; }

        //năm mất
        public string ExtraText3 { get; set; }
        public string ExtraText4 { get; set; }

        //Mã số thuế người phụ thuộc
        public string ExtraText5 { get; set; }
        public string ExtraText6 { get; set; }
        public string ExtraText7 { get; set; }
        public string ExtraText8 { get; set; }
        public string ExtraText9 { get; set; }
        public string ExtraText10 { get; set; }

        //giới tính
        public double? ExtraNumber1 { get; set; }

        //Dân tộc
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

        //đã mất
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

        //Số chứng minh nd/CCCD
        public string IDCardNo { get; set; }

        //Mã số thuế cá nhân
        public string PITCode { get; set; }

        //Ngày cấp
        public DateTime? IDCardIssueDate { get; set; }

        //Ngày hết hạn
        public DateTime? IDCardExpireDate { get; set; }

        //Nơi cấp
        public int? IDCardIssuePlaceID { get; set; }

        //Thu nhập
        public double? Income { get; set; }
        public string CurrencyID { get; set; }

        //Quyển số
        public string CerID { get; set; }

        //Số giấy khai sinh
        public string CerNumber { get; set; }

        //Quốc tịch
        public int? CerTerritoryID { get; set; }

        //Tỉnh/TP
        public int? CerLocationID { get; set; }

        //Quận/huyện
        public int? CerDistrictID { get; set; }

        //Phường/xã
        public int? CerWardID { get; set; }
        public int? TaxRelationship { get; set; }

        //Nơi ở hiện tại
        public string TypeHouse { get; set; }
        public string StayWith { get; set; }
        public string HoKhauThuongTru { get; set; }
    }
    public class StaffFamilyModel: StaffFamily
    {
        public string CountryName { get; set; }
        public string FamilyRelationshipName { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }

        public string LocationName { get; set; }
        public string EthnicName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }


        //tên nơi cấp
        public string IDCardIssuePlaceName { get; set; }
    }

    public class StaffFamilyCreateModel : StaffFamily
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffFamilyCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }

    public class StaffFamilyUpdateModel : StaffFamily
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffFamilyUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }

}
