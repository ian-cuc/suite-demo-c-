using System.Runtime.Serialization;


namespace Suite.Common
{
    public class AuthCorp
    {
        [DataMember(Order = 0)]
        public AuthCorpInfo auth_corp_info { get; set; }
        [DataMember(Order = 1)]
        public AuthUserInfo auth_user_info { get; set; }
        [DataMember(Order = 2)]
        public AuthInfo auth_info { get; set; }
        [DataMember(Order = 3)]
        public string errcode { get; set; }
        [DataMember(Order = 4)]
        public string errmsg { get; set; }

    }

    public class AuthCorpInfo
    {
        [DataMember(Order = 0)]
        public string corp_logo_url { get; set; }
        [DataMember(Order = 1)]
        public string corp_name { get; set; }
        [DataMember(Order = 2)]
        public string corpid { get; set; }
    }

    public class AuthUserInfo
    {
        [DataMember(Order = 0)]
        public string mobile { get; set; }
        [DataMember(Order = 1)]
        public string name { get; set; }
    }

    public class AuthInfo
    {
        public AgentInfo[] agent { get; set; }
    }

    public class AgentInfo
    {
        [DataMember(Order = 0)]
        public string agent_name { get; set; }
        [DataMember(Order = 1)]
        public string agentid { get; set; }
        [DataMember(Order = 2)]
        public string appid { get; set; }
        [DataMember(Order = 3)]
        public string logo_url { get; set; }

    }
}
