using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    public class ResponseModel
    {
        // RULES
        // If success, status code = 1, message is empty, data is not null
        // If error, status code = 0, message is error message, data is null

        public ResponseModel() { ID = 0; Title = ""; Error = false; Data = null; DelayTime = 0; }
        /// <summary>
        /// ID của bản ghi được thêm, sửa, xóa
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Thông báo
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Có lỗi hay không có lỗi
        /// </summary>
        public bool Error { get; set; }
        public int NextAction { get; set; } // 0: ko làm gì, 1: Redirect, 2: Mở tab
        public int DelayTime { get; set; } //đơn vị milisecon 
        /// <summary>
        /// Đối tượng attach kèm theo thông báo
		/// </summary>
		public object Data { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public int DataCount { get; set; }
        public int TotalCount { get; set; }
    }
}
