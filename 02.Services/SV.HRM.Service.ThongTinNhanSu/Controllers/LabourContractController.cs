using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using static SV.HRM.Service.ThongTinNhanSu.CustomAuthorize;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LabourContractController : ControllerBase
    {
        //private readonly IStaffHandler _StaffHandler;
        private readonly IBaseHandler _baseHandler;
        private readonly ILabourContractHandler _labourContractHandler;
        public LabourContractController(IStaffHandler dbStaffHandler, IBaseHandler baseHandler,
            ILabourContractHandler labourContractHandler)
        {
            //_StaffHandler = dbStaffHandler;
            _baseHandler = baseHandler;
            _labourContractHandler = labourContractHandler;
        }

        //[CustomAuthorize(Role.HDLD_MANAGER, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<LabourContractModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                return await _baseHandler.GetFilter<LabourContractModel>(queryFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy về combobox phụ lục của hợp đồng
        /// </summary>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<LabourContractModel>>> GetComboboxParentLabourContractID(string layoutCode, string keySearch, int q)
        {
            return await _labourContractHandler.GetComboboxParentLabourContractID(layoutCode, keySearch, q);
        }

        /// <summary>
        /// Hàm tạo hợp đồng lao động
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.HDLD_MANAGER, Right.CREATE)]
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] LabourContractCreateModel entity)
        {
            string whereClauses = $"{nameof(LabourContract.LabourContractNo)} = N'{entity.LabourContractNo}' AND StaffID = {entity.StaffID}";
            var isCheckDuplicate = await _baseHandler.CheckDuplicate<LabourContract>(whereClauses);
            if (isCheckDuplicate != null && isCheckDuplicate.Status == SUCCESS && isCheckDuplicate.Data)
            {
                InforError inforError = new InforError()
                {
                    ValidateType = 1,
                    ErrorMess = string.Format(Constant.ErrorCode.DUPPLICATE_CODE_MESS, entity.LabourContractNo),
                    FieldName = nameof(entity.LabourContractNo)

                };
                List<InforError> validateInfo = new List<InforError>();
                validateInfo.Add(inforError);
                return new Response<bool>(Constant.ErrorCode.DUPPLICATE_CODE, Constant.ErrorCode.DUPPLICATENAME_MESS, true, 0, 0, 0, 0, 0, validateInfo);
            }
            return await _labourContractHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<LabourContractModel>> FindById(int recordID)
        {
            return await _labourContractHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm tạo hợp đồng lao động của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.HDLD_MANAGER, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] LabourContractUpdateModel entity)
        {
            return await _labourContractHandler.Update(id, entity);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[CustomAuthorize(Role.HDLD_MANAGER, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            var res = await _baseHandler.DeleteMany<LabourContract>(entity);
            if (res != null && res.Status == SUCCESS && res.Data)
            {
                return await _baseHandler.DeleteManyFile(FileType.HOP_DONG_LAO_DONG, entity);
            }
            else return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
        }

    }
}
