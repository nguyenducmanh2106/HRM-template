using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class RewardBase
    {
        public int RewardID { get; set; }
        public string RewardCode { get; set; }
        public string RewardName { get; set; }
    }
    public class Reward : RewardBase
    {
        public bool? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
    public class RewardModel : Reward
    {
    }

    public class RewardCreateModel : Reward
    {
        public RewardCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class RewardUpdateModel : Reward
    {
        public RewardUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
