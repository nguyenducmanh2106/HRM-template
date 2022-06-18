using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class DisciplineTypeBase
    {
        public int DisciplineTypeID { get; set; }
        public string DisciplineTypeCode { get; set; }
        public int? DisciplineType { get; set; }

    }
    public class DisciplineType : DisciplineTypeBase
    {
        
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public int? Mark { get; set; }
        public string Note { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
    public class DisciplineTypeModel : DisciplineType
    {
    }
    public class DisciplineTypeCreateModel : DisciplineType
    {
        public DisciplineTypeCreateModel()
        {
            CreatedBy = 0;
            CreationDate = DateTime.Now;
        }
    }

    public class DisciplineTypeUpdateModel : DisciplineType
    {
        public DisciplineTypeUpdateModel()
        {
            LastUpdatedBy = 0;
            LastUpdatedDate = DateTime.Now;
        }
    }
}
