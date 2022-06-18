using Newtonsoft.Json;
using NLog;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SV.HRM.Service.LeaveManagement
{
    public class WorkflowHandler : IWorkflowHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public WorkflowHandler()
        {
        }

        #region Workflow engine version 3.5

        public Response<bool> CreateDocument(Document newDocument)
        {
            try
            {
                // Post data
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WFConfig.ApiUrl);

                    // Set the header of request
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var data = JsonConvert.SerializeObject(newDocument);
                    var utfBytes = System.Text.Encoding.UTF8.GetBytes(data);
                    var uri = "api/documents";
                    // Post data
                    var response = client.PostAsJsonAsync(client.BaseAddress + uri, newDocument).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return new Response<bool>(1, response.ReasonPhrase, true);
                    }
                    else
                    {
                        return new Response<bool>(-1, response.ReasonPhrase, false);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(-1, ex.Message, false);
            }
        }

        public Response<bool> DeleteDocument(Document document)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WFConfig.ApiUrl);

                    // Set the header of request
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var data = JsonConvert.SerializeObject(document);
                    var utfBytes = Encoding.UTF8.GetBytes(data);
                    var uri = "api/deletedocument";
                    // Post data
                    var response = client.PostAsJsonAsync(client.BaseAddress + uri, document).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return new Response<bool>(1, response.ReasonPhrase, true);
                    }
                    else
                    {
                        return new Response<bool>(-1, response.ReasonPhrase, false);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(-1, ex.Message, false);
            }
        }

        public Response<bool> ExecuteCommand(InputCommand dataCommand)
        {
            try
            {
                // Post data
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WFConfig.ApiUrl);
                    // Set the header of request
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var data = JsonConvert.SerializeObject(dataCommand);
                    var utfBytes = Encoding.UTF8.GetBytes(data);
                    var uri = "api/executecommand";
                    // Post data
                    var response = client.PostAsJsonAsync(client.BaseAddress + uri, dataCommand).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return new Response<bool>(1, response.ReasonPhrase, true);
                    }
                    else
                    {
                        return new Response<bool>(-1, response.ReasonPhrase, false);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(-1, ex.Message, false);
            }
        }

        public Response<List<string>> GetCommands(InputCommand dataCommand)
        {
            try
            {
                // Post data
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WFConfig.ApiUrl);
                    // Set the header of request
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var data = JsonConvert.SerializeObject(dataCommand);
                    var utfBytes = Encoding.UTF8.GetBytes(data);
                    var uri = "api/commands";
                    // Post data
                    var response = client.PostAsJsonAsync(client.BaseAddress + uri, dataCommand).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<Response<List<string>>>(response.Content.ReadAsStringAsync().Result);
                        return new Response<List<string>>(1, response.ReasonPhrase, result.Data);
                    }
                    else
                    {
                        return new Response<List<string>>(-1, response.ReasonPhrase, null);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<string>>(-1, ex.Message, null);
            }
        }

        public Response<List<DocumentHistoryViewModel>> GetDocumentHistory(Guid id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WFConfig.ApiUrl);

                    // Set the header of request
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var uri = "api/history/" + id;
                    // Post data
                    var response = client.PostAsJsonAsync(client.BaseAddress + uri, "").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<Response<List<DocumentHistoryViewModel>>>(response.Content.ReadAsStringAsync().Result);
                        return new Response<List<DocumentHistoryViewModel>>(1, response.ReasonPhrase, result.Data);
                    }
                    else
                    {
                        return new Response<List<DocumentHistoryViewModel>>(-1, response.ReasonPhrase, null);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<DocumentHistoryViewModel>>(-1, ex.Message, null);
            }
        }

        public Response<List<UserInfo>> GetNextUserProcess(InputCommand dataCommand)
        {
            try
            {
                // Post data
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WFConfig.ApiUrl);
                    // Set the header of request
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var data = JsonConvert.SerializeObject(dataCommand);
                    var utfBytes = Encoding.UTF8.GetBytes(data);
                    var uri = "api/nextuser";
                    // Post data
                    var response = client.PostAsJsonAsync(client.BaseAddress + uri, dataCommand).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<Response<List<UserInfo>>>(response.Content.ReadAsStringAsync().Result);
                        return new Response<List<UserInfo>>(1, response.ReasonPhrase, result.Data);
                    }
                    else
                    {
                        return new Response<List<UserInfo>>(-1, response.ReasonPhrase, null);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<UserInfo>>(-1, ex.Message, null);
            }
        }

        public Response<DocumentStateModel> GetStateNameOfDocuments(Guid id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WFConfig.ApiUrl);

                    // Set the header of request
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var uri = "api/statenameofdocument/" + id;
                    // Post data
                    var response = client.PostAsJsonAsync(client.BaseAddress + uri, "").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<Response<DocumentStateModel>>(response.Content.ReadAsStringAsync().Result);
                        return new Response<DocumentStateModel>(1, response.ReasonPhrase, result.Data);
                    }
                    else
                    {
                        return new Response<DocumentStateModel>(-1, response.ReasonPhrase, null);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<DocumentStateModel>(-1, ex.Message, null);
            }
        }

        #endregion
    }
}
