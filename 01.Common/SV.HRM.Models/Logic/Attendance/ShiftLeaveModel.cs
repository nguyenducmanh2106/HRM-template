using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class ShiftLeaveModel
    {
        public string ShiftLeaveID { get; set; }
        public string ShiftLeaveCode { get; set; }
        public string ShiftLeaveName { get; set; }
        public int ShiftLeaveType { get; set; }
    }
    public class ShiftLeaveComboboxModel : ShiftLeaveModel
    {

    }
}
