using Microsoft.Extensions.Logging;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using NLog;

namespace SV.HRM.Service.BaseServices
{
    public class GroupBoxHandler : IGroupBoxHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;

        public GroupBoxHandler(IDapperUnitOfWork dapperUnitOfWork
            )
        {
            _dapperUnitOfWork = dapperUnitOfWork;
        }

        public async Task<Response<bool>> BulkCreate(List<GroupBoxModel> groupBoxes, List<GroupBoxField> groupBoxFields, int? userID)
        {
            try
            {
                bool success = false;
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(GroupBox), "BulkCreate_GroupBoxAndGroupBoxField");

                foreach (var groupbox in groupBoxes)
                {
                    var groupBoxFieldBelongGroupBoxs = groupBoxFields.Where(g => g.groupBoxID == groupbox.ID).ToList();
                    string groupBoxFieldBelongGroupBoxJson = JsonSerializer.Serialize(new { GroupBoxField = groupBoxFieldBelongGroupBoxs });
                    var param = new
                    {
                        @GroupBoxText = groupbox.groupBoxText,//thuộc tính của groupbox
                        @TypeBox = groupbox.typeBox,
                        @IsUse = groupbox.isUse,
                        @SortOrder = groupbox.sortOrder,
                        @UserID = userID,
                        @Code = groupbox.code,
                        @GroupBoxFields = groupBoxFieldBelongGroupBoxJson
                    };
                    await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);
                    if (groupbox.ID == groupBoxes[groupBoxes.Count() - 1].ID)
                    {
                        success = true;
                    }
                }
                return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, success);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Response<bool>> BulkUpdate(List<GroupBox> groupBoxes, List<GroupBoxField> groupBoxFields)
        {
            try
            {
                //update list groupbox
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(GroupBox), "BulkUpdate_GroupBox");
                var objectParams = new object[groupBoxes.Count];
                for (var index = 0; index < groupBoxes.Count; index++)
                {
                    var model = groupBoxes[index];
                    objectParams[index] = new
                    {
                        @IsUse = model.isUse,
                        @SortOrder = model.sortOrder,
                        @ID = model.ID
                    };
                }
                var result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, objectParams);

                //update list groupbox field
                string sqlQueryBulkUpdateGroupBoxField = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(GroupBox), "BulkUpdate_GroupBoxField");
                var UpdateGroupBoxField = new object[groupBoxFields.Count];
                for (var index = 0; index < groupBoxFields.Count; index++)
                {
                    var model = groupBoxFields[index];
                    UpdateGroupBoxField[index] = new
                    {
                        @IsUse = model.isUse,
                        @ColumnNumber = model.columnNumber,
                        @GroupBoxID = model.groupBoxID,
                        @SortOrder = model.sortOrder,
                        @ID = model.ID
                    };
                }
                var resultBulkUpdateGroupBoxField = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQueryBulkUpdateGroupBoxField, UpdateGroupBoxField);

                return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm trả ra group box với từng cột
        /// </summary>
        /// <param name="groupBox"></param>
        /// <param name="groupBoxField"></param>
        public void GetColumnForGroupBox(ref List<GroupBoxModel> groupBox, List<GroupBoxField> groupBoxField)
        {
            groupBox?.ForEach(group =>
            {
                group.colOne = groupBoxField.Where(g => g.groupBoxID == group.ID && g.columnNumber == 1 && g.isUse == true).ToList();
                group.colTwo = groupBoxField.Where(g => g.groupBoxID == group.ID && g.columnNumber == 2 && g.isUse == true).ToList();
            });
        }

        /// <summary>
        /// Lấy về layout theo user
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityGeneric"></param>
        /// <returns></returns>
        public async Task<Response<object>> GetLayout(string layoutCode, int? userID = null)
        {
            try
            {
                string sqlQueryTableField_ByUserID = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(GroupBox), "GetDefaultLayout");

                var resultTableField_ByUserID = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQueryTableField_ByUserID, new { @LayoutCode = layoutCode, @UserID = userID }, null, CommandType.StoredProcedure,
                   gr =>
                       gr.Read<GroupBoxModel>(),
                   gr =>
                       gr.Read<GroupBoxField>()
                   );
                int? nullableInt = null;

                var resultTableFieldDefault = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQueryTableField_ByUserID, new { @LayoutCode = layoutCode, @UserID = nullableInt }, null, CommandType.StoredProcedure,
                   gr =>
                       gr.Read<GroupBoxModel>(),
                   gr =>
                       gr.Read<GroupBoxField>()
                   );

                List<GroupBoxModel> groupBoxs = new List<GroupBoxModel>();
                List<GroupBoxField> groupBoxFields = new List<GroupBoxField>();

                //nếu user có layout rồi
                if (resultTableField_ByUserID != null && (((List<GroupBoxModel>)resultTableField_ByUserID[0]).Count() > 0 || ((List<GroupBoxField>)resultTableField_ByUserID[1]).Count() > 0))
                {
                    groupBoxs = (List<GroupBoxModel>)resultTableField_ByUserID[0];
                    groupBoxFields = (List<GroupBoxField>)resultTableField_ByUserID[1];
                }
                else// nếu chưa có thì thêm layout mặc định cho user
                {
                    var resultCreate = await BulkCreate((List<GroupBoxModel>)resultTableFieldDefault[0], (List<GroupBoxField>)resultTableFieldDefault[1], userID);
                    if (resultCreate != null && resultCreate.Status == Constant.SUCCESS && resultCreate.Data)
                    {
                        resultTableField_ByUserID = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQueryTableField_ByUserID, new { @LayoutCode = layoutCode, @UserID = userID }, null, CommandType.StoredProcedure,
                   gr =>
                       gr.Read<GroupBoxModel>(),
                   gr =>
                       gr.Read<GroupBoxField>()
                   );
                    }
                    if (resultTableField_ByUserID != null && (((List<GroupBoxModel>)resultTableField_ByUserID[0]).Count() > 0 || ((List<GroupBoxField>)resultTableField_ByUserID[1]).Count() > 0))
                    {
                        groupBoxs = (List<GroupBoxModel>)resultTableField_ByUserID[0];
                        groupBoxFields = (List<GroupBoxField>)resultTableField_ByUserID[1];
                    }
                }

                //Lấy các groupbox đang sử dụng và chưa sử dụng
                List<GroupBoxModel> groupBoxUses = new List<GroupBoxModel>();
                List<GroupBoxModel> groupBoxIsNotUses = new List<GroupBoxModel>();
                foreach (var groupBox in groupBoxs)
                {
                    if (groupBox.isUse.HasValue && groupBox.isUse == true)
                    {
                        groupBoxUses.Add(groupBox);
                    }
                    if (!groupBox.isUse.HasValue || (groupBox.isUse.HasValue && groupBox.isUse == false))
                    {
                        groupBoxIsNotUses.Add(groupBox);
                    }
                }

                //Lấy các groupbox field đang sử dụng và chưa sử dụng
                List<GroupBoxField> groupBoxFieldUses = new List<GroupBoxField>();
                List<GroupBoxField> groupBoxFieldIsNotUses = new List<GroupBoxField>();
                foreach (var groupBoxField in groupBoxFields)
                {
                    if (groupBoxField.isUse.HasValue && groupBoxField.isUse == true)
                    {
                        groupBoxFieldUses.Add(groupBoxField);
                    }
                    if (!groupBoxField.isUse.HasValue || (groupBoxField.isUse.HasValue && groupBoxField.isUse == false))
                    {
                        groupBoxFieldIsNotUses.Add(groupBoxField);
                    }
                }

                //tiến hành lấy các groupbox với các cột nằm trong nó để vẽ giao diện
                GetColumnForGroupBox(ref groupBoxUses, groupBoxFieldUses);

                object result = new
                {
                    groupBoxUses,
                    groupBoxIsNotUses,
                    groupBoxFieldUses,
                    groupBoxFieldIsNotUses
                };

                return new Response<object>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
