using System.Collections.Generic;
using System.Net;

namespace SV.HRM.Logging.Models
{
    public class ErrorModel
    {
        public ErrorModel(HttpStatusCode statusCode, List<string> messages)
        {
            StatusCode = (int)statusCode;
            Messages = messages;
        }

        public ErrorModel()
        {
        }

        public int StatusCode { get; set; }
        public List<string> Messages { get; set; }
        public object AdditionalData { get; set; }
    }
}