using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class ReportEntity
    {
        public string store { get; set; }
        public string reportName { get; set; }
        public string ExtraText { get; set; }
        public DateTime tuNgay { get; set; }
        public DateTime denNgay { get; set; }
    }

    public class ReportFileEntity
    {
        public string store { get; set; }
        public string reportName { get; set; }
        public DateTime tuNgay { get; set; }
        public DateTime denNgay { get; set; }

    }
}
