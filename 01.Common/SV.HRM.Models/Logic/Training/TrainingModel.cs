
using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class QuanLyDaoTaoBase
    {
        public int QuanLyDaoTaoID { get; set; }
    }
    public class QuanLyDaoTao : QuanLyDaoTaoBase
    {
        public int? StaffID { get; set; }

        public int? ChuyenNganhID { get; set; }
        public int? DiplomaID { get; set; }
        public int? TrinhDoDaoTaoID { get; set; }
        public int? HinhThucDaoTao { get; set; }
        public int? DonViDaoTaoID { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public double? PhuCapChucVu { get; set; }
        public bool? TrangThai { get; set; }
        public string XepLoai { get; set; }
        public DateTime? NgayCapVBCC { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime? NgayQuyetDinh { get; set; }
        public double? BenhVienHoTro { get; set; }
        public double? CaNhanTuTuc { get; set; }
        public double? ChiPhiKhac { get; set; }
        public string GhiChu { get; set; }
        public int CreatedBy { get; set; }
        public int? QuyDoiSoTiet { get; set; }
        public DateTime? ThoiGianDiHoc { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class QuanLyDaoTaoModel : QuanLyDaoTao
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public string StaffCode { get; set; }
        public string FullName { get; set; }

        public string ChuyenNganhName { get; set; }
        public string DiplomaName { get; set; }
        public string TrinhDoDaoTaoName { get; set; }
        public string DonViDaoTaoName { get; set; }
    }
    public class QuanLyDaoTaoCreateModel : QuanLyDaoTao
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public QuanLyDaoTaoCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
    public class QuanLyDaoTaoUpdateModel : QuanLyDaoTao
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public QuanLyDaoTaoUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
