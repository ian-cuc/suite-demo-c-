using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using Suite.Common;
using System.Web.Security;

namespace Suite
{
    public partial class UserCheck : System.Web.UI.Page
    {
        public string appId = string.Empty;
        public string timestamp = string.Empty;
        public string nonceStr = string.Empty;
        public string signature = string.Empty;
        public string jsApiList = string.Empty;
        public string corpId = string.Empty;
        string corpSecret = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            corpId = ConfigurationManager.AppSettings["CorpId"];//从配置文件获取corpId
            if (string.IsNullOrEmpty(corpId))
            {
                Helper.WriteLog(string.Format("CorpId 配置项没有配置！"));
            }
            corpSecret = ConfigurationManager.AppSettings["CorpSecret"];//从配置文件获取CorpSecret
            if (string.IsNullOrEmpty(corpSecret))
            {
                Helper.WriteLog(string.Format("CorpSecret 配置项没有配置！"));
            }
            GetConfig();
        }

        public double GetTimeStamp()
        {
            DateTime dt1 = Convert.ToDateTime("1970-01-01 00:00:00");
            TimeSpan ts = DateTime.Now - dt1;
            return Math.Ceiling(ts.TotalSeconds);
        }

        private string GetConfig()
        {
            nonceStr = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            timestamp = GetTimeStamp().ToString();
            string url = Request.Url.ToString();
            appId = "4801092";
            string jsApiTicket = Helper.GetJsApiTicket();
            Helper.WriteLog("nonceStr：" + nonceStr);
            Helper.WriteLog("timestamp:" + timestamp);
            Helper.WriteLog("url:" + url);
            Helper.WriteLog("jsApiTicket:" + jsApiTicket);

            string string1 = "jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}";
            string1 = string.Format(string1, jsApiTicket, nonceStr, timestamp, url);
            Helper.WriteLog("signature not sha1:" + string1);

            signature = FormsAuthentication.HashPasswordForStoringInConfigFile(string1, "SHA1").ToLower();
            Helper.WriteLog("signature sha1:" + signature);

            return "";
        }

        

        
    }
}