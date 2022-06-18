using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System.Linq;
using NLog;

namespace SV.HRM.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class BaseController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IStaffHttpService _StaffService;
        private readonly IBaseHttpService _baseService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(IStaffHttpService StaffService, IBaseHttpService baseService, IHttpContextAccessor httpContextAccessor)
        {
            _StaffService = StaffService;
            _baseService = baseService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Hàm lấy về combobox
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/Staff")]
        [HttpGet]
        public async Task<Response<List<StaffComboboxModel>>> GetCombobox(string q, int page)
        {
            return await _baseService.GetCombobox<StaffComboboxModel>(Constant.TableInfo.Staff.TABLE_NAME, Constant.TableInfo.Staff.FIELD_NAME_SEARCH, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox vị trí chức danh
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/JobTitle")]
        [HttpGet]
        public async Task<Response<List<JobTitleComboboxModel>>> GetComboboxJobTitle(string q, int page)
        {
            return await _baseService.GetCombobox<JobTitleComboboxModel>(Constant.TableInfo.JobTitle.TABLE_NAME, Constant.TableInfo.JobTitle.FIELD_NAME, q, page);
        }

        [Route("Dictionary/TACategory")]
        [HttpGet]
        public async Task<Response<List<TACategoryModel>>> GetComboboxTACategory(string q, int page)
        {
            return await _baseService.GetCombobox<TACategoryModel>(nameof(TACategory), nameof(TACategory.TACategoryName), q, page);
        }

        /// <summary>
        /// Lấy danh sách danh xưng
        /// </summary>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/Title")]
        [HttpGet]
        public async Task<Response<List<TitleComboboxModel>>> GetComboboxTitle(string q, int page)
        {
            return await _baseService.GetCombobox<TitleComboboxModel>(Constant.TableInfo.Title.TABLE_NAME, Constant.TableInfo.Title.FIELD_NAME, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox loại chức danh
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/Category")]
        [HttpGet]
        public async Task<Response<List<CategoryComboboxModel>>> GetComboboxCategory(string q, int page)
        {
            return await _baseService.GetCombobox<CategoryComboboxModel>(Constant.TableInfo.Category.TABLE_NAME, Constant.TableInfo.Category.FIELD_NAME, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox nhóm làm việc
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/WorkGroup")]
        [HttpGet]
        public async Task<Response<List<WorkGroupComboboxModel>>> GetComboboxWorkGroup(string q, int page)
        {
            return await _baseService.GetCombobox<WorkGroupComboboxModel>(Constant.TableInfo.WorkGroup.TABLE_NAME, Constant.TableInfo.WorkGroup.FIELD_NAME, q, page);
        }

        /// <summary>
        /// Lấy danh sách trình độ chuyên môn
        /// </summary>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/Qualification")]
        [HttpGet]
        public async Task<Response<List<QualificationComboboxModel>>> GetComboboxQualification(string q, int page)
        {
            return await _baseService.GetCombobox<QualificationComboboxModel>(Constant.TableInfo.Qualification.TABLE_NAME, Constant.TableInfo.Qualification.FIELD_NAME, q, page);
        }

        /// <summary>
        /// Lấy danh sách học vị
        /// </summary>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/Degree")]
        [HttpGet]
        public async Task<Response<List<DegreeComboboxModel>>> GetComboboxDegree(string q, int page)
        {
            return await _baseService.GetCombobox<DegreeComboboxModel>(Constant.TableInfo.Degree.TABLE_NAME, Constant.TableInfo.Degree.FIELD_NAME, q, page);
        }

        /// <summary>
        /// Lấy danh sách học hàm
        /// </summary>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/AcademicRank")]
        [HttpGet]
        public async Task<Response<List<AcademicRankComboboxModel>>> GetComboboxAcademicRank(string q, int page)
        {
            return await _baseService.GetCombobox<AcademicRankComboboxModel>(Constant.TableInfo.AcademicRank.TABLE_NAME, Constant.TableInfo.AcademicRank.FIELD_NAME, q, page);
        }

        /// <summary>
        /// Lấy danh sách nhóm nhân viên
        /// </summary>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/StaffGroup")]
        [HttpGet]
        public async Task<Response<List<StaffGroupComboboxModel>>> GetComboboxStaffGroup(string q, int page)
        {
            return await _baseService.GetCombobox<StaffGroupComboboxModel>(Constant.TableInfo.StaffGroup.TABLE_NAME, Constant.TableInfo.StaffGroup.FIELD_NAME, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox chức danh
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/Position")]
        [HttpGet]
        public async Task<Response<List<PositionComboboxModel>>> GetComboboxPosition(string q, int page)
        {
            return await _baseService.GetCombobox<PositionComboboxModel>(Constant.TableInfo.Position.TABLE_NAME, Constant.TableInfo.Position.FIELD_NAME, q, page);
        }
        /// <summary>
        /// lấy combobox tôn giáo
        /// </summary>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/Religion")]
        [HttpGet]
        public async Task<Response<List<ReligionModel>>> GetComboboxReligion(string q, int page)
        {
            return await _baseService.GetCombobox<ReligionModel>(nameof(Religion), nameof(Religion.ReligionName), q, page);
        }

        /// <summary>
        /// Lấy danh sách combobox loại đồng phục
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet, Route("Dictionary/UniformItem")]
        public async Task<Response<List<UniformItemComboboxModel>>> GetComboboxUniformItem(string q, int page)
        {
            return await _baseService.GetCombobox<UniformItemComboboxModel>(Constant.TableInfo.UniformItem.TABLE_NAME, Constant.TableInfo.UniformItem.FIELD_NAME, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox đơn vị tính của tiền tệ
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/Currency")]
        [HttpGet]
        public async Task<Response<List<CurrencyComboboxModel>>> GetComboboxCurrency(string q, int page)
        {
            return await _baseService.GetCombobox<CurrencyComboboxModel>(Constant.TableInfo.Currency.TABLE_NAME, Constant.TableInfo.Currency.FIELD_NAME, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox nhóm ngạch lương
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/NhomNgachLuong")]
        [HttpGet]
        public async Task<Response<List<NhomNgachLuongModel>>> GetComboboxNhomNgachLuong(string q, int page)
        {
            return await _baseService.GetCombobox<NhomNgachLuongModel>(Constant.TableInfo.NhomNgachLuong.TABLE_NAME, Constant.TableInfo.NhomNgachLuong.FIELD_NAME, q, page);
        }
        /// <summary>
        /// Hàm lấy về toàn bộ combobox ngạch lương
        /// thêm ngày 7/3/2022 diennv
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/NgachLuong")]
        [HttpGet]
        public async Task<Response<List<NgachLuongModel>>> GetComboboxNgachLuong(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseService.GetCombobox<NgachLuongModel>(nameof(NgachLuong), nameof(NgachLuong.TenNgachLuong), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox ngạch lương theo id nhóm ngạch lương
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/NgachLuong/{idRelation}")]
        [HttpPost]
        public async Task<Response<List<NgachLuongModel>>> GetComboboxNgachLuong([FromRoute] int idRelation)
        {
            return await _baseService.GetComboboxByField<NgachLuongModel>(Constant.TableInfo.NgachLuong.TABLE_NAME, Constant.TableInfo.NgachLuong.FIELD_NAME, idRelation);
        }

        /// <summary>
        /// Hàm lấy về combobox bậc lương theo id  ngạch lương
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/BacLuong/{idRelation}")]
        [HttpPost]
        public async Task<Response<List<BacLuongModel>>> GetComboboxBacLuong([FromRoute] int idRelation)
        {
            return await _baseService.GetComboboxByField<BacLuongModel>(Constant.TableInfo.BacLuong.TABLE_NAME, Constant.TableInfo.BacLuong.FIELD_NAME, idRelation);
        }

        /// <summary>
        /// Hàm lấy về combobox hệ số lương theo bậc lương
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/HeSoLuong/{idRelation}")]
        [HttpPost]
        public async Task<Response<List<BacLuongModel>>> GetComboboxHeSoLuong([FromRoute] int idRelation)
        {
            return await _baseService.GetComboboxByField<BacLuongModel>(Constant.TableInfo.BacLuong.TABLE_NAME, $"{Constant.TableInfo.BacLuong.TABLE_NAME}ID", idRelation);
        }

        /// <summary>
        /// Hàm lấy về combobox đơn vị đào tạo(bảng School)
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/School")]
        [HttpGet]
        public async Task<Response<List<SchoolModel>>> GetComboboxSchool(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseService.GetCombobox<SchoolModel>(nameof(School), nameof(School.SchoolName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox hệ đào tạo(bảng Speciality)
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/Speciality")]
        [HttpGet]
        public async Task<Response<List<SpecialityModel>>> GetComboboxSpeciality(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseService.GetCombobox<SpecialityModel>(nameof(Speciality), nameof(Speciality.SpecialityName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox trình độ chuyên môn
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/TrinhDoChuyenMon")]
        [HttpGet]
        public async Task<Response<List<TrinhDoChuyenMonModel>>> GetComboboxTrinhDoChuyenMon(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseService.GetCombobox<TrinhDoChuyenMonModel>(nameof(TrinhDoChuyenMon), nameof(TrinhDoChuyenMon.TrinhDoChuyenMonName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox trình độ đào tạo
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/TrinhDoDaoTao")]
        [HttpGet]
        public async Task<Response<List<TrinhDoDaoTaoModel>>> GetComboboxTrinhDoDaoTao(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseService.GetCombobox<TrinhDoDaoTaoModel>(nameof(TrinhDoDaoTao), nameof(TrinhDoDaoTao.TrinhDoDaoTaoName), q, page);
        }
        /// <summary>
        /// Hàm lấy về combobox loại quyết định
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/TrinhDoDaoTaoPost")]
        [HttpPost]
        public async Task<Response<List<TrinhDoDaoTaoModel>>> GetComboboxTrinhDoDaoTaoPost(string q, int page)
        {
            return await _baseService.GetComboboxByField<TrinhDoDaoTaoModel>(nameof(TrinhDoDaoTao)+"Post", nameof(TrinhDoDaoTao.TrinhDoDaoTaoName), 1);
        }

        ///// <summary>
        ///// Hàm lấy về combobox trình độ đào tạo
        ///// </summary>
        ///// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        ///// <param name="q"></param>
        ///// <param name="page"></param>
        ///// <returns></returns>
        //[Route("Dictionary/TrinhDoDT")]
        //[HttpGet]
        //public async Task<Response<List<TrinhDoDTModel>>> GetComboboxTrinhDoDT(string layoutCode, string keySearch, string q, int page)
        //{
        //    return await _baseService.GetCombobox<TrinhDoDTModel>(nameof(TrinhDoDT), nameof(TrinhDoDT.TrinhDoDTName), q, page);
        //}

        /// <summary>
        /// Hàm lấy về combobox chuyên ngành
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/TrinhDoDT")]
        [HttpGet]
        public async Task<Response<List<TrinhDoDTModel>>> GetComboboxTrinhDoDT(string q, int page)
        {
            return await _baseService.GetCombobox<TrinhDoDTModel>(nameof(TrinhDoDT), nameof(TrinhDoDT.TrinhDoDTName), q, page);
        }
        /// <summary>
        /// Hàm lấy về combobox loại quyết định
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/TrinhDoDTPost")]
        [HttpPost]
        public async Task<Response<List<TrinhDoDTModel>>> GetComboboxTrinhDoDTPost(string q, int page)
        {
            return await _baseService.GetComboboxByField<TrinhDoDTModel>(nameof(TrinhDoDT) + "Post", nameof(TrinhDoDT.TrinhDoDTName), 1);
        }
        /// <summary>
        /// Hàm lấy về combobox loại bằng cấp(bảng Diploma)
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/Diploma")]
        [HttpGet]
        //[HttpPost]
        public async Task<Response<List<DiplomaModel>>> GetComboboxDiploma(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseService.GetCombobox<DiplomaModel>(nameof(Diploma), nameof(Diploma.DiplomaName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox loại hợp đồng
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/ContractType")]
        [HttpGet]
        public async Task<Response<List<ContractTypeModel>>> GetComboboxContractType(string q, int page)
        {
            return await _baseService.GetCombobox<ContractTypeModel>(nameof(ContractType), nameof(ContractType.ContractTypeName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox chi nhánh
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/Branch")]
        [HttpGet]
        public async Task<Response<List<BranchModel>>> GetComboboxBranch(string q, int page)
        {
            return await _baseService.GetCombobox<BranchModel>(nameof(Branch), nameof(Branch.BranchName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox quan hệ gia đình
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/FamilyRelationShip")]
        [HttpGet]
        public async Task<Response<List<FamilyRelationShipModel>>> GetComboboxFamilyRelationShip(string q, int page)
        {
            return await _baseService.GetCombobox<FamilyRelationShipModel>(nameof(FamilyRelationShip), nameof(FamilyRelationShip.FamilyRelationshipName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox thông tin chi bộ
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/PartyCell")]
        [HttpGet]
        public async Task<Response<List<PartyCellBase>>> GetComboboxPartyCell(string q, int page)
        {
            return await _baseService.GetCombobox<PartyCellBase>(nameof(PartyCell), nameof(PartyCell.PartyCellName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox thông tin chức vụ đảng
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/PartyTitle")]
        [HttpGet]
        public async Task<Response<List<PartyTitleBase>>> GetComboboxPartyTitle(string q, int page)
        {
            return await _baseService.GetCombobox<PartyTitleBase>(nameof(PartyTitle), nameof(PartyTitle.PartyTitleName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox hình thức kỷ luật
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/DisciplineType")]
        [HttpGet]
        public async Task<Response<List<DisciplineType>>> GetComboboxDisciplineType(string q, int page)
        {
            return await _baseService.GetCombobox<DisciplineType>(nameof(DisciplineType), nameof(DisciplineType.Description1), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox mức độ vi phạm
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/Period")]
        [HttpGet]
        public async Task<Response<List<MucDoViPhamKyLuatBase>>> GetComboboxPeriod(string q, int page)
        {
            return await _baseService.GetCombobox<MucDoViPhamKyLuatBase>(nameof(MucDoViPhamKyLuat), nameof(MucDoViPhamKyLuat.MucDoViPhamKyLuatName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox loại quyết định
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/DecisionItem")]
        [HttpGet]
        public async Task<Response<List<DecisionItemBase>>> GetComboboxDecisionItem(string q, int page)
        {
            return await _baseService.GetCombobox<DecisionItemBase>(nameof(DecisionItem), nameof(DecisionItem.DecisionItemName), q, page);
        }
        /// <summary>
        /// Hàm lấy về combobox Nhóm kiểu nghỉ
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/LeaveType")]
        [HttpGet]
        public async Task<Response<List<LeaveTypeModel>>> GetComboboxLeaveType(string q, int page)
        {
            return await _baseService.GetCombobox<LeaveTypeModel>(nameof(LeaveType), nameof(LeaveType.LeaveTypeName), q, page);
        }
        /// <summary>
        /// Hàm lấy về combobox kiểu nghỉ
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/Leave")]
        [HttpGet]
        public async Task<Response<List<LeaveModel>>> GetComboboxLeave(string q, int page)
        {
            return await _baseService.GetCombobox<LeaveModel>(nameof(Leave), nameof(Leave.LeaveName), q, page);
        }
        /// <summary>
        /// Check trùng theo trường nào đó
        /// </summary>
        /// <returns></returns>
        [Route("CheckDuplicate")]
        [HttpPost]
        public async Task<Response<bool>> CheckDuplicate(BaseCheckDuplicate model)
        {
            return await _baseService.CheckDuplicate(model);
        }

        /// <summary>
        /// Hàm lấy về combobox kỳ khám
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/HealthPeriod")]
        [HttpGet]
        public async Task<Response<List<HealthPeriodBase>>> GetComboboxHealthPeriod(string q, int page)
        {
            return await _baseService.GetCombobox<HealthPeriodBase>(nameof(HealthPeriod), nameof(HealthPeriod.HealthPeriodCode), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox danh hiệu khen thưởng
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/Reward")]
        [HttpGet]
        public async Task<Response<List<RewardBase>>> GetComboboxReward(string q, int page)
        {
            return await _baseService.GetCombobox<RewardBase>(nameof(Reward), nameof(Reward.RewardName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox công ty
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/Company")]
        [HttpGet]
        public async Task<Response<List<CompanyBase>>> GetComboboxCompany(string q, int page)
        {
            return await _baseService.GetCombobox<CompanyBase>(nameof(Company), nameof(Company.CompanyName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox loại chức danh
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/ChucVuChinhQuyen")]
        [HttpGet]
        public async Task<Response<List<ChucVuChinhQuyenComboboxModel>>> GetComboboxChucVuChinhQuyen(string q, int page)
        {
            return await _baseService.GetCombobox<ChucVuChinhQuyenComboboxModel>(Constant.TableInfo.ChucVuChinhQuyen.TABLE_NAME, Constant.TableInfo.ChucVuChinhQuyen.FIELD_NAME, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox loại chức danh
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/ChucVuKiemNhiem")]
        [HttpGet]
        public async Task<Response<List<ChucVuKiemNhiemComboboxModel>>> GetComboboxChucVuKiemNhiem(string q, int page)
        {
            return await _baseService.GetCombobox<ChucVuKiemNhiemComboboxModel>(Constant.TableInfo.ChucVuKiemNhiem.TABLE_NAME, Constant.TableInfo.ChucVuKiemNhiem.FIELD_NAME, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox loại quyết định
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/GetAllDecisionItem")]
        [HttpPost]
        public async Task<Response<List<DecisionItemBase>>> GetComboboxAllDecisionItem(string q, int page)
        {
            return await _baseService.GetComboboxByField<DecisionItemBase>(nameof(DecisionItem), "1", 1);
        }

        /// <summary>
        /// Hàm lấy về combobox loại quyết định
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("RenderDynamicColumn/{layoutCode}/{userID}")]
        [HttpPost]
        public async Task<Response<List<TableFieldModel>>> RenderDynamicColumn([FromRoute] string layoutCode, [FromRoute] int? userID)
        {
            return await _baseService.RenderDynamicColumn(layoutCode, userID);
        }

        /// <summary>
        /// Hàm lấy về combobox loại quyết định
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("GetDefaultColumn/{layoutCode}")]
        [HttpPost]
        public async Task<Response<List<TableField>>> GetDefaultColumn([FromRoute] string layoutCode)
        {
            return await _baseService.GetDefaultColumn(layoutCode);
        }

        [Route("ImportExcel")]
        [HttpPost]
        public async Task<Response<object>> ImportExcel(IFormFile file)
        {
            if (file.Length > 0)
            {
                var stopWatch = new Stopwatch();

                stopWatch.Start();
                DataTable getDataTable = new DataTable();
                string strMsg;

                //Use the OpenReadStream() method in IFormFile to directly read the file stream
                getDataTable = ExcelToDatatable(file.OpenReadStream(), Path.GetExtension(file.FileName), out strMsg);
                if (getDataTable.Rows.Count <= 0)
                {
                    return new Response<dynamic>(1, null, new object() { });
                }
                var userInfoList = new List<Dictionary<string, object>>();
                for (int i = 0; i < getDataTable.Rows.Count; i++)
                {
                    //xử lý thêm dữ liệu
                    var userInfo = new Dictionary<string, object>();
                    userInfo["a"] = getDataTable.Rows[i][0];

                    userInfoList.Add(userInfo);
                }

                stopWatch.Stop();
                var a = JsonConvert.SerializeObject(userInfoList);
                var b = JsonConvert.DeserializeObject(a);
                return new Response<object>(1, null, b);
            }
            return new Response<object>(1, null, new object() { });

        }


        public static DataTable ExcelToDatatable(Stream stream, string fileType, out string resultMsg, string sheetName = null)
        {
            resultMsg = "Excel File stream successfully converted to DataTable data source";
            var excelToDataTable = new DataTable();

            try
            {
                //Workbook Object represents a workbook,Define one first Excel Workbook
                IWorkbook workbook;

                //XSSFWorkbook apply XLSX Format, HSSFWorkbook apply XLS format
                #region judge Excel Edition
                switch (fileType)
                {
                    //.XLSX Is Version 07(Or above 07)Of Office Excel
                    case ".xlsx":
                        workbook = new XSSFWorkbook(stream);
                        break;
                    //.XLS It's version 03 Office Excel
                    case ".xls":
                        workbook = new HSSFWorkbook(stream);
                        break;
                    default:
                        throw new Exception("Excel Document format error");
                }
                #endregion

                var sheet = workbook.GetSheetAt(0);
                var rows = sheet.GetRowEnumerator();

                var headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;//Number of last rows and columns (that is, total columns)

                //Get the first row header column data source,Convert to dataTable Table header name for data source
                for (var j = 0; j < cellCount; j++)
                {
                    var cell = headerRow.GetCell(j);
                    excelToDataTable.Columns.Add(cell.ToString());
                }

                //Obtain Excel All data sources in the table except for the heading are converted to dataTable Table data source in
                for (var i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    var dataRow = excelToDataTable.NewRow();

                    var row = sheet.GetRow(i);

                    if (row == null) continue; //Rows without data default to null　

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)//Cell content non-empty validation
                        {
                            #region NPOI Obtain Excel Different types of data in cells
                            //Gets the specified cell information
                            var cell = row.GetCell(j);
                            switch (cell.CellType)
                            {
                                //First in NPOI Medium number and date belong to Numeric type
                                //adopt NPOI Mid-shipped DateUtil.IsCellDateFormatted Determine whether it is a time-date type
                                case CellType.Numeric when DateUtil.IsCellDateFormatted(cell):
                                    dataRow[j] = cell.DateCellValue;
                                    break;
                                case CellType.Numeric:
                                    //Other Number Types
                                    dataRow[j] = cell.NumericCellValue;
                                    break;
                                //Empty data type
                                case CellType.Blank:
                                    dataRow[j] = "";
                                    break;
                                //Formula type
                                case CellType.Formula:
                                    {
                                        HSSFFormulaEvaluator eva = new HSSFFormulaEvaluator(workbook);
                                        dataRow[j] = eva.Evaluate(cell).StringValue;
                                        break;
                                    }
                                //Boolean type
                                case CellType.Boolean:
                                    dataRow[j] = row.GetCell(j).BooleanCellValue;
                                    break;
                                //error
                                //case CellType.Error:
                                //    dataRow[j] = HSSFErrorConstants.GetText(row.GetCell(j).ErrorCellValue);
                                //    break;
                                //Other types are handled by string type (unknown type) CellType.Unknown，String type CellType.String)
                                default:
                                    dataRow[j] = cell.StringCellValue;
                                    break;
                            }
                            #endregion
                        }
                    }
                    excelToDataTable.Rows.Add(dataRow);
                }

                //isSuccess = true;
            }
            catch (Exception e)
            {
                resultMsg = e.Message;
            }

            return excelToDataTable;

        }
        [Route("ExportExcel")]
        [HttpPost]
        public async Task<Response<IActionResult>> ExportExcel([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                List<Dictionary<string, object>> persons = new List<Dictionary<string, object>>();
                var resultStaff = await _StaffService.ReportStaff(queryFilter);
                if (resultStaff != null)
                {
                    persons = resultStaff.Data;
                }
                // Lets converts our object data to Datatable for a simplified logic.
                // Datatable is most easy way to deal with complex datatypes for easy reading and formatting.

                //DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(persons), (typeof(DataTable)));

                //Create a blank workbook
                IWorkbook workbook = new XSSFWorkbook(); // create *.xlsx file, use HSSFWorkbook() for creating *.xls file.
                ISheet excelSheet = workbook.CreateSheet("Bao_Cao_Nhan_Vien");
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
                    //var cra = new CellRangeAddress(0, 0, 0, 1);
                    //excelSheet.AddMergedRegion(cra);

                    //ICell cell = excelSheet.GetRow(0).GetCell(0);
                    //cell.SetCellType(CellType.String);
                    //cell.SetCellValue("Supplier Provided Data");
                    //cell.CellStyle = cellStyleBorderAndColorGreen;
                    //excelSheet.GetRow(0).GetCell(1).CellStyle = cellStyleBorderAndColorGreen;

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
                    if (persons.Count() > 0)
                    {
                        foreach (var dic in persons)
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
                    //            if (dsrow[col.field].GetType() == typeof(DateTime))
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

                    // diennv set width for column

                    for (int i = 0; i < columns.Count(); i++)
                    {
                        //if (i == 0)
                        //{
                        //    excelSheet.SetColumnWidth(i, 15 * 256);
                        //}
                        //else
                        //{
                            excelSheet.AutoSizeColumn(i,true);

                        //}
                        GC.Collect();
                    }

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
                        FileDownloadName = "Báo cáo nhân viên.xlsx"
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

        /// <summary>
        /// Hàm lấy về combobox chuyên khoa
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/ChuyenKhoa")]
        [HttpGet]
        public async Task<Response<List<ChuyenKhoaModel>>> GetComboboxChuyenKhoa(string q, int page)
        {
            return await _baseService.GetCombobox<ChuyenKhoaModel>(nameof(ChuyenKhoa), nameof(ChuyenKhoa.TenChuyenKhoa), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox trình độ chuyên môn (phần bằng cấp/chứng chỉ) theo nghiệp vụ bệnh viện đức giang
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/TrinhDoChuyenMon/{idRelation}")]
        [HttpPost]
        public async Task<Response<List<TrinhDoChuyenMonModel>>> GetComboboxTrinhDoChuyenMonPost([FromRoute] int idRelation)
        {
            return await _baseService.GetComboboxByField<TrinhDoChuyenMonModel>(nameof(TrinhDoChuyenMon), nameof(TrinhDoChuyenMon.DiplomaID), idRelation);
        }

        /// <summary>
        /// Hàm lấy về combobox trình độ đào tạo (phần bằng cấp/chứng chỉ) theo nghiệp vụ bệnh viện đức giang
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/TrinhDoDaoTao/{idRelation}")]
        [HttpPost]
        public async Task<Response<List<TrinhDoDaoTaoModel>>> GetComboboxTrinhDoDaoTaoPost([FromRoute] int idRelation)
        {
            return await _baseService.GetComboboxByField<TrinhDoDaoTaoModel>(nameof(TrinhDoDaoTao), nameof(TrinhDoDaoTao.DiplomaID), idRelation);
        }

        /// <summary>
        /// Hàm lấy về combobox bảng nghề
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/Occupation")]
        [HttpPost]
        public async Task<Response<List<Occupation>>> GetComboboxOccupation()
        {
            return await _baseService.GetComboboxByField<Occupation>(nameof(Occupation), "1", 1);
        }

        /// <summary>
        /// Hàm lấy về combobox đơn vị đào tạo(Bảng DonViDaoTao)
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/DonViDaoTao")]
        [HttpPost]
        public async Task<Response<List<DonViDaoTao>>> GetComboboxDonViDaoTao()
        {
            return await _baseService.GetComboboxByField<DonViDaoTao>(nameof(DonViDaoTao), "1", 1);
        }
        /// <summary>
        /// Hàm lấy về toàn bộ combobox nghề
        /// thêm ngày 9/3/2022
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/Occupation")]
        [HttpGet]
        public async Task<Response<List<Occupation>>> GetComboboxOccupation(string q, int page)
        {
            return await _baseService.GetCombobox<Occupation>(nameof(Occupation), nameof(Occupation.OccupationCode), q, page);
        }


        /// <summary>
        /// Hàm update lại cột của TableField
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("UpdateTableField")]
        [HttpPost]
        public async Task<Response<bool>> UpdateTabelField(List<TableField> models)
        {
            return await _baseService.UpdateTableField(models);
        }

        /// <summary>
        /// Hàm lấy về combobox quốc tịch từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/Country")]
        [HttpPost]
        public async Task<Response<List<CountryBase>>> GetComboboxCountries()
        {
            return await _baseService.GetComboboxFromQTHT<CountryBase>("Countries", "1", 1);
        }

        /// <summary>
        /// Hàm lấy về combobox chuyên ngành
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/ChuyenNganh")]
        [HttpGet]
        public async Task<Response<List<ChuyenNganhModel>>> GetComboboxChuyenNganh(string q, int page)
        {
            return await _baseService.GetCombobox<ChuyenNganhModel>(nameof(ChuyenNganh), nameof(ChuyenNganh.ChuyenNganhName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox chuyên ngành đào tạo
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("Dictionary/ChuyenNganhDaoTao")]
        [HttpGet]
        public async Task<Response<List<ChuyenNganhDaoTaoModel>>> GetComboboxChuyenNganhDaoTao(string q, int page)
        {
            return await _baseService.GetCombobox<ChuyenNganhDaoTaoModel>(nameof(ChuyenNganhDaoTao), nameof(ChuyenNganhDaoTao.ChuyenNganhDaoTaoName), q, page);
        }

        /// <summary>
        /// Hàm lấy về layout theo user
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="userID"></param>
        /// <returns></returns>

        [Route("GetLayout/{layoutCode}/{userID}")]
        [HttpPost]
        public async Task<Response<object>> GetLayout([FromRoute] string layoutCode, [FromRoute] int? userID = null)
        {
            return await _baseService.GetLayout(layoutCode, userID);
        }

        /// <summary>
        /// Hàm update layout
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>

        [Route("UpdateLayout")]
        [HttpPost]
        public async Task<Response<bool>> BulkUpdateGroupBoxField(GroupBoxFieldUpdate models)
        {
            return await _baseService.BulkUpdateGroupBoxField(models);
        }

        /// <summary>
        /// Hàm lấy về combobox dân tộc từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/Ethnic")]
        [HttpPost]
        public async Task<Response<List<EthnicBase>>> GetComboboxEthnic()
        {
            return await _baseService.GetComboboxFromQTHT<EthnicBase>("Ethnics", "1", 1);
        }

        /// <summary>
        /// Hàm lấy về combobox Tỉnh/TP từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/Location")]
        [HttpPost]
        public async Task<Response<List<LocationBaseModel>>> GetComboboxLocation()
        {
            return await _baseService.GetComboboxFromQTHT<LocationBaseModel>("Locations", "1", 1);
        }

        /// <summary>
        /// Hàm lấy về combobox Quận/huyện từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/District/{locationId}")]
        [HttpPost]
        public async Task<Response<List<DistrictBase>>> GetComboboxDistricts([FromRoute] int? locationId)
        {
            return await _baseService.GetComboboxFromQTHT<DistrictBase>("Districts", nameof(District.LocationId), (int)locationId);
        }

        /// <summary>
        /// Hàm lấy về combobox Phường/xã từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/Ward/{districtId}")]
        [HttpPost]
        public async Task<Response<List<WardBase>>> GetComboboxWards([FromRoute] int? districtId)
        {
            return await _baseService.GetComboboxFromQTHT<WardBase>("Wards", nameof(Ward.DistrictId), (int)districtId);
        }


        /// <summary>
        /// Hàm lấy thông tin đơn vị hành chính đang chọn
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/Locale/{locationType}/{idLocation}")]
        [HttpPost]
        public async Task<Response<object>> GetLocation([FromRoute] string locationType, [FromRoute] int? idLocation)
        {
            return await _baseService.GetNameLocation<object>($"{locationType}s", $"{locationType}ID", (int)idLocation);
        }


        /// <summary>
        /// Hàm lấy về combobox bệnh viện từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/Hospital")]
        [HttpPost]
        public async Task<Response<List<HospitalBase>>> GetComboboxHospital()
        {
            return await _baseService.GetComboboxByField<HospitalBase>("Hospital", "1", 1);
        }

        /// <summary>
        /// Hàm lấy về combobox Chuyên ngành
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/ChuyenNganhDaoTao")]
        [HttpPost]
        public async Task<Response<List<ChuyenNganhDaoTaoModel>>> GetComboboxChuyenNganhDaoTao()
        {
            return await _baseService.GetComboboxByField<ChuyenNganhDaoTaoModel>(nameof(ChuyenNganh), "1", 1);
        }

        /// <summary>
        /// Hàm lấy về combobox Chuyên ngành
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch">Tên của cột dùng để tìm kiếm trên combobox</param>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("Dictionary/ChuyenNganh")]
        [HttpPost]
        public async Task<Response<List<ChuyenNganhModel>>> GetComboboxChuyenNganh()
        {
            return await _baseService.GetComboboxByField<ChuyenNganhModel>(nameof(ChuyenNganh), "1", 1);
        }
        /// <summary>
        /// hàm lấy về combobox công 
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/ShiftLeave")]
        [HttpGet]
        public async Task<Response<List<ShiftLeaveComboboxModel>>> GetComboboxShiftLeave(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseService.GetCombobox<ShiftLeaveComboboxModel>("ShiftLeave", "ShiftLeaveName", q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox quy trình
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/Workflow")]
        [HttpGet]
        public async Task<Response<List<WorkflowModel>>> GetComboboxWorkflow(string q, int page)
        {
            return await _baseService.GetCombobox<WorkflowModel>(nameof(Workflow), nameof(Workflow.WorkflowName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox bước quy trình
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/WorkflowCommand")]
        [HttpGet]
        public async Task<Response<List<WorkflowCommandModel>>> GetComboboxWorkflowCommand(string q, int page)
        {
            return await _baseService.GetCombobox<WorkflowCommandModel>(nameof(WorkflowCommand), nameof(WorkflowCommand.WorkflowCommandName), q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox trạng thái quy trình
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("Dictionary/WorkflowState")]
        [HttpGet]
        public async Task<Response<List<WorkflowStateModel>>> GetComboboxWorkflowState(string q, int page)
        {
            return await _baseService.GetCombobox<WorkflowStateModel>(nameof(WorkflowState), nameof(WorkflowState.WorkflowStateName), q, page);
        }
    }
}