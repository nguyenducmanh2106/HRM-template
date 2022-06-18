using System;
namespace SV.HRM.Models
{
    public class HealthPeriodBase
    {
        public int HealthPeriodID { get; set; }
        public string HealthPeriodCode { get; set; }
    }
    public class HealthPeriod : HealthPeriodBase
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Note { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
    public class HealthPeriodModel : HealthPeriod
    {
    }
    public class HealthPeriodCreateModel : HealthPeriod
    {
        public HealthPeriodCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class HealthPeriodUpdateModel : HealthPeriod
    {
        public HealthPeriodUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
