using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IBaseHandler
    {
        /// <summary>
        /// Grid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityGeneric"></param>
        /// <returns></returns>
        Task<Response<List<T>>> GetFilter<T>(EntityGeneric entityGeneric);

        /// <summary>
        /// Tạo mới đối tượng
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layout"></param>
        /// <param name="entityGeneric"></param>
        /// <returns></returns>
        Task<Response<bool>> CreateObject<T>(string layout, T entityGeneric);

        /// <summary>
        /// Update đối tượng
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layout"></param>
        /// <param name="id"></param>
        /// <param name="entityGeneric"></param>
        /// <returns></returns>
        Task<Response<bool>> UpdateObject<T>(string layout, int id, T entityGeneric);

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany<T>(List<int> recordID);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<T>> FindById<T>(int recordID);

        /// <summary>
        /// Insert File đính kèm
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        Task<Response<bool>> InsertManyFile(List<HRM_AttachmentModel> records);

        /// <summary>
        /// Xóa danh sách file đính kèm 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteManyFile(int Type, List<int> TypeId);

        /// <summary>
        /// Lấy thông tin file đính kèm theo Type và TypeId
        /// </summary>
        /// <param name="Type">Loại danh mục</param>
        /// <param name="TypeId">Id của bản ghi thuộc loại danh mục</param>
        /// <returns></returns>
        Task<Response<List<HRM_AttachmentModel>>> GetAttachmentByTypeAndTypeId(int Type, int TypeId);

        /// <summary>
        /// Lấy danh sách organization từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        List<OrganizationModel> GetOrganization();

        /// <summary>
        /// Check trùng theo trường nào đó
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckDuplicate<T>(string whereClauses);

        /// <summary>
        /// Lấy danh sách dân tộc từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        IEnumerable<EthnicBase> GetEthnics();

        /// <summary>
        /// Lấy danh sách quốc tịch từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        IEnumerable<CountryBase> GetCountries();

        /// <summary>
        /// Lấy danh sách tỉnh/thành từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        IEnumerable<LocationBaseModel> GetLocations();

        /// <summary>
        /// Lấy danh sách quận/huyện từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        IEnumerable<DistrictBase> GetDistricts();

        /// <summary>
        /// Lấy danh sách xã/phường từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        IEnumerable<WardBase> GetWards();

        /// <summary>
        /// hàm lấy về danh sách id của cơ cấu tổ chức con và chính nó
        /// </summary>
        /// <param name="organizationId">Id của cơ cấu tổ chức</param>
        /// <returns></returns>
        IEnumerable<OrganizationModel> GetListChildOrganizationAndOrganizationId(string organizationIds);


        /// <summary>
        /// Tìm bản ghi quá trình công tác gần nhất có trạng thái khác điều động tăng cường
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<HistoryModel> FindRecordLastest(int staffID);

        /// <summary>
        /// Lấy dân tộc theo id từ phần mềm QTHT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EthnicBase GetEthnicsById(int id);

        /// <summary>
        /// Lấy quốc tịch theo id từ phần mềm QTHT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CountryBase GetCountriesById(int id);

        /// <summary>
        /// Lấy tỉnh/thành theo id từ phần mềm QTHT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LocationBaseModel GetLocationsById(int id);

        /// <summary>
        /// Lấy quận/huyện theo id từ phần mềm QTHT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DistrictBase GetDistrictsById(int id);

        /// <summary>
        /// Lấy xã/phường theo id từ phần mềm QTHT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WardBase GetWardsById(int id);

        /// <summary>
        /// Lấy danh sách dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <param name="hasStatusColumn">đối với bảng có cột trạng thái</param>
        /// <returns></returns>
        List<T> GetListData<T>(string tableName, string keySearch, int value, bool? hasStatusColumn = false);

        /// <summary>
        /// Lấy danh sách dữ liệu QTHT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="keySearch"></param>
        /// <param name="value"></param>
        /// <param name="hasStatusColumn"></param>
        /// <returns></returns>
        List<T> GetSystemListData<T>(string tableName, string keySearch, int value, bool? hasStatusColumn = false);

        /// <summary>
        /// Tạo log các thao tác
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Response<bool> CreateLog(SystemLogModel model);
    }
}
