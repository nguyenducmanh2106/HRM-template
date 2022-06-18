using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class ImportExcelHttpService:IImportExcelHttpService
    {
        private IHttpHelper _httpHelper;
        public ImportExcelHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<string>> ImportStaffData([FromBody] ImportExcelRequest formData, string dataType = "")
        {
            var fileContent = string.Empty;
            if (formData.File.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    formData.File.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    fileContent = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data
                }
            }
            var model = new ImportExcelServiceModel()
            {
                FileName = formData.File.FileName,
                FileContent = fileContent,
                DataType = dataType
            };
            return await _httpHelper.PostAsync<Response<string>>("ImportExcel/ImportStaffData?dataType="+dataType, model);
        }
    }
}
