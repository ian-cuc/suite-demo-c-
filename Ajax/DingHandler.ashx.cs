using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using Suite.Common;

namespace Suite.Ajax
{
    /// <summary>
    /// DingHandler 的摘要说明
    /// </summary>
    public class DingHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string methodName = context.Request["method"];
            if (!string.IsNullOrEmpty(methodName))
            {
                CallMethod(methodName, context);
            }
        }

        private void CallMethod(string method, HttpContext context)
        {
            switch (method)
            {
                case "getuserinfo":
                    GetUserCode(context);
                    break;
            }
        }

        private void GetUserCode(HttpContext context)
        {
            try
            {
                string code = context.Request["code"];
                Helper.WriteLog("authcode: " + code);

                string url = "https://oapi.dingtalk.com/user/getuserinfo?access_token={0}&code={1}";

                string accessToken = Helper.GetAccessToken();

                url = string.Format(url, accessToken, code);
                Helper.WriteLog("url: " + url);

                string result = Helper.GetCorpExecuteResult(url);
                Helper.WriteLog("result: " + result);

                var ser = new DataContractJsonSerializer(typeof(UserCode));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                UserCode userCode = (UserCode)ser.ReadObject(ms);

                result = GetUserInfo(userCode.userid);
                context.Response.Write(result);
            }
            catch (Exception ex)
            {
                Helper.WriteLog("result: " + ex.Message);
                context.Response.Write(ex.Message);
            }
        }

        private string GetUserInfo(string userId)
        {
            string accessToken = Helper.GetAccessToken();
            string url = "https://oapi.dingtalk.com/user/get?access_token={0}&userid={1}";

            url = string.Format(url, accessToken, userId);
            Helper.WriteLog("url: " + url);

            string result = Helper.GetCorpExecuteResult(url);
            Helper.WriteLog("result: " + result);

            return result;
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