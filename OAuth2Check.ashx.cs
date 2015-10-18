using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Services;
using Suite.Common;

namespace Suite
{
    /// <summary>
    /// OAuth2Check 的摘要说明
    /// </summary>
    public class OAuth2Check : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {            //code说明 ：code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。
            string code = HttpContext.Current.Request["code"];
            Helper.WriteLog("code:" + code);
            string corpId = ConfigurationManager.AppSettings["CorpId"];//从配置文件获取corpId
            if (string.IsNullOrEmpty(corpId))
            {
                Helper.WriteLog(string.Format("CorpId 配置项没有配置！"));
            }
            string corpSecret = ConfigurationManager.AppSettings["CorpSecret"];//从配置文件获取CorpSecret
            if (string.IsNullOrEmpty(corpSecret))
            {
                Helper.WriteLog(string.Format("CorpSecret 配置项没有配置！"));
            }

            string redirectUrl = "http://live.fumasoft.com:8011/Test.aspx";
            redirectUrl = HttpUtility.UrlEncode(redirectUrl);

            string url = "https://oapi.dingtalk.com/connect/oauth2/authorize?appid="+ corpId + "&redirect_uri="+ redirectUrl + "&response_type=code&scope=snsapi_base&state=abcd1234";
            Helper.WriteLog("url:" + url);

            string result = Helper.GetCorpExecuteResult(url);
            Helper.WriteLog("result:" + result);

            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}