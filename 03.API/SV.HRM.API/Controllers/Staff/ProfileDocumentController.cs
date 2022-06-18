using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers.Staff
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProfileDocumentController
    {
        private readonly IProfileDocumentHttpService _profileDocumentHttpService;
        private readonly IStaffHttpService _staffHttpService;

        public ProfileDocumentController(IProfileDocumentHttpService profileDocumentHttpService, IStaffHttpService staffHttpService)
        {
            _profileDocumentHttpService = profileDocumentHttpService;
            _staffHttpService = staffHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<ProfileDocumentModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _profileDocumentHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới hồ sơ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(ProfileDocumentCreate model)
        {
            return await _profileDocumentHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng ProfileDocument
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ProfileDocumentModel>> FindById(int recordID)
        {
            return await _profileDocumentHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _profileDocumentHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm cập nhật hồ sơ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, ProfileDocumentUpdate model)
        {
            return await _profileDocumentHttpService.Update(id, model);
        }


        [HttpPost]
        public async Task<Response<IActionResult>> ExportExcel([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                List<ProfileDocument> persons = new List<ProfileDocument>();
                var resultDocument = await _profileDocumentHttpService.GetFilter(queryFilter);
                if (resultDocument != null)
                {
                    //diennv
                    persons = (List<ProfileDocument>)(resultDocument.Data?.Select(g => new ProfileDocument() { 
                        ProfileDocumentID = g.ProfileDocumentID,
                        ProfileDocumentName = g.ProfileDocumentName,
                        ProfileDocumentNo = g.ProfileDocumentNo,
                        IssueDate = g.IssueDate,
                        IssuePlace = g.IssuePlace,
                        StaffID = g.StaffID,
                        Note = g.Note
                    })?.ToList());
                    //end diennv
                }

                StaffDetailModel infoStaff = new StaffDetailModel();
                var resultStaff = await _staffHttpService.GetStaffGeneralInfoById(Convert.ToInt32(queryFilter.CustomPagingData["StaffID"]));
                if (resultStaff != null)
                {
                    infoStaff = resultStaff.Data;
                }
                // Lets converts our object data to Datatable for a simplified logic.
                // Datatable is most easy way to deal with complex datatypes for easy reading and formatting.

                DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(persons), (typeof(DataTable)));

                //Create a blank workbook
                IWorkbook workbook = new XSSFWorkbook(); // create *.xlsx file, use HSSFWorkbook() for creating *.xls file.
                ISheet excelSheet = workbook.CreateSheet("Danh_Sach_Ho_So");

                //style export
                var font = workbook.CreateFont();
                //font.FontHeightInPoints = 13;
                // diennv ycau fontsize 12
                font.FontHeightInPoints = 12;
                font.FontName = "Times New Roman";
                font.IsBold = true;

                var fontNormal = workbook.CreateFont();
                fontNormal.FontHeightInPoints = 13;
                fontNormal.FontName = "Times New Roman";
                fontNormal.IsBold = false;

                var cellStyleBorder = workbook.CreateCellStyle();
                cellStyleBorder.BorderBottom = BorderStyle.Thin;
                cellStyleBorder.BorderLeft = BorderStyle.Thin;
                cellStyleBorder.BorderRight = BorderStyle.Thin;
                cellStyleBorder.BorderTop = BorderStyle.Thin;
                cellStyleBorder.Alignment = HorizontalAlignment.Center;
                cellStyleBorder.VerticalAlignment = VerticalAlignment.Center;

                var cellStyleTitle = workbook.CreateCellStyle();
                cellStyleTitle.FillPattern = FillPattern.SolidForeground;
                ((XSSFCellStyle)cellStyleTitle).SetFillForegroundColor(new XSSFColor(new byte[] { 255, 255, 255 }));
                cellStyleTitle.SetFont(font);


                var cellStyleBorderAndColorWhite = workbook.CreateCellStyle();
                cellStyleBorderAndColorWhite.CloneStyleFrom(cellStyleBorder);
                cellStyleBorderAndColorWhite.FillPattern = FillPattern.SolidForeground;
                ((XSSFCellStyle)cellStyleBorderAndColorWhite).SetFillForegroundColor(new XSSFColor(new byte[] { 255, 255, 255 }));
                cellStyleBorderAndColorWhite.SetFont(font);

                var cellStyleNormal = workbook.CreateCellStyle();
                cellStyleNormal.CloneStyleFrom(cellStyleBorder);
                cellStyleNormal.FillPattern = FillPattern.SolidForeground;
                ((XSSFCellStyle)cellStyleNormal).SetFillForegroundColor(new XSSFColor(new byte[] { 255, 255, 255 }));
                cellStyleNormal.SetFont(fontNormal);


                List<TableFieldExportExcel> columnExports = queryFilter.ColumnExports;

                MemoryStream ms = new MemoryStream();
                using (MemoryStream tempStream = new MemoryStream())
                {
                    List<TableFieldExportExcel> columns = new List<TableFieldExportExcel>();
                    IRow row = excelSheet.CreateRow(0);
                    int columnIndexMerge = 0;
                    foreach (TableFieldExportExcel column in columnExports)
                    {
                        if ((column.hide == null || column.hide != true) && column.field != "FileUpload")
                        {
                            columns.Add(column);
                            var cellTitle = row.CreateCell(columnIndexMerge);
                            cellTitle.CellStyle = cellStyleTitle;
                            columnIndexMerge++;
                        }
                    }

                    // bỏ merge 
                    //var cra = new CellRangeAddress(0, 0, 0, columnIndexMerge - 1);
                    //excelSheet.AddMergedRegion(cra);

                    // diennv 30/2/2022
                    // bỏ cột danh mục hồ sơ 
                    //ICell cell = excelSheet.GetRow(0).GetCell(0);
                    //cell.SetCellType(CellType.String);
                    //cell.SetCellValue("Danh mục hồ sơ");
                    //excelSheet.GetRow(0).GetCell(columnIndexMerge - 1).CellStyle = cellStyleBorderAndColorWhite;

                    var rowOne = excelSheet.CreateRow(0);
                    var rowOneCellOne = rowOne.CreateCell(0);
                    rowOneCellOne.SetCellValue("Họ và tên : ");
                    rowOneCellOne.CellStyle = cellStyleTitle;
                    var rowOneCellTwo = rowOne.CreateCell(2);
                    rowOneCellTwo.SetCellValue(infoStaff.FullName);
                    rowOneCellTwo.CellStyle = cellStyleTitle;

                    // merge cell 0 và 1 của row vì column STT bé
                    var cra = new CellRangeAddress(0, 0, 0, 1);
                    excelSheet.AddMergedRegion(cra);

                    var rowTwo = excelSheet.CreateRow(1);
                    var rowTwoCellOne = rowTwo.CreateCell(0);
                    rowTwoCellOne.SetCellValue("Ngày sinh : ");
                    rowTwoCellOne.CellStyle = cellStyleTitle;
                    var rowTwoCellTwo = rowTwo.CreateCell(2);
                    rowTwoCellTwo.SetCellValue(infoStaff.Birthday?.ToString("dd/MM/yyyy"));
                    rowTwoCellTwo.CellStyle = cellStyleTitle;

                    var cra1 = new CellRangeAddress(1, 1, 0, 1);
                    excelSheet.AddMergedRegion(cra1);

                    var rowThree = excelSheet.CreateRow(2);
                    var rowThreeCellOne = rowThree.CreateCell(0);
                    rowThreeCellOne.SetCellValue("Mã hồ sơ : ");
                    rowThreeCellOne.CellStyle = cellStyleTitle;
                    var rowThreeCellTwo = rowThree.CreateCell(2);
                    rowThreeCellTwo.SetCellValue(infoStaff.StaffCode);
                    rowThreeCellTwo.CellStyle = cellStyleTitle;

                    var cra2 = new CellRangeAddress(2, 2, 0, 1);
                    excelSheet.AddMergedRegion(cra2);

                    //var rowFour = excelSheet.CreateRow(4);
                    var rowFour = excelSheet.CreateRow(3);
                    int columnIndex = 0;

                    //Render header
                    foreach (var column in columns)
                    {
                        var cellHead = rowFour.CreateCell(columnIndex);
                        cellHead.SetCellValue(column.headerName);
                        cellHead.CellStyle = cellStyleBorderAndColorWhite;
                        columnIndex++;
                    }

                    //merge cell
                    //var cra = new CellRangeAddress(0, 0, 0, 1);
                    //excelSheet.AddMergedRegion(cra);

                    //ICell cell = excelSheet.GetRow(0).GetCell(0);
                    //cell.SetCellType(CellType.String);
                    //cell.SetCellValue("Supplier Provided Data");
                    //cell.CellStyle = cellStyleBorderAndColorGreen;
                    //excelSheet.GetRow(0).GetCell(1).CellStyle = cellStyleBorderAndColorGreen;

                    //Render Data
                    // dien nv đổi 5 thành 4 vì bỏ row hiện danh mục hồ sơ 
                    var rowFive = excelSheet.CreateRow(4);
                    int rowIndex = 4;
                    foreach (DataRow dsrow in table.Rows)
                    {
                        rowFive = excelSheet.CreateRow(rowIndex);
                        int cellIndex = 0;
                        foreach (var col in columns)
                        {
                            var cellRowFive = rowFive.CreateCell(cellIndex);
                            if (dsrow.Table.Columns[col.field] != null)
                            {
                                if (dsrow[col.field].GetType() == typeof(DateTime))
                                {
                                    string strValue = dsrow[col.field] != null ? Convert.ToDateTime(dsrow[col.field]).ToString("dd/MM/yyyy") : "";
                                    cellRowFive.SetCellValue(strValue);
                                }
                                else
                                {
                                    string strValue = dsrow[col.field] != null ? dsrow[col.field]?.ToString() : "";
                                    cellRowFive.SetCellValue(strValue);
                                }
                            }
                            else
                            {
                                if (col.field == "Order")
                                {
                                    cellRowFive.SetCellValue((rowIndex - 4).ToString());
                                }
                                else
                                {
                                    cellRowFive.SetCellValue("");
                                }


                            }
                            cellRowFive.CellStyle = cellStyleNormal;
                            // diennv set wraptext
                            cellRowFive.CellStyle.WrapText = true;
                            cellIndex++;
                        }
                        rowIndex++;
                    }

                    // diennv set width for column
                    // đang lấy đơn vị px
                    for (int i = 0; i < columns.Count(); i++)
                    {
                        // stt
                        if (i == 0)
                        {
                            //excelSheet.SetColumnWidth(0, 15 * 256);
                            // 1 ký tự = 7px = 0.58 = 256th
                            excelSheet.SetColumnWidth(0, (int)(42 / 7 * 256));
                        }
                        // số và ký hiệu
                        if (i == 1)
                        {
                            //excelSheet.SetColumnWidth(0, 15 * 256);
                            // 1 ký tự = 7px = 0.58 = 256th
                            excelSheet.SetColumnWidth(1, (int)(108 / 7 * 256));
                        }
                        // ngày ban hành
                        if (i == 2)
                        {
                            //excelSheet.SetColumnWidth(0, 15 * 256);
                            // 1 ký tự = 7px = 0.58 
                            excelSheet.SetColumnWidth(2, (int)(119 / 7 * 256));
                        }
                        // tên loại và trích yếu nội dung
                        if (i == 3)
                        {
                            //excelSheet.SetColumnWidth(0, 15 * 256);
                            // 1 ký tự = 7px = 0.58 
                            excelSheet.SetColumnWidth(3, (int)(224 / 7 * 256));
                        }
                        // Nơi ban hành
                        if (i == 4)
                        {
                            //excelSheet.SetColumnWidth(0, 15 * 256);
                            // 1 ký tự = 7px = 0.58 
                            excelSheet.SetColumnWidth(4, (int)(124 / 7 * 256));
                        }
                        // tạm thời cho ghi chú và số bản bằng nhau
                        // số bản
                        if (i == 5)
                        {
                            //excelSheet.SetColumnWidth(0, 15 * 256);
                            // 1 ký tự = 7px = 0.58 
                            excelSheet.SetColumnWidth(5, (int)(78 / 7 * 256));
                        }
                        // ghi chú
                        if (i == 6)
                        {
                            //excelSheet.SetColumnWidth(0, 15 * 256);
                            // 1 ký tự = 7px = 0.58 
                            excelSheet.SetColumnWidth(6, (int)(160 / 7 * 256));
                        }

                        GC.Collect();
                    }

                    //end diennv

                    // diennv set margin print
                    excelSheet.SetMargin(MarginType.TopMargin,0.75);
                    excelSheet.SetMargin(MarginType.LeftMargin, 0.25);
                    excelSheet.SetMargin(MarginType.BottomMargin, 0.75);
                    excelSheet.SetMargin(MarginType.RightMargin, 0.25);
                    excelSheet.SetMargin(MarginType.HeaderMargin, 0.3);
                    excelSheet.SetMargin(MarginType.FooterMargin, 0.3);
                    // end diennv set margin print
                    // diennv set page layout
                    excelSheet.PrintSetup.Scale = 84;
                    //excelSheet.PrintSetup.FitWidth = 1;
                    //excelSheet.PrintSetup.FitHeight = 1;
                    //excelSheet.FitToPage = true;
                    //end diennv

                    workbook.Write(tempStream);
                    var byteArray = tempStream.ToArray();
                    ms.Write(byteArray, 0, byteArray.Length);
                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                    result.Content = new StreamContent(ms);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue
                           ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    result.Content.Headers.ContentDisposition =
                           new ContentDispositionHeaderValue("attachment")
                           {
                               FileName = $"abc_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx"
                           };
                    //return result;
                    //Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                    string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    var a = new FileContentResult(byteArray, mimeType)
                    {
                        FileDownloadName = "Danh mục hồ sơ.xlsx"
                    };
                    return new Response<IActionResult>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, a);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
