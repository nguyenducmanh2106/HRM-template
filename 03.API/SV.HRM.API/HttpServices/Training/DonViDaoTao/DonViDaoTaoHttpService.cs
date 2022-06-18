﻿using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class DonViDaoTaoHttpService : IDonViDaoTaoHttpService
    {
        private IHttpHelper _httpHelper;
        public DonViDaoTaoHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }
        public async Task<Response<List<DonViDaoTaoModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<DonViDaoTaoModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(DonViDaoTaoCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"DonViDaoTao/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng DonViDaoTao
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<DonViDaoTaoModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<DonViDaoTaoModel>>($"DonViDaoTao/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"DonViDaoTao/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"DonViDaoTao/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, DonViDaoTaoUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"DonViDaoTao/Update?id={id}", model);
        }
        /// <summary>
        /// check bản ghi có sử dụng không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"DonViDaoTao/FindIdInUse", recordID);
        }
    }
}
