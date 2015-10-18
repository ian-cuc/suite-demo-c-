using System.Runtime.Serialization;

namespace Suite.Common
{
    public class PermanentCode
    {
        [DataMember(Order = 0)]
        public string permanent_code { get; set; }
        [DataMember(Order = 1)]
        public CorpInfo auth_corp_info { get; set; }
        [DataMember(Order = 2)]
        public string errcode { get; set; }
        [DataMember(Order = 3)]
        public string errmsg { get; set; }

    }

    public class CorpInfo {
        [DataMember(Order = 0)]
        public string corpid { get; set; }
        [DataMember(Order = 1)]
        public string corp_name { get; set; }
    }
}
