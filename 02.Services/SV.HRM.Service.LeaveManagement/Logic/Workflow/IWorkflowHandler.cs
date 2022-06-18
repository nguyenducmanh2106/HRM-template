using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;

namespace SV.HRM.Service.LeaveManagement
{
    public interface IWorkflowHandler
    {
        Response<bool> CreateDocument(Document newDocument);
        Response<bool> DeleteDocument(Document document);
        Response<bool> ExecuteCommand(InputCommand dataCommand);
        Response<List<string>> GetCommands(InputCommand dataCommand);
        Response<List<DocumentHistoryViewModel>> GetDocumentHistory(Guid id);
        Response<List<UserInfo>> GetNextUserProcess(InputCommand dataCommand);
        Response<DocumentStateModel> GetStateNameOfDocuments(Guid id);
    }
}
