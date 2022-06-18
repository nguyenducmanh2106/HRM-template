using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class WKTime
    {
        public int TACategoryID { get; set; }
        public DateTime WKDate { get; set; }
        public double? WKDay { get; set; }
        public double? WKHour { get; set; }
        public int? OVTTypeInShift { get; set; }
        public int? OVTTypeOutShift { get; set; }
        public string Note { get; set; }
        public bool? LeaveWithSalary { get; set; }
    }
    public class WKTimeModel : WKTime
    {
        public string TACategoryName { get; set; }
        public int RowNum { get; set; }
    }
    public class WKTimeCreate : WKTime
    {

    }
    public class WKTimeRequestCreate : WKTime
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ExceptDayOfWeek { get; set; }
    }
}
