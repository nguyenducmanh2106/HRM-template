using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models.Core
{
    public class CustomMembershipUser
    {
        public string UserName { get; set; }
        public string NickName { get; set; }
        public object ProviderUserKey { get; set; }
        public string Email { get; set; }
        public string PasswordQuestion { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastLockedOutDate { get; set; }

        public string CreatedByUserID { get; set; }
        public string LastModifiedByUserID { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public string FullName { get; set; }
        
        public string UserID { get; set; }
        public bool IsDeleted { get; set; }
        

        public CustomMembershipUser(string username, string nickname, object providerUserKey, string email, string passwordQuestion, string comment, 
            bool isApproved, bool isLockedOut, DateTime creationDate, DateTime lastLoginDate, DateTime lastActivityDate, DateTime lastPasswordChangedDate, 
            DateTime lastLockedOutDate, string createdByUserID, string lastModifiedByUserID, DateTime lastModifiedOnDate, string fullName, string userID, bool isDeleted)
        {
            UserName = username;
            NickName = nickname;
            ProviderUserKey = providerUserKey;
            Email = email;
            PasswordQuestion = passwordQuestion;
            Comment = comment;
            IsApproved = isApproved;
            IsLockedOut = isLockedOut;
            CreationDate = creationDate;
            LastLoginDate = lastLoginDate;
            LastActivityDate = lastActivityDate;
            LastPasswordChangedDate = lastPasswordChangedDate;
            LastLockedOutDate = lastLockedOutDate;
            CreatedByUserID = createdByUserID;
            LastModifiedByUserID = lastModifiedByUserID;
            LastModifiedOnDate = lastModifiedOnDate;
            FullName = fullName;
            UserID = userID;
            IsDeleted = isDeleted;
        }
    }
}
