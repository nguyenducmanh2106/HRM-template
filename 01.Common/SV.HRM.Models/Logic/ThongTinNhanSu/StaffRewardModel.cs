using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class StaffRewardBase
    {
        public int StaffRewardID { get; set; }
    }
    public class StaffReward : StaffRewardBase
    {

        //ngày cấp
        public DateTime? RewardDate { get; set; }

        //Số quyết định
        public string RewardDecisionNo { get; set; }

        //loại khen thưởng | 1:Khen thưởng cá nhân - 2:Khen thưởng phòng ban - 3:Khen thưởng chi nhánh - 4: Khen thưởng công ty
        public int? RewardType { get; set; }

        //danh hiệu khen thưởng
        public int? RewardID { get; set; }
        public int? StaffID { get; set; }
        public int? DeptID { get; set; }

        //id của chi nhánh
        public int? BranchID { get; set; }

        //ghi chú
        public string Note { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        //Từ năm
        public string FromYear { get; set; }

        //Đến năm
        public string ToYear { get; set; }

        //id công ty- map với bảng company
        public int? CompanyID { get; set; }
    }
    public class StaffRewardModel : StaffReward
    {
        public string FullName { get; set; }
        public string DeptName { get; set; }
        public string RewardName { get; set; }
        public string RewardTypeName { get; set; }
        public string CompanyName { get; set; }
        public List<HRM_AttachmentModel> FileUpload { get; set; }
    }

    public class StaffRewardCreateModel : StaffReward
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffRewardCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
    public class StaffRewardUpdateModel : StaffReward
    {
        public List<HRM_AttachmentModel> FileUpload { get; set; }
        public StaffRewardUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
