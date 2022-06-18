using SV.HRM.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SV.HRM.Models
{
    public class EntityGeneric
    {
        public string Columns { get; set; }
        [JsonConverter(typeof(DictionaryStringObjectJsonConverter))]
        public Dictionary<string, object> CustomPagingData { get; set; }
        public bool IsGetCache { get; set; }
        public string LayoutCode { get; set; }
        public List<Filters> Filters { get; set; }
        public EntityGeneric()
        {
            IsGetCache = false;
        }
        public string TextSearch { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<SortByClass> Sorts { get; set; }
        public List<TableFieldExportExcel> ColumnExports { get; set; }
        public string Store { get; set; }
        public string ReportName { get; set; }
        public string FileDownload { get; set; }
    }

    public class SortByClass
    {
        public string SortBy { get; set; }
        public TypeOrder Type { get; set; }
    }
    public enum TypeOrder
    {
        ASC = 0,
        DESC = 1,
    }

    public class BaseCheckDuplicate
    {
        //Dữ liệu truyền vào để check
        [JsonConverter(typeof(DictionaryStringObjectJsonConverter))]
        public Dictionary<string, object> CustomData { get; set; }
        //Tên trường check - dùng để map với file json đọc store tương ứng
        public string FieldName { get; set; }

        //Tên bảng check - dùng để map với file json đọc store tương ứng
        public string TableName { get; set; }
    }

    public class Filters
    {
        [Column]
        public string name { get; set; }
        [Column]
        public dynamic value { get; set; }
        [Column]
        public int type { get; set; }
        public string label { get; set; }
    }
}
