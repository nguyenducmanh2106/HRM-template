using Microsoft.Extensions.Logging;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SV.HRM.Core.Utils;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using SV.HRM.Caching.Interface;
using Microsoft.AspNetCore.Http;
using static SV.HRM.Core.Utils.Constant;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class ImportExcelHandler : IImportExcelHandler
    {
        #region Variable

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBaseHandler _baseHandler;
        private readonly IStaffHandler _staffHandler;
        private readonly IHistoryHandler _historyHandler;
        private readonly IStaffDiplomaHandler _diplomaHandler;
        private readonly IStaffPartyHandler _partyHandler;
        private readonly IDecisionHandler _decisionHandler;
        private readonly IStaffRewardHandler _rewardHandler;
        private readonly IDisciplineDetailHandler _disciplineHandler;
        private readonly IStaffFamilyHandler _familyHandler;
        private readonly IStaffSalaryHandler _salaryHandler;
        private readonly IQuanLySucKhoeHandler _qlskHandler;
        private readonly ILabourContractHandler _contractHandler;
        private readonly ICached _cached;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Contructor

        public ImportExcelHandler(IBaseHandler baseHandler, IStaffHandler staffHandler, IHistoryHandler historyHandler, IStaffDiplomaHandler diplomaHandler, IStaffPartyHandler partyHandler, IDecisionHandler decisionHandler, IStaffRewardHandler rewardHandler, IDisciplineDetailHandler disciplineHandler, IStaffFamilyHandler familyHandler, IStaffSalaryHandler salaryHandler, IQuanLySucKhoeHandler qlskHandler, ILabourContractHandler contractHandler, ICached cached, IHttpContextAccessor httpContextAccessor)
        {
            _baseHandler = baseHandler;
            _staffHandler = staffHandler;
            _historyHandler = historyHandler;
            _diplomaHandler = diplomaHandler;
            _partyHandler = partyHandler;
            _decisionHandler = decisionHandler;
            _rewardHandler = rewardHandler;
            _disciplineHandler = disciplineHandler;
            _familyHandler = familyHandler;
            _salaryHandler = salaryHandler;
            _qlskHandler = qlskHandler;
            _contractHandler = contractHandler;
            _cached = cached;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Main function Import data

        /// <summary>
        /// Import Staff Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<string>> ImportStaffData(ImportExcelServiceModel model)
        {
            try
            {
                var bytes = Convert.FromBase64String(model.FileContent);
                var contents = new StreamContent(new MemoryStream(bytes));
                var fileExt = Path.GetExtension(model.FileName);

                //Declare the sheet interface
                ISheet sheet;

                //Get the Excel file according to the extension
                if (fileExt.ToLower() == Constant.ExcelFileFormat.XLS)
                {
                    //Use the NPOI Excel xls object
                    var hssfwb = new HSSFWorkbook(await contents.ReadAsStreamAsync());
                    //Assign the sheet
                    sheet = hssfwb.GetSheetAt(0);
                }
                else if (fileExt.ToLower() == Constant.ExcelFileFormat.XLSX)
                {
                    //Use the NPOI Excel xlsx object
                    var hssfwb = new XSSFWorkbook(await contents.ReadAsStreamAsync());

                    //Assign the sheet
                    sheet = hssfwb.GetSheetAt(0);
                }
                else
                {
                    return new Response<string>(0, string.Empty, "Không đúng định dạng file import");
                }
                var result = string.Empty;
                switch (model.DataType)
                {
                    case Constant.StaffTab.TAB_BANG_CAP_CHUNG_CHI:
                        result = await ImportBangCapChungChi(sheet);
                        break;
                    case Constant.StaffTab.TAB_HOP_DONG_LAO_DONG:
                        result = await ImportHopDongLaoDong(sheet);
                        break;
                    case Constant.StaffTab.TAB_KHEN_THUONG:
                        result = await ImportKhenThuong(sheet);
                        break;
                    case Constant.StaffTab.TAB_KY_LUAT:
                        result = await ImportKyLuat(sheet);
                        break;
                    case Constant.StaffTab.TAB_QUAN_HE_GIA_DINH:
                        result = await ImportQuanHeGiaDinh(sheet);
                        break;
                    case Constant.StaffTab.TAB_QUA_TRINH_CONG_TAC:
                        result = await ImportQuaTrinhCongTac(sheet);
                        break;
                    case Constant.StaffTab.TAB_QUA_TRINH_LUONG:
                        result = await ImportQuaTrinhLuong(sheet);
                        break;
                    case Constant.StaffTab.TAB_QUYET_DINH:
                        result = await ImportQuyetDinh(sheet);
                        break;
                    case Constant.StaffTab.TAB_SUC_KHOE:
                        result = await ImportSucKhoe(sheet);
                        break;
                    case Constant.StaffTab.TAB_THONG_TIN_CHUNG:
                        result = await ImportThongTinChung(sheet);
                        break;
                    case Constant.StaffTab.TAB_THONG_TIN_DANG:
                        result = await ImportThongTinDang(sheet);
                        break;
                    case Constant.StaffTab.TAB_THONG_TIN_KHAC:
                        result = await ImportThongTinKhac(sheet);
                        break;
                }
                return new Response<string>(1, string.Empty, result);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<string>(0, string.Empty, "Có lỗi khi thực hiện import dữ liệu");
            }
        }

        #endregion

        #region Import Staff Data

        /// <summary>
        /// Import bằng cấp chứng chỉ
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportBangCapChungChi(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 1)
                {
                    return "Không đúng cấu trúc file";
                }
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);


                var lstLoaiBangCap = _baseHandler.GetListData<DiplomaModel>("Diploma", "1", 1);
                var lstTrinhDoDaoTao = _baseHandler.GetListData<TrinhDoDaoTaoModel>("TrinhDoDaoTao", "1", 1);
                var lstTrinhDoChuyenMon = _baseHandler.GetListData<TrinhDoChuyenMonModel>("TrinhDoChuyenMon", "1", 1);
                var lstChuyenKhoa = _baseHandler.GetListData<ChuyenKhoaModel>("ChuyenKhoa", "1", 1);
                var lstChuyenNganh = _baseHandler.GetListData<ChuyenNganhModel>("ChuyenNganh", "1", 1);
                var lstDonViDaoTao = _baseHandler.GetListData<SchoolModel>("School", "1", 1);
                var lstHeDaoTao = _baseHandler.GetListData<SpecialityModel>("Speciality", "1", 1);

                var success = 0;
                var fail = 0;
                bool isValidate;

                var curRow = 1;
                while (curRow <= sheet.LastRowNum)
                {
                    var maNV = sheet.GetRow(curRow).GetCell(1);
                    if (maNV != null)
                    {
                        isValidate = true;

                        #region Get data from excel

                        var loaibangcap = sheet.GetRow(curRow).GetCell(2);
                        var tenloaivanbang = sheet.GetRow(curRow).GetCell(3);
                        var trinhdodaotao = sheet.GetRow(curRow).GetCell(4);
                        var trinhdochuyenmon = sheet.GetRow(curRow).GetCell(5);
                        var chuyenkhoa = sheet.GetRow(curRow).GetCell(6);
                        var chuyennganh = sheet.GetRow(curRow).GetCell(7);
                        var sovanbang = sheet.GetRow(curRow).GetCell(8);
                        var ngaycapbang = sheet.GetRow(curRow).GetCell(9);
                        var donvidaotao = sheet.GetRow(curRow).GetCell(10);
                        var hedaotao = sheet.GetRow(curRow).GetCell(11);
                        var xeploai = sheet.GetRow(curRow).GetCell(12);
                        var ghichu = sheet.GetRow(curRow).GetCell(13);

                        #endregion

                        var staffId = _staffHandler.GetStaffIDByStaffCode(maNV.ToString().Trim());
                        if (staffId > 0)
                        {
                            var diplomaNo = string.Empty;
                            if ((sovanbang!=null) && (!string.IsNullOrEmpty(sovanbang.ToString().Trim())))
                            {
                                diplomaNo = sovanbang.ToString().Trim();
                            }
                            if (string.IsNullOrEmpty(diplomaNo))
                            {
                                isValidate = false;
                            }

                            if (isValidate)
                            {
                                var staffDiploma = await _diplomaHandler.FindByStaffAndDiplomaNo(staffId, diplomaNo);
                                if (staffDiploma != null)
                                {
                                    #region Update model - Bang cap chung chi

                                    //Cap nhat bang cap
                                    var model = new StaffDiplomaUpdateRequestModel();
                                    model.StaffDiplomaID = staffDiploma.StaffDiplomaID;
                                    model.StaffID = staffId;
                                    model.DiplomaNo = diplomaNo;
                                    model.FileUpload = staffDiploma.FileUpload;
                                    if ((loaibangcap != null) && (!string.IsNullOrEmpty(loaibangcap.ToString().Trim())))
                                    {
                                        model.DiplomaID = lstLoaiBangCap.Where(x => x.DiplomaName.Trim() == loaibangcap.ToString().Trim()).FirstOrDefault()?.DiplomaID;
                                        if(model.DiplomaID == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((tenloaivanbang != null) && (!string.IsNullOrEmpty(tenloaivanbang.ToString().Trim())))
                                    {
                                        model.ExtraText10 = tenloaivanbang.ToString().Trim();
                                    }
                                    if ((trinhdodaotao != null) && (!string.IsNullOrEmpty(trinhdodaotao.ToString().Trim())))
                                    {
                                        model.TrinhDoDaoTaoID = lstTrinhDoDaoTao.Where(x => (x.TrinhDoDaoTaoName.Trim() == trinhdodaotao.ToString().Trim()) && (x.DiplomaID == model.DiplomaID)).FirstOrDefault()?.TrinhDoDaoTaoID;
                                    }
                                    if ((trinhdochuyenmon != null) && (!string.IsNullOrEmpty(trinhdochuyenmon.ToString().Trim())))
                                    {
                                        model.TrinhDoChuyenMonID = lstTrinhDoChuyenMon.Where(x => (x.TrinhDoChuyenMonName.Trim() == trinhdochuyenmon.ToString().Trim()) && (x.DiplomaID == model.DiplomaID)).FirstOrDefault()?.TrinhDoChuyenMonID;
                                    }
                                    if ((chuyenkhoa != null) && (!string.IsNullOrEmpty(chuyenkhoa.ToString().Trim())))
                                    {
                                        model.ChuyenKhoaID = lstChuyenKhoa.Where(x => (x.TenChuyenKhoa.Trim() == chuyenkhoa.ToString().Trim())).FirstOrDefault()?.ChuyenKhoaID;
                                    }
                                    if ((chuyennganh != null) && (!string.IsNullOrEmpty(chuyennganh.ToString().Trim())))
                                    {
                                        model.ExtraNumber1 = lstChuyenNganh.Where(x => (x.ChuyenNganhName.Trim() == chuyennganh.ToString().Trim())).FirstOrDefault()?.ChuyenNganhID;
                                    }
                                    //if ((chuyennganh != null) && (!string.IsNullOrEmpty(chuyennganh.ToString().Trim())))
                                    //{
                                    //    model.MainSubject = chuyennganh.ToString().Trim();
                                    //}
                                    if ((ngaycapbang != null) && (!string.IsNullOrEmpty(ngaycapbang.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.ExtraDate3 = DateTime.ParseExact(ngaycapbang.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((donvidaotao != null) && (!string.IsNullOrEmpty(donvidaotao.ToString().Trim())))
                                    {
                                        model.SchoolID = lstDonViDaoTao.Where(x => (x.SchoolName.Trim() == donvidaotao.ToString().Trim())).FirstOrDefault()?.SchoolID;
                                    }
                                    if ((hedaotao != null) && (!string.IsNullOrEmpty(hedaotao.ToString().Trim())))
                                    {
                                        model.SpecialityID = lstHeDaoTao.Where(x => (x.SpecialityName.Trim() == hedaotao.ToString().Trim())).FirstOrDefault()?.SpecialityID;
                                    }
                                    if ((xeploai != null) && (!string.IsNullOrEmpty(xeploai.ToString().Trim())))
                                    {
                                        model.ExtraText1 = xeploai.ToString().Trim();
                                    }
                                    if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                    {
                                        model.Note = ghichu.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _diplomaHandler.Update(model.StaffDiplomaID, model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                                else
                                {
                                    #region Create model - Bang cap chung chi

                                    //Them moi bang cap
                                    var model = new StaffDiplomaCreateRequestModel();
                                    
                                    model.StaffID = staffId;
                                    model.DiplomaNo = diplomaNo;
                                    if ((loaibangcap != null) && (!string.IsNullOrEmpty(loaibangcap.ToString().Trim())))
                                    {
                                        model.DiplomaID = lstLoaiBangCap.Where(x => x.DiplomaName.Trim() == loaibangcap.ToString().Trim()).FirstOrDefault()?.DiplomaID;
                                        if (model.DiplomaID == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((tenloaivanbang != null) && (!string.IsNullOrEmpty(tenloaivanbang.ToString().Trim())))
                                    {
                                        model.ExtraText10 = tenloaivanbang.ToString().Trim();
                                    }
                                    if ((trinhdodaotao != null) && (!string.IsNullOrEmpty(trinhdodaotao.ToString().Trim())))
                                    {
                                        model.TrinhDoDaoTaoID = lstTrinhDoDaoTao.Where(x => (x.TrinhDoDaoTaoName.Trim() == trinhdodaotao.ToString().Trim()) && (x.DiplomaID == model.DiplomaID)).FirstOrDefault()?.TrinhDoDaoTaoID;
                                    }
                                    if ((trinhdochuyenmon != null) && (!string.IsNullOrEmpty(trinhdochuyenmon.ToString().Trim())))
                                    {
                                        model.TrinhDoChuyenMonID = lstTrinhDoChuyenMon.Where(x => (x.TrinhDoChuyenMonName.Trim() == trinhdochuyenmon.ToString().Trim()) && (x.DiplomaID == model.DiplomaID)).FirstOrDefault()?.TrinhDoChuyenMonID;
                                    }
                                    if ((chuyenkhoa != null) && (!string.IsNullOrEmpty(chuyenkhoa.ToString().Trim())))
                                    {
                                        model.ChuyenKhoaID = lstChuyenKhoa.Where(x => (x.TenChuyenKhoa.Trim() == chuyenkhoa.ToString().Trim())).FirstOrDefault()?.ChuyenKhoaID;
                                    }
                                    if ((chuyennganh != null) && (!string.IsNullOrEmpty(chuyennganh.ToString().Trim())))
                                    {
                                        model.ExtraNumber1 = lstChuyenNganh.Where(x => (x.ChuyenNganhName.Trim() == chuyennganh.ToString().Trim())).FirstOrDefault()?.ChuyenNganhID;
                                    }
                                    //if ((chuyennganh != null) && (!string.IsNullOrEmpty(chuyennganh.ToString().Trim())))
                                    //{
                                    //    model.MainSubject = chuyennganh.ToString().Trim();
                                    //}
                                    if ((ngaycapbang != null) && (!string.IsNullOrEmpty(ngaycapbang.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.ExtraDate3 = DateTime.ParseExact(ngaycapbang.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((donvidaotao != null) && (!string.IsNullOrEmpty(donvidaotao.ToString().Trim())))
                                    {
                                        model.SchoolID = lstDonViDaoTao.Where(x => (x.SchoolName.Trim() == donvidaotao.ToString().Trim())).FirstOrDefault()?.SchoolID;
                                    }
                                    if ((hedaotao != null) && (!string.IsNullOrEmpty(hedaotao.ToString().Trim())))
                                    {
                                        model.SpecialityID = lstHeDaoTao.Where(x => (x.SpecialityName.Trim() == hedaotao.ToString().Trim())).FirstOrDefault()?.SpecialityID;
                                    }
                                    if ((xeploai != null) && (!string.IsNullOrEmpty(xeploai.ToString().Trim())))
                                    {
                                        model.ExtraText1 = xeploai.ToString().Trim();
                                    }
                                    if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                    {
                                        model.Note = ghichu.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _diplomaHandler.Create(model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                    }
                    curRow = curRow + 1;
                }
                // Write log
                _baseHandler.CreateLog(new SystemLogModel
                {
                    ApplicationId = userModel.ApplicationId,
                    UserId = userModel.UserId,
                    ActionByUser = LogAction.IMPORT,
                    Date = DateTime.Now,
                    Content = $"Import bằng cấp chứng chỉ: Thành công: " + success + " bản ghi. Lỗi: " + fail + " bản ghi"
                });
                return "Import thành công: " + success + " bản ghi. Import lỗi: " + fail + " bản ghi";
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        /// <summary>
        /// Import hợp đồng lao động
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportHopDongLaoDong(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 1)
                {
                    return "Không đúng cấu trúc file";
                }
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                var lstLoaiHopDong = _baseHandler.GetListData<ContractTypeModel>("ContractType", "1", 1);

                var success = 0;
                var fail = 0;
                bool isValidate;

                var curRow = 1;
                while (curRow <= sheet.LastRowNum)
                {
                    var maNV = sheet.GetRow(curRow).GetCell(1);
                    if (maNV != null)
                    {
                        isValidate = true;

                        #region Get data from excel

                        var sohopdong = sheet.GetRow(curRow).GetCell(2);
                        var ngaykyhopdong = sheet.GetRow(curRow).GetCell(3);
                        var ngayhieuluctu = sheet.GetRow(curRow).GetCell(4);
                        var ngayhieulucden = sheet.GetRow(curRow).GetCell(5);
                        var loaihopdong = sheet.GetRow(curRow).GetCell(6);
                        var doituonglaodong = sheet.GetRow(curRow).GetCell(7);
                        var ghichu = sheet.GetRow(curRow).GetCell(8);

                        #endregion

                        var staffId = _staffHandler.GetStaffIDByStaffCode(maNV.ToString().Trim());
                        if (staffId > 0)
                        {
                            var labourContractNo = string.Empty;
                            if ((sohopdong != null) && (!string.IsNullOrEmpty(sohopdong.ToString().Trim())))
                            {
                                labourContractNo = sohopdong.ToString().Trim();
                            }
                            if (string.IsNullOrEmpty(labourContractNo))
                            {
                                isValidate = false;
                            }
                            int? contractType = null;
                            if ((loaihopdong != null) && (!string.IsNullOrEmpty(loaihopdong.ToString().Trim())))
                            {
                                contractType = lstLoaiHopDong.Where(x => (x.ContractTypeName.Trim() == loaihopdong.ToString().Trim())).FirstOrDefault()?.ContractTypeID;
                                if (!contractType.HasValue)
                                {
                                    isValidate = false;
                                }
                            }
                            else
                            {
                                isValidate = false;
                            }
                            int? labourObject = null;
                            if ((doituonglaodong != null) && (!string.IsNullOrEmpty(doituonglaodong.ToString().Trim())))
                            {
                                switch (doituonglaodong.ToString().Trim())
                                {
                                    case Constant.DoiTuongLaoDong.VIEN_CHUC:
                                        labourObject = Constant.DoiTuongLaoDong.NUM_VIEN_CHUC;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_68:
                                        labourObject = Constant.DoiTuongLaoDong.NUM_HOP_DONG_68;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_KHONG_XAC_DINH_THOI_HAN:
                                        labourObject = Constant.DoiTuongLaoDong.NUM_HOP_DONG_KHONG_XAC_DINH_THOI_HAN;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_NGAN_HAN:
                                        labourObject = Constant.DoiTuongLaoDong.NUM_HOP_DONG_NGAN_HAN;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_THU_VIEC:
                                        labourObject = Constant.DoiTuongLaoDong.NUM_HOP_DONG_THU_VIEC;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_CHUYEN_GIA:
                                        labourObject = Constant.DoiTuongLaoDong.NUM_HOP_DONG_CHUYEN_GIA;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_THOI_VU:
                                        labourObject = Constant.DoiTuongLaoDong.NUM_HOP_DONG_THOI_VU;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_KHAC:
                                        labourObject = Constant.DoiTuongLaoDong.NUM_HOP_DONG_KHAC;
                                        break;
                                    default:
                                        labourObject = null;
                                        break;
                                }
                                if (!labourObject.HasValue)
                                {
                                    isValidate = false;
                                }
                            }
                            else
                            {
                                isValidate = false;
                            }
                            DateTime? contractDate = null;
                            if ((ngaykyhopdong != null) && (!string.IsNullOrEmpty(ngaykyhopdong.ToString().Trim())))
                            {
                                try
                                {
                                    contractDate = DateTime.ParseExact(ngaykyhopdong.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    if (contractDate > DateTime.Now)
                                    {
                                        isValidate = false;
                                    }
                                }
                                catch (Exception)
                                {
                                    isValidate = false;
                                }
                            }
                            else
                            {
                                isValidate = false;
                            }
                            DateTime? contractFromDate = null;
                            if ((ngayhieuluctu != null) && (!string.IsNullOrEmpty(ngayhieuluctu.ToString().Trim())))
                            {
                                try
                                {
                                    contractFromDate = DateTime.ParseExact(ngayhieuluctu.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                    isValidate = false;
                                }
                            }
                            else
                            {
                                isValidate = false;
                            }
                            DateTime? contractToDate = null;
                            if ((ngayhieulucden != null) && (!string.IsNullOrEmpty(ngayhieulucden.ToString().Trim())))
                            {
                                try
                                {
                                    contractToDate = DateTime.ParseExact(ngayhieulucden.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((contractFromDate.HasValue) && (contractToDate.HasValue) && (contractFromDate.Value > contractToDate.Value))
                            {
                                isValidate = false;
                            }

                            if (isValidate)
                            {
                                var labourContract = await _contractHandler.FindByStaffAndContractInfo(staffId, labourContractNo, contractType.Value, contractFromDate, contractToDate);
                                if (labourContract != null)
                                {
                                    #region Update model - Hop dong lao dong

                                    //Cap nhat bang cap
                                    var model = new LabourContractUpdateModel();
                                    model.LabourContractID = labourContract.LabourContractID;
                                    model.StaffID = staffId;
                                    model.LabourContractNo = labourContractNo;
                                    model.FileUpload = labourContract.FileUpload;
                                    model.ContractTypeID = contractType;
                                    model.ContractDate = contractDate;
                                    model.ContractFromDate = contractFromDate;
                                    model.ContractToDate = contractToDate;
                                    model.ExtraNumber2 = labourObject.Value;

                                    if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                    {
                                        model.ExtraText9 = ghichu.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _contractHandler.Update(model.LabourContractID, model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                                else
                                {
                                    #region Create model - Hop dong lao dong

                                    //Them moi bang cap
                                    var model = new LabourContractCreateModel();

                                    model.StaffID = staffId;
                                    model.LabourContractNo = labourContractNo;
                                    model.ContractTypeID = contractType;
                                    model.ContractDate = contractDate;
                                    model.ContractFromDate = contractFromDate;
                                    model.ContractToDate = contractToDate;
                                    model.ExtraNumber2 = labourObject.Value;

                                    if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                    {
                                        model.ExtraText9 = ghichu.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _contractHandler.Create(model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        //else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                    }
                    curRow = curRow + 1;
                }
                // Write log
                _baseHandler.CreateLog(new SystemLogModel
                {
                    ApplicationId = userModel.ApplicationId,
                    UserId = userModel.UserId,
                    ActionByUser = LogAction.IMPORT,
                    Date = DateTime.Now,
                    Content = $"Import hợp đồng lao động: Thành công: " + success + " bản ghi. Lỗi: " + fail + " bản ghi"
                });
                return "Import thành công: " + success + " bản ghi. Import lỗi: " + fail + " bản ghi";
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        /// <summary>
        /// Import khen thưởng
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportKhenThuong(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 1)
                {
                    return "Không đúng cấu trúc file";
                }
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                var lstOrganization = _baseHandler.GetSystemListData<OrganizationModel>("Organizations", "1", 1);
                var lstDanhHieuKhenThuong = _baseHandler.GetListData<RewardModel>("Reward", "1", 1);

                var success = 0;
                var fail = 0;
                bool isValidate;

                var curRow = 1;
                while (curRow <= sheet.LastRowNum)
                {
                    #region Get data from excel

                    var loaikhenthuong = sheet.GetRow(curRow).GetCell(1);
                    var manhanvien = sheet.GetRow(curRow).GetCell(2);
                    var khoaphong = sheet.GetRow(curRow).GetCell(3);
                    var soquyetdinh = sheet.GetRow(curRow).GetCell(4);
                    var ngaycap = sheet.GetRow(curRow).GetCell(5);
                    var namkhenthuong = sheet.GetRow(curRow).GetCell(6);
                    var dennam = sheet.GetRow(curRow).GetCell(7);
                    var danhhieukhenthuong = sheet.GetRow(curRow).GetCell(8);
                    var ghichu = sheet.GetRow(curRow).GetCell(9);

                    #endregion

                    isValidate = true;

                    if ((loaikhenthuong == null) || (string.IsNullOrEmpty(loaikhenthuong.ToString().Trim())))
                    {
                        isValidate = false;
                    }
                    if((loaikhenthuong != null) && !loaikhenthuong.ToString().Trim().Equals(Constant.LoaiKhenThuong.KHEN_THUONG_CA_NHAN) && !loaikhenthuong.ToString().Trim().Equals(Constant.LoaiKhenThuong.KHEN_THUONG_CA_NHAN))
                    {
                        isValidate = false;
                    }

                    if (isValidate)
                    {
                        var rewardType = 0;
                        switch (loaikhenthuong.ToString().Trim())
                        {
                            case Constant.LoaiKhenThuong.KHEN_THUONG_CA_NHAN:
                                rewardType = Constant.LoaiKhenThuong.NUM_KHEN_THUONG_CA_NHAN;
                                break;
                            case Constant.LoaiKhenThuong.KHEN_THUONG_TAP_THE:
                                rewardType = Constant.LoaiKhenThuong.NUM_KHEN_THUONG_TAP_THE;
                                break;
                        }
                        //Khen thưởng cá nhân
                        if (rewardType.Equals(Constant.LoaiKhenThuong.NUM_KHEN_THUONG_CA_NHAN))
                        {
                            if ((manhanvien != null) && (!string.IsNullOrEmpty(manhanvien.ToString().Trim())))
                            {
                                var staffId = _staffHandler.GetStaffIDByStaffCode(manhanvien.ToString().Trim());
                                if (staffId > 0)
                                {
                                    var decisionNo = string.Empty;
                                    if ((soquyetdinh != null) && (!string.IsNullOrEmpty(soquyetdinh.ToString().Trim())))
                                    {
                                        decisionNo = soquyetdinh.ToString().Trim();
                                    }
                                    if (string.IsNullOrEmpty(decisionNo))
                                    {
                                        isValidate = false;
                                    }
                                    if (isValidate)
                                    {
                                        var reward = await _rewardHandler.FindByDecisionNo(Constant.LoaiKhenThuong.NUM_KHEN_THUONG_CA_NHAN, staffId, decisionNo);

                                        if (reward != null)
                                        {
                                            #region Update model - Khen thưởng

                                            var model = new StaffRewardUpdateModel();
                                            model.StaffRewardID = reward.StaffRewardID;
                                            model.StaffID = staffId;
                                            model.RewardType = Constant.LoaiKhenThuong.NUM_KHEN_THUONG_CA_NHAN;
                                            model.RewardDecisionNo = decisionNo;
                                            model.FileUpload = reward.FileUpload;
                                            if ((ngaycap != null) && (!string.IsNullOrEmpty(ngaycap.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.RewardDate = DateTime.ParseExact(ngaycap.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                                    if (model.RewardDate > DateTime.Now)
                                                    {
                                                        isValidate = false;
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    isValidate = false;
                                                }
                                            }
                                            else
                                            {
                                                isValidate = false;
                                            }

                                            if ((namkhenthuong != null) && (!string.IsNullOrEmpty(namkhenthuong.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.FromYear = Convert.ToInt32(namkhenthuong.ToString().Trim()).ToString().Trim();
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }

                                            if ((dennam != null) && (!string.IsNullOrEmpty(dennam.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.ToYear = Convert.ToInt32(dennam.ToString().Trim()).ToString().Trim();
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }

                                            if ((danhhieukhenthuong != null) && (!string.IsNullOrEmpty(danhhieukhenthuong.ToString().Trim())))
                                            {
                                                model.RewardID = lstDanhHieuKhenThuong.Where(x => x.RewardName.Trim() == danhhieukhenthuong.ToString().Trim()).FirstOrDefault()?.RewardID;
                                                if (model.RewardID == null)
                                                {
                                                    isValidate = false;
                                                }
                                            }
                                            else
                                            {
                                                isValidate = false;
                                            }
                                            if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                            {
                                                model.Note = ghichu.ToString().Trim();
                                            }

                                            if (isValidate)
                                            {
                                                var result = await _rewardHandler.Update(model.StaffRewardID, model);
                                                if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                                {
                                                    success = success + 1;
                                                }
                                                else
                                                {
                                                    fail = fail + 1;
                                                }
                                            }
                                            else
                                            {
                                                fail = fail + 1;
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            #region Create model - Khen thưởng

                                            var model = new StaffRewardCreateModel();
                                            model.StaffID = staffId;
                                            model.RewardType = Constant.LoaiKhenThuong.NUM_KHEN_THUONG_CA_NHAN;
                                            model.RewardDecisionNo = decisionNo;
                                            if ((ngaycap != null) && (!string.IsNullOrEmpty(ngaycap.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.RewardDate = DateTime.ParseExact(ngaycap.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                                    if (model.RewardDate > DateTime.Now)
                                                    {
                                                        isValidate = false;
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    isValidate = false;
                                                }
                                            }
                                            else
                                            {
                                                isValidate = false;
                                            }

                                            if ((namkhenthuong != null) && (!string.IsNullOrEmpty(namkhenthuong.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.FromYear = Convert.ToInt32(namkhenthuong.ToString().Trim()).ToString().Trim();
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }

                                            if ((dennam != null) && (!string.IsNullOrEmpty(dennam.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.ToYear = Convert.ToInt32(dennam.ToString().Trim()).ToString().Trim();
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }

                                            if ((danhhieukhenthuong != null) && (!string.IsNullOrEmpty(danhhieukhenthuong.ToString().Trim())))
                                            {
                                                model.RewardID = lstDanhHieuKhenThuong.Where(x => x.RewardName.Trim() == danhhieukhenthuong.ToString().Trim()).FirstOrDefault()?.RewardID;
                                                if (model.RewardID == null)
                                                {
                                                    isValidate = false;
                                                }
                                            }
                                            else
                                            {
                                                isValidate = false;
                                            }
                                            if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                            {
                                                model.Note = ghichu.ToString().Trim();
                                            }

                                            if (isValidate)
                                            {
                                                var result = await _rewardHandler.Create(model);
                                                if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                                {
                                                    success = success + 1;
                                                }
                                                else
                                                {
                                                    fail = fail + 1;
                                                }
                                            }
                                            else
                                            {
                                                fail = fail + 1;
                                            }

                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    fail = fail + 1;
                                }
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                        //Khen thưởng tập thể
                        if (rewardType.Equals(Constant.LoaiKhenThuong.NUM_KHEN_THUONG_TAP_THE))
                        {
                            if ((khoaphong != null) && (!string.IsNullOrEmpty(khoaphong.ToString().Trim())))
                            {
                                var deptId = lstOrganization.Where(x => x.OrganizationName.Trim() == khoaphong.ToString().Trim()).FirstOrDefault()?.OrganizationId;
                                if ((deptId.HasValue) &&(deptId.Value > 0))
                                {
                                    var decisionNo = string.Empty;
                                    if ((soquyetdinh != null) && (!string.IsNullOrEmpty(soquyetdinh.ToString().Trim())))
                                    {
                                        decisionNo = soquyetdinh.ToString().Trim();
                                    }
                                    if (string.IsNullOrEmpty(decisionNo))
                                    {
                                        isValidate = false;
                                    }
                                    if (isValidate)
                                    {
                                        var reward = await _rewardHandler.FindByDecisionNo(Constant.LoaiKhenThuong.NUM_KHEN_THUONG_TAP_THE, deptId.Value, decisionNo);

                                        if (reward != null)
                                        {
                                            #region Update model - Khen thưởng

                                            var model = new StaffRewardUpdateModel();
                                            model.StaffRewardID = reward.StaffRewardID;
                                            model.DeptID = deptId.Value;
                                            model.RewardType = Constant.LoaiKhenThuong.NUM_KHEN_THUONG_TAP_THE;
                                            model.RewardDecisionNo = decisionNo;
                                            model.FileUpload = reward.FileUpload;
                                            if ((ngaycap != null) && (!string.IsNullOrEmpty(ngaycap.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.RewardDate = DateTime.ParseExact(ngaycap.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                                    if (model.RewardDate > DateTime.Now)
                                                    {
                                                        isValidate = false;
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    isValidate = false;
                                                }
                                            }
                                            else
                                            {
                                                isValidate = false;
                                            }

                                            if ((namkhenthuong != null) && (!string.IsNullOrEmpty(namkhenthuong.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.FromYear = Convert.ToInt32(namkhenthuong.ToString().Trim()).ToString().Trim();
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }

                                            if ((dennam != null) && (!string.IsNullOrEmpty(dennam.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.ToYear = Convert.ToInt32(dennam.ToString().Trim()).ToString().Trim();
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }

                                            if ((danhhieukhenthuong != null) && (!string.IsNullOrEmpty(danhhieukhenthuong.ToString().Trim())))
                                            {
                                                model.RewardID = lstDanhHieuKhenThuong.Where(x => x.RewardName.Trim() == danhhieukhenthuong.ToString().Trim()).FirstOrDefault()?.RewardID;
                                                if (model.RewardID == null)
                                                {
                                                    isValidate = false;
                                                }
                                            }
                                            else
                                            {
                                                isValidate = false;
                                            }
                                            if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                            {
                                                model.Note = ghichu.ToString().Trim();
                                            }

                                            if (isValidate)
                                            {
                                                var result = await _rewardHandler.Update(model.StaffRewardID, model);
                                                if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                                {
                                                    success = success + 1;
                                                }
                                                else
                                                {
                                                    fail = fail + 1;
                                                }
                                            }
                                            else
                                            {
                                                fail = fail + 1;
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            #region Create model - Khen thưởng

                                            var model = new StaffRewardCreateModel();
                                            model.DeptID = deptId.Value;
                                            model.RewardType = Constant.LoaiKhenThuong.NUM_KHEN_THUONG_TAP_THE;
                                            model.RewardDecisionNo = decisionNo;
                                            if ((ngaycap != null) && (!string.IsNullOrEmpty(ngaycap.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.RewardDate = DateTime.ParseExact(ngaycap.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                                    if (model.RewardDate > DateTime.Now)
                                                    {
                                                        isValidate = false;
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    isValidate = false;
                                                }
                                            }
                                            else
                                            {
                                                isValidate = false;
                                            }

                                            if ((namkhenthuong != null) && (!string.IsNullOrEmpty(namkhenthuong.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.FromYear = Convert.ToInt32(namkhenthuong.ToString().Trim()).ToString().Trim();
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }

                                            if ((dennam != null) && (!string.IsNullOrEmpty(dennam.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.ToYear = Convert.ToInt32(dennam.ToString().Trim()).ToString().Trim();
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }

                                            if ((danhhieukhenthuong != null) && (!string.IsNullOrEmpty(danhhieukhenthuong.ToString().Trim())))
                                            {
                                                model.RewardID = lstDanhHieuKhenThuong.Where(x => x.RewardName.Trim() == danhhieukhenthuong.ToString().Trim()).FirstOrDefault()?.RewardID;
                                                if (model.RewardID == null)
                                                {
                                                    isValidate = false;
                                                }
                                            }
                                            else
                                            {
                                                isValidate = false;
                                            }
                                            if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                            {
                                                model.Note = ghichu.ToString().Trim();
                                            }

                                            if (isValidate)
                                            {
                                                var result = await _rewardHandler.Create(model);
                                                if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                                {
                                                    success = success + 1;
                                                }
                                                else
                                                {
                                                    fail = fail + 1;
                                                }
                                            }
                                            else
                                            {
                                                fail = fail + 1;
                                            }

                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    fail = fail + 1;
                                }
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                    }
                    else
                    {
                        fail = fail + 1;
                    }
                    curRow = curRow + 1;
                }
                // Write log
                _baseHandler.CreateLog(new SystemLogModel
                {
                    ApplicationId = userModel.ApplicationId,
                    UserId = userModel.UserId,
                    ActionByUser = LogAction.IMPORT,
                    Date = DateTime.Now,
                    Content = $"Import khen thưởng: Thành công: " + success + " bản ghi. Lỗi: " + fail + " bản ghi"
                });
                return "Import thành công: " + success + " bản ghi. Import lỗi: " + fail + " bản ghi";
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        /// <summary>
        /// Import kỷ luật
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportKyLuat(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 1)
                {
                    return "Không đúng cấu trúc file";
                }
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                var lstOrganization = _baseHandler.GetSystemListData<OrganizationModel>("Organizations", "1", 1);
                var lstHinhThucKyLuat = _baseHandler.GetListData<DisciplineTypeModel>("DisciplineType", "1", 1);
                var lstMucDoViPhamKyLuat = _baseHandler.GetListData<MucDoViPhamKyLuatModel>("MucDoViPhamKyLuat", "1", 1);
                var lstTrinhDoChuyenMon = _baseHandler.GetListData<TrinhDoChuyenMonModel>("TrinhDoChuyenMon", "1", 1);

                var success = 0;
                var fail = 0;
                bool isValidate;

                var curRow = 1;
                while (curRow <= sheet.LastRowNum)
                {
                    var maNV = sheet.GetRow(curRow).GetCell(1);
                    if (maNV != null)
                    {
                        isValidate = true;

                        #region Get data from excel

                        var soquyetdinh = sheet.GetRow(curRow).GetCell(2);
                        var ngayquyetdinh = sheet.GetRow(curRow).GetCell(3);
                        var hinhthuckyluat = sheet.GetRow(curRow).GetCell(4);
                        var mucdovipham = sheet.GetRow(curRow).GetCell(5);
                        var ngayhieuluc = sheet.GetRow(curRow).GetCell(6);
                        var denngay = sheet.GetRow(curRow).GetCell(7);
                        var hanhvivipham = sheet.GetRow(curRow).GetCell(8);
                        var mucboithuong = sheet.GetRow(curRow).GetCell(9);
                        var phuongthucboithuong = sheet.GetRow(curRow).GetCell(10);

                        #endregion

                        var staffId = _staffHandler.GetStaffIDByStaffCode(maNV.ToString().Trim());
                        if (staffId > 0)
                        {
                            var decisionNo = string.Empty;
                            if ((soquyetdinh != null) && (!string.IsNullOrEmpty(soquyetdinh.ToString().Trim())))
                            {
                                decisionNo = soquyetdinh.ToString().Trim();
                            }
                            if (string.IsNullOrEmpty(decisionNo))
                            {
                                isValidate = false;
                            }

                            if (isValidate)
                            {
                                var discipline = await _disciplineHandler.FindByStaffAndDecisionNo(staffId, decisionNo);
                                if (discipline != null)
                                {
                                    #region Update model - Ky luat

                                    var model = new DisciplineDetailUpdateModel();
                                    model.DisciplineDetailID = discipline.DisciplineDetailID;
                                    model.StaffID = staffId;
                                    model.DecisionNo = decisionNo;
                                    model.FileUpload = discipline.FileUpload;
                                    if ((ngayquyetdinh != null) && (!string.IsNullOrEmpty(ngayquyetdinh.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.DisciplineDate = DateTime.ParseExact(ngayquyetdinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                            if (model.DisciplineDate > DateTime.Now)
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((hinhthuckyluat != null) && (!string.IsNullOrEmpty(hinhthuckyluat.ToString().Trim())))
                                    {
                                        model.DisciplineTypeID = lstHinhThucKyLuat.Where(x => (x.Description1.Trim() == hinhthuckyluat.ToString().Trim())).FirstOrDefault()?.DisciplineTypeID;
                                        if (model.DisciplineTypeID == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((mucdovipham != null) && (!string.IsNullOrEmpty(mucdovipham.ToString().Trim())))
                                    {
                                        model.PeriodID = lstMucDoViPhamKyLuat.Where(x => (x.MucDoViPhamKyLuatName.Trim() == mucdovipham.ToString().Trim())).FirstOrDefault()?.MucDoViPhamKyLuatID;
                                        if (model.PeriodID == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((ngayhieuluc != null) && (!string.IsNullOrEmpty(ngayhieuluc.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.EffectiveFrom = DateTime.ParseExact(ngayhieuluc.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((denngay != null) && (!string.IsNullOrEmpty(denngay.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.EffectiveTo = DateTime.ParseExact(denngay.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((model.EffectiveFrom.HasValue) && (model.EffectiveTo.HasValue) && (model.EffectiveTo < model.EffectiveFrom))
                                    {
                                        isValidate = false;
                                    }
                                    if ((hanhvivipham != null) && (!string.IsNullOrEmpty(hanhvivipham.ToString().Trim())))
                                    {
                                        model.Note = hanhvivipham.ToString().Trim();
                                    }
                                    if ((mucboithuong != null) && (!string.IsNullOrEmpty(mucboithuong.ToString().Trim())))
                                    {
                                        model.CompensationLevel = Convert.ToDouble(mucboithuong.ToString().Trim());
                                    }
                                    if ((phuongthucboithuong != null) && (!string.IsNullOrEmpty(phuongthucboithuong.ToString().Trim())))
                                    {
                                        model.CompensationMode = phuongthucboithuong.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _disciplineHandler.Update(model.DisciplineDetailID, model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                                else
                                {
                                    #region Create model - Ky luat

                                    var model = new DisciplineDetailCreateModel();

                                    model.StaffID = staffId;
                                    model.DecisionNo = decisionNo;
                                    //model.FileUpload = discipline.FileUpload;
                                    if ((ngayquyetdinh != null) && (!string.IsNullOrEmpty(ngayquyetdinh.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.DisciplineDate = DateTime.ParseExact(ngayquyetdinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                            if (model.DisciplineDate > DateTime.Now)
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((hinhthuckyluat != null) && (!string.IsNullOrEmpty(hinhthuckyluat.ToString().Trim())))
                                    {
                                        model.DisciplineTypeID = lstHinhThucKyLuat.Where(x => (x.Description1.Trim() == hinhthuckyluat.ToString().Trim())).FirstOrDefault()?.DisciplineTypeID;
                                        if (model.DisciplineTypeID == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((mucdovipham != null) && (!string.IsNullOrEmpty(mucdovipham.ToString().Trim())))
                                    {
                                        model.PeriodID = lstMucDoViPhamKyLuat.Where(x => (x.MucDoViPhamKyLuatName.Trim() == mucdovipham.ToString().Trim())).FirstOrDefault()?.MucDoViPhamKyLuatID;
                                        if (model.PeriodID == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((ngayhieuluc != null) && (!string.IsNullOrEmpty(ngayhieuluc.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.EffectiveFrom = DateTime.ParseExact(ngayhieuluc.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((denngay != null) && (!string.IsNullOrEmpty(denngay.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.EffectiveTo = DateTime.ParseExact(denngay.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((model.EffectiveFrom.HasValue) && (model.EffectiveTo.HasValue) && (model.EffectiveTo < model.EffectiveFrom))
                                    {
                                        isValidate = false;
                                    }
                                    if ((hanhvivipham != null) && (!string.IsNullOrEmpty(hanhvivipham.ToString().Trim())))
                                    {
                                        model.Note = hanhvivipham.ToString().Trim();
                                    }
                                    if ((mucboithuong != null) && (!string.IsNullOrEmpty(mucboithuong.ToString().Trim())))
                                    {
                                        model.CompensationLevel = Convert.ToDouble(mucboithuong.ToString().Trim());
                                    }
                                    if ((phuongthucboithuong != null) && (!string.IsNullOrEmpty(phuongthucboithuong.ToString().Trim())))
                                    {
                                        model.CompensationMode = phuongthucboithuong.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _disciplineHandler.Create(model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                    }
                    curRow = curRow + 1;
                }
                // Write log
                _baseHandler.CreateLog(new SystemLogModel
                {
                    ApplicationId = userModel.ApplicationId,
                    UserId = userModel.UserId,
                    ActionByUser = LogAction.IMPORT,
                    Date = DateTime.Now,
                    Content = $"Import ky luật: Thành công: " + success + " bản ghi. Lỗi: " + fail + " bản ghi"
                });
                return "Import thành công: " + success + " bản ghi. Import lỗi: " + fail + " bản ghi";
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        /// <summary>
        /// Import quan hệ gia đình
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportQuanHeGiaDinh(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 2)
                {
                    return "Không đúng cấu trúc file";
                }
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                var lstEthnic = _baseHandler.GetSystemListData<EthnicModel>("Ethnics", "1", 1);
                var lstCountry = _baseHandler.GetSystemListData<CountryModel>("Countries", "1", 1);
                var lstLocation = _baseHandler.GetSystemListData<LocationModel>("Locations", "1", 1);
                var lstDistrict = _baseHandler.GetSystemListData<DistrictModel>("Districts", "1", 1);
                var lstWard = _baseHandler.GetSystemListData<WardModel>("Wards", "1", 1);
                var lstQuanHeGiaDinh = _baseHandler.GetListData<FamilyRelationShipModel>("FamilyRelationship", "1", 1);

                var success = 0;
                var fail = 0;
                bool isValidate;

                var curRow = 2;
                while (curRow <= sheet.LastRowNum)
                {
                    var maNV = sheet.GetRow(curRow).GetCell(1);
                    if (maNV != null)
                    {
                        isValidate = true;

                        #region Get data from excel

                        var quanhegiadinh = sheet.GetRow(curRow).GetCell(2);
                        var hoten = sheet.GetRow(curRow).GetCell(3);
                        var ngaysinh = sheet.GetRow(curRow).GetCell(4);
                        var gioitinh = sheet.GetRow(curRow).GetCell(5);
                        var cmnd = sheet.GetRow(curRow).GetCell(6);
                        var noicapcmnd = sheet.GetRow(curRow).GetCell(7);
                        var ngaycapcmnd = sheet.GetRow(curRow).GetCell(8);
                        var ngayhethancmnd = sheet.GetRow(curRow).GetCell(9);
                        var sodienthoai = sheet.GetRow(curRow).GetCell(10);
                        var mahogiadinh = sheet.GetRow(curRow).GetCell(11);
                        var masothuecanhan = sheet.GetRow(curRow).GetCell(12);
                        var masothuenguoiphuthuoc = sheet.GetRow(curRow).GetCell(13);
                        var masobhxh = sheet.GetRow(curRow).GetCell(14);
                        var quequan = sheet.GetRow(curRow).GetCell(15);
                        var hokhauthuongtru = sheet.GetRow(curRow).GetCell(16);
                        var noiohientai = sheet.GetRow(curRow).GetCell(17);
                        var nghenghiephientai = sheet.GetRow(curRow).GetCell(18);
                        var thunhap = sheet.GetRow(curRow).GetCell(19);
                        var gksquyenso = sheet.GetRow(curRow).GetCell(20);
                        var gkssogiaykhaisinh = sheet.GetRow(curRow).GetCell(21);
                        var gksdantoc = sheet.GetRow(curRow).GetCell(22);
                        var gksquoctich = sheet.GetRow(curRow).GetCell(23);
                        var gkstinhthanh = sheet.GetRow(curRow).GetCell(24);
                        var gksquanhuyen = sheet.GetRow(curRow).GetCell(25);
                        var gksphuongxa = sheet.GetRow(curRow).GetCell(26);

                        #endregion

                        var staffId = _staffHandler.GetStaffIDByStaffCode(maNV.ToString().Trim());
                        if (staffId > 0)
                        {
                            int? relationshipId = null;
                            if ((quanhegiadinh != null) && (!string.IsNullOrEmpty(quanhegiadinh.ToString().Trim())))
                            {
                                relationshipId = lstQuanHeGiaDinh.Where(x => x.FamilyRelationshipName.Trim() == quanhegiadinh.ToString().Trim()).FirstOrDefault()?.FamilyRelationshipID;
                                if (!relationshipId.HasValue)
                                {
                                    isValidate = false;
                                }
                            }
                            else
                            {
                                isValidate = false;
                            }
                            var name = string.Empty;
                            if ((hoten != null) && (!string.IsNullOrEmpty(hoten.ToString().Trim())))
                            {
                                name = hoten.ToString().Trim();
                            }
                            if (string.IsNullOrEmpty(name))
                            {
                                isValidate = false;
                            }

                            if (isValidate)
                            {
                                var staffFamily = await _familyHandler.FindByStaffAndRelation(staffId, relationshipId.Value, name);
                                if (staffFamily != null)
                                {
                                    #region Update model - Quan he gia dinh

                                    var model = new StaffFamilyUpdateModel();
                                    model.StaffFamilyID = staffFamily.StaffFamilyID;
                                    model.StaffID = staffId;
                                    model.Relationship = relationshipId.Value;
                                    model.FullName = name;
                                    model.FileUpload = staffFamily.FileUpload;
                                    if ((ngaysinh != null) && (!string.IsNullOrEmpty(ngaysinh.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.Birthday = DateTime.ParseExact(ngaysinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                            if (model.Birthday > DateTime.Now)
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((gioitinh != null) && (!string.IsNullOrEmpty(gioitinh.ToString().Trim())))
                                    {
                                        switch (gioitinh.ToString().Trim())
                                        {
                                            case Constant.GioiTinh.NU:
                                                model.ExtraNumber1 = Constant.GioiTinh.NUM_NU;
                                                break;
                                            case Constant.GioiTinh.NAM:
                                                model.ExtraNumber1 = Constant.GioiTinh.NUM_NAM;
                                                break;
                                            case Constant.GioiTinh.KHAC:
                                                model.ExtraNumber1 = Constant.GioiTinh.NUM_KHAC;
                                                break;
                                            default:
                                                model.ExtraNumber1 = null;
                                                break;
                                        }
                                        if (model.ExtraNumber1 == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((cmnd != null) && (!string.IsNullOrEmpty(cmnd.ToString().Trim())))
                                    {
                                        model.IDCardNo = cmnd.ToString().Trim();
                                        if (string.IsNullOrEmpty(model.IDCardNo))
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    if ((noicapcmnd != null) && (!string.IsNullOrEmpty(noicapcmnd.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.IDCardIssuePlaceID = lstLocation.Where(x => x.LocationName.Trim() == noicapcmnd.ToString().Trim()).FirstOrDefault().LocationID;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((ngaycapcmnd != null) && (!string.IsNullOrEmpty(ngaycapcmnd.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.IDCardIssueDate = DateTime.ParseExact(ngaycapcmnd.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((ngayhethancmnd != null) && (!string.IsNullOrEmpty(ngayhethancmnd.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.IDCardExpireDate = DateTime.ParseExact(ngayhethancmnd.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((model.IDCardIssueDate.HasValue) && (model.IDCardExpireDate.HasValue) && (model.IDCardExpireDate < model.IDCardIssueDate))
                                    {
                                        isValidate = false;
                                    }
                                    if ((sodienthoai != null) && (!string.IsNullOrEmpty(sodienthoai.ToString().Trim())))
                                    {
                                        model.Telephone = sodienthoai.ToString().Trim();
                                    }
                                    if ((mahogiadinh != null) && (!string.IsNullOrEmpty(mahogiadinh.ToString().Trim())))
                                    {
                                        model.ExtraText2 = mahogiadinh.ToString().Trim();
                                    }
                                    if ((masothuecanhan != null) && (!string.IsNullOrEmpty(masothuecanhan.ToString().Trim())))
                                    {
                                        model.PITCode = masothuecanhan.ToString().Trim();
                                    }
                                    if ((masothuenguoiphuthuoc != null) && (!string.IsNullOrEmpty(masothuenguoiphuthuoc.ToString().Trim())))
                                    {
                                        model.ExtraText5 = masothuenguoiphuthuoc.ToString().Trim();
                                    }
                                    if ((masobhxh != null) && (!string.IsNullOrEmpty(masobhxh.ToString().Trim())))
                                    {
                                        model.ExtraText1 = masobhxh.ToString().Trim();
                                    }
                                    if ((quequan != null) && (!string.IsNullOrEmpty(quequan.ToString().Trim())))
                                    {
                                        model.Addr = quequan.ToString().Trim();
                                    }
                                    if ((hokhauthuongtru != null) && (!string.IsNullOrEmpty(hokhauthuongtru.ToString().Trim())))
                                    {
                                        model.HoKhauThuongTru = hokhauthuongtru.ToString().Trim();
                                    }
                                    if ((noiohientai != null) && (!string.IsNullOrEmpty(noiohientai.ToString().Trim())))
                                    {
                                        model.TypeHouse = noiohientai.ToString().Trim();
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((nghenghiephientai != null) && (!string.IsNullOrEmpty(nghenghiephientai.ToString().Trim())))
                                    {
                                        model.Job = nghenghiephientai.ToString().Trim();
                                    }
                                    if ((quequan != null) && (!string.IsNullOrEmpty(quequan.ToString().Trim())))
                                    {
                                        model.Addr = quequan.ToString().Trim();
                                    }
                                    if ((thunhap != null) && (!string.IsNullOrEmpty(thunhap.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.Income = Convert.ToDouble(thunhap.ToString().Trim());
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((gksquyenso != null) && (!string.IsNullOrEmpty(gksquyenso.ToString().Trim())))
                                    {
                                        model.CerID = gksquyenso.ToString().Trim();
                                    }
                                    if ((gkssogiaykhaisinh != null) && (!string.IsNullOrEmpty(gkssogiaykhaisinh.ToString().Trim())))
                                    {
                                        model.CerNumber = gkssogiaykhaisinh.ToString().Trim();
                                    }
                                    if ((gksdantoc != null) && (!string.IsNullOrEmpty(gksdantoc.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.ExtraNumber2 = lstLocation.Where(x => x.LocationName.Trim() == gksdantoc.ToString().Trim()).FirstOrDefault().LocationID;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((gksquoctich != null) && (!string.IsNullOrEmpty(gksquoctich.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.CerTerritoryID = lstCountry.Where(x => x.CountryName.Trim() == gksquoctich.ToString().Trim()).FirstOrDefault().CountryId;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((gkstinhthanh != null) && (!string.IsNullOrEmpty(gkstinhthanh.ToString().Trim())))
                                    {
                                        //if (model.CerTerritoryID.HasValue)
                                        //{
                                            try
                                            {
                                                model.CerLocationID = lstLocation.Where(x => (x.LocationName.Trim() == gkstinhthanh.ToString().Trim())).FirstOrDefault().LocationID;
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        //}
                                    }
                                    if ((gksquanhuyen != null) && (!string.IsNullOrEmpty(gksquanhuyen.ToString().Trim())))
                                    {
                                        if (model.CerLocationID.HasValue)
                                        {
                                            try
                                            {
                                                model.CerDistrictID = lstDistrict.Where(x => ((x.DistrictName.Trim() == gksquanhuyen.ToString().Trim()) && (x.LocationId == model.CerLocationID))).FirstOrDefault().DistrictId;
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                    }
                                    if ((gksphuongxa != null) && (!string.IsNullOrEmpty(gksphuongxa.ToString().Trim())))
                                    {
                                        if (model.CerDistrictID.HasValue)
                                        {
                                            try
                                            {
                                                model.CerWardID = lstWard.Where(x => ((x.WardName.Trim() == gksphuongxa.ToString().Trim()) && (x.DistrictId == model.CerDistrictID))).FirstOrDefault().WardId;
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _familyHandler.Update(model.StaffFamilyID, model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                                else
                                {
                                    #region Create model - Quan he gia dinh

                                    var model = new StaffFamilyCreateModel();

                                    model.StaffID = staffId;
                                    model.Relationship = relationshipId.Value;
                                    model.FullName = name;
                                    if ((ngaysinh != null) && (!string.IsNullOrEmpty(ngaysinh.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.Birthday = DateTime.ParseExact(ngaysinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                            if (model.Birthday > DateTime.Now)
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((gioitinh != null) && (!string.IsNullOrEmpty(gioitinh.ToString().Trim())))
                                    {
                                        switch (gioitinh.ToString().Trim())
                                        {
                                            case Constant.GioiTinh.NU:
                                                model.ExtraNumber1 = Constant.GioiTinh.NUM_NU;
                                                break;
                                            case Constant.GioiTinh.NAM:
                                                model.ExtraNumber1 = Constant.GioiTinh.NUM_NAM;
                                                break;
                                            case Constant.GioiTinh.KHAC:
                                                model.ExtraNumber1 = Constant.GioiTinh.NUM_KHAC;
                                                break;
                                            default:
                                                model.ExtraNumber1 = null;
                                                break;
                                        }
                                        if (model.ExtraNumber1 == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((cmnd != null) && (!string.IsNullOrEmpty(cmnd.ToString().Trim())))
                                    {
                                        model.IDCardNo = cmnd.ToString().Trim();
                                        if (string.IsNullOrEmpty(model.IDCardNo))
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    if ((noicapcmnd != null) && (!string.IsNullOrEmpty(noicapcmnd.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.IDCardIssuePlaceID = lstLocation.Where(x => x.LocationName.Trim() == noicapcmnd.ToString().Trim()).FirstOrDefault().LocationID;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((ngaycapcmnd != null) && (!string.IsNullOrEmpty(ngaycapcmnd.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.IDCardIssueDate = DateTime.ParseExact(ngaycapcmnd.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((ngayhethancmnd != null) && (!string.IsNullOrEmpty(ngayhethancmnd.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.IDCardExpireDate = DateTime.ParseExact(ngayhethancmnd.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((model.IDCardIssueDate.HasValue) && (model.IDCardExpireDate.HasValue) && (model.IDCardExpireDate < model.IDCardIssueDate))
                                    {
                                        isValidate = false;
                                    }
                                    if ((sodienthoai != null) && (!string.IsNullOrEmpty(sodienthoai.ToString().Trim())))
                                    {
                                        model.Telephone = sodienthoai.ToString().Trim();
                                    }
                                    if ((mahogiadinh != null) && (!string.IsNullOrEmpty(mahogiadinh.ToString().Trim())))
                                    {
                                        model.ExtraText2 = mahogiadinh.ToString().Trim();
                                    }
                                    if ((masothuecanhan != null) && (!string.IsNullOrEmpty(masothuecanhan.ToString().Trim())))
                                    {
                                        model.PITCode = masothuecanhan.ToString().Trim();
                                    }
                                    if ((masothuenguoiphuthuoc != null) && (!string.IsNullOrEmpty(masothuenguoiphuthuoc.ToString().Trim())))
                                    {
                                        model.ExtraText5 = masothuenguoiphuthuoc.ToString().Trim();
                                    }
                                    if ((masobhxh != null) && (!string.IsNullOrEmpty(masobhxh.ToString().Trim())))
                                    {
                                        model.ExtraText1 = masobhxh.ToString().Trim();
                                    }
                                    if ((quequan != null) && (!string.IsNullOrEmpty(quequan.ToString().Trim())))
                                    {
                                        model.Addr = quequan.ToString().Trim();
                                    }
                                    if ((hokhauthuongtru != null) && (!string.IsNullOrEmpty(hokhauthuongtru.ToString().Trim())))
                                    {
                                        model.HoKhauThuongTru = hokhauthuongtru.ToString().Trim();
                                    }
                                    if ((noiohientai != null) && (!string.IsNullOrEmpty(noiohientai.ToString().Trim())))
                                    {
                                        model.TypeHouse = noiohientai.ToString().Trim();
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((nghenghiephientai != null) && (!string.IsNullOrEmpty(nghenghiephientai.ToString().Trim())))
                                    {
                                        model.Job = nghenghiephientai.ToString().Trim();
                                    }
                                    if ((quequan != null) && (!string.IsNullOrEmpty(quequan.ToString().Trim())))
                                    {
                                        model.Addr = quequan.ToString().Trim();
                                    }
                                    if ((thunhap != null) && (!string.IsNullOrEmpty(thunhap.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.Income = Convert.ToDouble(thunhap.ToString().Trim());
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((gksquyenso != null) && (!string.IsNullOrEmpty(gksquyenso.ToString().Trim())))
                                    {
                                        model.CerID = gksquyenso.ToString().Trim();
                                    }
                                    if ((gkssogiaykhaisinh != null) && (!string.IsNullOrEmpty(gkssogiaykhaisinh.ToString().Trim())))
                                    {
                                        model.CerNumber = gkssogiaykhaisinh.ToString().Trim();
                                    }
                                    if ((gksdantoc != null) && (!string.IsNullOrEmpty(gksdantoc.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.ExtraNumber2 = lstLocation.Where(x => x.LocationName.Trim() == gksdantoc.ToString().Trim()).FirstOrDefault().LocationID;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((gksquoctich != null) && (!string.IsNullOrEmpty(gksquoctich.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.CerTerritoryID = lstCountry.Where(x => x.CountryName.Trim() == gksquoctich.ToString().Trim()).FirstOrDefault().CountryId;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((gkstinhthanh != null) && (!string.IsNullOrEmpty(gkstinhthanh.ToString().Trim())))
                                    {
                                        //if (model.CerTerritoryID.HasValue)
                                        //{
                                            try
                                            {
                                                model.CerLocationID = lstLocation.Where(x => (x.LocationName.Trim() == gkstinhthanh.ToString().Trim())).FirstOrDefault().LocationID;
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        //}
                                    }
                                    if ((gksquanhuyen != null) && (!string.IsNullOrEmpty(gksquanhuyen.ToString().Trim())))
                                    {
                                        if (model.CerLocationID.HasValue)
                                        {
                                            try
                                            {
                                                model.CerDistrictID = lstDistrict.Where(x => ((x.DistrictName.Trim() == gksquanhuyen.ToString().Trim()) && (x.LocationId == model.CerLocationID))).FirstOrDefault().DistrictId;
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                    }
                                    if ((gksphuongxa != null) && (!string.IsNullOrEmpty(gksphuongxa.ToString().Trim())))
                                    {
                                        if (model.CerDistrictID.HasValue)
                                        {
                                            try
                                            {
                                                model.CerWardID = lstWard.Where(x => ((x.WardName.Trim() == gksphuongxa.ToString().Trim()) && (x.DistrictId == model.CerDistrictID))).FirstOrDefault().WardId;
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _familyHandler.Create(model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                    }
                    curRow = curRow + 1;
                }
                // Write log
                _baseHandler.CreateLog(new SystemLogModel
                {
                    ApplicationId = userModel.ApplicationId,
                    UserId = userModel.UserId,
                    ActionByUser = LogAction.IMPORT,
                    Date = DateTime.Now,
                    Content = $"Import quan hệ gia đình: Thành công: " + success + " bản ghi. Lỗi: " + fail + " bản ghi"
                });
                return "Import thành công: " + success + " bản ghi. Import lỗi: " + fail + " bản ghi";
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        /// <summary>
        /// Import quá trình công tác
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportQuaTrinhCongTac(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 1)
                {
                    return "Không đúng cấu trúc file";
                }

                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                var lstOrganization = _baseHandler.GetSystemListData<OrganizationModel>("Organizations", "1", 1);
                var lstChucDanhNgheNghiep = _baseHandler.GetListData<PositionModel>("Position", "1", 1);
                var lstChucVuChinhQuyen = _baseHandler.GetListData<ChucVuChinhQuyenModel>("ChucVuChinhQuyen", "1", 1);
                var lstChucVuKiemNhiem = _baseHandler.GetListData<ChucVuKiemNhiemModel>("ChucVuKiemNhiem", "1", 1);
                var lstViTriViecLam = _baseHandler.GetListData<JobTitleModel>("JobTitle", "1", 1);
                //var lstTrinhDoChuyenMon = _baseHandler.GetListData<TrinhDoChuyenMonModel>("TrinhDoChuyenMon", "1", 1);

                var success = 0;
                var fail = 0;
                bool isValidate;

                var curRow = 1;
                while (curRow <= sheet.LastRowNum)
                {
                    var maNV = sheet.GetRow(curRow).GetCell(1);
                    if (maNV != null)
                    {
                        isValidate = true;

                        #region Get data from excel

                        var trangthai = sheet.GetRow(curRow).GetCell(2);
                        var khoaphong = sheet.GetRow(curRow).GetCell(3);
                        var soquyetdinh = sheet.GetRow(curRow).GetCell(4);
                        var ngayquyetdinh = sheet.GetRow(curRow).GetCell(5);
                        var tungay = sheet.GetRow(curRow).GetCell(6);
                        var denngay = sheet.GetRow(curRow).GetCell(7);
                        var chucdanhnghenghiep = sheet.GetRow(curRow).GetCell(8);
                        var vitrivieclam = sheet.GetRow(curRow).GetCell(9);
                        //var trinhdochuyenmon = sheet.GetRow(curRow).GetCell(10);
                        var chucvuchinhquyen = sheet.GetRow(curRow).GetCell(10);
                        var chucvukiemnhiem = sheet.GetRow(curRow).GetCell(11);
                        var ngaybatdaunhiemky = sheet.GetRow(curRow).GetCell(12);
                        var ngayketthucnhiemky = sheet.GetRow(curRow).GetCell(13);
                        var ghichu = sheet.GetRow(curRow).GetCell(14);

                        #endregion

                        var staffId = _staffHandler.GetStaffIDByStaffCode(maNV.ToString().Trim());
                        if (staffId > 0)
                        {
                            int? status = null;
                            DateTime? fromDate = null;
                            DateTime? toDate = null;
                            if ((trangthai != null) && (!string.IsNullOrEmpty(trangthai.ToString().Trim())))
                            {
                                switch (trangthai.ToString().Trim())
                                {
                                    case Constant.TrangThaiQuaTrinhCongTac.CHINH_THUC:
                                        status = Constant.TrangThaiQuaTrinhCongTac.NUM_CHINH_THUC;
                                        break;
                                    case Constant.TrangThaiQuaTrinhCongTac.DAO_TAO_VA_TAP_NGHE:
                                        status = Constant.TrangThaiQuaTrinhCongTac.NUM_DAO_TAO_VA_TAP_NGHE;
                                        break;
                                    case Constant.TrangThaiQuaTrinhCongTac.DIEU_DONG_TANG_CUONG:
                                        status = Constant.TrangThaiQuaTrinhCongTac.NUM_DIEU_DONG_TANG_CUONG;
                                        break;
                                    case Constant.TrangThaiQuaTrinhCongTac.THU_VIEC:
                                        status = Constant.TrangThaiQuaTrinhCongTac.NUM_THU_VIEC;
                                        break;
                                    default:
                                        status = null;
                                        break;
                                }
                                if (!status.HasValue)
                                {
                                    isValidate = false;
                                }
                            }
                            else
                            {
                                isValidate = false;
                            }

                            if ((tungay != null) && (!string.IsNullOrEmpty(tungay.ToString().Trim())))
                            {
                                try
                                {
                                    fromDate = DateTime.ParseExact(tungay.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                    isValidate = false;
                                }
                                if (!fromDate.HasValue)
                                {
                                    isValidate = false;
                                }
                            }
                            else
                            {
                                isValidate = false;
                            }

                            if ((denngay != null) && (!string.IsNullOrEmpty(denngay.ToString().Trim())))
                            {
                                try
                                {
                                    toDate = DateTime.ParseExact(denngay.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }

                            if ((fromDate.HasValue) && (toDate.HasValue) && (fromDate.Value > toDate.Value))
                            {
                                isValidate = false;
                            }

                            if (isValidate)
                            {
                                var history = await _historyHandler.FindByStaffAndDate(staffId, status.Value, fromDate, toDate);
                                if (history != null)
                                {
                                    #region Update model - QTCT

                                    var model = new HistoryUpdateRequestModel();
                                    model.HistoryID = history.HistoryID;
                                    model.StaffID = staffId;
                                    model.FileUpload = history.FileUpload;
                                    model.Status = status.Value;
                                    model.FromDate = fromDate;
                                    model.Todate = toDate;
                                    model.ExtraLogic1 = Constant.LoaiQuaTrinhCongTac.QTCT_TAI_DON_VI;
                                    
                                    if ((khoaphong != null) && (!string.IsNullOrEmpty(khoaphong.ToString().Trim())))
                                    {
                                        model.DeptID = lstOrganization.Where(x => x.OrganizationName.Trim() == khoaphong.ToString().Trim()).FirstOrDefault()?.OrganizationId;
                                        if (model.DeptID == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }

                                    if ((soquyetdinh != null) && (!string.IsNullOrEmpty(soquyetdinh.ToString().Trim())))
                                    {
                                        model.HistoryNo = soquyetdinh.ToString().Trim();
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }

                                    if ((ngayquyetdinh != null) && (!string.IsNullOrEmpty(ngayquyetdinh.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.DecisionDate = DateTime.ParseExact(ngayquyetdinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }

                                    if ((chucdanhnghenghiep != null) && (!string.IsNullOrEmpty(chucdanhnghenghiep.ToString().Trim())))
                                    {
                                        model.PositionID = lstChucDanhNgheNghiep.Where(x => x.PositionName.Trim() == chucdanhnghenghiep.ToString().Trim()).FirstOrDefault()?.PositionID;
                                    }

                                    if ((vitrivieclam != null) && (!string.IsNullOrEmpty(vitrivieclam.ToString().Trim())))
                                    {
                                        model.JobTitleID = lstViTriViecLam.Where(x => x.JobTitleName.Trim() == vitrivieclam.ToString().Trim()).FirstOrDefault()?.JobTitleID;
                                    }

                                    //if ((trinhdochuyenmon != null) && (!string.IsNullOrEmpty(trinhdochuyenmon.ToString().Trim())))
                                    //{
                                    //    model.TrinhDoChuyenMonID = lstTrinhDoChuyenMon.Where(x => x.TrinhDoChuyenMonName.Trim() == trinhdochuyenmon.ToString().Trim()).FirstOrDefault()?.TrinhDoChuyenMonID;
                                    //}

                                    if ((chucvuchinhquyen != null) && (!string.IsNullOrEmpty(chucvuchinhquyen.ToString().Trim())))
                                    {
                                        model.ExtraNumber3 = lstChucVuChinhQuyen.Where(x => x.ChucVuChinhQuyenName.Trim() == chucvuchinhquyen.ToString().Trim()).FirstOrDefault()?.ChucVuChinhQuyenID;
                                    }

                                    if ((chucvukiemnhiem != null) && (!string.IsNullOrEmpty(chucvukiemnhiem.ToString().Trim())))
                                    {
                                        model.ExtraNumber4 = lstChucVuKiemNhiem.Where(x => x.ChucVuKiemNhiemName.Trim() == chucvukiemnhiem.ToString().Trim()).FirstOrDefault()?.ChucVuKiemNhiemID;
                                    }

                                    if ((ngaybatdaunhiemky != null) && (!string.IsNullOrEmpty(ngaybatdaunhiemky.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.ExtraDate1 = DateTime.ParseExact(ngaybatdaunhiemky.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }

                                    if ((ngayketthucnhiemky != null) && (!string.IsNullOrEmpty(ngayketthucnhiemky.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.ExtraDate2 = DateTime.ParseExact(ngayketthucnhiemky.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }

                                    if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                    {
                                        model.Note = ghichu.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _historyHandler.Update(model.HistoryID, model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                                else
                                {
                                    #region Create model - QTCT

                                    //Ton tai thong tin nhan vien
                                    var model = new HistoryCreateRequestModel();
                                    model.StaffID = staffId;
                                    model.Status = status.Value;
                                    model.FromDate = fromDate;
                                    model.Todate = toDate;
                                    model.ExtraLogic1 = Constant.LoaiQuaTrinhCongTac.QTCT_TAI_DON_VI;
                                    if ((trangthai != null) && (!string.IsNullOrEmpty(trangthai.ToString().Trim())))
                                    {
                                        switch (trangthai.ToString().Trim())
                                        {
                                            case Constant.TrangThaiQuaTrinhCongTac.CHINH_THUC:
                                                model.Status = Constant.TrangThaiQuaTrinhCongTac.NUM_CHINH_THUC;
                                                break;
                                            case Constant.TrangThaiQuaTrinhCongTac.DAO_TAO_VA_TAP_NGHE:
                                                model.Status = Constant.TrangThaiQuaTrinhCongTac.NUM_DAO_TAO_VA_TAP_NGHE;
                                                break;
                                            case Constant.TrangThaiQuaTrinhCongTac.DIEU_DONG_TANG_CUONG:
                                                model.Status = Constant.TrangThaiQuaTrinhCongTac.NUM_DIEU_DONG_TANG_CUONG;
                                                break;
                                            case Constant.TrangThaiQuaTrinhCongTac.THU_VIEC:
                                                model.Status = Constant.TrangThaiQuaTrinhCongTac.NUM_THU_VIEC;
                                                break;
                                            default:
                                                model.Status = null;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }

                                    if ((khoaphong != null) && (!string.IsNullOrEmpty(khoaphong.ToString().Trim())))
                                    {
                                        model.DeptID = lstOrganization.Where(x => x.OrganizationName.Trim() == khoaphong.ToString().Trim()).FirstOrDefault()?.OrganizationId;
                                        if (model.DeptID == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }

                                    if ((soquyetdinh != null) && (!string.IsNullOrEmpty(soquyetdinh.ToString().Trim())))
                                    {
                                        model.HistoryNo = soquyetdinh.ToString().Trim();
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }

                                    if ((ngayquyetdinh != null) && (!string.IsNullOrEmpty(ngayquyetdinh.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.DecisionDate = DateTime.ParseExact(ngayquyetdinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }

                                    if ((chucdanhnghenghiep != null) && (!string.IsNullOrEmpty(chucdanhnghenghiep.ToString().Trim())))
                                    {
                                        model.PositionID = lstChucDanhNgheNghiep.Where(x => x.PositionName.Trim() == chucdanhnghenghiep.ToString().Trim()).FirstOrDefault()?.PositionID;
                                    }

                                    if ((vitrivieclam != null) && (!string.IsNullOrEmpty(vitrivieclam.ToString().Trim())))
                                    {
                                        model.JobTitleID = lstViTriViecLam.Where(x => x.JobTitleName.Trim() == vitrivieclam.ToString().Trim()).FirstOrDefault()?.JobTitleID;
                                    }

                                    //if ((trinhdochuyenmon != null) && (!string.IsNullOrEmpty(trinhdochuyenmon.ToString().Trim())))
                                    //{
                                    //    model.TrinhDoChuyenMonID = lstTrinhDoChuyenMon.Where(x => x.TrinhDoChuyenMonName.Trim() == trinhdochuyenmon.ToString().Trim()).FirstOrDefault()?.TrinhDoChuyenMonID;
                                    //}

                                    if ((chucvuchinhquyen != null) && (!string.IsNullOrEmpty(chucvuchinhquyen.ToString().Trim())))
                                    {
                                        model.ExtraNumber3 = lstChucVuChinhQuyen.Where(x => x.ChucVuChinhQuyenName.Trim() == chucvuchinhquyen.ToString().Trim()).FirstOrDefault()?.ChucVuChinhQuyenID;
                                    }

                                    if ((chucvukiemnhiem != null) && (!string.IsNullOrEmpty(chucvukiemnhiem.ToString().Trim())))
                                    {
                                        model.ExtraNumber4 = lstChucVuKiemNhiem.Where(x => x.ChucVuKiemNhiemName.Trim() == chucvukiemnhiem.ToString().Trim()).FirstOrDefault()?.ChucVuKiemNhiemID;
                                    }

                                    if ((ngaybatdaunhiemky != null) && (!string.IsNullOrEmpty(ngaybatdaunhiemky.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.ExtraDate1 = DateTime.ParseExact(ngaybatdaunhiemky.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }

                                    if ((ngayketthucnhiemky != null) && (!string.IsNullOrEmpty(ngayketthucnhiemky.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.ExtraDate2 = DateTime.ParseExact(ngayketthucnhiemky.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }

                                    if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                    {
                                        model.Note = ghichu.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _historyHandler.Create(model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                    }
                    curRow = curRow + 1;
                }
                // Write log
                _baseHandler.CreateLog(new SystemLogModel
                {
                    ApplicationId = userModel.ApplicationId,
                    UserId = userModel.UserId,
                    ActionByUser = LogAction.IMPORT,
                    Date = DateTime.Now,
                    Content = $"Import quá trình công tác: Thành công: " + success + " bản ghi. Lỗi: " + fail + " bản ghi"
                });
                return "Import thành công: " + success + " bản ghi. Import lỗi: " + fail + " bản ghi";
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        /// <summary>
        /// Import quá trình lương
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportQuaTrinhLuong(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 1)
                {
                    return "Không đúng cấu trúc file";
                }
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                var lstNhomNgachLuong = _baseHandler.GetListData<NhomNgachLuongModel>("NhomNgachLuong", "1", 1);
                var lstNgachLuong = _baseHandler.GetListData<NgachLuongModel>("NgachLuong", "1", 1);
                var lstBacLuong = _baseHandler.GetListData<BacLuongModel>("BacLuong", "1", 1);

                var success = 0;
                var fail = 0;
                bool isValidate;

                var curRow = 1;
                while (curRow <= sheet.LastRowNum)
                {
                    var maNV = sheet.GetRow(curRow).GetCell(1);
                    if (maNV != null)
                    {
                        isValidate = true;

                        #region Get data from excel

                        var soquyetdinh = sheet.GetRow(curRow).GetCell(2);
                        var ngayquyetdinh = sheet.GetRow(curRow).GetCell(3);
                        var loainangluong = sheet.GetRow(curRow).GetCell(4);
                        var nhomngachluong = sheet.GetRow(curRow).GetCell(5);
                        var ngachluong = sheet.GetRow(curRow).GetCell(6);
                        var bacluong = sheet.GetRow(curRow).GetCell(7);
                        var hesobaoluu = sheet.GetRow(curRow).GetCell(8);
                        var hesothamnienvuotkhung = sheet.GetRow(curRow).GetCell(9);
                        var nangluongtruocthoihan = sheet.GetRow(curRow).GetCell(10);
                        var keodaithoihannangluong = sheet.GetRow(curRow).GetCell(11);
                        var tungay = sheet.GetRow(curRow).GetCell(12);
                        var ghichu = sheet.GetRow(curRow).GetCell(13);

                        #endregion

                        var staffId = _staffHandler.GetStaffIDByStaffCode(maNV.ToString().Trim());
                        if (staffId > 0)
                        {
                            string decisionNo = null;
                            if ((soquyetdinh != null) && (!string.IsNullOrEmpty(soquyetdinh.ToString().Trim())))
                            {
                                decisionNo = soquyetdinh.ToString().Trim();
                            }
                            //if (string.IsNullOrEmpty(decisionNo))
                            //{
                            //    isValidate = false;
                            //}
                            DateTime? decisionDate = null;
                            if ((ngayquyetdinh != null) && (!string.IsNullOrEmpty(ngayquyetdinh.ToString().Trim())))
                            {
                                try
                                {
                                    decisionDate = DateTime.ParseExact(ngayquyetdinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    if (!decisionDate.HasValue)
                                    {
                                        isValidate = false;
                                    }
                                }
                                catch (Exception)
                                {
                                    //isValidate = false;
                                }
                            }
                            //else
                            //{
                            //    isValidate = false;
                            //}

                            if (isValidate)
                            {
                                var staffSalary = await _salaryHandler.FindByStaffAndDecisionNo(staffId, decisionNo, decisionDate);
                                if (staffSalary != null)
                                {
                                    #region Update model - Qua trinh luong

                                    var model = new StaffSalaryUpdateRequestModel();
                                    model.StaffSalaryID = staffSalary.StaffSalaryID;
                                    model.StaffID = staffId;
                                    model.SoQuyetDinh = decisionNo;
                                    model.NgayQuyetDinh = decisionDate;
                                    model.FileUpload = staffSalary.FileUpload;
                                    
                                    if ((loainangluong != null) && (!string.IsNullOrEmpty(loainangluong.ToString().Trim())))
                                    {
                                        switch (loainangluong.ToString().Trim())
                                        {
                                            case Constant.LoaiNangLuong.NANG_LUONG_THUONG_XUYEN:
                                                model.LoaiNangLuong = Constant.LoaiNangLuong.NUM_NANG_LUONG_THUONG_XUYEN;
                                                break;
                                            case Constant.LoaiNangLuong.NANG_LUONG_TRUOC_THOI_HAN:
                                                model.LoaiNangLuong = Constant.LoaiNangLuong.NUM_NANG_LUONG_TRUOC_THOI_HAN;
                                                break;
                                            case Constant.LoaiNangLuong.KEO_DAI_THOI_HAN_NANG_LUONG:
                                                model.LoaiNangLuong = Constant.LoaiNangLuong.NUM_KEO_DAI_THOI_HAN_NANG_LUONG;
                                                break;
                                            default:
                                                model.LoaiNangLuong = null;
                                                break;
                                        }

                                        if(!model.LoaiNangLuong.HasValue)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }

                                    //Nhom ngach - Ngach - Bac
                                    if ((nhomngachluong != null) && (!string.IsNullOrEmpty(nhomngachluong.ToString().Trim())))
                                    {
                                        model.NhomNgachLuongID = lstNhomNgachLuong.Where(x => x.TenNhomNgachLuong.Trim() == nhomngachluong.ToString().Trim()).FirstOrDefault()?.NhomNgachLuongID;
                                        if (!model.NhomNgachLuongID.HasValue)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((ngachluong != null) && (!string.IsNullOrEmpty(ngachluong.ToString().Trim())))
                                    {
                                        if (model.NhomNgachLuongID.HasValue)
                                        {
                                            model.NgachLuongID = lstNgachLuong.Where(x => (x.TenNgachLuong.Trim() == ngachluong.ToString().Trim()) && (x.NhomNgachLuongID == model.NhomNgachLuongID.Value)).FirstOrDefault()?.NgachLuongID;
                                            if (!model.NgachLuongID.HasValue)
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        else
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    if ((bacluong != null) && (!string.IsNullOrEmpty(bacluong.ToString().Trim())))
                                    {
                                        if (model.NgachLuongID.HasValue)
                                        {
                                            model.BacLuongID = lstBacLuong.Where(x => (x.TenBacLuong.Trim() == ("Bậc " + bacluong.ToString().Trim())) && (x.NgachLuongID == model.NgachLuongID.Value)).FirstOrDefault()?.BacLuongID;
                                            if(!model.BacLuongID.HasValue)
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        else
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    
                                    if(model.LoaiNangLuong.HasValue)
                                    {
                                        if (model.LoaiNangLuong.Equals(Constant.LoaiNangLuong.NANG_LUONG_TRUOC_THOI_HAN))
                                        {
                                            if ((nangluongtruocthoihan != null) && (!string.IsNullOrEmpty(nangluongtruocthoihan.ToString().Trim())))
                                            {
                                                switch (nangluongtruocthoihan.ToString().Trim())
                                                {
                                                    case Constant.NangLuongTruocThoiHan.MONTH_4:
                                                        model.SoThangNangLuongTruocHan = Constant.NangLuongTruocThoiHan.NUM_MONTH_4;
                                                        break;
                                                    case Constant.NangLuongTruocThoiHan.MONTH_6:
                                                        model.SoThangNangLuongTruocHan = Constant.NangLuongTruocThoiHan.NUM_MONTH_6;
                                                        break;
                                                    case Constant.NangLuongTruocThoiHan.MONTH_8:
                                                        model.SoThangNangLuongTruocHan = Constant.NangLuongTruocThoiHan.NUM_MONTH_8;
                                                        break;
                                                    case Constant.NangLuongTruocThoiHan.MONTH_12:
                                                        model.SoThangNangLuongTruocHan = Constant.NangLuongTruocThoiHan.NUM_MONTH_12;
                                                        break;
                                                    default:
                                                        model.SoThangNangLuongTruocHan = null;
                                                        break;
                                                }
                                            }
                                        }
                                        if (model.LoaiNangLuong.Equals(Constant.LoaiNangLuong.KEO_DAI_THOI_HAN_NANG_LUONG))
                                        {
                                            if ((keodaithoihannangluong != null) && (!string.IsNullOrEmpty(keodaithoihannangluong.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    model.SoThangKeoDaiNangLuong = Convert.ToInt32(keodaithoihannangluong.ToString().Trim());
                                                }
                                                catch (Exception)
                                                {
                                                    model.SoThangKeoDaiNangLuong = null;
                                                }
                                            }
                                        }
                                    }

                                    if ((tungay != null) && (!string.IsNullOrEmpty(tungay.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.TuNgay = DateTime.ParseExact(tungay.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                        if ((model.NgachLuongID.HasValue) && (model.LoaiNangLuong.HasValue))
                                        {
                                            if (model.TuNgay.HasValue)
                                            {
                                                //Tinh den ngay
                                                var ngach = lstNgachLuong.Where(x => x.NgachLuongID == model.NgachLuongID).FirstOrDefault();
                                                if ((ngach!=null) && (ngach.ThoiGianNangLuong.HasValue))
                                                {
                                                    if (model.LoaiNangLuong.Equals(Constant.LoaiNangLuong.NUM_NANG_LUONG_THUONG_XUYEN))
                                                    {
                                                        model.DenNgay = model.TuNgay.Value.AddYears(ngach.ThoiGianNangLuong.Value);
                                                    }
                                                    if (model.LoaiNangLuong.Equals(Constant.LoaiNangLuong.NUM_NANG_LUONG_TRUOC_THOI_HAN))
                                                    {
                                                        if (model.SoThangNangLuongTruocHan.HasValue)
                                                        {
                                                            model.DenNgay = model.TuNgay.Value.AddYears(ngach.ThoiGianNangLuong.Value).AddMonths(model.SoThangNangLuongTruocHan.Value);
                                                        }
                                                    }
                                                    if (model.LoaiNangLuong.Equals(Constant.LoaiNangLuong.NUM_KEO_DAI_THOI_HAN_NANG_LUONG))
                                                    {
                                                        if (model.SoThangKeoDaiNangLuong.HasValue)
                                                        {
                                                            model.DenNgay = model.TuNgay.Value.AddYears(ngach.ThoiGianNangLuong.Value).AddMonths((-1)* model.SoThangKeoDaiNangLuong.Value);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        else
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((ghichu!=null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                    {
                                        model.GhiChu = ghichu.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _salaryHandler.Update(model.StaffSalaryID, model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                                else
                                {
                                    #region Create model - Qua trinh luong

                                    var createModel = new StaffSalaryCreateRequestModel();

                                    createModel.StaffID = staffId;
                                    createModel.SoQuyetDinh = decisionNo;
                                    createModel.NgayQuyetDinh = decisionDate;

                                    if ((loainangluong != null) && (!string.IsNullOrEmpty(loainangluong.ToString().Trim())))
                                    {
                                        switch (loainangluong.ToString().Trim())
                                        {
                                            case Constant.LoaiNangLuong.NANG_LUONG_THUONG_XUYEN:
                                                createModel.LoaiNangLuong = Constant.LoaiNangLuong.NUM_NANG_LUONG_THUONG_XUYEN;
                                                break;
                                            case Constant.LoaiNangLuong.NANG_LUONG_TRUOC_THOI_HAN:
                                                createModel.LoaiNangLuong = Constant.LoaiNangLuong.NUM_NANG_LUONG_TRUOC_THOI_HAN;
                                                break;
                                            case Constant.LoaiNangLuong.KEO_DAI_THOI_HAN_NANG_LUONG:
                                                createModel.LoaiNangLuong = Constant.LoaiNangLuong.NUM_KEO_DAI_THOI_HAN_NANG_LUONG;
                                                break;
                                            default:
                                                createModel.LoaiNangLuong = null;
                                                break;
                                        }

                                        if (!createModel.LoaiNangLuong.HasValue)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }

                                    //Nhom ngach - Ngach - Bac
                                    if ((nhomngachluong != null) && (!string.IsNullOrEmpty(nhomngachluong.ToString().Trim())))
                                    {
                                        createModel.NhomNgachLuongID = lstNhomNgachLuong.Where(x => x.TenNhomNgachLuong == nhomngachluong.ToString().Trim()).FirstOrDefault()?.NhomNgachLuongID;
                                        if (!createModel.NhomNgachLuongID.HasValue)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((ngachluong != null) && (!string.IsNullOrEmpty(ngachluong.ToString().Trim())))
                                    {
                                        if (createModel.NhomNgachLuongID.HasValue)
                                        {
                                            createModel.NgachLuongID = lstNgachLuong.Where(x => (x.TenNgachLuong == ngachluong.ToString().Trim()) && (x.NhomNgachLuongID == createModel.NhomNgachLuongID.Value)).FirstOrDefault()?.NgachLuongID;
                                            if (!createModel.NgachLuongID.HasValue)
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        else
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    if ((bacluong != null) && (!string.IsNullOrEmpty(bacluong.ToString().Trim())))
                                    {
                                        if (createModel.NgachLuongID.HasValue)
                                        {
                                            createModel.BacLuongID = lstBacLuong.Where(x => (x.TenBacLuong == ("Bậc " + bacluong.ToString().Trim())) && (x.NgachLuongID == createModel.NgachLuongID.Value)).FirstOrDefault()?.BacLuongID;
                                            if (!createModel.BacLuongID.HasValue)
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        else
                                        {
                                            isValidate = false;
                                        }
                                    }

                                    if (createModel.LoaiNangLuong.HasValue)
                                    {
                                        if (createModel.LoaiNangLuong.Equals(Constant.LoaiNangLuong.NANG_LUONG_TRUOC_THOI_HAN))
                                        {
                                            if ((nangluongtruocthoihan != null) && (!string.IsNullOrEmpty(nangluongtruocthoihan.ToString().Trim())))
                                            {
                                                switch (nangluongtruocthoihan.ToString().Trim())
                                                {
                                                    case Constant.NangLuongTruocThoiHan.MONTH_4:
                                                        createModel.SoThangNangLuongTruocHan = Constant.NangLuongTruocThoiHan.NUM_MONTH_4;
                                                        break;
                                                    case Constant.NangLuongTruocThoiHan.MONTH_6:
                                                        createModel.SoThangNangLuongTruocHan = Constant.NangLuongTruocThoiHan.NUM_MONTH_6;
                                                        break;
                                                    case Constant.NangLuongTruocThoiHan.MONTH_8:
                                                        createModel.SoThangNangLuongTruocHan = Constant.NangLuongTruocThoiHan.NUM_MONTH_8;
                                                        break;
                                                    case Constant.NangLuongTruocThoiHan.MONTH_12:
                                                        createModel.SoThangNangLuongTruocHan = Constant.NangLuongTruocThoiHan.NUM_MONTH_12;
                                                        break;
                                                    default:
                                                        createModel.SoThangNangLuongTruocHan = null;
                                                        break;
                                                }
                                            }
                                        }
                                        if (createModel.LoaiNangLuong.Equals(Constant.LoaiNangLuong.KEO_DAI_THOI_HAN_NANG_LUONG))
                                        {
                                            if ((keodaithoihannangluong != null) && (!string.IsNullOrEmpty(keodaithoihannangluong.ToString().Trim())))
                                            {
                                                try
                                                {
                                                    createModel.SoThangKeoDaiNangLuong = Convert.ToInt32(keodaithoihannangluong.ToString().Trim());
                                                }
                                                catch (Exception)
                                                {
                                                    createModel.SoThangKeoDaiNangLuong = null;
                                                }
                                            }
                                        }
                                    }

                                    if ((tungay != null) && (!string.IsNullOrEmpty(tungay.ToString().Trim())))
                                    {
                                        try
                                        {
                                            createModel.TuNgay = DateTime.ParseExact(tungay.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                        if ((createModel.NgachLuongID.HasValue) && (createModel.LoaiNangLuong.HasValue))
                                        {
                                            if (createModel.TuNgay.HasValue)
                                            {
                                                //Tinh den ngay
                                                var ngach = lstNgachLuong.Where(x => x.NgachLuongID == createModel.NgachLuongID).FirstOrDefault();
                                                if ((ngach != null) && (ngach.ThoiGianNangLuong.HasValue))
                                                {
                                                    if (createModel.LoaiNangLuong.Equals(Constant.LoaiNangLuong.NUM_NANG_LUONG_THUONG_XUYEN))
                                                    {
                                                        createModel.DenNgay = createModel.TuNgay.Value.AddYears(ngach.ThoiGianNangLuong.Value);
                                                    }
                                                    if (createModel.LoaiNangLuong.Equals(Constant.LoaiNangLuong.NUM_NANG_LUONG_TRUOC_THOI_HAN))
                                                    {
                                                        if (createModel.SoThangNangLuongTruocHan.HasValue)
                                                        {
                                                            createModel.DenNgay = createModel.TuNgay.Value.AddYears(ngach.ThoiGianNangLuong.Value).AddMonths(createModel.SoThangNangLuongTruocHan.Value);
                                                        }
                                                    }
                                                    if (createModel.LoaiNangLuong.Equals(Constant.LoaiNangLuong.NUM_KEO_DAI_THOI_HAN_NANG_LUONG))
                                                    {
                                                        if (createModel.SoThangKeoDaiNangLuong.HasValue)
                                                        {
                                                            createModel.DenNgay = createModel.TuNgay.Value.AddYears(ngach.ThoiGianNangLuong.Value).AddMonths((-1) * createModel.SoThangKeoDaiNangLuong.Value);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        else
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                    {
                                        createModel.GhiChu = ghichu.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _salaryHandler.Create(createModel);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                    }
                    curRow = curRow + 1;
                }
                // Write log
                _baseHandler.CreateLog(new SystemLogModel
                {
                    ApplicationId = userModel.ApplicationId,
                    UserId = userModel.UserId,
                    ActionByUser = LogAction.IMPORT,
                    Date = DateTime.Now,
                    Content = $"Import quá trình lương: Thành công: " + success + " bản ghi. Lỗi: " + fail + " bản ghi"
                });
                return "Import thành công: " + success + " bản ghi. Import lỗi: " + fail + " bản ghi";
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        /// <summary>
        /// Import quyết định
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportQuyetDinh(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 1)
                {
                    return "Không đúng cấu trúc file";
                }

                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                //var lstOrganization = _baseHandler.GetSystemListData<OrganizationModel>("Organizations", "1", 1);
                var lstLoaiQuyetDinh = _baseHandler.GetListData<DecisionItemModel>("DecisionItem", "1", 1);
                var lstViTriViecLam = _baseHandler.GetListData<JobTitleModel>("JobTitle", "1", 1);
                //var lstTrinhDoChuyenMon = _baseHandler.GetListData<TrinhDoChuyenMonModel>("TrinhDoChuyenMon", "1", 1);

                var success = 0;
                var fail = 0;
                bool isValidate;

                var curRow = 1;
                while (curRow <= sheet.LastRowNum)
                {
                    var maNV = sheet.GetRow(curRow).GetCell(1);
                    if (maNV != null)
                    {
                        isValidate = true;

                        #region Get data from excel

                        var soquyetdinh = sheet.GetRow(curRow).GetCell(2);
                        var ngayquyetdinh = sheet.GetRow(curRow).GetCell(3);
                        var loaiquyetdinh = sheet.GetRow(curRow).GetCell(4);
                        var tenquyetdinh = sheet.GetRow(curRow).GetCell(5);
                        var ngayhieuluc = sheet.GetRow(curRow).GetCell(6);
                        var ngayhethan = sheet.GetRow(curRow).GetCell(7);
                        var noidung = sheet.GetRow(curRow).GetCell(8);

                        #endregion

                        var staffId = _staffHandler.GetStaffIDByStaffCode(maNV.ToString().Trim());
                        if (staffId > 0)
                        {
                            var decisionNo = string.Empty;
                            if ((soquyetdinh != null) && (!string.IsNullOrEmpty(soquyetdinh.ToString().Trim())))
                            {
                                decisionNo = soquyetdinh.ToString().Trim();
                            }
                            if (string.IsNullOrEmpty(decisionNo))
                            {
                                isValidate = false;
                            }

                            if (isValidate)
                            {
                                var decision = await _decisionHandler.FindByStaffAndDecisionNo(staffId, decisionNo);
                                if (decision != null)
                                {
                                    #region Update model - Quyet dinh

                                    //Cap nhat bang cap
                                    var model = new DecisionUpdateModel();
                                    model.DecisionID = decision.DecisionID;
                                    model.StaffID = staffId;
                                    model.DecisionNo = decisionNo;
                                    model.FileUpload = decision.FileUpload;
                                    if ((ngayquyetdinh != null) && (!string.IsNullOrEmpty(ngayquyetdinh.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.DecisionDate = DateTime.ParseExact(ngayquyetdinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                            if(model.DecisionDate>DateTime.Now)
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((loaiquyetdinh != null) && (!string.IsNullOrEmpty(loaiquyetdinh.ToString().Trim())))
                                    {
                                        model.DecisionItemID = lstLoaiQuyetDinh.Where(x => (x.DecisionItemName.Trim() == loaiquyetdinh.ToString().Trim())).FirstOrDefault()?.DecisionItemID;
                                        if (model.DecisionItemID == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((tenquyetdinh != null) && (!string.IsNullOrEmpty(tenquyetdinh.ToString().Trim())))
                                    {
                                        model.ExtraText10 = tenquyetdinh.ToString().Trim();
                                        if(string.IsNullOrEmpty(model.ExtraText10))
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    if ((ngayhieuluc != null) && (!string.IsNullOrEmpty(ngayhieuluc.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.EffectiveFromDate = DateTime.ParseExact(ngayhieuluc.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((ngayhethan != null) && (!string.IsNullOrEmpty(ngayhethan.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.EffectiveToDate = DateTime.ParseExact(ngayhethan.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                            //isValidate = false;
                                        }
                                    }
                                    //else
                                    //{
                                    //    isValidate = false;
                                    //}
                                    if ((model.EffectiveFromDate.HasValue) && (model.EffectiveToDate.HasValue) && (model.EffectiveToDate< model.EffectiveFromDate))
                                    {
                                        isValidate = false;
                                    }
                                    if ((noidung != null) && (!string.IsNullOrEmpty(noidung.ToString().Trim())))
                                    {
                                        model.Note = noidung.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _decisionHandler.Update(model.DecisionID, model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                                else
                                {
                                    #region Create model - Bang cap chung chi

                                    //Them moi bang cap
                                    var model = new DecisionCreateModel();

                                    model.StaffID = staffId;
                                    model.DecisionNo = decisionNo;
                                    if ((ngayquyetdinh != null) && (!string.IsNullOrEmpty(ngayquyetdinh.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.DecisionDate = DateTime.ParseExact(ngayquyetdinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                            if (model.DecisionDate > DateTime.Now)
                                            {
                                                isValidate = false;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((loaiquyetdinh != null) && (!string.IsNullOrEmpty(loaiquyetdinh.ToString().Trim())))
                                    {
                                        model.DecisionItemID = lstLoaiQuyetDinh.Where(x => (x.DecisionItemName.Trim() == loaiquyetdinh.ToString().Trim())).FirstOrDefault()?.DecisionItemID;
                                        if (model.DecisionItemID == null)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((tenquyetdinh != null) && (!string.IsNullOrEmpty(tenquyetdinh.ToString().Trim())))
                                    {
                                        model.ExtraText10 = tenquyetdinh.ToString().Trim();
                                        if (string.IsNullOrEmpty(model.ExtraText10))
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    if ((ngayhieuluc != null) && (!string.IsNullOrEmpty(ngayhieuluc.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.EffectiveFromDate = DateTime.ParseExact(ngayhieuluc.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                            isValidate = false;
                                        }
                                    }
                                    else
                                    {
                                        isValidate = false;
                                    }
                                    if ((ngayhethan != null) && (!string.IsNullOrEmpty(ngayhethan.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.EffectiveToDate = DateTime.ParseExact(ngayhethan.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                            //isValidate = false;
                                        }
                                    }
                                    //else
                                    //{
                                    //    isValidate = false;
                                    //}
                                    if ((model.EffectiveFromDate.HasValue) && (model.EffectiveToDate.HasValue) && (model.EffectiveToDate < model.EffectiveFromDate))
                                    {
                                        isValidate = false;
                                    }
                                    if ((noidung != null) && (!string.IsNullOrEmpty(noidung.ToString().Trim())))
                                    {
                                        model.Note = noidung.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _decisionHandler.Create(model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                    }
                    curRow = curRow + 1;
                }
                // Write log
                _baseHandler.CreateLog(new SystemLogModel
                {
                    ApplicationId = userModel.ApplicationId,
                    UserId = userModel.UserId,
                    ActionByUser = LogAction.IMPORT,
                    Date = DateTime.Now,
                    Content = $"Import quyết định: Thành công: " + success + " bản ghi. Lỗi: " + fail + " bản ghi"
                });
                return "Import thành công: " + success + " bản ghi. Import lỗi: " + fail + " bản ghi";
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        /// <summary>
        /// Import sức khỏe
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportSucKhoe(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 1)
                {
                    return "Không đúng cấu trúc file";
                }
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                var lstKyKhamSucKhoe = _baseHandler.GetListData<HealthPeriodModel>("HealthPeriod", "1", 1);

                var success = 0;
                var fail = 0;
                bool isValidate;

                var curRow = 1;
                while (curRow <= sheet.LastRowNum)
                {
                    var maNV = sheet.GetRow(curRow).GetCell(1);
                    if (maNV != null)
                    {
                        isValidate = true;

                        #region Get data from excel

                        var kykham = sheet.GetRow(curRow).GetCell(2);
                        var ngaykham = sheet.GetRow(curRow).GetCell(3);
                        var phamvikham = sheet.GetRow(curRow).GetCell(4);
                        var chieucao = sheet.GetRow(curRow).GetCell(5);
                        var cannang = sheet.GetRow(curRow).GetCell(6);
                        var nhommau = sheet.GetRow(curRow).GetCell(7);
                        var xeploai = sheet.GetRow(curRow).GetCell(8);
                        var benhly = sheet.GetRow(curRow).GetCell(9);
                        var canhbaobenhtat = sheet.GetRow(curRow).GetCell(10);
                        var tuvanhuongdan = sheet.GetRow(curRow).GetCell(11);
                        var ghichu = sheet.GetRow(curRow).GetCell(12);

                        #endregion

                        var staffId = _staffHandler.GetStaffIDByStaffCode(maNV.ToString().Trim());
                        if (staffId > 0)
                        {
                            int? healthPeriodId = null;
                            if ((kykham != null) && (!string.IsNullOrEmpty(kykham.ToString().Trim())))
                            {
                                healthPeriodId = lstKyKhamSucKhoe.Where(x => x.HealthPeriodCode.Trim() == kykham.ToString().Trim()).FirstOrDefault()?.HealthPeriodID;
                            }
                            if (!healthPeriodId.HasValue)
                            {
                                isValidate = false;
                            }

                            if (isValidate)
                            {
                                var qlsk = await _qlskHandler.FindByStaffAndPeriod(staffId, healthPeriodId.Value);
                                if (qlsk != null)
                                {
                                    #region Update model - Quan ly suc khoe

                                    var model = new QuanLySucKhoeUpdateModel();
                                    model.QuanLySucKhoeID = qlsk.QuanLySucKhoeID;
                                    model.StaffID = staffId;
                                    model.HealthPeriodID = healthPeriodId;
                                    model.FileUpload = qlsk.FileUpload;
                                    if ((ngaykham != null) && (!string.IsNullOrEmpty(ngaykham.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.NgayKham = DateTime.ParseExact(ngaykham.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((phamvikham != null) && (!string.IsNullOrEmpty(phamvikham.ToString().Trim())))
                                    {
                                        model.PhamViKham = phamvikham.ToString().Trim();
                                    }
                                    if ((chieucao != null) && (!string.IsNullOrEmpty(chieucao.ToString().Trim())))
                                    {
                                        model.ChieuCao = Convert.ToDouble(chieucao.ToString().Trim());
                                    }
                                    if ((cannang != null) && (!string.IsNullOrEmpty(cannang.ToString().Trim())))
                                    {
                                        model.CanNang = Convert.ToDouble(cannang.ToString().Trim());
                                    }
                                    if ((nhommau != null) && (!string.IsNullOrEmpty(nhommau.ToString().Trim())))
                                    {
                                        switch (nhommau.ToString().Trim())
                                        {
                                            case Constant.NhomMau.A:
                                                model.NhomMau = Constant.NhomMau.A;
                                                break;
                                            case Constant.NhomMau.B:
                                                model.NhomMau = Constant.NhomMau.B;
                                                break;
                                            case Constant.NhomMau.O:
                                                model.NhomMau = Constant.NhomMau.O;
                                                break;
                                            case Constant.NhomMau.AB:
                                                model.NhomMau = Constant.NhomMau.AB;
                                                break;
                                        }
                                    }
                                    if ((xeploai != null) && (!string.IsNullOrEmpty(xeploai.ToString().Trim())))
                                    {
                                        switch (xeploai.ToString().Trim())
                                        {
                                            case Constant.XepLoaiSucKhoe.LOAI_1:
                                                model.XepLoaiSucKhoe = Constant.XepLoaiSucKhoe.NUM_LOAI_1;
                                                break;
                                            case Constant.XepLoaiSucKhoe.LOAI_2:
                                                model.XepLoaiSucKhoe = Constant.XepLoaiSucKhoe.NUM_LOAI_2;
                                                break;
                                            case Constant.XepLoaiSucKhoe.LOAI_3:
                                                model.XepLoaiSucKhoe = Constant.XepLoaiSucKhoe.NUM_LOAI_3;
                                                break;
                                            case Constant.XepLoaiSucKhoe.LOAI_4:
                                                model.XepLoaiSucKhoe = Constant.XepLoaiSucKhoe.NUM_LOAI_4;
                                                break;
                                            case Constant.XepLoaiSucKhoe.LOAI_5:
                                                model.XepLoaiSucKhoe = Constant.XepLoaiSucKhoe.NUM_LOAI_5;
                                                break;
                                        }
                                    }
                                    if ((benhly != null) && (!string.IsNullOrEmpty(benhly.ToString().Trim())))
                                    {
                                        model.BenhLy = benhly.ToString().Trim();
                                    }
                                    if ((canhbaobenhtat != null) && (!string.IsNullOrEmpty(canhbaobenhtat.ToString().Trim())))
                                    {
                                        model.CanhBaoBenhTat = canhbaobenhtat.ToString().Trim();
                                    }
                                    if ((tuvanhuongdan != null) && (!string.IsNullOrEmpty(tuvanhuongdan.ToString().Trim())))
                                    {
                                        model.TuVanHuongDan = tuvanhuongdan.ToString().Trim();
                                    }
                                    if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                    {
                                        model.GhiChu = ghichu.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _qlskHandler.Update(model.QuanLySucKhoeID, model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                                else
                                {
                                    #region Create model - Quan ly suc khoe

                                    var model = new QuanLySucKhoeCreateModel();

                                    model.StaffID = staffId;
                                    model.HealthPeriodID = healthPeriodId;
                                    if ((ngaykham != null) && (!string.IsNullOrEmpty(ngaykham.ToString().Trim())))
                                    {
                                        try
                                        {
                                            model.NgayKham = DateTime.ParseExact(ngaykham.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if ((phamvikham != null) && (!string.IsNullOrEmpty(phamvikham.ToString().Trim())))
                                    {
                                        model.PhamViKham = phamvikham.ToString().Trim();
                                    }
                                    if ((chieucao != null) && (!string.IsNullOrEmpty(chieucao.ToString().Trim())))
                                    {
                                        model.ChieuCao = Convert.ToDouble(chieucao.ToString().Trim());
                                    }
                                    if ((cannang != null) && (!string.IsNullOrEmpty(cannang.ToString().Trim())))
                                    {
                                        model.CanNang = Convert.ToDouble(cannang.ToString().Trim());
                                    }
                                    if ((nhommau != null) && (!string.IsNullOrEmpty(nhommau.ToString().Trim())))
                                    {
                                        switch (nhommau.ToString().Trim())
                                        {
                                            case Constant.NhomMau.A:
                                                model.NhomMau = Constant.NhomMau.A;
                                                break;
                                            case Constant.NhomMau.B:
                                                model.NhomMau = Constant.NhomMau.B;
                                                break;
                                            case Constant.NhomMau.O:
                                                model.NhomMau = Constant.NhomMau.O;
                                                break;
                                            case Constant.NhomMau.AB:
                                                model.NhomMau = Constant.NhomMau.AB;
                                                break;
                                        }
                                    }
                                    if ((xeploai != null) && (!string.IsNullOrEmpty(xeploai.ToString().Trim())))
                                    {
                                        switch (xeploai.ToString().Trim())
                                        {
                                            case Constant.XepLoaiSucKhoe.LOAI_1:
                                                model.XepLoaiSucKhoe = Constant.XepLoaiSucKhoe.NUM_LOAI_1;
                                                break;
                                            case Constant.XepLoaiSucKhoe.LOAI_2:
                                                model.XepLoaiSucKhoe = Constant.XepLoaiSucKhoe.NUM_LOAI_2;
                                                break;
                                            case Constant.XepLoaiSucKhoe.LOAI_3:
                                                model.XepLoaiSucKhoe = Constant.XepLoaiSucKhoe.NUM_LOAI_3;
                                                break;
                                            case Constant.XepLoaiSucKhoe.LOAI_4:
                                                model.XepLoaiSucKhoe = Constant.XepLoaiSucKhoe.NUM_LOAI_4;
                                                break;
                                            case Constant.XepLoaiSucKhoe.LOAI_5:
                                                model.XepLoaiSucKhoe = Constant.XepLoaiSucKhoe.NUM_LOAI_5;
                                                break;
                                        }
                                    }
                                    if ((benhly != null) && (!string.IsNullOrEmpty(benhly.ToString().Trim())))
                                    {
                                        model.BenhLy = benhly.ToString().Trim();
                                    }
                                    if ((canhbaobenhtat != null) && (!string.IsNullOrEmpty(canhbaobenhtat.ToString().Trim())))
                                    {
                                        model.CanhBaoBenhTat = canhbaobenhtat.ToString().Trim();
                                    }
                                    if ((tuvanhuongdan != null) && (!string.IsNullOrEmpty(tuvanhuongdan.ToString().Trim())))
                                    {
                                        model.TuVanHuongDan = tuvanhuongdan.ToString().Trim();
                                    }
                                    if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                    {
                                        model.GhiChu = ghichu.ToString().Trim();
                                    }

                                    #endregion

                                    if (isValidate)
                                    {
                                        var result = await _qlskHandler.Create(model);
                                        if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                        {
                                            success = success + 1;
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                    }
                    curRow = curRow + 1;
                }
                // Write log
                _baseHandler.CreateLog(new SystemLogModel
                {
                    ApplicationId = userModel.ApplicationId,
                    UserId = userModel.UserId,
                    ActionByUser = LogAction.IMPORT,
                    Date = DateTime.Now,
                    Content = $"Import sức khỏe: Thành công: " + success + " bản ghi. Lỗi: " + fail + " bản ghi"
                });
                return "Import thành công: " + success + " bản ghi. Import lỗi: " + fail + " bản ghi";
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        /// <summary>
        /// Import thông tin chung
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportThongTinChung(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 2)
                {
                    return "Không đúng cấu trúc file";
                }
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                var lstDanhXung = _baseHandler.GetListData<TitleModel>("Title", "1", 1);
                var lstTrinhDoChuyenMon = _baseHandler.GetListData<TrinhDoChuyenMonModel>("TrinhDoChuyenMon", "1", 1);
                var lstHocVi = _baseHandler.GetListData<DegreeModel>("Degree", "1", 1);
                var lstHocHam = _baseHandler.GetListData<AcademicRankModel>("AcademicRank", "1", 1);
                var lstBenhVien = _baseHandler.GetListData<HospitalModel>("Hospital", "1", 1);
                var lstTonGiao = _baseHandler.GetListData<ReligionModel>("Religion", "1", 1);

                var lstOrganization = _baseHandler.GetSystemListData<OrganizationModel>("Organizations", "1", 1);
                var lstEthnic = _baseHandler.GetSystemListData<EthnicModel>("Ethnics", "1", 1);
                var lstCountry = _baseHandler.GetSystemListData<CountryModel>("Countries", "1", 1);
                var lstLocation = _baseHandler.GetSystemListData<LocationModel>("Locations", "1", 1);
                var lstDistrict = _baseHandler.GetSystemListData<DistrictModel>("Districts", "1", 1);
                var lstWard = _baseHandler.GetSystemListData<WardModel>("Wards", "1", 1);

                var success = 0;
                var fail = 0;

                var curRow = 2;
                while (curRow <= sheet.LastRowNum)
                {
                    var maNV = sheet.GetRow(curRow).GetCell(1);
                    if ((maNV != null) && (!string.IsNullOrEmpty(maNV.ToString().Trim())))
                    {
                        #region Get data from excel

                        var ho = sheet.GetRow(curRow).GetCell(2);
                        var dem = sheet.GetRow(curRow).GetCell(3);
                        var ten = sheet.GetRow(curRow).GetCell(4);
                        var ngayvaodonvi = sheet.GetRow(curRow).GetCell(5);
                        var donvi = sheet.GetRow(curRow).GetCell(6);
                        var lydonghiviec = sheet.GetRow(curRow).GetCell(7);
                        var ngaynghiviec = sheet.GetRow(curRow).GetCell(8);
                        var tengoikhac = sheet.GetRow(curRow).GetCell(9);
                        var ngaysinh = sheet.GetRow(curRow).GetCell(10);
                        var gioitinh = sheet.GetRow(curRow).GetCell(11);
                        var danhxung = sheet.GetRow(curRow).GetCell(12);
                        var tinhtranghonnhan = sheet.GetRow(curRow).GetCell(13);
                        var quoctich = sheet.GetRow(curRow).GetCell(14);
                        var dantoc = sheet.GetRow(curRow).GetCell(15);
                        var tongiao = sheet.GetRow(curRow).GetCell(16);
                        var sodienthoai = sheet.GetRow(curRow).GetCell(17);
                        var email = sheet.GetRow(curRow).GetCell(18);
                        var socmnd = sheet.GetRow(curRow).GetCell(19);
                        var noicapcmnd = sheet.GetRow(curRow).GetCell(20);
                        var ngaycapcmnd = sheet.GetRow(curRow).GetCell(21);
                        var ngayhethancmnd = sheet.GetRow(curRow).GetCell(22);
                        var socchn = sheet.GetRow(curRow).GetCell(23);
                        var phamvihanhnghe = sheet.GetRow(curRow).GetCell(24);
                        var ngaycapcchn = sheet.GetRow(curRow).GetCell(25);
                        var noicapcchn = sheet.GetRow(curRow).GetCell(26);
                        var trinhdovanhoa = sheet.GetRow(curRow).GetCell(27);
                        var trinhdochuyenmon = sheet.GetRow(curRow).GetCell(28);
                        var hocvi = sheet.GetRow(curRow).GetCell(29);
                        var hocham = sheet.GetRow(curRow).GetCell(30);
                        var doituonglaodong = sheet.GetRow(curRow).GetCell(31);
                        var hinhthucluong = sheet.GetRow(curRow).GetCell(32);
                        var xuatthan = sheet.GetRow(curRow).GetCell(33);
                        var nguoigioithieu = sheet.GetRow(curRow).GetCell(34);
                        var lienhekhancap = sheet.GetRow(curRow).GetCell(35);
                        var quequantinh = sheet.GetRow(curRow).GetCell(36);
                        var quequanquanhuyen = sheet.GetRow(curRow).GetCell(37);
                        var quequanphuongxa = sheet.GetRow(curRow).GetCell(38);
                        var quequandiachi = sheet.GetRow(curRow).GetCell(39);
                        var hktttinh = sheet.GetRow(curRow).GetCell(40);
                        var hkttquanhuyen = sheet.GetRow(curRow).GetCell(41);
                        var hkttphuongxa = sheet.GetRow(curRow).GetCell(42);
                        var hkttdiachi = sheet.GetRow(curRow).GetCell(43);
                        var noiotinh = sheet.GetRow(curRow).GetCell(44);
                        var noioquanhuyen = sheet.GetRow(curRow).GetCell(45);
                        var noiophuongxa = sheet.GetRow(curRow).GetCell(46);
                        var noiodiachi = sheet.GetRow(curRow).GetCell(47);
                        var noisinhtinh = sheet.GetRow(curRow).GetCell(48);
                        var noisinhquanhuyen = sheet.GetRow(curRow).GetCell(49);
                        var noisinhphuongxa = sheet.GetRow(curRow).GetCell(50);
                        var noisinhdiachi = sheet.GetRow(curRow).GetCell(51);
                        var masothuecanhan = sheet.GetRow(curRow).GetCell(52);
                        var noicapmasothue = sheet.GetRow(curRow).GetCell(53);
                        var mahogiadinh = sheet.GetRow(curRow).GetCell(54);
                        var masobhxh = sheet.GetRow(curRow).GetCell(55);
                        var noicapbhxh = sheet.GetRow(curRow).GetCell(56);
                        var ngaycapbhxh = sheet.GetRow(curRow).GetCell(57);
                        var noikcbbandau = sheet.GetRow(curRow).GetCell(58);
                        var nghenghieptruoctuyendung = sheet.GetRow(curRow).GetCell(59);
                        var congvieclamlaunhat = sheet.GetRow(curRow).GetCell(60);
                        var sogiaypheplaixe = sheet.GetRow(curRow).GetCell(61);
                        var noicapgiaypheplaixe = sheet.GetRow(curRow).GetCell(62);
                        var ngaycapgiaypheplaixe = sheet.GetRow(curRow).GetCell(63);
                        var danhhieuduocphong = sheet.GetRow(curRow).GetCell(64);
                        var sotruong = sheet.GetRow(curRow).GetCell(65);
                        var dacdiemlichsubanthan = sheet.GetRow(curRow).GetCell(66);
                        var ngaythamgiactxh = sheet.GetRow(curRow).GetCell(67);
                        var ngayvaodoan = sheet.GetRow(curRow).GetCell(68);
                        var hangthuongbinh = sheet.GetRow(curRow).GetCell(69);
                        var giadinhchinhsach = sheet.GetRow(curRow).GetCell(70);
                        var vietinbank = sheet.GetRow(curRow).GetCell(71);
                        var vietcombank = sheet.GetRow(curRow).GetCell(72);
                        var ghichu = sheet.GetRow(curRow).GetCell(73);

                        #endregion

                        var staffId = _staffHandler.GetStaffIDByStaffCode(maNV.ToString().Trim());
                        if (staffId > 0)
                        {
                            #region Update model - Thong tin chung & thong tin khac

                            //Cap nhat
                            var model = new StaffUpdateRequestModel();
                            model.StaffID = staffId;
                            if ((maNV != null) && (!string.IsNullOrEmpty(maNV.ToString().Trim())))
                            {
                                model.StaffCode = maNV.ToString().Trim();
                            }
                            if ((ho != null) && (!string.IsNullOrEmpty(ho.ToString().Trim())))
                            {
                                model.FirstName = ho.ToString().Trim();
                            }
                            if ((dem != null) && (!string.IsNullOrEmpty(dem.ToString().Trim())))
                            {
                                model.MidName = dem.ToString().Trim();
                            }
                            if ((ten != null) && (!string.IsNullOrEmpty(ten.ToString().Trim())))
                            {
                                model.LastName = ten.ToString().Trim();
                            }
                            if ((ngayvaodonvi != null) && (!string.IsNullOrEmpty(ngayvaodonvi.ToString().Trim())))
                            {
                                try
                                {
                                    model.JoiningDate = DateTime.ParseExact(ngayvaodonvi.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((donvi != null) && (!string.IsNullOrEmpty(donvi.ToString().Trim())))
                            {
                                model.DeptID = lstOrganization.Where(x => x.OrganizationName.Trim() == donvi.ToString().Trim()).FirstOrDefault()?.OrganizationId;
                            }
                            if ((lydonghiviec != null) && (!string.IsNullOrEmpty(lydonghiviec.ToString().Trim())))
                            {
                                switch (lydonghiviec.ToString().Trim())
                                {
                                    case Constant.LyDoNghiViec.NGHI_HUU:
                                        model.ExtraNumber1 = Constant.LyDoNghiViec.NUM_NGHI_HUU;
                                        break;
                                    case Constant.LyDoNghiViec.TU_VONG:
                                        model.ExtraNumber1 = Constant.LyDoNghiViec.NUM_TU_VONG;
                                        break;
                                    case Constant.LyDoNghiViec.THOI_VIEC:
                                        model.ExtraNumber1 = Constant.LyDoNghiViec.NUM_THOI_VIEC;
                                        break;
                                    default:
                                        model.ExtraNumber1 = null;
                                        break;
                                }
                            }
                            if ((ngaynghiviec != null) && (!string.IsNullOrEmpty(ngaynghiviec.ToString().Trim())))
                            {
                                try
                                {
                                    model.LeavingDate = DateTime.ParseExact(ngaynghiviec.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((tengoikhac != null) && (!string.IsNullOrEmpty(tengoikhac.ToString().Trim())))
                            {
                                model.TenGoiKhac = tengoikhac.ToString().Trim();
                            }
                            if ((ngaysinh != null) && (!string.IsNullOrEmpty(ngaysinh.ToString().Trim())))
                            {
                                try
                                {
                                    model.Birthday = DateTime.ParseExact(ngaysinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((gioitinh != null) && (!string.IsNullOrEmpty(gioitinh.ToString().Trim())))
                            {
                                switch (gioitinh.ToString().Trim())
                                {
                                    case Constant.GioiTinh.NU:
                                        model.Gender = Constant.GioiTinh.NUM_NU;
                                        break;
                                    case Constant.GioiTinh.NAM:
                                        model.Gender = Constant.GioiTinh.NUM_NAM;
                                        break;
                                    case Constant.GioiTinh.KHAC:
                                        model.Gender = Constant.GioiTinh.NUM_KHAC;
                                        break;
                                    default:
                                        model.Gender = null;
                                        break;
                                }
                            }
                            if ((danhxung != null) && (!string.IsNullOrEmpty(danhxung.ToString().Trim())))
                            {
                                model.TitleID = lstDanhXung.Where(x => x.TitleName.Trim() == danhxung.ToString().Trim()).FirstOrDefault()?.TitleID;
                            }
                            if ((tinhtranghonnhan != null) && (!string.IsNullOrEmpty(ho.ToString().Trim())))
                            {
                                switch (tinhtranghonnhan.ToString().Trim())
                                {

                                    case Constant.TinhTrangHonNhan.CHUA_XAC_DINH:
                                        model.MaritalStatus = Constant.TinhTrangHonNhan.NUM_CHUA_XAC_DINH;
                                        break;
                                    case Constant.TinhTrangHonNhan.DOC_THAN:
                                        model.MaritalStatus = Constant.TinhTrangHonNhan.NUM_DOC_THAN;
                                        break;
                                    case Constant.TinhTrangHonNhan.DA_KET_HON:
                                        model.MaritalStatus = Constant.TinhTrangHonNhan.NUM_DA_KET_HON;
                                        break;
                                    case Constant.TinhTrangHonNhan.VO_CHONG_CUNG_CO_QUAN:
                                        model.MaritalStatus = Constant.TinhTrangHonNhan.NUM_VO_CHONG_CUNG_CO_QUAN;
                                        break;
                                    default:
                                        model.MaritalStatus = null;
                                        break;
                                }
                            }
                            if ((quoctich != null) && (!string.IsNullOrEmpty(quoctich.ToString().Trim())))
                            {
                                model.TerritoryID = lstCountry.Where(x => x.CountryName.Trim() == quoctich.ToString().Trim()).FirstOrDefault()?.CountryId;
                            }
                            if ((dantoc != null) && (!string.IsNullOrEmpty(dantoc.ToString().Trim())))
                            {
                                model.EthnicID = lstEthnic.Where(x => x.EthnicName.Trim() == dantoc.ToString().Trim()).FirstOrDefault()?.EthnicId;
                            }
                            if ((tongiao != null) && (!string.IsNullOrEmpty(tongiao.ToString().Trim())))
                            {
                                model.ReligionID = lstTonGiao.Where(x => x.ReligionName.Trim() == tongiao.ToString().Trim()).FirstOrDefault()?.ReligionID;
                            }
                            if ((sodienthoai != null) && (!string.IsNullOrEmpty(sodienthoai.ToString().Trim())))
                            {
                                model.Mobiphone = sodienthoai.ToString().Trim();
                            }
                            if ((email != null) && (!string.IsNullOrEmpty(email.ToString().Trim())))
                            {
                                model.PersonalEmail = email.ToString().Trim();
                            }
                            if ((trinhdovanhoa != null) && (!string.IsNullOrEmpty(trinhdovanhoa.ToString().Trim())))
                            {
                                switch (trinhdovanhoa.ToString().Trim())
                                {
                                    case Constant.TrinhDoVanHoa.TRINH_DO_9_9:
                                        model.ExtraNumber3 = Constant.TrinhDoVanHoa.NUM_TRINH_DO_9_9;
                                        break;
                                    case Constant.TrinhDoVanHoa.TRINH_DO_10_10:
                                        model.ExtraNumber3 = Constant.TrinhDoVanHoa.NUM_TRINH_DO_10_10;
                                        break;
                                    case Constant.TrinhDoVanHoa.TRINH_DO_12_12:
                                        model.ExtraNumber3 = Constant.TrinhDoVanHoa.NUM_TRINH_DO_12_12;
                                        break;
                                    default:
                                        model.ExtraNumber3 = null;
                                        break;
                                }
                            }
                            if ((trinhdochuyenmon != null) && (!string.IsNullOrEmpty(trinhdochuyenmon.ToString().Trim())))
                            {
                                model.QualificationID = lstTrinhDoChuyenMon.Where(x => x.TrinhDoChuyenMonName.Trim() == trinhdochuyenmon.ToString().Trim()).FirstOrDefault()?.TrinhDoChuyenMonID;
                            }
                            if ((hocvi != null) && (!string.IsNullOrEmpty(hocvi.ToString().Trim())))
                            {
                                model.DegreeID = lstHocVi.Where(x => x.DegreeName.Trim() == hocvi.ToString().Trim()).FirstOrDefault()?.DegreeID;
                            }
                            if ((hocham != null) && (!string.IsNullOrEmpty(hocham.ToString().Trim())))
                            {
                                model.AcademicRankID = lstHocHam.Where(x => x.AcademicRankName.Trim() == hocham.ToString().Trim()).FirstOrDefault()?.AcademicRankID;
                            }
                            if ((doituonglaodong != null) && (!string.IsNullOrEmpty(doituonglaodong.ToString().Trim())))
                            {
                                switch (doituonglaodong.ToString().Trim())
                                {
                                    case Constant.DoiTuongLaoDong.VIEN_CHUC:
                                        model.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_VIEN_CHUC;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_68:
                                        model.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_68;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_KHONG_XAC_DINH_THOI_HAN:
                                        model.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_KHONG_XAC_DINH_THOI_HAN;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_NGAN_HAN:
                                        model.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_NGAN_HAN;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_THU_VIEC:
                                        model.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_THU_VIEC;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_CHUYEN_GIA:
                                        model.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_CHUYEN_GIA;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_THOI_VU:
                                        model.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_THOI_VU;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_KHAC:
                                        model.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_KHAC;
                                        break;
                                    default:
                                        model.ExtraNumber2 = null;
                                        break;
                                }
                            }
                            if ((hinhthucluong != null) && (!string.IsNullOrEmpty(hinhthucluong.ToString().Trim())))
                            {
                                switch (hinhthucluong.ToString().Trim())
                                {
                                    case Constant.HinhThucLuong.LUONG_THEO_THOI_GIAN:
                                        model.SalaryStatus = Constant.HinhThucLuong.NUM_LUONG_THEO_THOI_GIAN;
                                        break;
                                    case Constant.HinhThucLuong.LUONG_THEO_SAN_PHAM:
                                        model.SalaryStatus = Constant.HinhThucLuong.NUM_LUONG_THEO_THOI_GIAN;
                                        break;
                                    case Constant.HinhThucLuong.LUONG_KHOAN:
                                        model.SalaryStatus = Constant.HinhThucLuong.NUM_LUONG_THEO_THOI_GIAN;
                                        break;
                                    default:
                                        model.SalaryStatus = null;
                                        break;
                                }
                            }
                            if ((xuatthan != null) && (!string.IsNullOrEmpty(xuatthan.ToString().Trim())))
                            {
                                model.XuatThan = xuatthan.ToString().Trim();
                            }
                            if ((nguoigioithieu != null) && (!string.IsNullOrEmpty(nguoigioithieu.ToString().Trim())))
                            {
                                model.IntroducedBy = nguoigioithieu.ToString().Trim();
                            }
                            if ((lienhekhancap != null) && (!string.IsNullOrEmpty(lienhekhancap.ToString().Trim())))
                            {
                                model.Emergency = lienhekhancap.ToString().Trim();
                            }

                            //Thong tin khac
                            if ((masothuecanhan != null) && (!string.IsNullOrEmpty(masothuecanhan.ToString().Trim())))
                            {
                                model.PITCode = masothuecanhan.ToString().Trim();
                            }
                            if ((noicapmasothue != null) && (!string.IsNullOrEmpty(noicapmasothue.ToString().Trim())))
                            {
                                model.ExtraNumber5 = lstLocation.Where(x => x.LocationName.Trim() == noicapmasothue.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((mahogiadinh != null) && (!string.IsNullOrEmpty(mahogiadinh.ToString().Trim())))
                            {
                                model.LabourBook = mahogiadinh.ToString().Trim();
                            }
                            if ((masobhxh != null) && (!string.IsNullOrEmpty(masobhxh.ToString().Trim())))
                            {
                                model.InsuranceNo = masobhxh.ToString().Trim();
                            }
                            if ((noicapbhxh != null) && (!string.IsNullOrEmpty(noicapbhxh.ToString().Trim())))
                            {
                                model.InsuranceIssuePlaceID = lstLocation.Where(x => x.LocationName.Trim() == noicapbhxh.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((ngaycapbhxh != null) && (!string.IsNullOrEmpty(ngaycapbhxh.ToString().Trim())))
                            {
                                try
                                {
                                    model.InsuranceIssueDate = DateTime.ParseExact(ngaycapbhxh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((noikcbbandau != null) && (!string.IsNullOrEmpty(noikcbbandau.ToString().Trim())))
                            {
                                model.HospitalID = lstBenhVien.Where(x => x.HospitalName.Trim() == noikcbbandau.ToString().Trim()).FirstOrDefault()?.HospitalID;
                            }
                            if ((nghenghieptruoctuyendung != null) && (!string.IsNullOrEmpty(nghenghieptruoctuyendung.ToString().Trim())))
                            {
                                model.NgheNghiepTruocTuyenDung = nghenghieptruoctuyendung.ToString().Trim();
                            }
                            if ((congvieclamlaunhat != null) && (!string.IsNullOrEmpty(congvieclamlaunhat.ToString().Trim())))
                            {
                                model.CongViecLamLauNhat = congvieclamlaunhat.ToString().Trim();
                            }
                            if ((sogiaypheplaixe != null) && (!string.IsNullOrEmpty(sogiaypheplaixe.ToString().Trim())))
                            {
                                model.DriverLicenseNo = sogiaypheplaixe.ToString().Trim();
                            }
                            if ((noicapgiaypheplaixe != null) && (!string.IsNullOrEmpty(noicapgiaypheplaixe.ToString().Trim())))
                            {
                                model.DriverLicenseIssuePlaceID = lstLocation.Where(x => x.LocationName.Trim() == noicapgiaypheplaixe.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((ngaycapgiaypheplaixe != null) && (!string.IsNullOrEmpty(ngaycapgiaypheplaixe.ToString().Trim())))
                            {
                                try
                                {
                                    model.DriverLicenseIssueDate = DateTime.ParseExact(ngaycapgiaypheplaixe.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((danhhieuduocphong != null) && (!string.IsNullOrEmpty(danhhieuduocphong.ToString().Trim())))
                            {
                                model.DanhHieuDuocPhong = danhhieuduocphong.ToString().Trim();
                            }
                            if ((sotruong != null) && (!string.IsNullOrEmpty(sotruong.ToString().Trim())))
                            {
                                model.Hobby = sotruong.ToString().Trim();
                            }
                            if ((dacdiemlichsubanthan != null) && (!string.IsNullOrEmpty(dacdiemlichsubanthan.ToString().Trim())))
                            {
                                model.DacDiemLichSuBanThan1 = dacdiemlichsubanthan.ToString().Trim();
                            }
                            if ((ngaythamgiactxh != null) && (!string.IsNullOrEmpty(ngaythamgiactxh.ToString().Trim())))
                            {
                                try
                                {
                                    model.NgayThamGiaToChucChinhTriXaHoi = DateTime.ParseExact(ngaythamgiactxh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((ngayvaodoan != null) && (!string.IsNullOrEmpty(ngayvaodoan.ToString().Trim())))
                            {
                                try
                                {
                                    model.NgayVaoDoan = DateTime.ParseExact(ngayvaodoan.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((hangthuongbinh != null) && (!string.IsNullOrEmpty(hangthuongbinh.ToString().Trim())))
                            {
                                switch (hangthuongbinh.ToString().Trim())
                                {
                                    case Constant.HangThuongBinh.HANG_1_4:
                                        model.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_1_4;
                                        break;
                                    case Constant.HangThuongBinh.HANG_2_4:
                                        model.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_2_4;
                                        break;
                                    case Constant.HangThuongBinh.HANG_3_4:
                                        model.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_3_4;
                                        break;
                                    case Constant.HangThuongBinh.HANG_4_4:
                                        model.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_4_4;
                                        break;
                                    default:
                                        model.HangThuongBinh = null;
                                        break;
                                }
                            }
                            if ((giadinhchinhsach != null) && (!string.IsNullOrEmpty(giadinhchinhsach.ToString().Trim())))
                            {
                                switch (giadinhchinhsach.ToString().Trim())
                                {
                                    case Constant.GiaDinhChinhSach.CON_THUONG_BINH:
                                        model.GiaDinhChinhSach = Constant.GiaDinhChinhSach.NUM_CON_THUONG_BINH;
                                        break;
                                    case Constant.GiaDinhChinhSach.CON_LIET_SI:
                                        model.GiaDinhChinhSach = Constant.GiaDinhChinhSach.NUM_CON_LIET_SI;
                                        break;
                                    case Constant.GiaDinhChinhSach.NGUOI_NHIEM_CHAT_DOC_DA_CAM:
                                        model.GiaDinhChinhSach = Constant.GiaDinhChinhSach.NUM_NGUOI_NHIEM_CHAT_DOC_DA_CAM;
                                        break;
                                    default:
                                        model.GiaDinhChinhSach = null;
                                        break;
                                }
                            }
                            if ((vietinbank != null) && (!string.IsNullOrEmpty(vietinbank.ToString().Trim())))
                            {
                                model.Vietinbank = vietinbank.ToString().Trim();
                            }
                            if ((vietcombank != null) && (!string.IsNullOrEmpty(vietcombank.ToString().Trim())))
                            {
                                model.Vietcombank = vietcombank.ToString().Trim();
                            }
                            if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                            {
                                model.GhiChu = ghichu.ToString().Trim();
                            }

                            //Tinh thanh - Quan huyen - Phuong xa
                            if ((quequantinh != null) && (!string.IsNullOrEmpty(quequantinh.ToString().Trim())))
                            {
                                model.QueQuanLocationID = lstLocation.Where(x => x.LocationName.Trim() == quequantinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((quequanquanhuyen != null) && (!string.IsNullOrEmpty(quequanquanhuyen.ToString().Trim())))
                            {
                                if (model.QueQuanLocationID != null)
                                {
                                    model.QueQuanDistrictID = lstDistrict.Where(x => (x.DistrictName.Trim() == quequanquanhuyen.ToString().Trim()) && (x.LocationId == model.QueQuanLocationID)).FirstOrDefault()?.DistrictId;
                                }
                            }
                            if ((quequanphuongxa != null) && (!string.IsNullOrEmpty(quequanphuongxa.ToString().Trim())))
                            {
                                if (model.QueQuanDistrictID != null)
                                {
                                    model.QueQuanWardID = lstWard.Where(x => (x.WardName.Trim() == quequanphuongxa.ToString().Trim()) && (x.DistrictId == model.QueQuanDistrictID)).FirstOrDefault()?.WardId;
                                }
                            }
                            if ((quequandiachi != null) && (!string.IsNullOrEmpty(quequandiachi.ToString().Trim())))
                            {
                                model.QueQuanAddr = quequandiachi.ToString().Trim();
                            }
                            if ((hktttinh != null) && (!string.IsNullOrEmpty(hktttinh.ToString().Trim())))
                            {
                                model.PermanentLocationID = lstLocation.Where(x => x.LocationName.Trim() == hktttinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((hkttquanhuyen != null) && (!string.IsNullOrEmpty(hkttquanhuyen.ToString().Trim())))
                            {
                                if (model.PermanentLocationID != null)
                                {
                                    model.PermanentDistrictID = lstDistrict.Where(x => (x.DistrictName.Trim() == quequanquanhuyen.ToString().Trim()) && (x.LocationId == model.QueQuanLocationID)).FirstOrDefault()?.DistrictId;
                                }
                            }
                            if ((hkttphuongxa != null) && (!string.IsNullOrEmpty(hkttphuongxa.ToString().Trim())))
                            {
                                if (model.PermanentDistrictID != null)
                                {
                                    model.PermanentWardID = lstWard.Where(x => (x.WardName.Trim() == quequanphuongxa.ToString().Trim()) && (x.DistrictId == model.QueQuanDistrictID)).FirstOrDefault()?.WardId;
                                }
                            }
                            if ((hkttdiachi != null) && (!string.IsNullOrEmpty(hkttdiachi.ToString().Trim())))
                            {
                                model.PermanentAddr = hkttdiachi.ToString().Trim();
                            }
                            if ((noiotinh != null) && (!string.IsNullOrEmpty(noiotinh.ToString().Trim())))
                            {
                                model.ContactLocationID = lstLocation.Where(x => x.LocationName.Trim() == noiotinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((noioquanhuyen != null) && (!string.IsNullOrEmpty(noioquanhuyen.ToString().Trim())))
                            {
                                if (model.ContactLocationID != null)
                                {
                                    model.ContactAddrDistrictID = lstDistrict.Where(x => (x.DistrictName.Trim() == noioquanhuyen.ToString().Trim()) && (x.LocationId == model.ContactLocationID)).FirstOrDefault()?.DistrictId;
                                }
                            }
                            if ((noiophuongxa != null) && (!string.IsNullOrEmpty(noiophuongxa.ToString().Trim())))
                            {
                                if (model.ContactAddrDistrictID != null)
                                {
                                    model.ContactAddrWardID = lstWard.Where(x => (x.WardName.Trim() == noiophuongxa.ToString().Trim()) && (x.DistrictId == model.ContactAddrDistrictID)).FirstOrDefault()?.WardId;
                                }
                            }
                            if ((noiodiachi != null) && (!string.IsNullOrEmpty(noiodiachi.ToString().Trim())))
                            {
                                model.ContactAddr = noiodiachi.ToString().Trim();
                            }
                            if ((noisinhtinh != null) && (!string.IsNullOrEmpty(noisinhtinh.ToString().Trim())))
                            {
                                model.BirthPlaceID = lstLocation.Where(x => x.LocationName.Trim() == noisinhtinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((noisinhquanhuyen != null) && (!string.IsNullOrEmpty(noisinhquanhuyen.ToString().Trim())))
                            {
                                if (model.BirthPlaceID != null)
                                {
                                    model.BirthPlaceDistrictID = lstDistrict.Where(x => (x.DistrictName.Trim() == noisinhquanhuyen.ToString().Trim()) && (x.LocationId == model.BirthPlaceID)).FirstOrDefault()?.DistrictId;
                                }
                            }
                            if ((noisinhphuongxa != null) && (!string.IsNullOrEmpty(noisinhphuongxa.ToString().Trim())))
                            {
                                if (model.BirthPlaceDistrictID != null)
                                {
                                    model.BirthPlaceWardID = lstWard.Where(x => (x.WardName.Trim() == noisinhphuongxa.ToString().Trim()) && (x.DistrictId == model.BirthPlaceDistrictID)).FirstOrDefault()?.WardId;
                                }
                            }
                            if ((noisinhdiachi != null) && (!string.IsNullOrEmpty(noisinhdiachi.ToString().Trim())))
                            {
                                model.BirthPlaceAddr = noisinhdiachi.ToString().Trim();
                            }

                            //Giay to dinh kem
                            List<StaffLicenseModel> lstStaffLicense = new List<StaffLicenseModel>();
                            var cmnd = new StaffLicenseModel();
                            var cchn = new StaffLicenseModel();
                            //Chung minh nhan dan
                            cmnd.Type = Constant.Licenses.CMND;
                            cchn.Type = Constant.Licenses.CCHN;
                            if ((socmnd != null) && (!string.IsNullOrEmpty(socmnd.ToString().Trim())))
                            {
                                cmnd.LicenseNumber = socmnd.ToString().Trim();
                            }
                            if ((noicapcmnd != null) && (!string.IsNullOrEmpty(noicapcmnd.ToString().Trim())))
                            {
                                var location = lstLocation.Where(x => x.LocationName.Trim() == noicapcmnd.ToString().Trim()).FirstOrDefault();
                                if (location != null)
                                {
                                    cmnd.IssuePlaceID = location.LocationID;
                                }
                            }
                            if ((ngaycapcmnd != null) && (!string.IsNullOrEmpty(ngaycapcmnd.ToString().Trim())))
                            {
                                try
                                {
                                    cmnd.IssueDate = DateTime.ParseExact(ngaycapcmnd.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((ngayhethancmnd != null) && (!string.IsNullOrEmpty(ngayhethancmnd.ToString().Trim())))
                            {
                                try
                                {
                                    cmnd.ExpiredIssueDate = DateTime.ParseExact(ngayhethancmnd.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            //Chung chi hanh nghe
                            if ((socchn != null) && (!string.IsNullOrEmpty(socchn.ToString().Trim())))
                            {
                                cchn.LicenseNumber = socchn.ToString().Trim();
                            }
                            if ((phamvihanhnghe != null) && (!string.IsNullOrEmpty(phamvihanhnghe.ToString().Trim())))
                            {
                                cchn.PracticingScope = phamvihanhnghe.ToString().Trim();
                            }
                            if ((ngaycapcchn != null) && (!string.IsNullOrEmpty(ngaycapcchn.ToString().Trim())))
                            {
                                try
                                {
                                    cchn.IssueDate = DateTime.ParseExact(ngaycapcchn.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((noicapcchn != null) && (!string.IsNullOrEmpty(noicapcchn.ToString().Trim())))
                            {
                                var location = lstLocation.Where(x => x.LocationName.Trim() == noicapcmnd.ToString().Trim()).FirstOrDefault();
                                if (location != null)
                                {
                                    cchn.IssuePlaceID = location.LocationID;
                                }
                            }
                            if (!string.IsNullOrEmpty(cmnd.LicenseNumber))
                            {
                                lstStaffLicense.Add(cmnd);
                            }
                            if (!string.IsNullOrEmpty(cchn.LicenseNumber))
                            {
                                lstStaffLicense.Add(cchn);
                            }
                            if (lstStaffLicense.Count > 0)
                            {
                                model.StaffLicense = lstStaffLicense;
                            }

                            #endregion

                            var result = await _staffHandler.UpdateStaffGeneralInfo(model);
                            if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                            {
                                success = success + 1;
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                        else
                        {
                            #region Create model - Thong tin chung

                            ////Them moi
                            var createModel = new StaffCreateRequestModel();
                            if ((maNV != null) && (!string.IsNullOrEmpty(maNV.ToString().Trim())))
                            {
                                createModel.StaffCode = maNV.ToString().Trim();
                            }
                            if ((ho != null) && (!string.IsNullOrEmpty(ho.ToString().Trim())))
                            {
                                createModel.FirstName = ho.ToString().Trim();
                            }
                            if ((dem != null) && (!string.IsNullOrEmpty(dem.ToString().Trim())))
                            {
                                createModel.MidName = dem.ToString().Trim();
                            }
                            if ((ten != null) && (!string.IsNullOrEmpty(ten.ToString().Trim())))
                            {
                                createModel.LastName = ten.ToString().Trim();
                            }
                            if ((ngayvaodonvi != null) && (!string.IsNullOrEmpty(ngayvaodonvi.ToString().Trim())))
                            {
                                try
                                {
                                    createModel.JoiningDate = DateTime.ParseExact(ngayvaodonvi.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((donvi != null) && (!string.IsNullOrEmpty(donvi.ToString().Trim())))
                            {
                                createModel.DeptID = lstOrganization.Where(x => x.OrganizationName.Trim() == donvi.ToString().Trim()).FirstOrDefault()?.OrganizationId;
                            }
                            if ((lydonghiviec != null) && (!string.IsNullOrEmpty(lydonghiviec.ToString().Trim())))
                            {
                                switch (lydonghiviec.ToString().Trim())
                                {
                                    case Constant.LyDoNghiViec.NGHI_HUU:
                                        createModel.ExtraNumber1 = Constant.LyDoNghiViec.NUM_NGHI_HUU;
                                        break;
                                    case Constant.LyDoNghiViec.TU_VONG:
                                        createModel.ExtraNumber1 = Constant.LyDoNghiViec.NUM_TU_VONG;
                                        break;
                                    case Constant.LyDoNghiViec.THOI_VIEC:
                                        createModel.ExtraNumber1 = Constant.LyDoNghiViec.NUM_THOI_VIEC;
                                        break;
                                    default:
                                        createModel.ExtraNumber1 = null;
                                        break;
                                }
                            }
                            if ((ngaynghiviec != null) && (!string.IsNullOrEmpty(ngaynghiviec.ToString().Trim())))
                            {
                                try
                                {
                                    createModel.LeavingDate = DateTime.ParseExact(ngaynghiviec.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((tengoikhac != null) && (!string.IsNullOrEmpty(tengoikhac.ToString().Trim())))
                            {
                                createModel.TenGoiKhac = tengoikhac.ToString().Trim();
                            }
                            if ((ngaysinh != null) && (!string.IsNullOrEmpty(ngaysinh.ToString().Trim())))
                            {
                                try
                                {
                                    createModel.Birthday = DateTime.ParseExact(ngaysinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((gioitinh != null) && (!string.IsNullOrEmpty(gioitinh.ToString().Trim())))
                            {
                                switch (gioitinh.ToString().Trim())
                                {
                                    case Constant.GioiTinh.NU:
                                        createModel.Gender = Constant.GioiTinh.NUM_NU;
                                        break;
                                    case Constant.GioiTinh.NAM:
                                        createModel.Gender = Constant.GioiTinh.NUM_NAM;
                                        break;
                                    case Constant.GioiTinh.KHAC:
                                        createModel.Gender = Constant.GioiTinh.NUM_KHAC;
                                        break;
                                    default:
                                        createModel.Gender = null;
                                        break;
                                }
                            }
                            if ((danhxung != null) && (!string.IsNullOrEmpty(danhxung.ToString().Trim())))
                            {
                                createModel.TitleID = lstDanhXung.Where(x => x.TitleName.Trim() == danhxung.ToString().Trim()).FirstOrDefault()?.TitleID;
                            }
                            if ((tinhtranghonnhan != null) && (!string.IsNullOrEmpty(ho.ToString().Trim())))
                            {
                                switch (tinhtranghonnhan.ToString().Trim())
                                {

                                    case Constant.TinhTrangHonNhan.CHUA_XAC_DINH:
                                        createModel.MaritalStatus = Constant.TinhTrangHonNhan.NUM_CHUA_XAC_DINH;
                                        break;
                                    case Constant.TinhTrangHonNhan.DOC_THAN:
                                        createModel.MaritalStatus = Constant.TinhTrangHonNhan.NUM_DOC_THAN;
                                        break;
                                    case Constant.TinhTrangHonNhan.DA_KET_HON:
                                        createModel.MaritalStatus = Constant.TinhTrangHonNhan.NUM_DA_KET_HON;
                                        break;
                                    case Constant.TinhTrangHonNhan.VO_CHONG_CUNG_CO_QUAN:
                                        createModel.MaritalStatus = Constant.TinhTrangHonNhan.NUM_VO_CHONG_CUNG_CO_QUAN;
                                        break;
                                    default:
                                        createModel.MaritalStatus = null;
                                        break;
                                }
                            }
                            if ((quoctich != null) && (!string.IsNullOrEmpty(quoctich.ToString().Trim())))
                            {
                                createModel.TerritoryID = lstCountry.Where(x => x.CountryName.Trim() == quoctich.ToString().Trim()).FirstOrDefault()?.CountryId;
                            }
                            if ((dantoc != null) && (!string.IsNullOrEmpty(dantoc.ToString().Trim())))
                            {
                                createModel.EthnicID = lstEthnic.Where(x => x.EthnicName.Trim() == dantoc.ToString().Trim()).FirstOrDefault()?.EthnicId;
                            }
                            if ((tongiao != null) && (!string.IsNullOrEmpty(tongiao.ToString().Trim())))
                            {
                                createModel.ReligionID = lstTonGiao.Where(x => x.ReligionName.Trim() == tongiao.ToString().Trim()).FirstOrDefault()?.ReligionID;
                            }
                            if ((sodienthoai != null) && (!string.IsNullOrEmpty(sodienthoai.ToString().Trim())))
                            {
                                createModel.Telephone = sodienthoai.ToString().Trim();
                            }
                            if ((email != null) && (!string.IsNullOrEmpty(email.ToString().Trim())))
                            {
                                createModel.PersonalEmail = email.ToString().Trim();
                            }
                            if ((trinhdovanhoa != null) && (!string.IsNullOrEmpty(trinhdovanhoa.ToString().Trim())))
                            {
                                switch (trinhdovanhoa.ToString().Trim())
                                {
                                    case Constant.TrinhDoVanHoa.TRINH_DO_9_9:
                                        createModel.ExtraNumber3 = Constant.TrinhDoVanHoa.NUM_TRINH_DO_9_9;
                                        break;
                                    case Constant.TrinhDoVanHoa.TRINH_DO_10_10:
                                        createModel.ExtraNumber3 = Constant.TrinhDoVanHoa.NUM_TRINH_DO_10_10;
                                        break;
                                    case Constant.TrinhDoVanHoa.TRINH_DO_12_12:
                                        createModel.ExtraNumber3 = Constant.TrinhDoVanHoa.NUM_TRINH_DO_12_12;
                                        break;
                                    default:
                                        createModel.ExtraNumber3 = null;
                                        break;
                                }
                            }
                            if ((trinhdochuyenmon != null) && (!string.IsNullOrEmpty(trinhdochuyenmon.ToString().Trim())))
                            {
                                createModel.QualificationID = lstTrinhDoChuyenMon.Where(x => x.TrinhDoChuyenMonName.Trim() == trinhdochuyenmon.ToString().Trim()).FirstOrDefault()?.TrinhDoChuyenMonID;
                            }
                            if ((hocvi != null) && (!string.IsNullOrEmpty(hocvi.ToString().Trim())))
                            {
                                createModel.DegreeID = lstHocVi.Where(x => x.DegreeName.Trim() == hocvi.ToString().Trim()).FirstOrDefault()?.DegreeID;
                            }
                            if ((hocham != null) && (!string.IsNullOrEmpty(hocham.ToString().Trim())))
                            {
                                createModel.AcademicRankID = lstHocHam.Where(x => x.AcademicRankName.Trim() == hocham.ToString().Trim()).FirstOrDefault()?.AcademicRankID;
                            }
                            if ((doituonglaodong != null) && (!string.IsNullOrEmpty(doituonglaodong.ToString().Trim())))
                            {
                                switch (doituonglaodong.ToString().Trim())
                                {
                                    case Constant.DoiTuongLaoDong.VIEN_CHUC:
                                        createModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_VIEN_CHUC;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_68:
                                        createModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_68;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_KHONG_XAC_DINH_THOI_HAN:
                                        createModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_KHONG_XAC_DINH_THOI_HAN;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_NGAN_HAN:
                                        createModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_NGAN_HAN;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_THU_VIEC:
                                        createModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_THU_VIEC;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_CHUYEN_GIA:
                                        createModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_CHUYEN_GIA;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_THOI_VU:
                                        createModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_THOI_VU;
                                        break;
                                    case Constant.DoiTuongLaoDong.HOP_DONG_KHAC:
                                        createModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_KHAC;
                                        break;
                                    default:
                                        createModel.ExtraNumber2 = null;
                                        break;
                                }
                            }
                            if ((hinhthucluong != null) && (!string.IsNullOrEmpty(hinhthucluong.ToString().Trim())))
                            {
                                switch (hinhthucluong.ToString().Trim())
                                {
                                    case Constant.HinhThucLuong.LUONG_THEO_THOI_GIAN:
                                        createModel.SalaryStatus = Constant.HinhThucLuong.NUM_LUONG_THEO_THOI_GIAN;
                                        break;
                                    case Constant.HinhThucLuong.LUONG_THEO_SAN_PHAM:
                                        createModel.SalaryStatus = Constant.HinhThucLuong.NUM_LUONG_THEO_THOI_GIAN;
                                        break;
                                    case Constant.HinhThucLuong.LUONG_KHOAN:
                                        createModel.SalaryStatus = Constant.HinhThucLuong.NUM_LUONG_THEO_THOI_GIAN;
                                        break;
                                    default:
                                        createModel.SalaryStatus = null;
                                        break;
                                }
                            }
                            if ((xuatthan != null) && (!string.IsNullOrEmpty(xuatthan.ToString().Trim())))
                            {
                                createModel.XuatThan = xuatthan.ToString().Trim();
                            }
                            if ((nguoigioithieu != null) && (!string.IsNullOrEmpty(nguoigioithieu.ToString().Trim())))
                            {
                                createModel.IntroducedBy = nguoigioithieu.ToString().Trim();
                            }
                            if ((lienhekhancap != null) && (!string.IsNullOrEmpty(lienhekhancap.ToString().Trim())))
                            {
                                createModel.Emergency = lienhekhancap.ToString().Trim();
                            }

                            //Thong tin khac
                            if ((masothuecanhan != null) && (!string.IsNullOrEmpty(masothuecanhan.ToString().Trim())))
                            {
                                createModel.PITCode = masothuecanhan.ToString().Trim();
                            }
                            if ((noicapmasothue != null) && (!string.IsNullOrEmpty(noicapmasothue.ToString().Trim())))
                            {
                                createModel.ExtraNumber5 = lstLocation.Where(x => x.LocationName.Trim() == noicapmasothue.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((mahogiadinh != null) && (!string.IsNullOrEmpty(mahogiadinh.ToString().Trim())))
                            {
                                createModel.LabourBook = mahogiadinh.ToString().Trim();
                            }
                            if ((masobhxh != null) && (!string.IsNullOrEmpty(masobhxh.ToString().Trim())))
                            {
                                createModel.InsuranceNo = masobhxh.ToString().Trim();
                            }
                            if ((noicapbhxh != null) && (!string.IsNullOrEmpty(noicapbhxh.ToString().Trim())))
                            {
                                createModel.InsuranceIssuePlaceID = lstLocation.Where(x => x.LocationName.Trim() == noicapbhxh.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((ngaycapbhxh != null) && (!string.IsNullOrEmpty(ngaycapbhxh.ToString().Trim())))
                            {
                                try
                                {
                                    createModel.InsuranceIssueDate = DateTime.ParseExact(ngaycapbhxh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((noikcbbandau != null) && (!string.IsNullOrEmpty(noikcbbandau.ToString().Trim())))
                            {
                                createModel.HospitalID = lstBenhVien.Where(x => x.HospitalName.Trim() == noikcbbandau.ToString().Trim()).FirstOrDefault()?.HospitalID;
                            }
                            if ((nghenghieptruoctuyendung != null) && (!string.IsNullOrEmpty(nghenghieptruoctuyendung.ToString().Trim())))
                            {
                                createModel.NgheNghiepTruocTuyenDung = nghenghieptruoctuyendung.ToString().Trim();
                            }
                            if ((congvieclamlaunhat != null) && (!string.IsNullOrEmpty(congvieclamlaunhat.ToString().Trim())))
                            {
                                createModel.CongViecLamLauNhat = congvieclamlaunhat.ToString().Trim();
                            }
                            if ((sogiaypheplaixe != null) && (!string.IsNullOrEmpty(sogiaypheplaixe.ToString().Trim())))
                            {
                                createModel.DriverLicenseNo = sogiaypheplaixe.ToString().Trim();
                            }
                            if ((noicapgiaypheplaixe != null) && (!string.IsNullOrEmpty(noicapgiaypheplaixe.ToString().Trim())))
                            {
                                createModel.DriverLicenseIssuePlaceID = lstLocation.Where(x => x.LocationName.Trim() == noicapgiaypheplaixe.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((ngaycapgiaypheplaixe != null) && (!string.IsNullOrEmpty(ngaycapgiaypheplaixe.ToString().Trim())))
                            {
                                try
                                {
                                    createModel.DriverLicenseIssueDate = DateTime.ParseExact(ngaycapgiaypheplaixe.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((danhhieuduocphong != null) && (!string.IsNullOrEmpty(danhhieuduocphong.ToString().Trim())))
                            {
                                createModel.DanhHieuDuocPhong = danhhieuduocphong.ToString().Trim();
                            }
                            if ((sotruong != null) && (!string.IsNullOrEmpty(sotruong.ToString().Trim())))
                            {
                                createModel.Hobby = sotruong.ToString().Trim();
                            }
                            if ((dacdiemlichsubanthan != null) && (!string.IsNullOrEmpty(dacdiemlichsubanthan.ToString().Trim())))
                            {
                                createModel.DacDiemLichSuBanThan1 = dacdiemlichsubanthan.ToString().Trim();
                            }
                            if ((ngaythamgiactxh != null) && (!string.IsNullOrEmpty(ngaythamgiactxh.ToString().Trim())))
                            {
                                try
                                {
                                    createModel.NgayThamGiaToChucChinhTriXaHoi = DateTime.ParseExact(ngaythamgiactxh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((ngayvaodoan != null) && (!string.IsNullOrEmpty(ngayvaodoan.ToString().Trim())))
                            {
                                try
                                {
                                    createModel.NgayVaoDoan = DateTime.ParseExact(ngayvaodoan.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((hangthuongbinh != null) && (!string.IsNullOrEmpty(hangthuongbinh.ToString().Trim())))
                            {
                                switch (hangthuongbinh.ToString().Trim())
                                {
                                    case Constant.HangThuongBinh.HANG_1_4:
                                        createModel.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_1_4;
                                        break;
                                    case Constant.HangThuongBinh.HANG_2_4:
                                        createModel.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_2_4;
                                        break;
                                    case Constant.HangThuongBinh.HANG_3_4:
                                        createModel.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_3_4;
                                        break;
                                    case Constant.HangThuongBinh.HANG_4_4:
                                        createModel.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_4_4;
                                        break;
                                    default:
                                        createModel.HangThuongBinh = null;
                                        break;
                                }
                            }
                            if ((giadinhchinhsach != null) && (!string.IsNullOrEmpty(giadinhchinhsach.ToString().Trim())))
                            {
                                switch (giadinhchinhsach.ToString().Trim())
                                {
                                    case Constant.GiaDinhChinhSach.CON_THUONG_BINH:
                                        createModel.GiaDinhChinhSach = Constant.GiaDinhChinhSach.NUM_CON_THUONG_BINH;
                                        break;
                                    case Constant.GiaDinhChinhSach.CON_LIET_SI:
                                        createModel.GiaDinhChinhSach = Constant.GiaDinhChinhSach.NUM_CON_LIET_SI;
                                        break;
                                    case Constant.GiaDinhChinhSach.NGUOI_NHIEM_CHAT_DOC_DA_CAM:
                                        createModel.GiaDinhChinhSach = Constant.GiaDinhChinhSach.NUM_NGUOI_NHIEM_CHAT_DOC_DA_CAM;
                                        break;
                                    default:
                                        createModel.GiaDinhChinhSach = null;
                                        break;
                                }
                            }
                            if ((vietinbank != null) && (!string.IsNullOrEmpty(vietinbank.ToString().Trim())))
                            {
                                createModel.Vietinbank = vietinbank.ToString().Trim();
                            }
                            if ((vietcombank != null) && (!string.IsNullOrEmpty(vietcombank.ToString().Trim())))
                            {
                                createModel.Vietcombank = vietcombank.ToString().Trim();
                            }
                            if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                            {
                                createModel.GhiChu = ghichu.ToString().Trim();
                            }

                            //Tinh thanh - Quan huyen - Phuong xa
                            if ((quequantinh != null) && (!string.IsNullOrEmpty(quequantinh.ToString().Trim())))
                            {
                                createModel.QueQuanLocationID = lstLocation.Where(x => x.LocationName.Trim() == quequantinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((quequanquanhuyen != null) && (!string.IsNullOrEmpty(quequanquanhuyen.ToString().Trim())))
                            {
                                if (createModel.QueQuanLocationID != null)
                                {
                                    createModel.QueQuanDistrictID = lstDistrict.Where(x => (x.DistrictName.Trim() == quequanquanhuyen.ToString().Trim()) && (x.LocationId == createModel.QueQuanLocationID)).FirstOrDefault()?.DistrictId;
                                }
                            }
                            if ((quequanphuongxa != null) && (!string.IsNullOrEmpty(quequanphuongxa.ToString().Trim())))
                            {
                                if (createModel.QueQuanDistrictID != null)
                                {
                                    createModel.QueQuanWardID = lstWard.Where(x => (x.WardName.Trim() == quequanphuongxa.ToString().Trim()) && (x.DistrictId == createModel.QueQuanDistrictID)).FirstOrDefault()?.WardId;
                                }
                            }
                            if ((quequandiachi != null) && (!string.IsNullOrEmpty(quequandiachi.ToString().Trim())))
                            {
                                createModel.QueQuanAddr = quequandiachi.ToString().Trim();
                            }
                            if ((hktttinh != null) && (!string.IsNullOrEmpty(hktttinh.ToString().Trim())))
                            {
                                createModel.PermanentLocationID = lstLocation.Where(x => x.LocationName.Trim() == hktttinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((hkttquanhuyen != null) && (!string.IsNullOrEmpty(hkttquanhuyen.ToString().Trim())))
                            {
                                if (createModel.PermanentLocationID != null)
                                {
                                    createModel.PermanentDistrictID = lstDistrict.Where(x => (x.DistrictName.Trim() == quequanquanhuyen.ToString().Trim()) && (x.LocationId == createModel.QueQuanLocationID)).FirstOrDefault()?.DistrictId;
                                }
                            }
                            if ((hkttphuongxa != null) && (!string.IsNullOrEmpty(hkttphuongxa.ToString().Trim())))
                            {
                                if (createModel.PermanentDistrictID != null)
                                {
                                    createModel.PermanentWardID = lstWard.Where(x => (x.WardName.Trim() == quequanphuongxa.ToString().Trim()) && (x.DistrictId == createModel.QueQuanDistrictID)).FirstOrDefault()?.WardId;
                                }
                            }
                            if ((hkttdiachi != null) && (!string.IsNullOrEmpty(hkttdiachi.ToString().Trim())))
                            {
                                createModel.PermanentAddr = hkttdiachi.ToString().Trim();
                            }
                            if ((noiotinh != null) && (!string.IsNullOrEmpty(noiotinh.ToString().Trim())))
                            {
                                createModel.ContactLocationID = lstLocation.Where(x => x.LocationName.Trim() == noiotinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((noioquanhuyen != null) && (!string.IsNullOrEmpty(noioquanhuyen.ToString().Trim())))
                            {
                                if (createModel.ContactLocationID != null)
                                {
                                    createModel.ContactAddrDistrictID = lstDistrict.Where(x => (x.DistrictName.Trim() == noioquanhuyen.ToString().Trim()) && (x.LocationId == createModel.ContactLocationID)).FirstOrDefault()?.DistrictId;
                                }
                            }
                            if ((noiophuongxa != null) && (!string.IsNullOrEmpty(noiophuongxa.ToString().Trim())))
                            {
                                if (createModel.ContactAddrDistrictID != null)
                                {
                                    createModel.ContactAddrWardID = lstWard.Where(x => (x.WardName.Trim() == noiophuongxa.ToString().Trim()) && (x.DistrictId == createModel.ContactAddrDistrictID)).FirstOrDefault()?.WardId;
                                }
                            }
                            if ((noiodiachi != null) && (!string.IsNullOrEmpty(noiodiachi.ToString().Trim())))
                            {
                                createModel.ContactAddr = noiodiachi.ToString().Trim();
                            }
                            if ((noisinhtinh != null) && (!string.IsNullOrEmpty(noisinhtinh.ToString().Trim())))
                            {
                                createModel.BirthPlaceID = lstLocation.Where(x => x.LocationName.Trim() == noisinhtinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                            }
                            if ((noisinhquanhuyen != null) && (!string.IsNullOrEmpty(noisinhquanhuyen.ToString().Trim())))
                            {
                                if (createModel.BirthPlaceID != null)
                                {
                                    createModel.BirthPlaceDistrictID = lstDistrict.Where(x => (x.DistrictName.Trim() == noisinhquanhuyen.ToString().Trim()) && (x.LocationId == createModel.BirthPlaceID)).FirstOrDefault()?.DistrictId;
                                }
                            }
                            if ((noisinhphuongxa != null) && (!string.IsNullOrEmpty(noisinhphuongxa.ToString().Trim())))
                            {
                                if (createModel.BirthPlaceDistrictID != null)
                                {
                                    createModel.BirthPlaceWardID = lstWard.Where(x => (x.WardName.Trim() == noisinhphuongxa.ToString().Trim()) && (x.DistrictId == createModel.BirthPlaceDistrictID)).FirstOrDefault()?.WardId;
                                }
                            }
                            if ((noisinhdiachi != null) && (!string.IsNullOrEmpty(noisinhdiachi.ToString().Trim())))
                            {
                                createModel.BirthPlaceAddr = noisinhdiachi.ToString().Trim();
                            }

                            //Giay to dinh kem
                            List<StaffLicenseModel> lstCreateStaffLicense = new List<StaffLicenseModel>();
                            var cmndCreated = new StaffLicenseModel();
                            var cchnCreated = new StaffLicenseModel();
                            //Chung minh nhan dan
                            cmndCreated.Type = Constant.Licenses.CMND;
                            cchnCreated.Type = Constant.Licenses.CCHN;
                            if ((socmnd != null) && (!string.IsNullOrEmpty(socmnd.ToString().Trim())))
                            {
                                cmndCreated.LicenseNumber = socmnd.ToString().Trim();
                            }
                            if ((noicapcmnd != null) && (!string.IsNullOrEmpty(noicapcmnd.ToString().Trim())))
                            {
                                var location = lstLocation.Where(x => x.LocationName.Trim() == noicapcmnd.ToString().Trim()).FirstOrDefault();
                                if (location != null)
                                {
                                    cmndCreated.IssuePlaceID = location.LocationID;
                                }
                            }
                            if ((ngaycapcmnd != null) && (!string.IsNullOrEmpty(ngaycapcmnd.ToString().Trim())))
                            {
                                try
                                {
                                    cmndCreated.IssueDate = DateTime.ParseExact(ngaycapcmnd.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((ngayhethancmnd != null) && (!string.IsNullOrEmpty(ngayhethancmnd.ToString().Trim())))
                            {
                                try
                                {
                                    cmndCreated.ExpiredIssueDate = DateTime.ParseExact(ngayhethancmnd.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            //Chung chi hanh nghe
                            if ((socchn != null) && (!string.IsNullOrEmpty(socchn.ToString().Trim())))
                            {
                                cchnCreated.LicenseNumber = socchn.ToString().Trim();
                            }
                            if ((phamvihanhnghe != null) && (!string.IsNullOrEmpty(phamvihanhnghe.ToString().Trim())))
                            {
                                cchnCreated.PracticingScope = phamvihanhnghe.ToString().Trim();
                            }
                            if ((ngaycapcchn != null) && (!string.IsNullOrEmpty(ngaycapcchn.ToString().Trim())))
                            {
                                try
                                {
                                    cchnCreated.IssueDate = DateTime.ParseExact(ngaycapcchn.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ((noicapcchn != null) && (!string.IsNullOrEmpty(noicapcchn.ToString().Trim())))
                            {
                                var location = lstLocation.Where(x => x.LocationName.Trim() == noicapcmnd.ToString().Trim()).FirstOrDefault();
                                if (location != null)
                                {
                                    cchnCreated.IssuePlaceID = location.LocationID;
                                }
                            }
                            if (!string.IsNullOrEmpty(cmndCreated.LicenseNumber))
                            {
                                lstCreateStaffLicense.Add(cmndCreated);
                            }
                            if (!string.IsNullOrEmpty(cchnCreated.LicenseNumber))
                            {
                                lstCreateStaffLicense.Add(cchnCreated);
                            }
                            if (lstCreateStaffLicense.Count > 0)
                            {
                                createModel.StaffLicense = lstCreateStaffLicense;
                            }

                            #endregion

                            var resultCreate = await _staffHandler.CreateStaffGeneralInfo(createModel);

                            if (resultCreate.Status == Constant.ErrorCode.SUCCESS_CODE)
                            {
                                #region Update model - Thong tin khac

                                var updateModel = new StaffUpdateRequestModel();
                                updateModel.StaffID = resultCreate.Data;
                                if ((maNV != null) && (!string.IsNullOrEmpty(maNV.ToString().Trim())))
                                {
                                    updateModel.StaffCode = maNV.ToString().Trim();
                                }
                                if ((ho != null) && (!string.IsNullOrEmpty(ho.ToString().Trim())))
                                {
                                    updateModel.FirstName = ho.ToString().Trim();
                                }
                                if ((dem != null) && (!string.IsNullOrEmpty(dem.ToString().Trim())))
                                {
                                    updateModel.MidName = dem.ToString().Trim();
                                }
                                if ((ten != null) && (!string.IsNullOrEmpty(ten.ToString().Trim())))
                                {
                                    updateModel.LastName = ten.ToString().Trim();
                                }
                                if ((ngayvaodonvi != null) && (!string.IsNullOrEmpty(ngayvaodonvi.ToString().Trim())))
                                {
                                    try
                                    {
                                        updateModel.JoiningDate = DateTime.ParseExact(ngayvaodonvi.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((lydonghiviec != null) && (!string.IsNullOrEmpty(lydonghiviec.ToString().Trim())))
                                {
                                    switch (lydonghiviec.ToString().Trim())
                                    {
                                        case Constant.LyDoNghiViec.NGHI_HUU:
                                            updateModel.ExtraNumber1 = Constant.LyDoNghiViec.NUM_NGHI_HUU;
                                            break;
                                        case Constant.LyDoNghiViec.TU_VONG:
                                            updateModel.ExtraNumber1 = Constant.LyDoNghiViec.NUM_TU_VONG;
                                            break;
                                        case Constant.LyDoNghiViec.THOI_VIEC:
                                            updateModel.ExtraNumber1 = Constant.LyDoNghiViec.NUM_THOI_VIEC;
                                            break;
                                        default:
                                            updateModel.ExtraNumber1 = null;
                                            break;
                                    }
                                }
                                if ((ngaynghiviec != null) && (!string.IsNullOrEmpty(ngaynghiviec.ToString().Trim())))
                                {
                                    try
                                    {
                                        updateModel.LeavingDate = DateTime.ParseExact(ngaynghiviec.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((tengoikhac != null) && (!string.IsNullOrEmpty(tengoikhac.ToString().Trim())))
                                {
                                    updateModel.TenGoiKhac = tengoikhac.ToString().Trim();
                                }
                                if ((ngaysinh != null) && (!string.IsNullOrEmpty(ngaysinh.ToString().Trim())))
                                {
                                    try
                                    {
                                        updateModel.Birthday = DateTime.ParseExact(ngaysinh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((gioitinh != null) && (!string.IsNullOrEmpty(gioitinh.ToString().Trim())))
                                {
                                    switch (gioitinh.ToString().Trim())
                                    {
                                        case Constant.GioiTinh.NU:
                                            updateModel.Gender = Constant.GioiTinh.NUM_NU;
                                            break;
                                        case Constant.GioiTinh.NAM:
                                            updateModel.Gender = Constant.GioiTinh.NUM_NAM;
                                            break;
                                        case Constant.GioiTinh.KHAC:
                                            updateModel.Gender = Constant.GioiTinh.NUM_KHAC;
                                            break;
                                        default:
                                            updateModel.Gender = null;
                                            break;
                                    }
                                }
                                if ((danhxung != null) && (!string.IsNullOrEmpty(danhxung.ToString().Trim())))
                                {
                                    updateModel.TitleID = lstDanhXung.Where(x => x.TitleName.Trim() == danhxung.ToString().Trim()).FirstOrDefault()?.TitleID;
                                }
                                if ((tinhtranghonnhan != null) && (!string.IsNullOrEmpty(ho.ToString().Trim())))
                                {
                                    switch (tinhtranghonnhan.ToString().Trim())
                                    {
                                        case Constant.TinhTrangHonNhan.CHUA_XAC_DINH:
                                            updateModel.MaritalStatus = Constant.TinhTrangHonNhan.NUM_CHUA_XAC_DINH;
                                            break;
                                        case Constant.TinhTrangHonNhan.DOC_THAN:
                                            updateModel.MaritalStatus = Constant.TinhTrangHonNhan.NUM_DOC_THAN;
                                            break;
                                        case Constant.TinhTrangHonNhan.DA_KET_HON:
                                            updateModel.MaritalStatus = Constant.TinhTrangHonNhan.NUM_DA_KET_HON;
                                            break;
                                        case Constant.TinhTrangHonNhan.VO_CHONG_CUNG_CO_QUAN:
                                            updateModel.MaritalStatus = Constant.TinhTrangHonNhan.NUM_VO_CHONG_CUNG_CO_QUAN;
                                            break;
                                        default:
                                            updateModel.MaritalStatus = null;
                                            break;
                                    }
                                }
                                if ((quoctich != null) && (!string.IsNullOrEmpty(quoctich.ToString().Trim())))
                                {
                                    updateModel.TerritoryID = lstCountry.Where(x => x.CountryName.Trim() == quoctich.ToString().Trim()).FirstOrDefault()?.CountryId;
                                }
                                if ((dantoc != null) && (!string.IsNullOrEmpty(dantoc.ToString().Trim())))
                                {
                                    updateModel.EthnicID = lstEthnic.Where(x => x.EthnicName.Trim() == dantoc.ToString().Trim()).FirstOrDefault()?.EthnicId;
                                }
                                if ((tongiao != null) && (!string.IsNullOrEmpty(tongiao.ToString().Trim())))
                                {
                                    updateModel.ReligionID = lstTonGiao.Where(x => x.ReligionName.Trim() == tongiao.ToString().Trim()).FirstOrDefault()?.ReligionID;
                                }
                                if ((sodienthoai != null) && (!string.IsNullOrEmpty(sodienthoai.ToString().Trim())))
                                {
                                    updateModel.Telephone = sodienthoai.ToString().Trim();
                                }
                                if ((email != null) && (!string.IsNullOrEmpty(email.ToString().Trim())))
                                {
                                    updateModel.PersonalEmail = email.ToString().Trim();
                                }
                                if ((trinhdovanhoa != null) && (!string.IsNullOrEmpty(trinhdovanhoa.ToString().Trim())))
                                {
                                    switch (trinhdovanhoa.ToString().Trim())
                                    {
                                        case Constant.TrinhDoVanHoa.TRINH_DO_9_9:
                                            updateModel.ExtraNumber3 = Constant.TrinhDoVanHoa.NUM_TRINH_DO_9_9;
                                            break;
                                        case Constant.TrinhDoVanHoa.TRINH_DO_10_10:
                                            updateModel.ExtraNumber3 = Constant.TrinhDoVanHoa.NUM_TRINH_DO_10_10;
                                            break;
                                        case Constant.TrinhDoVanHoa.TRINH_DO_12_12:
                                            updateModel.ExtraNumber3 = Constant.TrinhDoVanHoa.NUM_TRINH_DO_12_12;
                                            break;
                                        default:
                                            updateModel.ExtraNumber3 = null;
                                            break;
                                    }
                                }
                                if ((trinhdochuyenmon != null) && (!string.IsNullOrEmpty(trinhdochuyenmon.ToString().Trim())))
                                {
                                    updateModel.QualificationID = lstTrinhDoChuyenMon.Where(x => x.TrinhDoChuyenMonName.Trim() == trinhdochuyenmon.ToString().Trim()).FirstOrDefault()?.TrinhDoChuyenMonID;
                                }
                                if ((hocvi != null) && (!string.IsNullOrEmpty(hocvi.ToString().Trim())))
                                {
                                    updateModel.DegreeID = lstHocVi.Where(x => x.DegreeName.Trim() == hocvi.ToString().Trim()).FirstOrDefault()?.DegreeID;
                                }
                                if ((hocham != null) && (!string.IsNullOrEmpty(hocham.ToString().Trim())))
                                {
                                    updateModel.AcademicRankID = lstHocHam.Where(x => x.AcademicRankName.Trim() == hocham.ToString().Trim()).FirstOrDefault()?.AcademicRankID;
                                }
                                if ((doituonglaodong != null) && (!string.IsNullOrEmpty(doituonglaodong.ToString().Trim())))
                                {
                                    switch (doituonglaodong.ToString().Trim())
                                    {
                                        case Constant.DoiTuongLaoDong.VIEN_CHUC:
                                            updateModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_VIEN_CHUC;
                                            break;
                                        case Constant.DoiTuongLaoDong.HOP_DONG_68:
                                            updateModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_68;
                                            break;
                                        case Constant.DoiTuongLaoDong.HOP_DONG_KHONG_XAC_DINH_THOI_HAN:
                                            updateModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_KHONG_XAC_DINH_THOI_HAN;
                                            break;
                                        case Constant.DoiTuongLaoDong.HOP_DONG_NGAN_HAN:
                                            updateModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_NGAN_HAN;
                                            break;
                                        case Constant.DoiTuongLaoDong.HOP_DONG_THU_VIEC:
                                            updateModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_THU_VIEC;
                                            break;
                                        case Constant.DoiTuongLaoDong.HOP_DONG_CHUYEN_GIA:
                                            updateModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_CHUYEN_GIA;
                                            break;
                                        case Constant.DoiTuongLaoDong.HOP_DONG_THOI_VU:
                                            updateModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_THOI_VU;
                                            break;
                                        case Constant.DoiTuongLaoDong.HOP_DONG_KHAC:
                                            updateModel.ExtraNumber2 = Constant.DoiTuongLaoDong.NUM_HOP_DONG_KHAC;
                                            break;
                                        default:
                                            updateModel.ExtraNumber2 = null;
                                            break;
                                    }
                                }
                                if ((hinhthucluong != null) && (!string.IsNullOrEmpty(hinhthucluong.ToString().Trim())))
                                {
                                    switch (hinhthucluong.ToString().Trim())
                                    {
                                        case Constant.HinhThucLuong.LUONG_THEO_THOI_GIAN:
                                            updateModel.SalaryStatus = Constant.HinhThucLuong.NUM_LUONG_THEO_THOI_GIAN;
                                            break;
                                        case Constant.HinhThucLuong.LUONG_THEO_SAN_PHAM:
                                            updateModel.SalaryStatus = Constant.HinhThucLuong.NUM_LUONG_THEO_THOI_GIAN;
                                            break;
                                        case Constant.HinhThucLuong.LUONG_KHOAN:
                                            updateModel.SalaryStatus = Constant.HinhThucLuong.NUM_LUONG_THEO_THOI_GIAN;
                                            break;
                                        default:
                                            updateModel.SalaryStatus = null;
                                            break;
                                    }
                                }
                                if ((xuatthan != null) && (!string.IsNullOrEmpty(xuatthan.ToString().Trim())))
                                {
                                    updateModel.XuatThan = xuatthan.ToString().Trim();
                                }
                                if ((nguoigioithieu != null) && (!string.IsNullOrEmpty(nguoigioithieu.ToString().Trim())))
                                {
                                    updateModel.IntroducedBy = nguoigioithieu.ToString().Trim();
                                }
                                if ((lienhekhancap != null) && (!string.IsNullOrEmpty(lienhekhancap.ToString().Trim())))
                                {
                                    updateModel.Emergency = lienhekhancap.ToString().Trim();
                                }

                                //Thong tin khac
                                if ((masothuecanhan != null) && (!string.IsNullOrEmpty(masothuecanhan.ToString().Trim())))
                                {
                                    updateModel.PITCode = masothuecanhan.ToString().Trim();
                                }
                                if ((noicapmasothue != null) && (!string.IsNullOrEmpty(noicapmasothue.ToString().Trim())))
                                {
                                    updateModel.ExtraNumber5 = lstLocation.Where(x => x.LocationName.Trim() == noicapmasothue.ToString().Trim()).FirstOrDefault()?.LocationID;
                                }
                                if ((mahogiadinh != null) && (!string.IsNullOrEmpty(mahogiadinh.ToString().Trim())))
                                {
                                    updateModel.LabourBook = mahogiadinh.ToString().Trim();
                                }
                                if ((masobhxh != null) && (!string.IsNullOrEmpty(masobhxh.ToString().Trim())))
                                {
                                    updateModel.InsuranceNo = masobhxh.ToString().Trim();
                                }
                                if ((noicapbhxh != null) && (!string.IsNullOrEmpty(noicapbhxh.ToString().Trim())))
                                {
                                    updateModel.InsuranceIssuePlaceID = lstLocation.Where(x => x.LocationName.Trim() == noicapbhxh.ToString().Trim()).FirstOrDefault()?.LocationID;
                                }
                                if ((ngaycapbhxh != null) && (!string.IsNullOrEmpty(ngaycapbhxh.ToString().Trim())))
                                {
                                    try
                                    {
                                        updateModel.InsuranceIssueDate = DateTime.ParseExact(ngaycapbhxh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((noikcbbandau != null) && (!string.IsNullOrEmpty(noikcbbandau.ToString().Trim())))
                                {
                                    updateModel.HospitalID = lstBenhVien.Where(x => x.HospitalName.Trim() == noikcbbandau.ToString().Trim()).FirstOrDefault()?.HospitalID;
                                }
                                if ((nghenghieptruoctuyendung != null) && (!string.IsNullOrEmpty(nghenghieptruoctuyendung.ToString().Trim())))
                                {
                                    updateModel.NgheNghiepTruocTuyenDung = nghenghieptruoctuyendung.ToString().Trim();
                                }
                                if ((congvieclamlaunhat != null) && (!string.IsNullOrEmpty(congvieclamlaunhat.ToString().Trim())))
                                {
                                    updateModel.CongViecLamLauNhat = congvieclamlaunhat.ToString().Trim();
                                }
                                if ((sogiaypheplaixe != null) && (!string.IsNullOrEmpty(sogiaypheplaixe.ToString().Trim())))
                                {
                                    updateModel.DriverLicenseNo = sogiaypheplaixe.ToString().Trim();
                                }
                                if ((noicapgiaypheplaixe != null) && (!string.IsNullOrEmpty(noicapgiaypheplaixe.ToString().Trim())))
                                {
                                    updateModel.DriverLicenseIssuePlaceID = lstLocation.Where(x => x.LocationName.Trim() == noicapgiaypheplaixe.ToString().Trim()).FirstOrDefault()?.LocationID;
                                }
                                if ((ngaycapgiaypheplaixe != null) && (!string.IsNullOrEmpty(ngaycapgiaypheplaixe.ToString().Trim())))
                                {
                                    try
                                    {
                                        updateModel.DriverLicenseIssueDate = DateTime.ParseExact(ngaycapgiaypheplaixe.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((danhhieuduocphong != null) && (!string.IsNullOrEmpty(danhhieuduocphong.ToString().Trim())))
                                {
                                    updateModel.DanhHieuDuocPhong = danhhieuduocphong.ToString().Trim();
                                }
                                if ((sotruong != null) && (!string.IsNullOrEmpty(sotruong.ToString().Trim())))
                                {
                                    updateModel.Hobby = sotruong.ToString().Trim();
                                }
                                if ((dacdiemlichsubanthan != null) && (!string.IsNullOrEmpty(dacdiemlichsubanthan.ToString().Trim())))
                                {
                                    updateModel.DacDiemLichSuBanThan1 = dacdiemlichsubanthan.ToString().Trim();
                                }
                                if ((ngaythamgiactxh != null) && (!string.IsNullOrEmpty(ngaythamgiactxh.ToString().Trim())))
                                {
                                    try
                                    {
                                        updateModel.NgayThamGiaToChucChinhTriXaHoi = DateTime.ParseExact(ngaythamgiactxh.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((ngayvaodoan != null) && (!string.IsNullOrEmpty(ngayvaodoan.ToString().Trim())))
                                {
                                    try
                                    {
                                        updateModel.NgayVaoDoan = DateTime.ParseExact(ngayvaodoan.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((hangthuongbinh != null) && (!string.IsNullOrEmpty(hangthuongbinh.ToString().Trim())))
                                {
                                    switch (hangthuongbinh.ToString().Trim())
                                    {
                                        case Constant.HangThuongBinh.HANG_1_4:
                                            updateModel.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_1_4;
                                            break;
                                        case Constant.HangThuongBinh.HANG_2_4:
                                            updateModel.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_2_4;
                                            break;
                                        case Constant.HangThuongBinh.HANG_3_4:
                                            updateModel.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_3_4;
                                            break;
                                        case Constant.HangThuongBinh.HANG_4_4:
                                            updateModel.HangThuongBinh = Constant.HangThuongBinh.NUM_HANG_4_4;
                                            break;
                                        default:
                                            updateModel.HangThuongBinh = null;
                                            break;
                                    }
                                }
                                if ((giadinhchinhsach != null) && (!string.IsNullOrEmpty(giadinhchinhsach.ToString().Trim())))
                                {
                                    switch (giadinhchinhsach.ToString().Trim())
                                    {
                                        case Constant.GiaDinhChinhSach.CON_THUONG_BINH:
                                            updateModel.GiaDinhChinhSach = Constant.GiaDinhChinhSach.NUM_CON_THUONG_BINH;
                                            break;
                                        case Constant.GiaDinhChinhSach.CON_LIET_SI:
                                            updateModel.GiaDinhChinhSach = Constant.GiaDinhChinhSach.NUM_CON_LIET_SI;
                                            break;
                                        case Constant.GiaDinhChinhSach.NGUOI_NHIEM_CHAT_DOC_DA_CAM:
                                            updateModel.GiaDinhChinhSach = Constant.GiaDinhChinhSach.NUM_NGUOI_NHIEM_CHAT_DOC_DA_CAM;
                                            break;
                                        default:
                                            updateModel.GiaDinhChinhSach = null;
                                            break;
                                    }
                                }
                                if ((vietinbank != null) && (!string.IsNullOrEmpty(vietinbank.ToString().Trim())))
                                {
                                    updateModel.Vietinbank = vietinbank.ToString().Trim();
                                }
                                if ((vietcombank != null) && (!string.IsNullOrEmpty(vietcombank.ToString().Trim())))
                                {
                                    updateModel.Vietcombank = vietcombank.ToString().Trim();
                                }
                                if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                {
                                    updateModel.GhiChu = ghichu.ToString().Trim();
                                }

                                //Tinh thanh - Quan huyen - Phuong xa
                                if ((quequantinh != null) && (!string.IsNullOrEmpty(quequantinh.ToString().Trim())))
                                {
                                    updateModel.QueQuanLocationID = lstLocation.Where(x => x.LocationName.Trim() == quequantinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                                }
                                if ((quequanquanhuyen != null) && (!string.IsNullOrEmpty(quequanquanhuyen.ToString().Trim())))
                                {
                                    if (updateModel.QueQuanLocationID != null)
                                    {
                                        updateModel.QueQuanDistrictID = lstDistrict.Where(x => (x.DistrictName.Trim() == quequanquanhuyen.ToString().Trim()) && (x.LocationId == updateModel.QueQuanLocationID)).FirstOrDefault()?.DistrictId;
                                    }
                                }
                                if ((quequanphuongxa != null) && (!string.IsNullOrEmpty(quequanphuongxa.ToString().Trim())))
                                {
                                    if (updateModel.QueQuanDistrictID != null)
                                    {
                                        updateModel.QueQuanWardID = lstWard.Where(x => (x.WardName.Trim() == quequanphuongxa.ToString().Trim()) && (x.DistrictId == updateModel.QueQuanDistrictID)).FirstOrDefault()?.WardId;
                                    }
                                }
                                if ((quequandiachi != null) && (!string.IsNullOrEmpty(quequandiachi.ToString().Trim())))
                                {
                                    updateModel.QueQuanAddr = quequandiachi.ToString().Trim();
                                }
                                if ((hktttinh != null) && (!string.IsNullOrEmpty(hktttinh.ToString().Trim())))
                                {
                                    updateModel.PermanentLocationID = lstLocation.Where(x => x.LocationName.Trim() == hktttinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                                }
                                if ((hkttquanhuyen != null) && (!string.IsNullOrEmpty(hkttquanhuyen.ToString().Trim())))
                                {
                                    if (updateModel.PermanentLocationID != null)
                                    {
                                        updateModel.PermanentDistrictID = lstDistrict.Where(x => (x.DistrictName == quequanquanhuyen.ToString().Trim()) && (x.LocationId == updateModel.QueQuanLocationID)).FirstOrDefault()?.DistrictId;
                                    }
                                }
                                if ((hkttphuongxa != null) && (!string.IsNullOrEmpty(hkttphuongxa.ToString().Trim())))
                                {
                                    if (updateModel.PermanentDistrictID != null)
                                    {
                                        updateModel.PermanentWardID = lstWard.Where(x => (x.WardName.Trim() == quequanphuongxa.ToString().Trim()) && (x.DistrictId == updateModel.QueQuanDistrictID)).FirstOrDefault()?.WardId;
                                    }
                                }
                                if ((hkttdiachi != null) && (!string.IsNullOrEmpty(hkttdiachi.ToString().Trim())))
                                {
                                    updateModel.PermanentAddr = hkttdiachi.ToString().Trim();
                                }
                                if ((noiotinh != null) && (!string.IsNullOrEmpty(noiotinh.ToString().Trim())))
                                {
                                    updateModel.ContactLocationID = lstLocation.Where(x => x.LocationName.Trim() == noiotinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                                }
                                if ((noioquanhuyen != null) && (!string.IsNullOrEmpty(noioquanhuyen.ToString().Trim())))
                                {
                                    if (updateModel.ContactLocationID != null)
                                    {
                                        updateModel.ContactAddrDistrictID = lstDistrict.Where(x => (x.DistrictName.Trim() == noioquanhuyen.ToString().Trim()) && (x.LocationId == updateModel.ContactLocationID)).FirstOrDefault()?.DistrictId;
                                    }
                                }
                                if ((noiophuongxa != null) && (!string.IsNullOrEmpty(noiophuongxa.ToString().Trim())))
                                {
                                    if (updateModel.ContactAddrDistrictID != null)
                                    {
                                        updateModel.ContactAddrWardID = lstWard.Where(x => (x.WardName.Trim() == noiophuongxa.ToString().Trim()) && (x.DistrictId == updateModel.ContactAddrDistrictID)).FirstOrDefault()?.WardId;
                                    }
                                }
                                if ((noiodiachi != null) && (!string.IsNullOrEmpty(noiodiachi.ToString().Trim())))
                                {
                                    updateModel.ContactAddr = noiodiachi.ToString().Trim();
                                }
                                if ((noisinhtinh != null) && (!string.IsNullOrEmpty(noisinhtinh.ToString().Trim())))
                                {
                                    updateModel.BirthPlaceID = lstLocation.Where(x => x.LocationName.Trim() == noisinhtinh.ToString().Trim()).FirstOrDefault()?.LocationID;
                                }
                                if ((noisinhquanhuyen != null) && (!string.IsNullOrEmpty(noisinhquanhuyen.ToString().Trim())))
                                {
                                    if (updateModel.BirthPlaceID != null)
                                    {
                                        updateModel.BirthPlaceDistrictID = lstDistrict.Where(x => (x.DistrictName.Trim() == noisinhquanhuyen.ToString().Trim()) && (x.LocationId == updateModel.BirthPlaceID)).FirstOrDefault()?.DistrictId;
                                    }
                                }
                                if ((noisinhphuongxa != null) && (!string.IsNullOrEmpty(noisinhphuongxa.ToString().Trim())))
                                {
                                    if (updateModel.BirthPlaceDistrictID != null)
                                    {
                                        updateModel.BirthPlaceWardID = lstWard.Where(x => (x.WardName.Trim() == noisinhphuongxa.ToString().Trim()) && (x.DistrictId == updateModel.BirthPlaceDistrictID)).FirstOrDefault()?.WardId;
                                    }
                                }
                                if ((noisinhdiachi != null) && (!string.IsNullOrEmpty(noisinhdiachi.ToString().Trim())))
                                {
                                    updateModel.BirthPlaceAddr = noisinhdiachi.ToString().Trim();
                                }

                                //Giay to dinh kem
                                List<StaffLicenseModel> lstStaffLicense = new List<StaffLicenseModel>();
                                var cmnd = new StaffLicenseModel();
                                var cchn = new StaffLicenseModel();
                                //Chung minh nhan dan
                                cmnd.Type = Constant.Licenses.CMND;
                                cchn.Type = Constant.Licenses.CCHN;
                                if ((socmnd != null) && (!string.IsNullOrEmpty(socmnd.ToString().Trim())))
                                {
                                    cmnd.LicenseNumber = socmnd.ToString().Trim();
                                }
                                if ((noicapcmnd != null) && (!string.IsNullOrEmpty(noicapcmnd.ToString().Trim())))
                                {
                                    var location = lstLocation.Where(x => x.LocationName.Trim() == noicapcmnd.ToString().Trim()).FirstOrDefault();
                                    if (location != null)
                                    {
                                        cmnd.IssuePlaceID = location.LocationID;
                                    }
                                }
                                if ((ngaycapcmnd != null) && (!string.IsNullOrEmpty(ngaycapcmnd.ToString().Trim())))
                                {
                                    try
                                    {
                                        cmnd.IssueDate = DateTime.ParseExact(ngaycapcmnd.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((ngayhethancmnd != null) && (!string.IsNullOrEmpty(ngayhethancmnd.ToString().Trim())))
                                {
                                    try
                                    {
                                        cmnd.ExpiredIssueDate = DateTime.ParseExact(ngayhethancmnd.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                //Chung chi hanh nghe
                                if ((socchn != null) && (!string.IsNullOrEmpty(socchn.ToString().Trim())))
                                {
                                    cchn.LicenseNumber = socchn.ToString().Trim();
                                }
                                if ((phamvihanhnghe != null) && (!string.IsNullOrEmpty(phamvihanhnghe.ToString().Trim())))
                                {
                                    cchn.PracticingScope = phamvihanhnghe.ToString().Trim();
                                }
                                if ((ngaycapcchn != null) && (!string.IsNullOrEmpty(ngaycapcchn.ToString().Trim())))
                                {
                                    try
                                    {
                                        cchn.IssueDate = DateTime.ParseExact(ngaycapcchn.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((noicapcchn != null) && (!string.IsNullOrEmpty(noicapcchn.ToString().Trim())))
                                {
                                    var location = lstLocation.Where(x => x.LocationName.Trim() == noicapcmnd.ToString().Trim()).FirstOrDefault();
                                    if (location != null)
                                    {
                                        cchn.IssuePlaceID = location.LocationID;
                                    }
                                }
                                if (!string.IsNullOrEmpty(cmnd.LicenseNumber))
                                {
                                    lstStaffLicense.Add(cmnd);
                                }
                                if (!string.IsNullOrEmpty(cchn.LicenseNumber))
                                {
                                    lstStaffLicense.Add(cchn);
                                }
                                if (lstStaffLicense.Count > 0)
                                {
                                    updateModel.StaffLicense = lstStaffLicense;
                                }

                                #endregion

                                var result = await _staffHandler.UpdateStaffGeneralInfo(updateModel);
                                success = success + 1;
                            }
                            else
                            {
                                fail = fail + 1;
                            }
                        }
                    }
                    curRow = curRow + 1;
                }
                // Write log
                _baseHandler.CreateLog(new SystemLogModel
                {
                    ApplicationId = userModel.ApplicationId,
                    UserId = userModel.UserId,
                    ActionByUser = LogAction.IMPORT,
                    Date = DateTime.Now,
                    Content = $"Import thông tin chung: Thành công: " + success + " bản ghi. Lỗi: " + fail + " bản ghi"
                });
                return "Import thành công: " + success + " bản ghi. Import lỗi: " + fail + " bản ghi";
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        /// <summary>
        /// Import thông tin đảng
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportThongTinDang(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 1)
                {
                    return "Không đúng cấu trúc file";
                }

                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                var lstChiBo = _baseHandler.GetListData<PartyCellModel>("PartyCell", "1", 1);
                var lstChucVuDang = _baseHandler.GetListData<PartyTitleModel>("PartyTitle", "1", 1);

                var success = 0;
                var fail = 0;
                bool isValidate;

                var curRow = 1;
                while (curRow <= sheet.LastRowNum)
                {
                    var maNV = sheet.GetRow(curRow).GetCell(1);
                    if (maNV != null)
                    {
                        isValidate = true;

                        #region Get data from excel

                        var chibo = sheet.GetRow(curRow).GetCell(2);
                        var chucvudang = sheet.GetRow(curRow).GetCell(3);
                        var tungay = sheet.GetRow(curRow).GetCell(4);
                        var denngay = sheet.GetRow(curRow).GetCell(5);
                        var lyluanchinhtri = sheet.GetRow(curRow).GetCell(6);
                        var nghenghieptruockhivaodang = sheet.GetRow(curRow).GetCell(7);
                        var ngayvaodang = sheet.GetRow(curRow).GetCell(8);
                        var ngayvaodangchinhthuc = sheet.GetRow(curRow).GetCell(9);
                        var noiketnapdang = sheet.GetRow(curRow).GetCell(10);
                        var noicongnhan = sheet.GetRow(curRow).GetCell(11);
                        var solylich = sheet.GetRow(curRow).GetCell(12);
                        var sothedangvien = sheet.GetRow(curRow).GetCell(13);
                        var ngaychuyenden = sheet.GetRow(curRow).GetCell(14);
                        var noichuyenden = sheet.GetRow(curRow).GetCell(15);
                        var ngaychuyendi = sheet.GetRow(curRow).GetCell(16);
                        var noichuyendi = sheet.GetRow(curRow).GetCell(17);
                        var ngaybichet = sheet.GetRow(curRow).GetCell(18);
                        var lydochet = sheet.GetRow(curRow).GetCell(19);
                        var ngayrakhoidang = sheet.GetRow(curRow).GetCell(20);
                        var hinhthucrakhoidang = sheet.GetRow(curRow).GetCell(21);
                        var ghichu = sheet.GetRow(curRow).GetCell(22);

                        #endregion

                        var staffId = _staffHandler.GetStaffIDByStaffCode(maNV.ToString().Trim());
                        if (staffId > 0)
                        {
                            DateTime? fromDate = null;
                            DateTime? toDate = null;
                            try
                            {
                                fromDate = DateTime.ParseExact(tungay.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                            }
                            catch (Exception)
                            {
                                isValidate = false;
                            }
                            try
                            {
                                toDate = DateTime.ParseExact(denngay.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                            }
                            catch (Exception)
                            {
                                //isValidate = false;
                            }
                            var staffParty = await _partyHandler.FindByStaffAndTime(staffId, fromDate, toDate);
                            if (staffParty != null)
                            {
                                #region Update model - Thong tin Dang

                                //Cap nhat thong tin Dang
                                var model = new StaffPartyUpdateModel();
                                model.StaffPartyID = staffParty.StaffPartyID;
                                model.StaffID = staffId;
                                if (fromDate != null)
                                {
                                    model.TuNgay = fromDate;
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                model.DenNgay = toDate;
                                if ((chibo != null) && (!string.IsNullOrEmpty(chibo.ToString().Trim())))
                                {
                                    model.ChiBoID = lstChiBo.Where(x => (x.PartyCellName.Trim() == chibo.ToString().Trim())).FirstOrDefault()?.PartyCellID;
                                    if (model.ChiBoID==null)
                                    {
                                        isValidate = false;
                                    }
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((chucvudang != null) && (!string.IsNullOrEmpty(chucvudang.ToString().Trim())))
                                {
                                    model.ChucVuDangID = lstChucVuDang.Where(x => (x.PartyTitleName.Trim() == chucvudang.ToString().Trim())).FirstOrDefault()?.PartyTitleID;
                                    if (model.ChucVuDangID == null)
                                    {
                                        isValidate = false;
                                    }
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((lyluanchinhtri != null) && (!string.IsNullOrEmpty(lyluanchinhtri.ToString().Trim())))
                                {
                                    switch (lyluanchinhtri.ToString().Trim())
                                    {
                                        case Constant.LyLuanChinhTri_ThongTinDang.SO_CAP:
                                            model.LyLuanChinhTri = Constant.LyLuanChinhTri_ThongTinDang.SO_CAP;
                                            break;
                                        case Constant.LyLuanChinhTri_ThongTinDang.TRUNG_CAP:
                                            model.LyLuanChinhTri = Constant.LyLuanChinhTri_ThongTinDang.TRUNG_CAP;
                                            break;
                                        case Constant.LyLuanChinhTri_ThongTinDang.CAO_CAP:
                                            model.LyLuanChinhTri = Constant.LyLuanChinhTri_ThongTinDang.CAO_CAP;
                                            break;
                                        case Constant.LyLuanChinhTri_ThongTinDang.CU_NHAN:
                                            model.LyLuanChinhTri = Constant.LyLuanChinhTri_ThongTinDang.CU_NHAN;
                                            break;
                                    }
                                }
                                if ((nghenghieptruockhivaodang != null) && (!string.IsNullOrEmpty(nghenghieptruockhivaodang.ToString().Trim())))
                                {
                                    model.NgheNghiepTruocKhiVaoDang = nghenghieptruockhivaodang.ToString().Trim();
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((ngayvaodang != null) && (!string.IsNullOrEmpty(ngayvaodang.ToString().Trim())))
                                {
                                    try
                                    {
                                        model.NgayVaoDang = DateTime.ParseExact(ngayvaodang.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                        isValidate = false;
                                    }
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((ngayvaodangchinhthuc != null) && (!string.IsNullOrEmpty(ngayvaodangchinhthuc.ToString().Trim())))
                                {
                                    try
                                    {
                                        model.NgayVaoDangChinhThuc = DateTime.ParseExact(ngayvaodangchinhthuc.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                        isValidate = false;
                                    }
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((noiketnapdang != null) && (!string.IsNullOrEmpty(noiketnapdang.ToString().Trim())))
                                {
                                    model.NoiKetNapDang = noiketnapdang.ToString().Trim();
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((noicongnhan != null) && (!string.IsNullOrEmpty(noicongnhan.ToString().Trim())))
                                {
                                    model.NoiCongNhan = noicongnhan.ToString().Trim();
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((solylich != null) && (!string.IsNullOrEmpty(solylich.ToString().Trim())))
                                {
                                    model.SoLiLich = solylich.ToString().Trim();
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((sothedangvien != null) && (!string.IsNullOrEmpty(sothedangvien.ToString().Trim())))
                                {
                                    model.SoTheDangVien = sothedangvien.ToString().Trim();
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((ngaychuyenden != null) && (!string.IsNullOrEmpty(ngaychuyenden.ToString().Trim())))
                                {
                                    try
                                    {
                                        model.NgayChuyenDen = DateTime.ParseExact(ngaychuyenden.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((noichuyenden != null) && (!string.IsNullOrEmpty(noichuyenden.ToString().Trim())))
                                {
                                    model.NoiChuyenDen = noichuyenden.ToString().Trim();
                                }
                                if ((ngaychuyendi != null) && (!string.IsNullOrEmpty(ngaychuyendi.ToString().Trim())))
                                {
                                    try
                                    {
                                        model.NgayChuyenDi = DateTime.ParseExact(ngaychuyendi.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((noichuyendi != null) && (!string.IsNullOrEmpty(noichuyendi.ToString().Trim())))
                                {
                                    model.NoiChuyenDi = noichuyendi.ToString().Trim();
                                }
                                if ((ngaybichet != null) && (!string.IsNullOrEmpty(ngaybichet.ToString().Trim())))
                                {
                                    try
                                    {
                                        model.NgayBiChet = DateTime.ParseExact(ngaybichet.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((lydochet != null) && (!string.IsNullOrEmpty(lydochet.ToString().Trim())))
                                {
                                    model.LyDoChet = lydochet.ToString().Trim();
                                }
                                if ((ngayrakhoidang != null) && (!string.IsNullOrEmpty(ngayrakhoidang.ToString().Trim())))
                                {
                                    try
                                    {
                                        model.NgayRaKhoiDang = DateTime.ParseExact(ngayrakhoidang.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if ((hinhthucrakhoidang != null) && (!string.IsNullOrEmpty(hinhthucrakhoidang.ToString().Trim())))
                                {
                                    model.HinhThucRaKhoiDang = hinhthucrakhoidang.ToString().Trim();
                                }
                                if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                {
                                    model.GhiChu = ghichu.ToString().Trim();
                                }

                                #endregion

                                if (isValidate)
                                {
                                    var result = await _partyHandler.Update(model.StaffPartyID, model);
                                    if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                    {
                                        success = success + 1;
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                                else
                                {
                                    fail = fail + 1;
                                }
                            }
                            else
                            {
                                #region Create model - Thong tin Dang

                                var model = new StaffPartyCreateModel();

                                model.StaffID = staffId;
                                if (fromDate != null)
                                {
                                    model.TuNgay = fromDate;
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                model.DenNgay = toDate;
                                if ((chibo != null) && (!string.IsNullOrEmpty(chibo.ToString().Trim())))
                                {
                                    model.ChiBoID = lstChiBo.Where(x => (x.PartyCellName.Trim() == chibo.ToString().Trim())).FirstOrDefault()?.PartyCellID;
                                    if (model.ChiBoID == null)
                                    {
                                        isValidate = false;
                                    }
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((chucvudang != null) && (!string.IsNullOrEmpty(chucvudang.ToString().Trim())))
                                {
                                    model.ChucVuDangID = lstChucVuDang.Where(x => (x.PartyTitleName.Trim() == chucvudang.ToString().Trim())).FirstOrDefault()?.PartyTitleID;
                                    if (model.ChucVuDangID == null)
                                    {
                                        isValidate = false;
                                    }
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((lyluanchinhtri != null) && (!string.IsNullOrEmpty(lyluanchinhtri.ToString().Trim())))
                                {
                                    switch (lyluanchinhtri.ToString().Trim())
                                    {
                                        case Constant.LyLuanChinhTri_ThongTinDang.SO_CAP:
                                            model.LyLuanChinhTri = Constant.LyLuanChinhTri_ThongTinDang.SO_CAP;
                                            break;
                                        case Constant.LyLuanChinhTri_ThongTinDang.TRUNG_CAP:
                                            model.LyLuanChinhTri = Constant.LyLuanChinhTri_ThongTinDang.TRUNG_CAP;
                                            break;
                                        case Constant.LyLuanChinhTri_ThongTinDang.CAO_CAP:
                                            model.LyLuanChinhTri = Constant.LyLuanChinhTri_ThongTinDang.CAO_CAP;
                                            break;
                                        case Constant.LyLuanChinhTri_ThongTinDang.CU_NHAN:
                                            model.LyLuanChinhTri = Constant.LyLuanChinhTri_ThongTinDang.CU_NHAN;
                                            break;
                                    }
                                }
                                if ((nghenghieptruockhivaodang != null) && (!string.IsNullOrEmpty(nghenghieptruockhivaodang.ToString().Trim())))
                                {
                                    model.NgheNghiepTruocKhiVaoDang = nghenghieptruockhivaodang.ToString().Trim();
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((ngayvaodang != null) && (!string.IsNullOrEmpty(ngayvaodang.ToString().Trim())))
                                {
                                    try
                                    {
                                        model.NgayVaoDang = DateTime.ParseExact(ngayvaodang.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                        isValidate = false;
                                    }
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((ngayvaodangchinhthuc != null) && (!string.IsNullOrEmpty(ngayvaodangchinhthuc.ToString().Trim())))
                                {
                                    try
                                    {
                                        model.NgayVaoDangChinhThuc = DateTime.ParseExact(ngayvaodangchinhthuc.ToString().Trim(), Constant.DateTimeFormat.DDMMYYYY, CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                        isValidate = false;
                                    }
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((noiketnapdang != null) && (!string.IsNullOrEmpty(noiketnapdang.ToString().Trim())))
                                {
                                    model.NoiKetNapDang = noiketnapdang.ToString().Trim();
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((noicongnhan != null) && (!string.IsNullOrEmpty(noicongnhan.ToString().Trim())))
                                {
                                    model.NoiCongNhan = noicongnhan.ToString().Trim();
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((solylich != null) && (!string.IsNullOrEmpty(solylich.ToString().Trim())))
                                {
                                    model.SoLiLich = solylich.ToString().Trim();
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((sothedangvien != null) && (!string.IsNullOrEmpty(sothedangvien.ToString().Trim())))
                                {
                                    model.SoTheDangVien = sothedangvien.ToString().Trim();
                                }
                                else
                                {
                                    isValidate = false;
                                }
                                if ((ghichu != null) && (!string.IsNullOrEmpty(ghichu.ToString().Trim())))
                                {
                                    model.GhiChu = ghichu.ToString().Trim();
                                }

                                #endregion

                                if (isValidate)
                                {
                                    var result = await _partyHandler.Create(model);
                                    if (result.Status == Constant.ErrorCode.SUCCESS_CODE)
                                    {
                                        success = success + 1;
                                    }
                                    else
                                    {
                                        fail = fail + 1;
                                    }
                                }
                                else
                                {
                                    fail = fail + 1;
                                }
                            }
                        }
                    }
                    curRow = curRow + 1;
                }
                // Write log
                _baseHandler.CreateLog(new SystemLogModel
                {
                    ApplicationId = userModel.ApplicationId,
                    UserId = userModel.UserId,
                    ActionByUser = LogAction.IMPORT,
                    Date = DateTime.Now,
                    Content = $"Import thông tin Đảng: Thành công: " + success + " bản ghi. Lỗi: " + fail + " bản ghi"
                });
                return "Import thành công: " + success + " bản ghi. Import lỗi: " + fail + " bản ghi";
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        /// <summary>
        /// Import thông tin khác
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public async Task<string> ImportThongTinKhac(ISheet sheet)
        {
            try
            {
                if (sheet.LastRowNum < 2)
                {
                    return "Không đúng cấu trúc file";
                }
                var curRow = 2;
                while (curRow < sheet.LastRowNum)
                {
                    var maNV = sheet.GetRow(curRow).GetCell(1);
                    if (maNV != null)
                    {

                    }
                    curRow = curRow + 1;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return "Có lỗi khi thực hiện import dữ liệu";
            }
        }

        #endregion
    }
}
