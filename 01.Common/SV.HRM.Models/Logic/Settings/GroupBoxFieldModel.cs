using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SV.HRM.Models
{
    [Table("GroupBoxField")]
    public class GroupBoxField
    {
        public int ID { get; set; }


        /// <summary>
        /// Trường chứa fieldname dùng để map dữ liệu với model trong FrontEnd
        /// </summary>
        [Column("Name")]
        public string name { get; set; }

        /// <summary>
        /// giá trị của input field
        /// </summary>
        [Column("Value")]
        public string value { get; set; }

        /// <summary>
        /// Kiểu loại input
        /// </summary>
        [Column("Type")]
        public int? type { get; set; }

        /// <summary>
        /// Các cấu hình của input field
        /// </summary>
        [Column("Config")]
        public string config { get; set; }

        /// <summary>
        /// input field có yêu cầu bắt buộc không
        /// </summary>
        [Column("Required")]
        public bool? required { get; set; }

        /// <summary>
        /// tên label của input field
        /// </summary>
        [Column("Lable")]
        public string label { get; set; }

        /// <summary>
        /// place holder của input
        /// </summary>
        [Column("Placeholder")]
        public string placeholder { get; set; }

        /// <summary>
        /// permission input 
        /// </summary>
        [Column("PermissionDisable")]
        public int? permissionDisable { get; set; }

        /// <summary>
        /// độ rộng tối thiểu của lable
        /// </summary>
        [Column("MinWidthLabel")]
        public string minWidthLabel { get; set; }

        /// <summary>
        /// ToolTip của input
        /// </summary>
        [Column("ToolTip")]
        public string toolTip { get; set; }

        /// <summary>
        /// Input field có đang được sử dụng hay không
        /// </summary>
        [Column("IsUse")]
        public bool? isUse { get; set; }

        /// <summary>
        /// Input đang thuộc cột thứ mấy của groupbox
        /// </summary>
        [Column("ColumnNumber")]
        public int? columnNumber { get; set; }

        /// <summary>
        /// ID của groupbox - map với bảng GroupBox
        /// </summary>
        [Column("GroupBoxID")]
        public int? groupBoxID { get; set; }

        /// <summary>
        /// Thứ tự của groupBox
        /// </summary>
        [Column("SortOrder")]
        public int? sortOrder { get; set; }
    }
    public class GroupBoxFieldModel : GroupBoxField
    {
    }
}
