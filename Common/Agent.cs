using System.Runtime.Serialization;


namespace Suite.Common
{
    public class Agent
    {
        [DataMember(Order = 0)]
        public string agentid { get; set; }
        [DataMember(Order = 1)]
        public string name { get; set; }
        [DataMember(Order = 2)]
        public string logo_url { get; set; }
        [DataMember(Order = 3)]
        public string description { get; set; }
        [DataMember(Order = 4)]
        public int close { get; set; }
        [DataMember(Order = 5)]
        public string errcode { get; set; }
        [DataMember(Order = 6)]
        public string errmsg { get; set; }

    }
}
