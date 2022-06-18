using SV.HRM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Models
{
    public class HRM_AttachmentModel
    {
        public Guid Id { get; set; }
        public Guid NodeId { get; set; }
        public int Type { get; set; }
        public int TypeId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string PhysicalName { get; set; }
        public int? Size { get; set; }
        public string Extension { get; set; }

        public string PhysicalPath { get; set; }
    }
}
