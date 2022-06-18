using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class StaffPartyBase
    {
        public int StaffPartyID { get; set; }
    }
    public class StaffParty: StaffPartyBase
    {
        //id của nhân viên
        public int? StaffID { get; set; }

        //từ ngày
        public DateTime? TuNgay { get; set; }

        //đến ngày
        public DateTime? DenNgay { get; set; }

        //id chi bộ - lấy từ bảng PartyCell
        public int? ChiBoID { get; set; }

        //chức vụ đảng id - lấy từ bảng PartyTitle
        public int? ChucVuDangID { get; set; }

        
        public string LyLuanChinhTri { get; set; }
        public DateTime? NgayVaoDang { get; set; }
        public DateTime? NgayVaoDangChinhThuc { get; set; }
        public string NoiKetNapDang { get; set; }
        public string NoiCongNhan { get; set; }
        public string SoLiLich { get; set; }
        public string SoTheDangVien { get; set; }
        public DateTime? NgayChuyenDen { get; set; }
        public string GhiChu { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string NgheNghiepTruocKhiVaoDang { get; set; }
        public string NoiChuyenDen { get; set; }
        public DateTime? NgayChuyenDi { get; set; }
        public string NoiChuyenDi { get; set; }
        public DateTime? NgayBiChet { get; set; }
        public string LyDoChet { get; set; }
        public DateTime? NgayRaKhoiDang { get; set; }
        public string HinhThucRaKhoiDang { get; set; }
    }
    public class StaffPartyModel: StaffParty
    {
        public string PartyCellName { get; set; }
        public string PartyTitleName { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }

    public class StaffPartyCreateModel : StaffParty
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffPartyCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }

    public class StaffPartyUpdateModel : StaffParty
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffPartyUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
