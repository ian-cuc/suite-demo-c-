using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;

namespace Suite.Common
{
    class Helper
    {
        public static void WriteLog(string strMemo)
        {
            string directoryPath = HttpContext.Current.Server.MapPath(@"Logs");
            string fileName = directoryPath + @"\log" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            StreamWriter sr = null;
            try
            {
                if (!File.Exists(fileName))
                {
                    sr = File.CreateText(fileName);
                }
                else
                {
                    sr = File.AppendText(fileName);
                }
                sr.WriteLine(DateTime.Now + ": " + strMemo);
            }
            catch(Exception ex)
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string GetCorpExecuteResult(string url, string postdata)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                CookieContainer cc = new CookieContainer();
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded;";
                byte[] postdatabyte = Encoding.UTF8.GetBytes(postdata);
                request.ContentLength = postdatabyte.Length;
                request.AllowAutoRedirect = false;
                request.CookieContainer = cc;
                request.KeepAlive = true;

                //提交请求
                Stream stream;
                stream = request.GetRequestStream();
                stream.Write(postdatabyte, 0, postdatabyte.Length);
                stream.Close();

                //接收响应
                response = (HttpWebResponse)request.GetResponse();
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);

                //CookieCollection cook = response.Cookies;
                ////Cookie字符串格式
                //string strcrook = request.CookieContainer.GetCookieHeader(request.RequestUri);
                Stream strm = response.GetResponseStream();

                StreamReader sr = new StreamReader(strm, System.Text.Encoding.UTF8);

                string line;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line + System.Environment.NewLine);
                }
                sr.Close();
                strm.Close();
                return sb.ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string GetCorpExecuteResult(string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                string serviceAddress = url;
                request = (HttpWebRequest)WebRequest.Create(serviceAddress);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                response = (HttpWebResponse)request.GetResponse();

                Stream strm = response.GetResponseStream();

                StreamReader sr = new StreamReader(strm, Encoding.UTF8);

                string line;

                StringBuilder sb = new StringBuilder();

                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line + Environment.NewLine);
                }
                sr.Close();
                strm.Close();
                return sb.ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static double GetTimeStamp()
        {
            DateTime dt1 = Convert.ToDateTime("1970-01-01 00:00:00");
            TimeSpan ts = DateTime.Now - dt1;
            return Math.Ceiling(ts.TotalSeconds);
        }

        private static bool CheckTokenString(string tokenString, string tokenExpires)
        {
            if (string.IsNullOrEmpty(tokenExpires) || string.IsNullOrEmpty(tokenString))
                return false;
            if (DateTime.Parse(tokenExpires) < DateTime.Now)
                return false;
            return true;
        }

        public static string GetAccessToken()
        {
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
            string accessTokenString = ConfigurationManager.AppSettings["AccessToken"];//从配置文件获取AccessToken
            Helper.WriteLog("AccessToken Cache:" + accessTokenString);
            string accessTokenExpires = ConfigurationManager.AppSettings["AccessTokenExpires"];//从配置文件获取AccessTokenExpires
            Helper.WriteLog("AccessToken Expires:" + accessTokenExpires);
            if (CheckTokenString(accessTokenString, accessTokenExpires))
            {
                Helper.WriteLog("AccessToken Cache:" + accessTokenString);
                return accessTokenString;
            }
            string url = "https://oapi.dingtalk.com/gettoken?corpid={0}&corpsecret={1}";
            url = string.Format(url, corpId, corpSecret);
            string token = Helper.GetCorpExecuteResult(url);
            var ser = new DataContractJsonSerializer(typeof(CorpAccessToken));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(token));
            CorpAccessToken accessToken = (CorpAccessToken)ser.ReadObject(ms);

            ConfigurationManager.AppSettings["AccessToken"] = accessToken.access_token;
            ConfigurationManager.AppSettings["AccessTokenExpires"] =
                DateTime.Now.AddSeconds(accessToken.expires_in).ToString("yyyy-MM-dd HH:mm:ss");

            Helper.WriteLog("AccessToken:" + accessToken.access_token);

            return accessToken.access_token;
        }

        public static string GetJsApiTicket()
        {
            string jsApiTicketString = ConfigurationManager.AppSettings["JsApiTicket"];//从配置文件获取JsApiTicket
            Helper.WriteLog("JsApiTicket Cache:" + jsApiTicketString);
            string jsApiTicketExpires = ConfigurationManager.AppSettings["JsApiTicketExpires"];//从配置文件获取JsApiTicketExpires
            Helper.WriteLog("JsApiTicket Expires:" + jsApiTicketExpires);
            if (CheckTokenString(jsApiTicketString, jsApiTicketExpires))
            {
                Helper.WriteLog("JsApiTicket Cache:" + jsApiTicketString);
                return jsApiTicketString;
            }

            string accessToken = GetAccessToken();
            string url = "https://oapi.dingtalk.com/get_jsapi_ticket?access_token={0}&type=jsapi";
            url = string.Format(url, accessToken);
            string jsApiTicketToken = Helper.GetCorpExecuteResult(url);
            var ser = new DataContractJsonSerializer(typeof(JsApiTicket));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsApiTicketToken));
            JsApiTicket jsApiTicket = (JsApiTicket)ser.ReadObject(ms);

            ConfigurationManager.AppSettings["JsApiTicket"] = jsApiTicket.ticket;
            ConfigurationManager.AppSettings["JsApiTicketExpires"] =
                DateTime.Now.AddSeconds(jsApiTicket.expires_in).ToString("yyyy-MM-dd HH:mm:ss");

            Helper.WriteLog("JsApiTicket:" + jsApiTicket.ticket);

            return jsApiTicket.ticket;
        }


    }
}
