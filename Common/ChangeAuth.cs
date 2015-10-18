using System.Runtime.Serialization;

namespace Suite.Common
{
    public class ChangeAuth
    {
        [DataMember(Order = 0)]
        public string SuiteKey { get; set; }
        [DataMember(Order = 1)]
        public string EventType { get; set; }
        [DataMember(Order = 2)]
        public string TimeStamp { get; set; }
        [DataMember(Order = 3)]
        public string AuthCorpID { get; set; }
    }
}
