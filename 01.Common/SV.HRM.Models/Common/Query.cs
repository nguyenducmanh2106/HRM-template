using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models.Common
{
    public enum QueryOrder
    {
        NGAY_TAO_DESC,
        NGAY_TAO_ASC,
        ID_DESC,
        ID_ASC,
        NGAY_XUAT_BAN_DESC,
        NGAY_XUAT_BAN_ASC,
        SO_LAN_XEM_DESC,
        SO_LAN_XEM_ASC,
        VERSION_DESC
    }

    public class QueryFilter
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string TextSearch { get; set; }
        public string UserId { get; set; }
        public string CurrentLoginUserId { get; set; }
        public string NickName { get; set; }
        public string AuthorName { get; set; }
        public QueryOrder Order { get; set; }
        public string Type { get; set; }
        public int Hours { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TaxanomyVocabulary { get; set; }
        public List<string> ListTaxanomyTerm { get; set; }
        public List<string> ListTaxanomyTermId { get; set; }
        public string TaxanomyTerm { get; set; }
        public string TaxonomyTermId { get; set; }
        public string[] WorkflowStatus { get; set; }
        public int[] DisplayStatus { get; set; }
        public List<string> lstURL { get; set; }
        public List<string> lstBlockInfo { get; set; }
        public List<string> lstTabInfo { get; set; }
        public List<string> lstTab { get; set; }
        public int? StoryLineType { get; set; }
        public string ApplicationId { get; set; }
        public string KeyWord { get; set; }
        public bool? CheckHasPage { get; set; }
        public bool? CheckHasAlarm { get; set; }
        public bool? IsReturnTotalRecord { get; set; }
        public bool? IsReturnTotalRecordOnly { get; set; }
        public int? PageLevel { get; set; }
        public bool? ActShow { get; set; }
        public DateTime? TimeDay { get; set; }
        public DateTime? SearchByDate { get; set; }
        public string StorylineId { get; set; }
        public int? HotNewsCount { get; set; }
        public List<string> ListStorylineId { get; set; }
        public int? LockStatus { get; set; }
        public string CreatByUser { get; set; }

        public List<int> ListYear { get; set; }
        public QueryFilter()
        {
            PageSize = 10;
            PageNumber = 1;
            TextSearch = string.Empty;
            // UserId = Guid.Empty.ToString();
            Order = QueryOrder.NGAY_TAO_DESC;
            //Type = string.Empty;
            Hours = 0;
            //FromDate = DateTime.Now;
            //ToDate = DateTime.Now;
            // DisplayStatus = new int[] { 2 };
        }
    }


    public class StoryQueryFilter
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string TextSearch { get; set; }
        public string UserId { get; set; }
        public string CurrentLoginUserId { get; set; }
        public string NickName { get; set; }
        public string AuthorName { get; set; }
        public QueryOrder Order { get; set; }
        public string Type { get; set; }
        public int Hours { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TaxanomyVocabulary { get; set; }
        public List<string> ListTaxanomyTerm { get; set; }
        public List<string> ListTaxanomyTermId { get; set; }
        public string TaxanomyTerm { get; set; }
        public string TaxonomyTermId { get; set; }
        public string[] WorkflowStatus { get; set; }
        public int DisplayStatus { get; set; }
        public List<string> lstURL { get; set; }
        public List<string> lstBlockInfo { get; set; }
        public List<string> lstTabInfo { get; set; }
        public List<string> lstTab { get; set; }
        public int? StoryLineType { get; set; }
        public string ApplicationId { get; set; }
        public string KeyWord { get; set; }
        public bool? CheckHasPage { get; set; }
        public bool? CheckHasAlarm { get; set; }
        public bool? ReturnTotalRecords { get; set; }
        public int? PageLevel { get; set; }
        public bool? ActShow { get; set; }
        public DateTime? TimeDay { get; set; }
        public DateTime? SearchByDate { get; set; }
        public string StorylineId { get; set; }
        public int? HotNewsCount { get; set; }
        public List<string> ListStorylineId { get; set; }
        public int? LockStatus { get; set; }
        public StoryQueryFilter()
        {
            TextSearch = string.Empty;

            Order = QueryOrder.NGAY_TAO_DESC;
        }
    }
}
