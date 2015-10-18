using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Suite.Common
{
    public class DingApiDispatch
    {
        private string suite_key;
        private string suite_secret;
        private string suite_ticket;

        public DingApiDispatch(string suiteKey, string suiteSecret, string suiteTicket)
        {
            suite_key = suiteKey;
            suite_secret = suiteSecret;
            suite_ticket = suiteTicket;
        }

        /// <summary>
        /// 将需要发送的消息内容以POST方式传送到指定的钉钉接口地址
        /// </summary>
        /// <param name="postUrl">调用钉钉接口地址</param>
        /// <param name="postdata">发送Post消息内容</param>
        /// <returns></returns>
        public string WebRequestPost(string postUrl, string postdata)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                CookieContainer cc = new CookieContainer();
                request = (HttpWebRequest)WebRequest.Create(postUrl);
                request.Method = "Post";
                request.ContentType = "application/json;charset=UTF-8";
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
        /// 获取套件访问Token（suite_access_token）
        /// </summary>
        /// <returns>
        /// suite_access_token	应用套件access_token
        ///  expires_in 有效期
        /// </returns>
        public SuiteAccessToken GetSuiteAccessToken()
        {
            try
            {
                string postUrl = "https://oapi.dingtalk.com/service/get_suite_token";
                string postData = "{\"suite_key\":\"" + suite_key + "\",\"suite_secret\": \"" + suite_secret + "\",  \"suite_ticket\": \"" + suite_ticket + "\"}";

                string result = WebRequestPost(postUrl, postData);

                var ser = new DataContractJsonSerializer(typeof(SuiteAccessToken));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                SuiteAccessToken suiteAccessToken = (SuiteAccessToken)ser.ReadObject(ms);

                return suiteAccessToken;
            }
            catch (Exception ex)
            {
                Helper.WriteLog("Err:" + ex.Message);
                return null;

            }
        }


        /// <summary>
        /// 获取企业的永久授权码
        /// </summary>
        /// <param name="tmpAuthCode">回调接口（tmp_auth_code）获取的临时授权码</param>
        /// <returns>
        /// permanent_code	永久授权码
        ///  corp_info 授权方企业信息
        /// corpid 授权方企业id
        /// corp_name 授权方企业名称
        /// </returns>
        public PermanentCode GetPermanentCode(string suiteAccessToken, string tmpAuthCode)
        {

            string postUrl = "https://oapi.dingtalk.com/service/get_permanent_code?suite_access_token=" + suiteAccessToken;
            string postData = "{\"tmp_auth_code\": \"" + tmpAuthCode + "\"}";

            string result = WebRequestPost(postUrl, postData);

            var ser = new DataContractJsonSerializer(typeof(PermanentCode));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            PermanentCode permanentCode = (PermanentCode)ser.ReadObject(ms);

            return permanentCode;

        }

        /// <summary>
        /// 获取企业授权的access_token
        /// </summary>
        /// <param name="authCorpid">授权方corpid</param>
        /// <param name="permanentCode">永久授权码，通过GetPermanentCode获取</param>
        /// <returns>access_token	授权方（企业）access_token
        ///  expires_in 授权方（企业）access_token超时时间
        /// </returns>
        public CorpAccessToken GetCorpToken(string suiteAccessToken, string authCorpid, string permanentCode)
        {
            string postUrl = "https://oapi.dingtalk.com/service/get_corp_token?suite_access_token=" + suiteAccessToken;
            string postData = "{\"auth_corpid\": \"" + authCorpid + "\",\"permanent_code\": \"" + permanentCode + "\"}";

            string result = WebRequestPost(postUrl, postData);
            var ser = new DataContractJsonSerializer(typeof(CorpAccessToken));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            CorpAccessToken corpAccessToken = (CorpAccessToken)ser.ReadObject(ms);

            ConfigurationManager.AppSettings["AccessToken"] = corpAccessToken.access_token;
            ConfigurationManager.AppSettings["AccessTokenExpires"] =
                DateTime.Now.AddSeconds(corpAccessToken.expires_in).ToString("yyyy-MM-dd HH:mm:ss");

            return corpAccessToken;

        }


        /// <summary>
        /// 获取企业授权的授权数据
        /// </summary>
        /// <param name="suiteKey">应用套件key</param>
        /// <param name="authCorpid">授权方corpid</param>
        /// <param name="permanentCode">永久授权码，通过get_permanent_code获取</param>
        /// <returns>
        ///auth_corp_info	授权方企业信息
        ///corpid           授权方企业id
        ///corp_name        授权方企业名称
        ///corp_logo_url    企业logo
        ///auth_info        授权信息
        ///agent            授权的应用信息
        ///agentid          授权方应用id
        ///agent_name       授权方应用名字
        ///logo_url         授权方应用头像
        ///appid            服务商套件中的对应应用id
        /// </returns>
        public AuthCorp GetAuthInfo(string suiteKey, string suiteAccessToken, string authCorpid, string permanentCode)
        {
            string postUrl = "https://oapi.dingtalk.com/service/get_auth_info?suite_access_token=" + suiteAccessToken;
            string postData = "{\"suite_key\": \"" + suiteKey + "\",\"auth_corpid\": \"" + authCorpid + "\",\"permanent_code\": \"" + permanentCode + "\"}";
            string result = WebRequestPost(postUrl, postData);
            Helper.WriteLog("result:"+ result);
            var ser = new DataContractJsonSerializer(typeof(AuthCorp));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            AuthCorp authCorp = (AuthCorp)ser.ReadObject(ms);

            return authCorp;

        }

        /// <summary>
        /// 获取企业的应用信息
        /// </summary>
        /// <param name="suiteAccessToken">套件访问Token</param>
        /// <param name="suiteKey">应用套件key</param>
        /// <param name="authCorpid">授权方corpid</param>
        /// <param name="permanentCode">永久授权码，从get_permanent_code接口中获取</param>
        /// <param name="agentId">授权方应用id</param>
        /// <returns>
        /// agentid	授权方企业应用id
        ///name 授权方企业应用名称
        ///logo_url 授权方企业应用头像
        ///description 授权方企业应用详情
        ///close 授权方企业应用是否被禁用
        /// </returns>
        public Agent GetAgent(string suiteKey, string suiteAccessToken, string authCorpid, string permanentCode, string agentId)
        {
            string postUrl = "https://oapi.dingtalk.com/service/get_agent?suite_access_token=" + suiteAccessToken;
            string postData = "{\"suite_key\": \"" + suiteKey + "\",\"auth_corpid\": \"" + authCorpid + "\",\"permanent_code\": \"" + permanentCode + "\",\"agentid\": \"" + agentId + "\"}";

            string result = WebRequestPost(postUrl, postData);

            var ser = new DataContractJsonSerializer(typeof(Agent));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            Agent agent = (Agent)ser.ReadObject(ms);

            return agent;
        }

        /// <summary>
        /// 激活授权套件
        /// </summary>
        /// <param name="suiteAccessToken"></param>
        /// <param name="suiteKey">应用套件key</param>
        /// <param name="authCorpid">授权方corpid</param>
        /// <param name="permanentCode">永久授权码，从get_permanent_code接口中获取</param>
        /// <returns></returns>
        public string ActivateSuite(string suiteKey, string suiteAccessToken, string authCorpid, string permanentCode)
        {
            string postUrl = "https://oapi.dingtalk.com/service/activate_suite?suite_access_token=" + suiteAccessToken;
            string postData = "{\"suite_key\": \"" + suiteKey + "\",\"auth_corpid\": \"" + authCorpid +
                              "\",\"permanent_code\": \"" + permanentCode + "\"}";

            string result = WebRequestPost(postUrl, postData);

            return result;
        }
    }
}
