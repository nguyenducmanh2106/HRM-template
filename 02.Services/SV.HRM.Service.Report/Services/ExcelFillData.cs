using NLog;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.Report.Services
{
    public class ExcelFillData
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void FillReport(string filename, string templatefilename, DataSet data)
        {
            FillReport(filename, templatefilename, data, new string[] { "%", "%" });
        }

        public static void FillReport(string filename, string templatefilename, DataSet data, string[] deliminator)
        {
            var pathFile = Path.Combine(Environment.CurrentDirectory, @"OutputExcel\" + filename);
            if (File.Exists(pathFile))
                File.Delete(pathFile);

            using (var file = new FileStream(Path.Combine(Environment.CurrentDirectory, @"OutputExcel\" + filename), FileMode.CreateNew))
            {
                using (var temp = new FileStream(Path.Combine(Environment.CurrentDirectory, @"template\" + templatefilename), FileMode.Open))
                {
                    using (var xls = new ExcelPackage(file, temp))
                    {
                        foreach (var n in xls.Workbook.Names)
                        {
                            FillWorksheetData(data, n.Worksheet, n, deliminator);
                        }

                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            foreach (var n in ws.Names)
                            {
                                FillWorksheetData(data, ws, n, deliminator);
                            }
                        }

                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            foreach (var c in ws.Cells)
                            {
                                var s = "" + c.Value;
                                if (s.StartsWith(deliminator[0]) == false &&
                                    s.EndsWith(deliminator[1]) == false)
                                    continue;
                                s = s.Replace(deliminator[0], "").Replace(deliminator[1], "");
                                var ss = s.Split('.');
                                try
                                {
                                    c.Value = data.Tables[ss[0]].Rows[0][ss[1]];
                                }
                                catch { }
                            }
                        }

                        xls.Save();
                    }
                }
            }
        }

        private static void FillWorksheetData(DataSet data, ExcelWorksheet ws, ExcelNamedRange n, string[] deliminator)
        {
            if (data.Tables.Contains(n.Name) == false)
                return;

            var dt = data.Tables[n.Name];

            int row = n.Start.Row;

            var cn = new string[n.Columns];
            var st = new int[n.Columns];
            for (int i = 0; i < n.Columns; i++)
            {
                var data1 = n;
                cn[i] = (data1.Value as object[,])[0, i].ToString()?.Replace(deliminator[0], "").Replace(deliminator[1], "");
                if (cn[i] != null && cn[i].Contains("."))
                    cn[i] = cn[i].Split('.')[1];
                st[i] = ws.Cells[row, n.Start.Column + i].StyleID;
            }

            foreach (DataRow r in dt.Rows)
            {

                for (int col = 0; col < n.Columns; col++)
                {
                    if (dt.Columns.Contains(cn[col]))
                    {
                        //format kiểu dữ liệu kiểu Date khi ghi vào file excel
                        switch (r[cn[col]].GetType().Name)
                        {
                            case nameof(DateTime):
                                ws.Cells[row, n.Start.Column + col].Value = r[cn[col]] != null ? Convert.ToDateTime(r[cn[col]]).ToString("dd/MM/yyyy") : "";
                                break;
                            default:
                                ws.Cells[row, n.Start.Column + col].Value = r[cn[col]];
                                break;
                        }

                    }

                    ws.Cells[row, n.Start.Column + col].StyleID = st[col];
                }


                row++;

                //thêm đoạn này để lúc render data vào bảng dữ liệu không bị chiếm mất dòng chữ ký dưới cùng
                ws.InsertRow(row, 1, row);
            }

            //xóa dòng thừa từ đoạn InsertRow bên trên, đồng thời xóa cả dòng thừa xét biến trong file excel khi không có dữ liệu được render ra
            ws.DeleteRow(row, 1);

            // extend table formatting range to all rows
            foreach (var t in ws.Tables)
            {
                var a = t.Address;
                if (n.Start.Row.Between(a.Start.Row, a.End.Row) &&
                    n.Start.Column.Between(a.Start.Column, a.End.Column))
                {
                    ExtendRows(t, dt.Rows.Count - 1);
                }

            }
        }
        public static void ExtendRows(ExcelTable excelTable, int count)
        {

            var ad = new ExcelAddress(excelTable.Address.Start.Row,
                                      excelTable.Address.Start.Column,
                                      excelTable.Address.End.Row + count,
                                      excelTable.Address.End.Column);
            //Address = ad;
        }
        public static DataTable DataTableServices(string Qr, string conn)
        {
            SqlConnection _conn = new SqlConnection(conn);
            DataTable dt = new DataTable();
            try
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }

                SqlDataAdapter ds = new SqlDataAdapter(Qr, _conn);
                ds.Fill(dt);
                _conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }

        }

        public static void FillReportGrid(string filename, string templatefilename, DataSet data, string[] deliminator)
        {
            var pathFile = Path.Combine(Environment.CurrentDirectory, @"OutputExcel\" + filename);
            if (File.Exists(pathFile))
                File.Delete(pathFile);

            using (var file = new FileStream(Path.Combine(Environment.CurrentDirectory, @"OutputExcel\" + filename), FileMode.CreateNew))
            {
                using (var temp = new FileStream(Path.Combine(Environment.CurrentDirectory, @"template\" + templatefilename), FileMode.Open))
                {
                    using (var xls = new ExcelPackage(file, temp))
                    {
                        foreach (var n in xls.Workbook.Names)
                        {
                            FillWorksheetData(data, n.Worksheet, n, deliminator);

                        }

                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            foreach (var n in ws.Names)
                            {
                                FillWorksheetData(data, ws, n, deliminator);

                            }
                        }

                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            foreach (var c in ws.Cells)
                            {
                                var s = "" + c.Value;
                                if (s.StartsWith(deliminator[0]) == false &&
                                    s.EndsWith(deliminator[1]) == false)
                                    continue;
                                s = s.Replace(deliminator[0], "").Replace(deliminator[1], "");
                                var ss = s.Split('.');
                                try
                                {
                                    //c.Value = data.Tables[ss[0]].Rows[0][ss[1]];
                                    c.Value = data.Tables["Master"].Rows[0][ss[0]];
                                }
                                catch { }
                            }
                        }

                        xls.Save();
                    }
                }
            }
        }

        public static void FillReportAttendance(string filename, string templatefilename, DataSet data, string[] deliminator,string[] dataStyle = null)
        {
            var pathFile = Path.Combine(Environment.CurrentDirectory, @"OutputExcel\" + filename);
            if (File.Exists(pathFile))
                File.Delete(pathFile);

            using (var file = new FileStream(Path.Combine(Environment.CurrentDirectory, @"OutputExcel\" + filename), FileMode.CreateNew))
            {
                using (var temp = new FileStream(Path.Combine(Environment.CurrentDirectory, @"template\" + templatefilename), FileMode.Open))
                {
                    using (var xls = new ExcelPackage(file, temp))
                    {
                        foreach (var n in xls.Workbook.Names)
                        {
                            FillWorksheetData(data, n.Worksheet, n, deliminator);
                        }

                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            foreach (var n in ws.Names)
                            {
                                FillWorksheetData(data, ws, n, deliminator);
                            }
                        }

                        foreach (var ws in xls.Workbook.Worksheets)
                        {
                            foreach (var c in ws.Cells)
                            {
                                var s = "" + c.Value;
                                if (s.StartsWith(deliminator[0]) == false &&
                                    s.EndsWith(deliminator[1]) == false)
                                    continue;
                                s = s.Replace(deliminator[0], "").Replace(deliminator[1], "");
                                var ss = s.Split('.');
                                try
                                {
                                    c.Value = data.Tables[ss[0]].Rows[0][ss[1]];
                                    //if (dataStyle != null)
                                    //{
                                    //    var dataValue = data.Tables[ss[0]].Rows[0][ss[1]];
                                    //    var indexArr = Array.FindIndex(dataStyle, x => x.ToLower().Equals(dataValue.ToString()));
                                    //    if (indexArr > -1)
                                    //    {
                                    //        var t7 = "T7";
                                    //        var cn = "CN";
                                    //        if (dataValue.ToString().ToLower().Equals(t7.ToLower()))
                                    //        {
                                    //            c.Style.Fill.SetBackground(System.Drawing.Color.Yellow);
                                    //        }
                                    //        if (dataValue.ToString().ToLower().Equals(cn.ToLower()))
                                    //        {
                                    //            c.Style.Fill.SetBackground(System.Drawing.Color.Blue);
                                    //        }
                                    //    }
                                    //}
                                }
                                catch { }
                            }
                        }

                        xls.Save();
                    }
                }
            }
        }

    }

    public static class int_between
    {
        public static bool Between(this int v, int a, int b)
        {
            return v >= a && v <= b;
        }
    }


}
