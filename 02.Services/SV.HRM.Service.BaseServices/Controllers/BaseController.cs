using Microsoft.AspNetCore.Mvc;
using NLog;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SV.HRM.Service.BaseServices.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BaseController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBaseHandler _baseHandler;

        public BaseController(IBaseHandler baseHandler)
        {
            _baseHandler = baseHandler;
        }

        /// <summary>
        /// Hàm lấy về combobox
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffComboboxModel>>> GetComboboxStaff(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetComboboxStaff(layoutCode, keySearch, q, page, CommandType.StoredProcedure);
        }
        /// <summary>
        /// Hàm lấy combobox shiftleave
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<ShiftLeaveComboboxModel>>> GetComboboxShiftLeave(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<ShiftLeaveComboboxModel>(layoutCode, keySearch, q, page, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Hàm lấy về combobox vị trí chức danh
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        [HttpGet]
        public async Task<Response<List<JobTitleComboboxModel>>> GetComboboxJobTitle(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<JobTitleComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox phân nhóm
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        [HttpGet]
        public async Task<Response<List<TACategoryModel>>> GetComboboxTACategory(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<TACategoryModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Lấy danh xưng
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<TitleComboboxModel>>> GetComboboxTitle(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<TitleComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox loại chức danh
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        [HttpGet]
        public async Task<Response<List<CategoryComboboxModel>>> GetComboboxCategory(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<CategoryComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Lấy trình độ chuyên môn
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        [HttpGet]
        public async Task<Response<List<QualificationComboboxModel>>> GetComboboxQualification(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<QualificationComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Lấy combobox ca làm việc
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        //[HttpGet]
        //public async Task<Response<List<ShiftModel>>> GetComboboxShift(string layoutCode, string keySearch, string q, int page)
        //{
        //    return await _baseHandler.GetCombobox<ShiftModel>(layoutCode, keySearch, q, page);
        //}

        /// <summary>
        /// Hàm lấy về combobox nhóm làm việc
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        [HttpGet]
        public async Task<Response<List<WorkGroupComboboxModel>>> GetComboboxWorkGroup(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<WorkGroupComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Lấy học vị
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        [HttpGet]
        public async Task<Response<List<DegreeComboboxModel>>> GetComboboxDegree(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<DegreeComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Lấy học hàm
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<AcademicRankComboboxModel>>> GetComboboxAcademicRank(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<AcademicRankComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Lấy nhóm nhân viên
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<StaffGroupComboboxModel>>> GetComboboxStaffGroup(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<StaffGroupComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox chức danh
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<PositionComboboxModel>>> GetComboboxPosition(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<PositionComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox chức vụ chính quyền
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<ChucVuChinhQuyenComboboxModel>>> GetComboboxChucVuChinhQuyen(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<ChucVuChinhQuyenComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox chức vụ kiêm nhiệm
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<ChucVuKiemNhiemComboboxModel>>> GetComboboxChucVuKiemNhiem(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<ChucVuKiemNhiemComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox loại bằng cấp
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<DiplomaComboboxModel>>> GetComboboxDiploma(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<DiplomaComboboxModel>(layoutCode, keySearch, q, page, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Hàm lấy về combobox đơn vị tính của tiền tệ
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<CurrencyComboboxModel>>> GetComboboxCurrency(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<CurrencyComboboxModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox nhóm ngạch lương
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<NhomNgachLuongModel>>> GetComboboxNhomNgachLuong(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<NhomNgachLuongModel>(layoutCode, keySearch, q, page);
        }
        /// <summary>
        /// hàm lấy về combobox tôn giáo
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<ReligionModel>>> GetComboboxReligion(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<ReligionModel>(layoutCode, keySearch, q, page);
        }
        /// <summary>
        /// Hàm lấy về toàn bộ combobox ngạch lương
        /// thêm ngày 7/3/2022
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<NgachLuongModel>>> GetComboboxNgachLuong(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<NgachLuongModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox ngạch lương theo nhóm ngạch lương
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<NgachLuongModel>>> GetComboboxNgachLuong(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxByField<NgachLuongModel>(layoutCode, keySearch, q);
        }

        /// <summary>
        /// Hàm lấy về combobox bậc lương theo ngạch lương
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<BacLuongModel>>> GetComboboxBacLuong(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxByField<BacLuongModel>(layoutCode, keySearch, q);
        }

        /// <summary>
        /// Hàm lấy về combobox đơn vị đào tạo(bảng School)
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<SchoolModel>>> GetComboboxSchool(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<SchoolModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox đơn vị đào tạo(bảng DonViDaoTao)
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<DonViDaoTao>>> GetComboboxDonViDaoTao(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxByField<DonViDaoTao>(layoutCode, keySearch, q);
        }

        /// <summary>
        /// Hàm lấy về combobox hệ đào tạo(bảng Speciality)
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<SpecialityModel>>> GetComboboxSpeciality(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<SpecialityModel>(layoutCode, keySearch, q, page, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Hàm lấy về combobox trình độ chuyên môn
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<TrinhDoChuyenMonModel>>> GetComboboxTrinhDoChuyenMon(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<TrinhDoChuyenMonModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox trình độ đào tạo
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<TrinhDoDTModel>>> GetComboboxTrinhDoDT(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<TrinhDoDTModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox trình độ đào tạo
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<TrinhDoDaoTaoModel>>> GetComboboxTrinhDoDaoTao(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<TrinhDoDaoTaoModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox loại hợp đồng
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<ContractTypeModel>>> GetComboboxContractType(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<ContractTypeModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox chi nhánh
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<BranchModel>>> GetComboboxBranch(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<BranchModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox quan hệ gia đình
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<FamilyRelationShipModel>>> GetComboboxFamilyRelationShip(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<FamilyRelationShipModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox thông tin chi bộ
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<PartyCellBase>>> GetComboboxPartyCell(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<PartyCellBase>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox thông tin chức vụ đảng
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<PartyTitleBase>>> GetComboboxPartyTitle(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<PartyTitleBase>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox hình thức kỷ luật
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<DisciplineType>>> GetComboboxDisciplineType(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<DisciplineType>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox mức độ vi phạm kỷ luật
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<MucDoViPhamKyLuatBase>>> GetComboboxMucDoViPhamKyLuat(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<MucDoViPhamKyLuatBase>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về toàn bộ combobox nhóm kiểu nghỉ
        /// thêm ngày 8/3/2022
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<LeaveTypeModel>>> GetComboboxLeaveType(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<LeaveTypeModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về toàn bộ combobox kiểu nghỉ
        /// thêm ngày 24/05/2022
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<LeaveModel>>> GetComboboxLeave(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<LeaveModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox loại quyết định
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<DecisionItemBase>>> GetComboboxDecisionItem(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<DecisionItemBase>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm check duplicate field
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> CheckDuplicate(BaseCheckDuplicate model)
        {
            return await _baseHandler.CheckDuplicate(model);
        }
        /// <summary>
        /// Hàm lấy về combobox kỳ khám
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<HealthPeriodBase>>> GetComboboxHealthPeriod(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<HealthPeriodBase>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox danh hiệu khen thưởng
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<RewardBase>>> GetComboboxReward(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<RewardBase>(layoutCode, keySearch, q, page);
        }


        /// <summary>
        /// Hàm lấy về combobox công ty
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<CompanyBase>>> GetComboboxCompany(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<CompanyBase>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Lấy danh mục loại đồng phục
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<UniformItemComboboxModel>>> GetComboboxUniformItem(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<UniformItemComboboxModel>(layoutCode, keySearch, q, page);
        }
        /// <summary>
        /// Hàm lấy về combobox loại quyết định
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<DecisionItemBase>>> GetComboboxDecisionItem(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxByField<DecisionItemBase>(layoutCode, keySearch, q);
        }

        /// <summary>
        /// Hàm lấy về combobox chuyên khoa
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<ChuyenKhoaModel>>> GetComboboxChuyenKhoa(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<ChuyenKhoaModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox trình độ chuyên môn (phần bằng cấp/chứng chỉ) theo nghiệp vụ bệnh viện đức giang
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<TrinhDoChuyenMonModel>>> GetComboboxTrinhDoChuyenMon(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxByField<TrinhDoChuyenMonModel>(layoutCode, keySearch, q);
        }

        /// <summary>
        /// Hàm lấy về combobox trình độ đào tạo (phần bằng cấp/chứng chỉ) theo nghiệp vụ bệnh viện đức giang
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<TrinhDoDaoTaoModel>>> GetComboboxTrinhDoDaoTao(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxByField<TrinhDoDaoTaoModel>(layoutCode, keySearch, q);
        }

        /// <summary>
        /// Hàm lấy về combobox trình độ đào tạo (quá trình đào tạo) theo nghiệp vụ bệnh viện đức giang
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<TrinhDoDTModel>>> GetComboboxTrinhDoDT(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxByField<TrinhDoDTModel>(layoutCode, keySearch, q);
        }

        /// <summary>
        /// Hàm lấy về combobox bảng nghề theo trường
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<Occupation>>> GetComboboxOccupation(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxByField<Occupation>(layoutCode, keySearch, q, true);
        }

        /// <summary>
        /// Hàm lấy về taonf bộ combobox bảng nghề
        /// thêm ngày 9/3/2022
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<Occupation>>> GetComboboxOccupation(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<Occupation>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox Quốc tịch từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<CountryBase>>> GetComboboxCountries(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxFromQTHT<CountryBase>(layoutCode, keySearch, q, true);
        }

        /// <summary>
        /// Hàm lấy về combobox chuyên ngành đào tạo
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<ChuyenNganhDaoTaoModel>>> GetComboboxChuyenNganhDaoTao(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<ChuyenNganhDaoTaoModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về combobox chuyên ngành
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<ChuyenNganhModel>>> GetComboboxChuyenNganh(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<ChuyenNganhModel>(layoutCode, keySearch, q, page);
        }
        /// <summary>
        /// Hàm lấy về combobox Quốc tịch từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<EthnicBase>>> GetComboboxEthnics(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxFromQTHT<EthnicBase>(layoutCode, keySearch, q, true);
        }

        /// <summary>
        /// Hàm lấy về combobox Tỉnh/TP từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<LocationBaseModel>>> GetComboboxLocations(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxFromQTHT<LocationBaseModel>(layoutCode, keySearch, q, true);
        }

        /// <summary>
        /// Hàm lấy về combobox Quận/huyện từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<DistrictBase>>> GetComboboxDistricts(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxFromQTHT<DistrictBase>(layoutCode, keySearch, q, true);
        }

        /// <summary>
        /// Hàm lấy về combobox Phường/xã từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<WardBase>>> GetComboboxWards(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxFromQTHT<WardBase>(layoutCode, keySearch, q, true);
        }

        /// <summary>
        /// Hàm lấy thông tin đơn vị hành chính đang chọn
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<object>> GetNameLocation(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetNameLocation<object>(layoutCode, keySearch, q);
        }

        /// <summary>
        /// Hàm lấy danh sách người dùng QTHT
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _baseHandler.GetAllUsers();
        }

        /// <summary>
        /// Lấy danh sách quyền của user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<Permissions>>> GetPermissionByUser(int userID, int? appID)
        {
            return await _baseHandler.GetPermissionByUser(userID, appID);
        }

        /// <summary>
        /// Lấy thông tin user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<UserInfoCacheModel>> GetUserInfo(string userName)
        {
            return await _baseHandler.GetUserInfo(userName);
        }

        /// <summary>
        /// Hàm lấy về combobox Bệnh viện từ QTHT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<HospitalBase>>> GetComboboxHospital(string layoutCode, string keySearch, int q)
        {
            return await _baseHandler.GetComboboxByField<HospitalBase>(layoutCode, keySearch, q, false);
        }

        /// <summary>
        /// Hàm lấy về combobox TrinhDoDT
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<TrinhDoDaoTaoModel>>> GetComboboxTrinhDoDaoTaoPost(string layoutCode, string keySearch, string q)
        {
            return await _baseHandler.GetCombobox<TrinhDoDaoTaoModel>(layoutCode, keySearch, q, 1, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Hàm lấy về combobox Chuyên ngành đào tạo
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<ChuyenNganhDaoTaoModel>>> GetComboboxChuyenNganhDaoTao(string layoutCode, string keySearch, string q)
        {
            return await _baseHandler.GetCombobox<ChuyenNganhDaoTaoModel>(layoutCode, keySearch, q, 1, CommandType.Text);
        }

        /// <summary>
        /// Hàm lấy về combobox TrinhDoDaoTao
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<TrinhDoDTModel>>> GetComboboxTrinhDoDTPost(string layoutCode, string keySearch, string q)
        {
            return await _baseHandler.GetCombobox<TrinhDoDTModel>(layoutCode, keySearch, q, 1, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Hàm lấy về combobox Chuyên ngành
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<ChuyenNganhModel>>> GetComboboxChuyenNganh(string layoutCode, string keySearch, string q)
        {
            return await _baseHandler.GetCombobox<ChuyenNganhModel>(layoutCode, keySearch, q, 1, CommandType.Text);
        }

        /// <summary>
        /// Hàm lấy về toàn bộ combobox quy trình
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<WorkflowModel>>> GetComboboxWorkflow(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<WorkflowModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về toàn bộ combobox bước quy trình
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<WorkflowCommandModel>>> GetComboboxWorkflowCommand(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<WorkflowCommandModel>(layoutCode, keySearch, q, page);
        }

        /// <summary>
        /// Hàm lấy về toàn bộ combobox trạng thái quy trình
        /// </summary>
        /// <param name="layoutCode">Tên của phân hệ dùng để map với key trong file json để render ra câu sql</param>
        /// <param name="q"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<WorkflowStateModel>>> GetComboboxWorkflowState(string layoutCode, string keySearch, string q, int page)
        {
            return await _baseHandler.GetCombobox<WorkflowStateModel>(layoutCode, keySearch, q, page);
        }
    }
}
