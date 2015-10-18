using System.Runtime.Serialization;


namespace Suite.Common
{
    public class JsApiTicket
    {
        [DataMember(Order = 0)]
        public string errcode { get; set; }
        [DataMember(Order = 1)]
        public string errmsg { get; set; }
        [DataMember(Order = 2)]
        public string ticket { get; set; }
        [DataMember(Order = 3)]
        public int expires_in { get; set; }
    }
}
