using System.Runtime.Serialization;

namespace Suite.Common
{
    public class SuiteAccessToken
    {
        [DataMember(Order = 0)]
        public string suite_access_token { get; set; }
        [DataMember(Order = 1)]
        public int expires_in { get; set; }
    }
}
