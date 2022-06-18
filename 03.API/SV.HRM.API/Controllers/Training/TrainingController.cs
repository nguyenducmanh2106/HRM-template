using Microsoft.AspNetCore.Mvc;
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

namespace SV.HRM.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingHttpService _trainingService;
        public TrainingController(ITrainingHttpService trainingService)
        {
            _trainingService = trainingService;
        }

        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> ReportTraining(EntityGeneric TrainingQueryFilter)
        {
            return await _trainingService.ReportTraining(TrainingQueryFilter);
        }
        [HttpPost]
        public async Task<Response<bool>> CreateObject(string layout, [FromBody] object createObject)
        {
            return await _trainingService.CreateObject(layout, createObject);
        }

        [HttpPost]
        public async Task<Response<bool>> UpdateObject(string layout, int id, [FromBody] object createObject)
        {
            return await _trainingService.UpdateObject(layout, id, createObject);
        }

        /// <summary>
        /// Tìm bản ghi
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<QuanLyDaoTaoModel>> FindById(int recordID)
        {
            return await _trainingService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _trainingService.Delete(recordID);
        }

        [HttpPost]
        public async Task<Response<IActionResult>> ExportExcel([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                List<Dictionary<string, object>> dics = new List<Dictionary<string, object>>();
                var resultStaff = await _trainingService.ReportTraining(queryFilter);
                if (resultStaff != null)
                {
                    dics = resultStaff.Data;
                }
                // Lets converts our object data to Datatable for a simplified logic.
                // Datatable is most easy way to deal with complex datatypes for easy reading and formatting.

                DataTable table = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dics));

                //Create a blank workbook
                IWorkbook workbook = new XSSFWorkbook(); // create *.xlsx file, use HSSFWorkbook() for creating *.xls file.
                ISheet excelSheet = workbook.CreateSheet("Bao_Cao");
                //style export
                var font = workbook.CreateFont();
                font.FontHeightInPoints = 13;
                font.FontName = "Times New Roman";
                font.IsBold = true;

                // diennv thêm font cell
                var fontCell = workbook.CreateFont();
                fontCell.FontHeightInPoints = 11;
                fontCell.FontName = "Times New Roman";

                var cellStyleBorder = workbook.CreateCellStyle();
                cellStyleBorder.BorderBottom = BorderStyle.Thin;
                cellStyleBorder.BorderLeft = BorderStyle.Thin;
                cellStyleBorder.BorderRight = BorderStyle.Thin;
                cellStyleBorder.BorderTop = BorderStyle.Thin;
                cellStyleBorder.Alignment = HorizontalAlignment.Center;
                cellStyleBorder.VerticalAlignment = VerticalAlignment.Center;

                var cellStyleBorderAndColorGreen = workbook.CreateCellStyle();
                cellStyleBorderAndColorGreen.CloneStyleFrom(cellStyleBorder);
                cellStyleBorderAndColorGreen.FillPattern = FillPattern.SolidForeground;
                ((XSSFCellStyle)cellStyleBorderAndColorGreen).SetFillForegroundColor(new XSSFColor(new byte[] { 198, 239, 206 }));
                cellStyleBorderAndColorGreen.SetFont(font);

                var cellStyleBorderAndColorWhite = workbook.CreateCellStyle();
                cellStyleBorderAndColorWhite.CloneStyleFrom(cellStyleBorder);
                cellStyleBorderAndColorWhite.FillPattern = FillPattern.SolidForeground;
                ((XSSFCellStyle)cellStyleBorderAndColorWhite).SetFillForegroundColor(new XSSFColor(new byte[] { 255, 255, 255 }));

                var cellStyleBorderAndColorYellow = workbook.CreateCellStyle();
                cellStyleBorderAndColorYellow.CloneStyleFrom(cellStyleBorder);
                cellStyleBorderAndColorYellow.FillPattern = FillPattern.SolidForeground;
                ((XSSFCellStyle)cellStyleBorderAndColorYellow).SetFillForegroundColor(new XSSFColor(new byte[] { 222, 225, 46 }));
                cellStyleBorderAndColorYellow.SetFont(font);

                List<TableFieldExportExcel> columnExports = queryFilter.ColumnExports;

                MemoryStream ms = new MemoryStream();
                using (MemoryStream tempStream = new MemoryStream())
                {
                    List<TableFieldExportExcel> columns = new List<TableFieldExportExcel>();
                    IRow row = excelSheet.CreateRow(0);
                    foreach (TableFieldExportExcel column in columnExports)
                    {
                        if (column.hide == null || column.hide != true)
                        {
                            if (column.children?.Count > 0)
                            {

                                foreach (var columnChild in column.children)
                                {
                                    if (columnChild.hide == null || columnChild.hide != true)
                                    {

                                        columns.Add(columnChild);
                                    }
                                }
                            }
                            if (column.children == null || column.children.Count == 0)
                            {
                                columns.Add(column);
                            }
                        }
                    }
                    var rowOne = excelSheet.CreateRow(1);
                    int columnIndex = 0;

                    //Render header
                    foreach (var column in columns)
                    {
                        row.CreateCell(columnIndex);
                        var cellHead = rowOne.CreateCell(columnIndex);
                        cellHead.SetCellValue(column.headerName);
                        cellHead.CellStyle = cellStyleBorderAndColorYellow;
                        columnIndex++;
                    }

                    //merge cell
                    int beginColMerge = 0;
                    int endColMerge = 0;
                    foreach (TableFieldExportExcel column in columnExports)
                    {
                        if (column.hide == null || column.hide != true)
                        {

                            if (column.children?.Count > 0 && column.children?.Count(g => g.hide != true || !g.hide.HasValue) > 0)
                            {
                                endColMerge = beginColMerge + (column.children.Count(g => g.hide != true || !g.hide.HasValue) - 1);

                                if (endColMerge != beginColMerge)
                                {
                                    var cra = new CellRangeAddress(0, 0, beginColMerge, endColMerge);
                                    excelSheet.AddMergedRegion(cra);
                                }



                                ICell cell = excelSheet.GetRow(0).GetCell(beginColMerge);
                                cell.SetCellType(CellType.String);
                                cell.SetCellValue(column.headerName);
                                cell.CellStyle = cellStyleBorderAndColorGreen;
                                excelSheet.GetRow(0).GetCell(endColMerge).CellStyle = cellStyleBorderAndColorGreen;

                                beginColMerge += column.children.Count(g => g.hide != true || !g.hide.HasValue);
                            }
                            if (column.children == null || column.children.Count == 0)
                            {
                                //columns.Add(column);
                                endColMerge = beginColMerge;
                                var cra = new CellRangeAddress(0, 1, beginColMerge, endColMerge);
                                excelSheet.AddMergedRegion(cra);

                                ICell cell = excelSheet.GetRow(0).GetCell(beginColMerge);
                                cell.SetCellType(CellType.String);
                                cell.SetCellValue(column.headerName);
                                cell.CellStyle = cellStyleBorderAndColorGreen;
                                excelSheet.GetRow(0).GetCell(endColMerge).CellStyle = cellStyleBorderAndColorGreen;

                                beginColMerge += 1;
                            }

                        }
                    }

                    //Render STT column
                    var rowTwo = excelSheet.CreateRow(2);
                    for (var index = 0; index < columns.Count; index++)
                    {
                        var cell = rowTwo.CreateCell(index);
                        cell.SetCellValue(index + 1);

                        cell.SetCellType(CellType.String);
                        cell.CellStyle = cellStyleBorderAndColorWhite;
                    }
                    //Render Data
                    var rowThree = excelSheet.CreateRow(3);
                    int rowIndex = 3;
                    if (dics.Count() > 0)
                    {
                        foreach (var dic in dics)
                        {
                            rowThree = excelSheet.CreateRow(rowIndex);
                            excelSheet.GetRow(rowIndex).Height = (short)-1;
                            int cellIndex = 0;
                            foreach (var col in columns)
                            {
                                var cell = rowThree.CreateCell(cellIndex);
                                if (dic.ContainsKey(col.field))
                                {
                                    if (dic[col.field]?.GetType() == typeof(DateTime))
                                    {
                                        string strValue = dic[col.field] != null ? Convert.ToDateTime(dic[col.field]).ToString("dd/MM/yyyy") : "";
                                        cell.SetCellValue(strValue);
                                    }
                                    else if (dic[col.field]?.GetType() == typeof(bool))
                                    {
                                        string strValue = (bool)dic[col.field] == true ? "x" : "";
                                        cell.SetCellValue(strValue);
                                    }
                                    else if (dic[col.field]?.GetType() == typeof(Int32) || dic[col.field]?.GetType() == typeof(Int16) || dic[col.field]?.GetType() == typeof(Int64))
                                    {
                                        string strValue = String.Format("{0:n0}", dic[col.field]);
                                        cell.SetCellValue(strValue);
                                    }
                                    else if (dic[col.field]?.GetType() == typeof(double) || dic[col.field]?.GetType() == typeof(float))
                                    {
                                        string strValue = String.Format("{0:n}", dic[col.field]);
                                        cell.SetCellValue(strValue);
                                    }
                                    else
                                    {
                                        string strValue = dic[col.field] != null ? dic[col.field]?.ToString() : "";
                                        cell.SetCellValue(strValue);
                                    }
                                }
                                else
                                {
                                    if (col.field == "Order")
                                    {
                                        cell.SetCellValue((rowIndex - 2).ToString());
                                    }
                                    else
                                    {
                                        cell.SetCellValue("");
                                    }


                                }
                                cell.CellStyle = cellStyleBorderAndColorWhite;
                                cell.CellStyle.WrapText = true;
                                cell.CellStyle.SetFont(fontCell);
                                cellIndex++;
                            }
                            rowIndex++;
                        }

                    }

                    //foreach (DataRow dsrow in table.Rows)
                    //{
                    //    rowThree = excelSheet.CreateRow(rowIndex);
                    //    excelSheet.GetRow(rowIndex).Height = (short)-1;
                    //    int cellIndex = 0;
                    //    foreach (var col in columns)
                    //    {
                    //        var cell = rowThree.CreateCell(cellIndex);
                    //        if (dsrow.Table.Columns[col.field] != null)
                    //        {
                    //            if (dsrow[col.field]?.GetType() == typeof(DateTime))
                    //            {
                    //                string strValue = dsrow[col.field] != null ? Convert.ToDateTime(dsrow[col.field]).ToString("dd/MM/yyyy") : "";
                    //                cell.SetCellValue(strValue);
                    //            }
                    //            else if (dsrow[col.field].GetType() == typeof(bool))
                    //            {
                    //                string strValue = (bool)dsrow[col.field] == true ? "x" : "";
                    //                cell.SetCellValue(strValue);
                    //            }
                    //            else
                    //            {
                    //                string strValue = dsrow[col.field] != null ? dsrow[col.field]?.ToString() : "";
                    //                cell.SetCellValue(strValue);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (col.field == "Order")
                    //            {
                    //                cell.SetCellValue((rowIndex - 2).ToString());
                    //            }
                    //            else
                    //            {
                    //                cell.SetCellValue("");
                    //            }


                    //        }
                    //        cell.CellStyle = cellStyleBorderAndColorWhite;
                    //        cell.CellStyle.WrapText = true;
                    //        cell.CellStyle.SetFont(fontCell);
                    //        cellIndex++;
                    //    }
                    //    rowIndex++;
                    //}


                    for (int i = 0; i < columns.Count(); i++)
                    {
                        //if (i == 0)
                        //{
                        //    excelSheet.SetColumnWidth(i, 15 * 256);
                        //}
                        //else
                        //{
                        excelSheet.AutoSizeColumn(i, true);

                        //}
                        GC.Collect();
                    }

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
                    string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    var a = new FileContentResult(byteArray, mimeType)
                    {
                        FileDownloadName = "Báo cáo đào tạo.xlsx"
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
