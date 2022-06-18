using System;
using System.Collections.Generic;

namespace SV.HRM.Models
{
    public class DashboardModel
    {
        public class BaseFilter
        {
            public int? OrganizationId { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }

        public class BoxFilter : BaseFilter
        {
            public int? HealthPeriodID { get; set; }
            public int? Month { get; set; }
            public int? Year { get; set; }
            public DateTime? DateNow { get; set; }
        }

        public class BoxModel
        {
            public int CountVC { get; set; } //Số nhân viên viên chức
            public int Count68 { get; set; } //Số nhân viên hợp đồng 68
            public int CountShortTime { get; set; } //Số nhân viên hợp đồng ngắn hạn
            public int TotalCountStaff { get; set; } //Tổng só nhân viên
        }

        public class OccupationObject
        {
            public int? OccupationID { get; set; }
            public string OccupationName { get; set; }
            public int Counter { get; set; }
            public int? Order { get; set; }
        }

        public class TrinhDoChuyenMonObject
        {
            public int? TrinhDoDaoTaoID { get; set; }
            public string TrinhDoDaoTaoIDs { get; set; }
            public string TrinhDoDaoTaoName { get; set; }
            public int Counter { get; set; }
            public int? Order { get; set; }
        }

        public class DonutsChartOptionsResult
        {
            public List<int> Series { get; set; } = new List<int>();
            public List<string> Labels { get; set; } = new List<string>();
            public List<string> ObjectIDs { get; set; } = new List<string>();
        }

        public class LineChart
        {
            public string Name { get; set; }
            public int[] Data { get; set; } = new int[12];
        }

        public class LineChartOptionsResult
        {
            public List<LineChart> Series { get; set; } = new List<LineChart>();
            public List<string> XAxis { get; set; } = new List<string>();
        }
    }
}
