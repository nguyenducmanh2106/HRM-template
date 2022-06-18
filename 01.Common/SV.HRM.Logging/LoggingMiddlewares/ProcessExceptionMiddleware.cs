// using SV.HRM.Logging.ExceptionCustom;
// using SV.HRM.Logging.Models;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
// using System;
// using System.Collections.Generic;
// using System.Net;
// using System.Text;
// using System.Threading.Tasks;
//
// namespace SV.HRM.Logging.LoggingMiddlewares
// {
//     public class ProcessExceptionMiddleware : BaseCustomMiddleware
//     {
//         public readonly ILogger logger;
//         public ProcessExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory) : base(next)
//         {
//             logger = loggerFactory.CreateLogger("HttpContext");
//         }
//
//         public override async Task InvokeAsync(HttpContext context)
//         {
//             ErrorModel error = null;
//             try
//             {
//                 await next(context);
//             }
//             catch (Exception e)
//             {
//                 error = new ErrorModel
//                 {
//                     StatusCode = (int)GetHttpStatusCode(e),
//                     Messages = new List<string> { e.Message }
//                 };
//                 logger.LogError(e, e.Message);
//             }
//
//             if (error != null)
//                 await WriteExceptionResponse(context, error);
//         }
//
//         private HttpStatusCode GetHttpStatusCode(Exception exception)
//         {
//             var exceptionType = exception.GetType();
//             if (exceptionType.Equals(typeof(NotFoundException)))
//                 return HttpStatusCode.NotFound;
//             return HttpStatusCode.InternalServerError;
//         }
//     }
// }

