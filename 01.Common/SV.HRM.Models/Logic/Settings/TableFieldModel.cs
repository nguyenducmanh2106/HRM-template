using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SV.HRM.Models
{
    [Table("TableField")]
    public class TableField
    {
        [Key]
        [Column("ID")]
        public Guid ID { get; set; }

        //Tên của phân hệ
        [Column("LayoutCode")]
        public string LayoutCode { get; set; }

        [Column("FilterParams")]
        public string filterParams { get; set; }

        //Loại - dùng để phân biệt khi render kiểu dữ liệu theo từng cột
        [Column("Type")]
        public int? type { get; set; }

        //Cột có bị ẩn không
        [Column("Hide")]
        public bool? hide { get; set; }

        //Độ rộng của cột
        [Column("Width")]
        public int width { get; set; }

        //Tên của cột map dữ liệu với api trả về
        [Column("Field")]
        public string field { get; set; }

        //Tên của cột map dữ liệu biến trong store dùng để lọc
        [Column("FieldFilter")]
        public string fieldFilter { get; set; }

        //Tên tiêu đề của cột
        [Column("HeaderName")]
        public string headerName { get; set; }

        //Thứ tự của cột
        [Column("SortOrder")]
        public int? sortOrder { get; set; }

        //ID của người sử dụng
        [Column("UserID")]
        public int? userID { get; set; }

        //Cho phép hiển thị bộ lọc trên cột header không
        [Column("SuppressMenu")]
        public bool? suppressMenu { get; set; }

        //ID của cột cha
        [Column("ParentColumnKey")]
        public int? parentColumnKey { get; set; }

        //ID của cột dùng để tạo quan hệ cha con
        [Column("ColumnKey")]
        public int? columnKey { get; set; }
    }
    public class TableFieldModel : TableField
    {
        //public List<TableFieldModel> children { get; set; }
    }
    public class TableFieldExportExcel
    {
        public Guid ID { get; set; }

        public string LayoutCode { get; set; }

        public string fieldFilter { get; set; }

        public int? type { get; set; }

        public bool? hide { get; set; }

        public int width { get; set; }

        public string field { get; set; }

        public string headerName { get; set; }

        public int? sortOrder { get; set; }

        public int? userID { get; set; }

        public bool? suppressMenu { get; set; }

        public int? parentColumnKey { get; set; }

        public int? columnKey { get; set; }
        public List<TableFieldExportExcel> children { get; set; }
    }
}
