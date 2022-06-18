using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class ProfileDocumentBase
    {
        public int ProfileDocumentID { get; set; }
    }
    public class ProfileDocument : ProfileDocumentBase
    {
        //Tên loại và trích yếu nội dung
        public string ProfileDocumentName { get; set; }

        //Số và ký hiệu
        public string ProfileDocumentNo { get; set; }

        //Ngày ban hành
        public DateTime? IssueDate { get; set; }

        //Nơi ban hành
        public string IssuePlace { get; set; }

        //ID nhân viên
        public int? StaffID { get; set; }

        //Ghi chú

        public string Note { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
    public class ProfileDocumentModel : ProfileDocument
    {
        public int Type { get; set; }
        //Số file
        public int FileRecord { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }
    public class ProfileDocumentCreate : ProfileDocument
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }
    public class ProfileDocumentUpdate : ProfileDocument
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }

}
