using System.Runtime.Serialization;

namespace Suite.Common
{
    public class UserInfo
    {
        [DataMember(Order = 0)]
        public string errcode { get; set; }
        [DataMember(Order = 1)]
        public string errmsg { get; set; }
        [DataMember(Order = 2)]
        public string userid { get; set; }
        [DataMember(Order = 3)]
        public string name { get; set; }
        [DataMember(Order = 4)]
        public string[] department { get; set; }
        [DataMember(Order = 5)]
        public string position { get; set; }
        [DataMember(Order = 6)]
        public string avatar { get; set; }
        [DataMember(Order = 7)]
        public string jobnumber { get; set; }
        [DataMember(Order = 8)]
        public Attrs[] extattr { get; set; }
        [DataMember(Order = 9)]
        public string openId { get; set; }
    }

    public class Attrs
    {
        [DataMember(Order = 0)]
        public string name { get; set; }
        [DataMember(Order = 1)]
        public string value { get; set; }
    }
}
