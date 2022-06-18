using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models.Common
{
    public class BaseQueryFilter
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string TextSearch { get; set; }
        public string UserId { get; set; }
        public string ApplicationId { get; set; }
        public QueryOrder Order { get; set; }
    }
}
