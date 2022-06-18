using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SV.HRM.Models
{
    [Table("GroupBox")]
    public class GroupBox
    {
        public int ID { get; set; }

        /// <summary>
        /// Tên của group box
        /// </summary>
        [Column("GroupBoxText")]
        public string groupBoxText { get; set; }

        /// <summary>
        /// Loại group box - Table hay Section 
        /// </summary>
        [Column("TypeBox")]
        public int? typeBox { get; set; }

        /// <summary>
        /// Group box có được sử dụng chưa
        /// </summary>
        [Column("IsUse")]
        public bool? isUse { get; set; }

        /// <summary>
        /// sắp xếp group box
        /// </summary>
        [Column("SortOrder")]
        public int? sortOrder { get; set; }

        /// <summary>
        /// id của người sử dụng
        /// </summary>
        [Column("UserID")]
        public int? userID { get; set; }

        /// <summary>
        /// Loại form xuất hiện
        /// </summary>
        [Column("FormType")]
        public int? formType { get; set; }

        /// <summary>
        /// Mã của groupbox
        /// </summary>
        [Column("Code")]
        public string? code { get; set; }
    }
    public class GroupBoxModel : GroupBox
    {
        public List<GroupBoxField> colOne { get; set; }
        public List<GroupBoxField> colTwo { get; set; }
    }

    public class GroupBoxFieldUpdate
    {
        public List<GroupBox> GroupBoxes { get; set; }
        public List<GroupBoxField> GroupBoxFields { get; set; }
    }
}
