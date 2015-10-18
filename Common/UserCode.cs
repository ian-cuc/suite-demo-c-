using System.Runtime.Serialization;


namespace Suite.Common
{
    public class UserCode
    {
        [DataMember(Order = 0)]
        public string deviceId { get; set; }
        [DataMember(Order = 1)]
        public string errcode { get; set; }
        [DataMember(Order = 2)]
        public string errmsg { get; set; }
        [DataMember(Order = 3)]
        public string userid { get; set; }
        [DataMember(Order = 4)]
        public bool is_sys { get; set; }
    }
}
