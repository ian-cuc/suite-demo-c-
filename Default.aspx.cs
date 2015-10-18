using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Suite.Common;
using System.Configuration;
using Suite.API;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using Model;

namespace Suite
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            #region 获取套件配置参数
            string m_Token = ConfigurationManager.AppSettings["Token"];
            if (string.IsNullOrEmpty(m_Token))
                Helper.WriteLog("token没有配置");
            string m_SuiteKey = ConfigurationManager.AppSettings["SuiteKey"];
            if (string.IsNullOrEmpty(m_Token))
                Helper.WriteLog("token没有配置");
            string m_EncodingAESKey = ConfigurationManager.AppSettings["EncodingAESKey"];
            if (string.IsNullOrEmpty(m_Token))
                Helper.WriteLog("token没有配置");
            #endregion
            //构造DingTalkCrypt
            DingTalkCrypt dingTalk = new DingTalkCrypt(m_Token, m_EncodingAESKey, m_SuiteKey);

            string sVerifyMsgSig = txtMsgSig.Text.Trim();
            string sVerifyTimeStamp = txtTimeStamp.Text.Trim();
            string sVerifyNonce = txtNonce.Text.Trim();
            string encryptStr = txtEncryptStr.Text.Trim();

            string sEchoStr = "";

            int ret = dingTalk.VerifyURL(sVerifyMsgSig, sVerifyTimeStamp, sVerifyNonce, encryptStr, ref sEchoStr);
            txtEchoStr.Text = sEchoStr;
            

            //string sMsg = "";
            //ret = dingTalk.DecryptMsg(sVerifyMsgSig, sVerifyTimeStamp, sVerifyNonce, encryptStr, ref sMsg);

            //string sEncryptMsg = "";
            //dingTalk.EncryptMsg("success", sVerifyTimeStamp, sVerifyNonce, ref sEncryptMsg);

            // sVerifyTimeStamp = "1441713878094";
            // sVerifyNonce = "ZjJmiMAu";
            //dingTalk.EncryptMsg("success", sVerifyTimeStamp, sVerifyNonce, ref sEncryptMsg);
        }
    }
}
