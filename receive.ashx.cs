using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using Model;
using Newtonsoft.Json;
using Suite.API;
using Suite.Common;

namespace Suite
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Receive : IHttpHandler
    {
        private string GetPostParam(HttpContext context)
        {
            if ("POST" == context.Request.RequestType)
            {
                Stream sm = context.Request.InputStream;//获取post正文
                int len = (int)sm.Length;//post数据长度
                byte[] inputByts = new byte[len];//字节数据,用于存储post数据
                sm.Read(inputByts, 0, len);//将post数据写入byte数组中
                sm.Close();//关闭IO流

                //**********下面是把字节数组类型转换成字符串**********

                string data = Encoding.UTF8.GetString(inputByts);//转为String
                data = data.Replace("{\"encrypt\":\"", "").Replace("\"}","");
                return data;
            }
            return "get方法";
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                #region 获取套件配置参数
                string mToken = ConfigurationManager.AppSettings["Token"];
                string mSuiteKey = "";
                string mEncodingAesKey = ConfigurationManager.AppSettings["EncodingAESKey"];
                //mSuiteKey = "suite4xxxxxxxxxxxxxxx";
                #endregion

                #region 获取回调URL里面的参数
                //url中的签名
                string msgSignature = context.Request["signature"];
                //url中的时间戳
                string timeStamp = context.Request["timestamp"];
                //url中的随机字符串
                string nonce = context.Request["nonce"];
                //post数据包数据中的加密数据
                string encryptStr = GetPostParam(context);
                #endregion

                string sEchoStr = "";

                #region 验证回调的url
                SuiteAuth suiteAuth = new SuiteAuth();

                var ret = suiteAuth.VerifyURL(mToken, mEncodingAesKey, msgSignature, timeStamp, nonce, encryptStr,
                    ref mSuiteKey);

                if (ret != 0)
                {
                    Helper.WriteLog("ERR: VerifyURL fail, ret: " + ret);
                    return;
                }
                #endregion

                #region
                //构造DingTalkCrypt
                DingTalkCrypt dingTalk = new DingTalkCrypt(mToken, mEncodingAesKey, mSuiteKey);

                string plainText = "";
                dingTalk.DecryptMsg(msgSignature, timeStamp, nonce, encryptStr, ref plainText);
                Hashtable tb = (Hashtable)JsonConvert.DeserializeObject(plainText, typeof(Hashtable));
                string eventType = tb["EventType"].ToString();
                string res = "success";
                Helper.WriteLog("plainText:" + plainText);
                Helper.WriteLog("eventType:" + eventType);
                switch (eventType)
                {
                    case "suite_ticket"://定时推送Ticket
                        ConfigurationManager.AppSettings["SuiteTicket"] = tb["SuiteTicket"].ToString();
                        mSuiteKey = tb["SuiteKey"].ToString();
                        suiteAuth.SaveSuiteTicket(tb);
                        break;
                    case "tmp_auth_code"://钉钉推送过来的临时授权码
                        ConfigurationManager.AppSettings["TmpAuthCode"] = tb["AuthCode"].ToString();
                        suiteAuth.SaveTmpAuthCode(tb);
                        break;
                    case "change_auth":// do something;
                        break;
                    case "check_update_suite_url":
                        res = tb["Random"].ToString();
                        mSuiteKey = tb["TestSuiteKey"].ToString();
                        break;
                }

                timeStamp = Helper.GetTimeStamp().ToString();
                string encrypt = "";
                string signature = "";
                dingTalk = new DingTalkCrypt(mToken, mEncodingAesKey, mSuiteKey);
                dingTalk.EncryptMsg(res, timeStamp, nonce, ref encrypt, ref signature);
                Hashtable jsonMap = new Hashtable
                {
                    {"msg_signature", signature},
                    {"encrypt", encrypt},
                    {"timeStamp", timeStamp},
                    {"nonce", nonce}
                };
                string result = JsonConvert.SerializeObject(jsonMap);
                context.Response.Write(result);
                #endregion
            }
            catch (Exception ex)
            {
                Helper.WriteLog(DateTime.Now + ex.Message);
            }
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
