using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models.Common
{
    public class FilterResultModel<T>
    {
        public IList<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int DataCount { get; set; }
        public int? ElapsedMilliseconds { get; set; }

        //For elasticsearch
        public string DebugInfomation { get; set; }
        public string ScrollId { get; set; }
        public string Scroll { get; set; }
        public Dictionary<string, dynamic> ListHighlightOtherField { get; set; }
    }
}
