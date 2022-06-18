
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using SV.HRM.Service.Report.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using SV.HRM.Caching.Interface;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using SV.HRM.DataProcess.Infrastructure.Impl;
using static SV.HRM.Core.Utils.Constant;
using NLog;

namespace SV.HRM.Service.Report.Logic
{
    public class DapperReport : IDapperReport
    {
        public static string editmode;
        public static string basicSalary;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _configuration;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseReportHandler _baseReportHandler;
        private readonly ICached _cached;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DapperReport(IConfiguration configuration, IDapperUnitOfWork dapperUnitOfWork, 
            IBaseReportHandler baseReportHandler, ICached cached, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseReportHandler = baseReportHandler;
            _cached = cached;
            _httpContextAccessor = httpContextAccessor;
        }


        public List<OrganizationModel> GetListOrganization()
        {
            IDbConnection connection = null;
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                connection = new SqlConnection(connectionString);
                //connection.Open();
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();
                    var ret = dal.Query<OrganizationModel>("SELECT OrganizationID,OrganizationName,ParentOrganizationId FROM Organizations WHERE Status = @status;", new { status = StatusRecord.ACTIVE });
                    return ret.ToList();
                }
                return null;
            }
            catch (Exception)
            {
                connection.Close();
                return null;
            }
        }

        /// <summary>
        /// hàm lấy về danh sách id của cơ cấu tổ chức mà user có quyền truy cập
        /// </summary>
        /// <param name="organizationId">Id của cơ cấu tổ chức</param>
        /// <returns></returns>
        public IEnumerable<OrganizationModel> GetPermissionAccessOrganizationId()
        {
            var userInfo = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
            IDbConnection connection = null;
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                connection = new SqlConnection(connectionString);
                //connection.Open();
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();
                    var ret = dal.Query<OrganizationModel>("proc_recursive_organization", new { @ApplicationId = userInfo.ApplicationId, @UserId = userInfo.UserId }, null, CommandType.StoredProcedure);
                    return ret;
                }
                return null;
            }
            catch (Exception)
            {
                connection.Close();
                return null;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// hàm lấy về danh sách id của cơ cấu tổ chức con và chính nó
        /// </summary>
        /// <param name="organizationId">Id của cơ cấu tổ chức</param>
        /// <returns></returns>
        public IEnumerable<OrganizationModel> GetListChildOrganizationAndOrganizationId(string organizationIds)
        {
            IDbConnection connection = null;
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                connection = new SqlConnection(connectionString);
                //connection.Open();
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();
                    var ret = dal.Query<OrganizationModel>("sp_GetListChildOrganizationAndOrganizationId", new { OrganizationId = organizationIds }, null, CommandType.StoredProcedure);
                    return ret;
                }
                return null;
            }
            catch (Exception)
            {
                connection.Close();
                return null;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        public JObject ExportExcel(ReportFileEntity reportFileEntity)
        {
            DateTime _denNgay = reportFileEntity.denNgay;
            DateTime _tuNgay = reportFileEntity.tuNgay;
            var storeKey = reportFileEntity.store;
            var storeName = reportFileEntity.reportName;
            var ExtraText = editmode;

            

            ResponseMess mess = new ResponseMess();
            JObject result = new JObject();
            var timeS = "ngày " + _denNgay.ToString("dd") + " tháng " + _denNgay.ToString("MM") + " năm " + _denNgay.ToString("yyyy");
            var timeS1 = " tháng " + _denNgay.ToString("MM") + " năm " + _denNgay.ToString("yyyy");
            var timeS2 = " Tháng " + _denNgay.ToString("MM") + " năm " + _denNgay.ToString("yyyy");

            try
            {
                DateTime a = _tuNgay;
                DateTime b = _denNgay;
                var _ky = "";

                DynamicParameters param = new DynamicParameters();
                param.Add("@tuNgay", a.ToString("yyyy/MM/dd"));
                param.Add("@denNgay", b.ToString("yyyy/MM/dd"));
                if (storeName == "dang-phi")
                {
                    param.Add("@ChiBo", ExtraText);
                }
                if (storeName == "staff-quality" || storeName == "nhanluc-ngoai-cohuu")
                {
                    param.Add("@key", "Excel");
                }
                if(Int32.Parse(b.ToString("MM")) > 6 )
                {
                    _ky = "Đợt II";
                } else
                {
                    _ky = "Đợt I";
                }

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", storeKey, storeName);
                DataTable dts = new DataTable();
                dts = _dapperUnitOfWork.GetRepository().ExecuteReport(sqlQuery, param, null, CommandType.StoredProcedure);


                DataTable dataMasterKSK = new DataTable();
                string keyStore;
                if (storeName == "kham-suc-khoe")
                {
                    keyStore = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "OutputMaster", storeName);
                    dataMasterKSK = _dapperUnitOfWork.GetRepository().ExecuteReport(keyStore, param, null, CommandType.StoredProcedure);
                     
                }
                if(storeName == "dang-phi")
                {
                    keyStore = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "OutputMaster", storeName);
                    dataMasterKSK = _dapperUnitOfWork.GetRepository().ExecuteReport(keyStore, param, null, CommandType.StoredProcedure);

                }


                // Tạo bảng dữ liệu chung ( Master ) 
                DataTable Master = new DataTable();
                Master.TableName = "Master";
                Master.Columns.Add("title");
                Master.Columns.Add("title2");
                Master.Columns.Add("title3");
                Master.Columns.Add("title4");
                Master.Columns.Add("title5");
                Master.Columns.Add("title6");
                Master.Columns.Add("title7");


                DataRow dr = null;
                dr = Master.NewRow();
                // Gắn tiêu đề theo từng báo cáo dự vào storeName truyền lên
                switch (storeName)
                {
                    case "staff-quality":

                        dr["title"] = "Có mặt tính đến " + timeS;
                        break;
                    case "list-party":
                        dr["title"] = "Có mặt tính đến " + timeS;
                        break;
                    case "tu-van-ksk":
                        dr["title"] = "DANH SÁCH CBCNV BỆNH VIỆN ĐA KHOA ĐỨC GIANG CẦN TƯ VẤN VỀ TÌNH TRẠNG SỨC KHỎE ĐỢT KHÁM SK ĐỊNH KỲ ";
                        dr["title2"] = _ky + " Năm " + _denNgay.ToString("yyyy");
                        //dr["title3"] = "Năm " + _denNgay.ToString("yyyy");
                        break;
                    case "tang-luong":
                        dr["title"] = "DANH SÁCH CÁN BỘ, CÔNG CHỨC, VIÊN CHỨC VÀ LAO ĐỘNG HỢP ĐỒNG" +
                                      "ĐƯỢC NÂNG BẬC LƯƠNG THƯỜNG XUYÊN " + timeS1;
                        break;
                    case "luong-hd-ngan-han":
                        dr["title"] = "DANH SÁCH HỢP ĐỒNG KHÔNG XÁC ĐỊNH THỜI HẠN DO ĐƠN VỊ KÝ ĐƯỢC THỎA THUẬN LẠI MỨC LƯƠNG " + timeS1;
                        break;
                    case "kham-suc-khoe":
                            dr["title"] = _ky + " Năm " + _denNgay.ToString("yyyy");
                            if(dataMasterKSK.Rows.Count > 0)
                            {
                                dr["title2"] = "Tổng số CBCNV khám: " + dataMasterKSK.Rows[0]["SumNVTest"].ToString() + "/"
                                                                      + dataMasterKSK.Rows[0]["SumNVAll"].ToString()
                                                                      + "( " + dataMasterKSK.Rows[0]["percentAll"].ToString()
                                                                      + " %)";

                                dr["title3"] = "Đạt sức khỏe loại I: " + dataMasterKSK.Rows[0]["SkLoaiMot"].ToString() + "/"
                                                                      + dataMasterKSK.Rows[0]["SumNVTest"].ToString()
                                                                      + "( " + dataMasterKSK.Rows[0]["percent1"].ToString()
                                                                      + " %)";

                                dr["title4"] = "Đạt sức khỏe loại II: " + dataMasterKSK.Rows[0]["SkLoaiHai"].ToString() + "/"
                                                                          + dataMasterKSK.Rows[0]["SumNVTest"].ToString()
                                                                          + "( " + dataMasterKSK.Rows[0]["percent2"].ToString()
                                                                          + " %)";

                                dr["title5"] = "Đạt sức khỏe loại III: " + dataMasterKSK.Rows[0]["SkLoaiBa"].ToString() + "/"
                                                                          + dataMasterKSK.Rows[0]["SumNVTest"].ToString()
                                                                          + "( " + dataMasterKSK.Rows[0]["percent3"].ToString()
                                                                          + " %)";

                                dr["title6"] = "Đạt sức khỏe loại IV: " + dataMasterKSK.Rows[0]["SkLoaiBon"].ToString() + "/"
                                                                          + dataMasterKSK.Rows[0]["SumNVTest"].ToString()
                                                                          + "( " + dataMasterKSK.Rows[0]["percent4"].ToString()
                                                                          + " %)";

                                dr["title7"] = "Đạt sức khỏe loại V: " + dataMasterKSK.Rows[0]["SkLoaiNam"].ToString() + "/"
                                                                          + dataMasterKSK.Rows[0]["SumNVTest"].ToString()
                                                                          + "( " + dataMasterKSK.Rows[0]["percent5"].ToString()
                                                                          + " %)";
                            } 
                            else
                            {
                                dr["title2"] = "Tổng số CBCNV khám: 00/00" ;

                                dr["title3"] = "Đạt sức khỏe loại I: 00/00 (00%)";

                                dr["title4"] = "Đạt sức khỏe loại II: 00/00 (00%)";

                                dr["title5"] = "Đạt sức khỏe loại III: 00/00 (00%)";

                                dr["title6"] = "Đạt sức khỏe loại IV: 00/00 (00%)";

                                dr["title7"] = "Đạt sức khỏe loại V: 00/00 (00%)";
                            }
                            
                        break;
                    case "nhanluc-ngoai-cohuu":
                        dr["title"] = "Có đến " + timeS;
                        break;
                    case "dang-phi":
                        dr["title"] = timeS2;
                        if(dataMasterKSK.Rows.Count > 0)
                        {
                            dr["title2"] = "Thành tiền (Theo "+ dataMasterKSK.Rows[0]["LuongCoBan"].ToString()+ " đồng";
                            dr["title3"] =  dataMasterKSK.Rows[0]["ChiBoName"].ToString();
                            dr["title4"] = "Tổng số tiền Đảng phí thu được: " + dataMasterKSK.Rows[0]["TongTien"].ToString() + " đồng";
                            dr["title5"] = "Trích để lại chi bộ (30%): " + dataMasterKSK.Rows[0]["TienTrichLai"].ToString() + " đồng";
                            dr["title6"] = "Nộp về Đảng bộ (70%): " + dataMasterKSK.Rows[0]["TienNop"].ToString() + " đồng";

                        }
                        else
                        {
                            dr["title2"] = "Thành tiền (Theo ... đồng)";
                            dr["title3"] = "";
                            dr["title4"] = "Tổng số tiền Đảng phí thu được: ... đồng";
                            dr["title5"] = "Trích để lại chi bộ (30%): ... đồng";
                            dr["title6"] = "Nộp về Đảng bộ (70%): ... đồng";
                        }
                        break;

                }

                Master.Rows.Add(dr);

                // Tạo bảng dữ liệu Chi tiết ( Details )
                DataTable detail = dts;
                detail.TableName = "Details";

                // Tạo Dataset ghi dữ liệu Master + Details 
                var ds = new DataSet();
                ds.Tables.Add(detail);
                ds.Tables.Add(Master);

                // Lấy tên file đầu vào và đầu ra 
                var fileTmp = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "Template", storeName);
                var fileOutput = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "Output", storeName);
                var pathExport = Path.Combine(Environment.CurrentDirectory, @"OutputExcel\" + fileOutput);

                ExcelFillData.FillReport(fileOutput, fileTmp, ds, new string[] { "{", "}" });

                byte[] imageArray = File.ReadAllBytes(pathExport);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                if (!string.IsNullOrEmpty(base64ImageRepresentation))
                {
                    result = new JObject
                    {
                        {"statusCode", "00"},
                        {"data", base64ImageRepresentation}
                    };
                }

            }
            catch (Exception ex)
            {
                result = new JObject
                    {
                        {"statusCode", "99"},
                        {"data", ex.Message}
                    };
            }
            return result;

        }

        public async Task<Response<object>> ExportExcel(EntityGeneric StaffQueryFilter)
        {
            ResponseMess mess = new ResponseMess();
            //var timeS = "ngày " + timeSearch.Substring(8, 2) + " tháng " + timeSearch.Substring(5, 2) + " năm " + timeSearch.Substring(0, 4);
            //var timeS1 = " tháng " + timeSearch.Substring(5, 2) + " năm " + timeSearch.Substring(0, 4);

            try
            {
                var lstApplication = _baseReportHandler.GetOrganization();
                var responseStaff = await _baseReportHandler.GetFilter<object>(StaffQueryFilter);
                List<Dictionary<string, object>> dictionarys = new List<Dictionary<string, object>>();
                if (responseStaff != null && responseStaff.Status == Constant.SUCCESS && lstApplication != null)
                {
                    List<object> staffs = new List<object>();
                    staffs = responseStaff.Data;
                    var json = JsonConvert.SerializeObject(staffs);
                    dictionarys = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                    //return new Response<List<object>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, staffs, responseStaff.DataCount, responseStaff.TotalCount, responseStaff.TotalPage, responseStaff.PageNumber, responseStaff.PageSize);
                    foreach (Dictionary<string, object> dictionary in dictionarys)
                    {
                        
                        //lấy tên và mã khoa phòng
                        if (dictionary.ContainsKey("DeptID"))
                        {
                            var organization = lstApplication.SingleOrDefault(g => dictionary["DeptID"] != null && (g.OrganizationId == (Int64)dictionary["DeptID"]));
                            dictionary["OrganizationName"] = organization?.OrganizationName ?? null;
                            dictionary["OrganizationCode"] = organization?.OrganizationCode ?? null;
                        }
                    }
                }
                // Lets converts our object data to Datatable for a simplified logic.
                // Datatable is most easy way to deal with complex datatypes for easy reading and formatting.

                DataTable dts = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dictionarys), (typeof(DataTable)));



                // Tạo bảng dữ liệu chung ( Master ) 
                DataTable Master = new DataTable();
                Master.TableName = "Master";
                Master.Columns.Add("title");
                Master.Columns.Add("title2");
                Master.Columns.Add("title3");
                DataRow dr = null;
                dr = Master.NewRow();
                // Gắn tiêu đề theo từng báo cáo dự vào storeName truyền lên
                switch (StaffQueryFilter.ReportName)
                {
                    case "staff-quality":

                        //dr["title"] = "Có mặt tính đến " + timeS;
                        break;
                    case "list-party":
                        //dr["title"] = "Có mặt tính đến " + timeS;
                        break;
                    case "tu-van-ksk":
                        dr["title"] = "DANH SÁCH CBCNV BỆNH VIỆN ĐA KHOA ĐỨC GIANG CẦN TƯ VẤN VỀ TÌNH TRẠNG SỨC KHỎE ĐỢT KHÁM SK ĐỊNH KỲ ";
                        dr["title2"] = "Kỳ ";
                        //dr["title3"] = "Năm " + timeSearch.Substring(0, 4);
                        break;
                    case "ExportExcel_TangLuongThuongXuyen":
                        dr["title"] = "DANH SÁCH CÁN BỘ, CÔNG CHỨC, VIÊN CHỨC VÀ LAO ĐỘNG HỢP ĐỒNG" +
                                      "ĐƯỢC NÂNG BẬC LƯƠNG THƯỜNG XUYÊN ";
                        break;
                    case "luong-hd-ngan-han":
                        dr["title"] = "DANH SÁCH HỢP ĐỒNG KHÔNG XÁC ĐỊNH THỜI HẠN DO ĐƠN VỊ KÝ ĐƯỢC THỎA THUẬN LẠI MỨC LƯƠNG ";
                        break;

                }

                Master.Rows.Add(dr);

                // Tạo bảng dữ liệu Chi tiết ( Details )
                DataTable detail = dts;
                detail.TableName = "Details";

                // Tạo Dataset ghi dữ liệu Master + Details 
                var ds = new DataSet();
                ds.Tables.Add(detail);
                ds.Tables.Add(Master);

                // Lấy tên file đầu vào và đầu ra 
                var fileTmp = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "Template", StaffQueryFilter.ReportName);
                var fileOutput = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "Output", StaffQueryFilter.ReportName);
                var pathExport = Path.Combine(Environment.CurrentDirectory, @"OutputExcel\" + fileOutput);

                ExcelFillData.FillReportGrid(fileOutput, fileTmp, ds, new string[] { "{", "}" });
                MemoryStream ms = new MemoryStream();
                byte[] byteArray = File.ReadAllBytes(pathExport);

                ms.Write(byteArray, 0, byteArray.Length);
                HttpResponseMessage result = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
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
                var fileContentResult = new FileContentResult(byteArray, mimeType)
                {
                    FileDownloadName = StaffQueryFilter.FileDownload
                };
                return new Response<object>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, fileContentResult);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }

        public JObject getReport(ReportEntity reportEntity)
        {

            editmode = null;
            DateTime _denNgay = reportEntity.denNgay;
            DateTime _tuNgay = reportEntity.tuNgay;
            var storeKey = reportEntity.store;
            var storeName = reportEntity.reportName;
            var ExtraText = reportEntity.ExtraText;
            editmode = ExtraText;
            

            ResponseMess mess = new ResponseMess();
            JObject result = new JObject();
            try
            {
                string sqlQueryMaster = "";
                DateTime a = _tuNgay;
                DateTime b = _denNgay;

                DynamicParameters param = new DynamicParameters();
                param.Add("@tuNgay", a.ToString("yyyy/MM/dd"));
                param.Add("@denNgay", b.ToString("yyyy/MM/dd"));
                if(storeName == "dang-phi")
                {
                    param.Add("@ChiBo", ExtraText);
                    sqlQueryMaster = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "OutputMaster", storeName);

                }
                if (storeName == "staff-quality" || storeName == "nhanluc-ngoai-cohuu")
                {
                    param.Add("@key", "LoadReport");
                }
                if(storeName == "kham-suc-khoe")
                {
                    sqlQueryMaster = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "OutputMaster", storeName);
                }

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", storeKey, storeName);
                DataTable dts = new DataTable();
                dts = _dapperUnitOfWork.GetRepository().ExecuteReport(sqlQuery, param, null, CommandType.StoredProcedure);

                if (dts.Rows.Count > 0)
                {
                    DataTable ds = new DataTable();

                    mess.code = "00";
                    string JSONString = JsonConvert.SerializeObject(dts);
                    dynamic json = JsonConvert.DeserializeObject(JSONString);
                    if(storeName == "kham-suc-khoe")
                    {
                        DataTable MasterTabble = new DataTable();
                        MasterTabble = _dapperUnitOfWork.GetRepository().ExecuteReport(sqlQueryMaster, param, null, CommandType.StoredProcedure);
                        string jsonMaster = JsonConvert.SerializeObject(MasterTabble);
                        dynamic resMaster = JsonConvert.DeserializeObject(jsonMaster);
                        result = new JObject
                        {
                            {"statusCode", mess.code},
                            {"data", json},
                            {"MasterData", resMaster}
                        };
                    } 
                    else if(storeName == "dang-phi")
                    {
                        DataTable MasterTabble = new DataTable();
                        MasterTabble = _dapperUnitOfWork.GetRepository().ExecuteReport(sqlQueryMaster, param, null, CommandType.StoredProcedure);
                        string jsonMaster = JsonConvert.SerializeObject(MasterTabble);
                        dynamic resMaster = JsonConvert.DeserializeObject(jsonMaster);
                        result = new JObject
                        {
                            {"statusCode", mess.code},
                            {"data", json},
                            {"MasterData", resMaster}
                        };
                    }
                     else 
                    {
                        result = new JObject
                        {
                            {"statusCode", mess.code},
                            {"data", json}
                        };
                    }

                }
                else
                {
                    mess.code = "11";
                    result = new JObject
                        {
                            {"statusCode", mess.code},
                            {"data", null}
                        };
                }
                return result;
            }
            catch (Exception ex)
            {
                return result = new JObject
                        {
                            {"statusCode", "99"},
                            {"data", ex.Message}
                        }; ;
            }

        }

        public async Task<Response<object>> ExportExcel_ListQualityStaff(EntityGeneric StaffQueryFilter)
        {
            ResponseMess mess = new ResponseMess();
            var ethnics = _baseReportHandler.GetEthnics();

            // lấy id của dân tộc kinh
            var ethnicID = ethnics.SingleOrDefault(g => g.EthnicName.ToLower().Contains("kinh"))?.EthnicId;
            StaffQueryFilter.CustomPagingData["EthnicID"] = ethnicID;

            try
            {
                var lstApplication = _baseReportHandler.GetOrganization();
                var responseStaff = await _baseReportHandler.GetFilter<object>(StaffQueryFilter, true);
                List<Dictionary<string, object>> dictionarys = new List<Dictionary<string, object>>();
                if (responseStaff != null && responseStaff.Status == Constant.SUCCESS && lstApplication != null)
                {
                    List<object> staffs = new List<object>();
                    staffs = responseStaff.Data;
                    var json = JsonConvert.SerializeObject(staffs);
                    dictionarys = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                    //return new Response<List<object>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, staffs, responseStaff.DataCount, responseStaff.TotalCount, responseStaff.TotalPage, responseStaff.PageNumber, responseStaff.PageSize);
                    foreach (Dictionary<string, object> dictionary in dictionarys)
                    {
                        if (dictionary.ContainsKey("DeptID"))
                        {
                            var organization = lstApplication.SingleOrDefault(g => dictionary["DeptID"] != null && (g.OrganizationId == (Int64)dictionary["DeptID"]));
                            dictionary["OrganizationName"] = organization?.OrganizationName ?? null;
                            dictionary["OrganizationCode"] = organization?.OrganizationCode ?? null;
                        }
                    }
                }
                // Lets converts our object data to Datatable for a simplified logic.
                // Datatable is most easy way to deal with complex datatypes for easy reading and formatting.

                DataTable dts = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dictionarys), (typeof(DataTable)));

                var jsonSummary = JsonConvert.SerializeObject(responseStaff.SummaryData);
                var summaryDataObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonSummary);
                // Tạo bảng dữ liệu chung ( Master ) 
                DataTable Master = new DataTable();
                Master.TableName = "Master";
                //Master.Columns.Add("title");
                //DataRow dr = null;
                //dr = Master.NewRow();
                //// Gắn tiêu đề theo từng báo cáo dự vào storeName truyền lên
                //switch (StaffQueryFilter.ReportName)
                //{
                //    case "staff-quality":

                //        //dr["title"] = "Có mặt tính đến " + timeS;
                //        break;
                //    case "list-party":
                //        //dr["title"] = "Có mặt tính đến " + timeS;
                //        break;
                //    case "tu-van-ksk":
                //        dr["title"] = "DANH SÁCH CBCNV BỆNH VIỆN ĐA KHOA ĐỨC GIANG CẦN TƯ VẤN VỀ TÌNH TRẠNG SỨC KHỎE ĐỢT KHÁM SK ĐỊNH KỲ ";
                //        dr["title2"] = "Kỳ ";
                //        //dr["title3"] = "Năm " + timeSearch.Substring(0, 4);
                //        break;
                //    case "ExportExcel_TangLuongThuongXuyen":
                //        dr["title"] = "DANH SÁCH CÁN BỘ, CÔNG CHỨC, VIÊN CHỨC VÀ LAO ĐỘNG HỢP ĐỒNG" +
                //                      "ĐƯỢC NÂNG BẬC LƯƠNG THƯỜNG XUYÊN ";
                //        break;
                //    case "luong-hd-ngan-han":
                //        dr["title"] = "DANH SÁCH HỢP ĐỒNG KHÔNG XÁC ĐỊNH THỜI HẠN DO ĐƠN VỊ KÝ ĐƯỢC THỎA THUẬN LẠI MỨC LƯƠNG ";
                //        break;

                //}

                DataRow dr = Master.NewRow();
                foreach (var dic in summaryDataObject)
                {
                    Master.Columns.Add(dic.Key);
                    dr[dic.Key] = dic.Value;
                }
                Master.Rows.Add(dr);

                // Tạo bảng dữ liệu Chi tiết ( Details )
                DataTable detail = dts;
                detail.TableName = "Details";

                // Tạo Dataset ghi dữ liệu Master + Details 
                var ds = new DataSet();
                ds.Tables.Add(detail);
                ds.Tables.Add(Master);

                // Lấy tên file đầu vào và đầu ra 
                var fileTmp = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "Template", StaffQueryFilter.ReportName);
                var fileOutput = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "Output", StaffQueryFilter.ReportName);
                var pathExport = Path.Combine(Environment.CurrentDirectory, @"OutputExcel\" + fileOutput);

                ExcelFillData.FillReportGrid(fileOutput, fileTmp, ds, new string[] { "{", "}" });
                MemoryStream ms = new MemoryStream();
                byte[] byteArray = File.ReadAllBytes(pathExport);

                ms.Write(byteArray, 0, byteArray.Length);
                HttpResponseMessage result = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
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
                var fileContentResult = new FileContentResult(byteArray, mimeType)
                {
                    FileDownloadName = StaffQueryFilter.FileDownload
                };
                return new Response<object>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, fileContentResult);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }
        /// <summary>
        /// báo cáo chấm công
        /// </summary>
        /// <param name="inputFilter"></param>
        /// <returns></returns>
        public async Task<Response<object>> ExportExcelAttendance(object inputFilter)
        {
            try
            {
                var lstDepartment = _baseReportHandler.GetOrganization();
                //var json = JsonConvert.SerializeObject(inputFilter);
                var inputParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(inputFilter.ToString());
                if (inputParam != null)
                {
                    var reportName = inputParam["reportName"] ?? "DayOfWorkInMonth";
                    var fileName = inputParam["fileName"] ?? "Tổng hợp ca làm.xlsx";
                    var month = inputParam["Month"] ?? "01";
                    var year = inputParam["Year"] ?? DateTime.Now.Year;

                    // xác định tháng để có báo cáo đúng số ngày theo tháng cho báo cáo bảng chấm công
                    if (reportName.ToString().Equals("Attendance"))
                    {
                        switch (Int64.Parse(month.ToString()))
                        {
                            case 4:
                            case 6:
                            case 9:
                            case 11:
                                reportName = "Attendance_30days";
                                break;
                            case 2:
                                if (Int64.Parse(year.ToString()) % 4 == 0)
                                {
                                    if (year.ToString().EndsWith("00"))
                                    {
                                        if (Int64.Parse(year.ToString()) % 400 == 0)
                                        {
                                            reportName = "Attendance_29days";
                                        }
                                        else
                                        {
                                            reportName = "Attendance_28days";
                                        }
                                    }else
                                    {
                                        reportName = "Attendance_29days";
                                    }
                                }else
                                {
                                    reportName = "Attendance_28days";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    // xác định tháng để có báo cáo đúng số ngày theo tháng cho báo cáo ngoài giờ định xuất
                    if (reportName.ToString().Equals("Attendance_OutSideTime"))
                    {
                        switch (Int64.Parse(month.ToString()))
                        {
                            case 4:
                            case 6:
                            case 9:
                            case 11:
                                reportName = "Attendance_OutSideTime_30days";
                                break;
                            case 2:
                                if (Int64.Parse(year.ToString()) % 4 == 0)
                                {
                                    if (year.ToString().EndsWith("00"))
                                    {
                                        if (Int64.Parse(year.ToString()) % 400 == 0)
                                        {
                                            reportName = "Attendance_OutSideTime_29days";
                                        }
                                        else
                                        {
                                            reportName = "Attendance_OutSideTime_28days";
                                        }
                                    }
                                    else
                                    {
                                        reportName = "Attendance_OutSideTime_29days";
                                    }
                                }
                                else
                                {
                                    reportName = "Attendance_OutSideTime_28days";
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    var sql = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "Report", reportName.ToString());
                    var deptID = inputParam["DeptID"];
                    var deptName = "";
                    if(deptID != null && deptID.ToString().Trim().Length > 0)
                    {
                        deptName = lstDepartment.SingleOrDefault(g => deptID != null && (g.OrganizationId == Int64.Parse(deptID.ToString())))?.OrganizationName ?? "";
                    }
                    List<string> valueDeptID = new List<string>();
                    if (deptID == null)
                    {
                        var orgs = GetPermissionAccessOrganizationId();
                        foreach (var item in orgs)
                        {
                            valueDeptID.Add(item.id.ToString());
                        }
                    }
                    else
                    {
                        valueDeptID.Add(inputParam["DeptID"]?.ToString());
                    }
                    var organization = GetListChildOrganizationAndOrganizationId(String.Join(',', valueDeptID)).Select(g => g.OrganizationId);
                    deptID = string.Join(',', organization);

                    List<object> lst = null;
                    // kết quả trả về store
                    List<object> objReturn = null;
                    // danh sách tên của ngày trong tuần
                    List<object> lstNameDayOfWeek = null;
                    var lstDataStyle = new string[] { };
                    var responseReport = await _dapperUnitOfWork.GetRepository().QueryAsync<object>(sql: sql, new { @Month = month, @Year = year, @DeptID = deptID }, null, CommandType.StoredProcedure);
                    objReturn = responseReport.ToList();

                    // bảng master
                    DataTable Master = new DataTable
                    {
                        TableName = "Master"
                    };
                    Master.Clear();
                    Master.Columns.Add("Department");
                    Master.Columns.Add("TotalStaff");
                    Master.Columns.Add("Title");
                    Master.Columns.Add("Footer");

                    //var titleRow = "";
                    //var footerRow = "";

                    DataRow dr = Master.NewRow();
                    switch (reportName.ToString())
                    {
                        case "DayOfWorkInMonth":
                            dr["Department"] = deptName;
                            dr["Title"] = "Tháng " + month.ToString() + " năm " + year.ToString();
                            dr["Footer"] = "Hà Nội, Ngày ... tháng ... năm ...";
                            
                            break;
                        case "ShiftOfWorkInMonth":
                            dr["Department"] = deptName;
                            dr["Title"] = "Tháng " + month.ToString() + " năm " + year.ToString();
                            dr["Footer"] = "Hà Nội, Ngày ... tháng ... năm ...";

                            break;
                        case "HaveLunch":
                            dr["Department"] = deptName;
                            dr["Title"] = "DANH SÁCH CBCNV ĐỀ NGHỊ ĐƯỢC HƯỞNG TIỀN ĂN TRƯA THÁNG " + month.ToString() + " NĂM " + year.ToString();
                            dr["Footer"] = "Hà Nội, Ngày ... tháng ... năm ...";

                            break;
                        case "Attendance":
                        case "Attendance_30days":
                        case "Attendance_29days":
                        case "Attendance_28days":
                            dr["Department"] = deptName;
                            dr["Title"] = "THÁNG " + month.ToString() + " NĂM " + year.ToString();
                            dr["Footer"] = "Hà Nội, Ngày ... tháng ... năm ...";
                            dr["TotalStaff"] = objReturn.Count();
                            fileName = "Bảng chấm công tháng " + month.ToString() + " năm " + year.ToString() + ".xlsx";
                            var sqlNameDayOfWeek = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "Attendance", "get-name-day-of-week");
                            var responseNameDayOfWeek = await _dapperUnitOfWork.GetRepository().QueryAsync<object>(sql: sqlNameDayOfWeek, new { @Month = month, @Year = year }, null, CommandType.StoredProcedure);
                            lstNameDayOfWeek = responseNameDayOfWeek.ToList();
                            lstDataStyle = new string[] { "T7", "CN"};
                            break;
                        case "Attendance_OutSideTime":
                        case "Attendance_OutSideTime_30days":
                        case "Attendance_OutSideTime_29days":
                        case "Attendance_OutSideTime_28days":
                            dr["Department"] = deptName;
                            dr["Title"] = "THÁNG " + month.ToString() + " NĂM " + year.ToString();
                            dr["Footer"] = "Hà Nội, Ngày ... tháng ... năm ...";
                            dr["TotalStaff"] = objReturn.Count() - 1;
                            fileName = "Tổng hợp ngoài giờ định xuất tháng " + month.ToString() + " năm " + year.ToString() + ".xlsx";
                            var sqlNameDayOfWeek2 = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "Attendance", "get-name-day-of-week");
                            var responseNameDayOfWeek2 = await _dapperUnitOfWork.GetRepository().QueryAsync<object>(sql: sqlNameDayOfWeek2, new { @Month = month, @Year = year }, null, CommandType.StoredProcedure);
                            lstNameDayOfWeek = responseNameDayOfWeek2.ToList();
                            lstDataStyle = new string[] { "T7", "CN" };
                            break;
                    }
                    Master.Rows.Add(dr);

                    
                    // list dữ liệu
                    DataTable dtl = null;
                    if (objReturn != null)
                    {
                        dtl = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(objReturn), (typeof(DataTable)));
                    }

                    // Tạo bảng dữ liệu Chi tiết ( Details )
                    DataTable detail = dtl;
                    detail.TableName = "Details";

                    // Tạo Dataset ghi dữ liệu Master + Details 
                    var ds = new DataSet();
                    ds.Tables.Add(detail);
                    ds.Tables.Add(Master);

                    if (lstNameDayOfWeek != null)
                    {
                        // danh sách tên ngày trong tuần
                        DataTable dtNamDayOfWeek = null;
                        if (lstNameDayOfWeek != null)
                        {
                            dtNamDayOfWeek = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(lstNameDayOfWeek), (typeof(DataTable)));
                        }
                        // tạo bảng dữ liệu
                        DataTable dtndow = dtNamDayOfWeek;
                        dtndow.TableName = "NameDayOfWeek";
                        // thêm vào dataset
                        ds.Tables.Add(dtndow);
                    }

                    // Lấy tên file đầu vào và đầu ra 
                    var fileTmp = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "Template", reportName.ToString());
                    var fileOutput = JSONObject.GetQueryFromJSON("SqlCommand/Report.json", "Output", reportName.ToString());
                    var pathExport = Path.Combine(Environment.CurrentDirectory, @"OutputExcel\" + fileOutput);

                    ExcelFillData.FillReportAttendance(fileOutput, fileTmp, ds, new string[] { "{", "}" }, lstDataStyle);
                    MemoryStream ms = new MemoryStream();
                    byte[] byteArray = File.ReadAllBytes(pathExport);

                    ms.Write(byteArray, 0, byteArray.Length);
                    HttpResponseMessage result = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new StreamContent(ms)
                    };
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
                    var fileContentResult = new FileContentResult(byteArray, mimeType)
                    {
                        FileDownloadName = fileName.ToString()
                    };
                    return new Response<object>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, fileContentResult);

                }

                return new Response<object>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, false);
            }
            catch (Exception ex)
            {

                return new Response<object>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, ex.Message);
            }
        }
    }
}
