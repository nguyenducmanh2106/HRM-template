using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models
{
    [Serializable]
    public class Ws02IS_ResponseTokenModel
    {
        public string error { get; set; }
        public string error_description { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string id_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    [Serializable]
    public class Ws02IS_ResponseSubModel
    {
        public string sub { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
        public bool email_verified { get; set; }
        public string name { get; set; }
        public string preferred_username { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string email { get; set; }
    }

    public class KeyCloack_Session
    {
        public string id { get; set; }
        public string username { get; set; }
        public string userId { get; set; }
        public string ipAddress { get; set; }
        public int start { get; set; }
        public int lastAccess { get; set; }
        public object clients { get; set; }
    }
}
