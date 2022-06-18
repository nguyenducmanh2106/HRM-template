using Microsoft.AspNetCore.Http;

namespace SV.HRM.Models
{
    public class ImportExcelRequest
    {
        /// <summary>
        ///File tải lên
        /// </summary>
        public IFormFile File { get; set; }
    }

    public class ImportExcelServiceModel
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }
        public string DataType { get; set; }
    }
}
