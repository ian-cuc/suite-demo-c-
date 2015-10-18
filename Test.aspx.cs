using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Runtime.Serialization.Json;
using Suite.Common;
using System.IO;
using System.Text;
using Model;
using SubSonic;
using Suite.API;

namespace Suite
{
    public partial class Test : System.Web.UI.Page
    {
        private string m_SuiteKey;
        private string m_SuiteSecret;

        protected void Page_Load(object sender, EventArgs e)
        {
            Helper.WriteLog("result:");
            ////#region 获取套件配置参数
            ////m_SuiteKey = ConfigurationManager.AppSettings["SuiteKey"];
            ////m_SuiteSecret = ConfigurationManager.AppSettings["SuiteSecret"];
            ////#endregion
            //string accessToken = Helper.GetAccessToken();
            //string url = "https://oapi.dingtalk.com/user/simplelist?access_token="+ accessToken + "&department_id=1";
            //string result = Helper.GetCorpExecuteResult(url);
            List<SuiteKeyInfo> suikKeyList = new Select().From(SuiteKeyInfo.Schema).ExecuteTypedList<SuiteKeyInfo>();
            foreach (SuiteKeyInfo suiteKeyInfo in suikKeyList)
            {
                DingApiDispatch dingApi = new DingApiDispatch(suiteKeyInfo.SuiteKey, suiteKeyInfo.SuiteSecret,
                  suiteKeyInfo.SuiteTicket);
                SuiteCorpInfo corpInfo= new SuiteCorpInfo(SuiteCorpInfo.Columns.SuiteKey, suiteKeyInfo.SuiteKey);
                AuthCorp authCorp = dingApi.GetAuthInfo(suiteKeyInfo.SuiteKey, suiteKeyInfo.SuiteToken, corpInfo.Corpid,
                   corpInfo.PermanentCode);
                if (authCorp.errcode == "0")
                {
                    foreach (AgentInfo angInfo in authCorp.auth_info.agent)
                    {
                        SuiteCorpAgent suiteCorpAgent = new Select().From(SuiteCorpAgent.Schema)
                            .Where(SuiteCorpAgent.Columns.CorpId).IsEqualTo(corpInfo.Corpid)
                            .And(SuiteCorpAgent.Columns.AgentId).IsEqualTo(angInfo.agentid)
                            .ExecuteSingle<SuiteCorpAgent>() ?? new SuiteCorpAgent();
                        Agent agent = dingApi.GetAgent(suiteKeyInfo.SuiteKey, suiteKeyInfo.SuiteToken, corpInfo.Corpid,
                            corpInfo.PermanentCode, suiteCorpAgent.AgentId);
                        suiteCorpAgent.Description = agent.description;
                        suiteCorpAgent.IsClose = agent.close;
                    }
                }
            }

            //List<SuiteCorpAgent> agentList = new Select().From(SuiteCorpAgent.Schema).ExecuteTypedList<SuiteCorpAgent>();
            //foreach (SuiteCorpAgent agent in agentList)
            //{
            //    SuiteCorpInfo suiteCorpInfo = new Select().From(SuiteCorpInfo.Schema)
            //        .Where(SuiteCorpInfo.Columns.SuiteKey).IsEqualTo(agent.SuiteKey)
            //        .ExecuteSingle<SuiteCorpInfo>();
            //    SuiteKeyInfo suiteKeyInfo = new SuiteKeyInfo(agent.SuiteKey);

            //    DingApiDispatch dingApi = new DingApiDispatch(suiteKeyInfo.SuiteKey, suiteKeyInfo.SuiteSecret,
            //      suiteKeyInfo.SuiteTicket);
            //    if (suiteCorpInfo.TokenExpires < DateTime.Now)
            //    {
            //        CorpAccessToken corpAccessToken = dingApi.GetCorpToken(suiteKeyInfo.SuiteToken, suiteCorpInfo.Corpid,
            //   suiteCorpInfo.PermanentCode);

            //        suiteCorpInfo.AccessToken = corpAccessToken.access_token;
            //        suiteCorpInfo.TokenExpires = DateTime.Now.AddSeconds(corpAccessToken.expires_in);
            //        suiteCorpInfo.Save();
            //    }

            //    string result = dingApi.ActivateSuite(suiteKeyInfo.SuiteKey, suiteKeyInfo.SuiteToken, suiteCorpInfo.Corpid,
            //        suiteCorpInfo.PermanentCode);
            //}

        }
        private void GetUserCode(string code)
        {
            try
            {
                //string url = "https://oapi.dingtalk.com/user/getuserinfo?access_token={0}&code={1}";

                //string accessToken = Helper.GetAccessToken();

                //url = string.Format(url, accessToken, code);

                string result = "{\"deviceId\":\"7535b6af00572de1bd22da2067156b9e\",\"errcode\":0,\"errmsg\":\"ok\",\"is_sys\":true,\"userid\":\"manager185\"}";//Helper.GetCorpExecuteResult(url);

                var ser = new DataContractJsonSerializer(typeof(UserCode));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                UserCode userCode = (UserCode)ser.ReadObject(ms);

                result = GetUserInfo(userCode.userid);
                //context.Response.Write(result);
            }
            catch (Exception ex)
            {
                Helper.WriteLog("result: " + ex.Message);
                //context.Response.Write(ex.Message);
            }
        }

        private string GetUserInfo(string userId)
        {
            string accessToken = Helper.GetAccessToken();
            string url = "https://oapi.dingtalk.com/user/get?access_token={0}&userid={1}";

            url = string.Format(url, accessToken, userId);

            string result = Helper.GetCorpExecuteResult(url);
            var ser = new DataContractJsonSerializer(typeof(UserInfo));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            UserInfo userInfo = (UserInfo)ser.ReadObject(ms);

            return result;
        }

        protected void btnGetSuiteToken_Click(object sender, EventArgs e)
        {
            string ticket = txtSuiteTicket.Text.Trim();
            DingApiDispatch dingApi = new DingApiDispatch(m_SuiteKey, m_SuiteSecret, ticket);
            //获取套件访问Token
            dingApi.GetSuiteAccessToken();
        }

        protected void btnGetPermanentCode_Click(object sender, EventArgs e)
        {
            string ticket = txtSuiteTicket.Text.Trim();
            DingApiDispatch dingApi = new DingApiDispatch(m_SuiteKey, m_SuiteSecret, ticket);
            //获取企业的永久授权码
            //PermanentCode perCode = dingApi.GetPermanentCode(txtTempAuthCode.Text.Trim());
            //txtPermanentCode.Text = perCode.permanent_code;
            //txtCorpId.Text = perCode.auth_corp_info.corpid;
        }

        protected void btnGetCorpToken_Click(object sender, EventArgs e)
        {
            string ticket = txtSuiteTicket.Text.Trim();
            DingApiDispatch dingApi = new DingApiDispatch(m_SuiteKey, m_SuiteSecret, ticket);
            string corpId = txtCorpId.Text.Trim();
            string permanentCode = txtPermanentCode.Text.Trim();
            //CorpAccessToken corpAccessToken = dingApi.GetCorpToken(corpId, permanentCode);
            //txtCorpAccessToken.Text = corpAccessToken.access_token;
        }

        protected void btnGetAuthInfo_Click(object sender, EventArgs e)
        {
            string ticket = txtSuiteTicket.Text.Trim();
            DingApiDispatch dingApi = new DingApiDispatch(m_SuiteKey, m_SuiteSecret, ticket);
            string corpId = txtCorpId.Text.Trim();
            string permanentCode = txtPermanentCode.Text.Trim();
            //AuthCorp authCorp = dingApi.GetAuthInfo(m_SuiteKey, corpId, permanentCode);
        }

        protected void btnActivateSuite_Click(object sender, EventArgs e)
        {
            string ticket = txtSuiteTicket.Text.Trim();
            DingApiDispatch dingApi = new DingApiDispatch(m_SuiteKey, m_SuiteSecret, ticket);
            string corpId = txtCorpId.Text.Trim();
            string permanentCode = txtPermanentCode.Text.Trim();
            //string result = dingApi.ActivateSuite(m_SuiteKey, corpId, permanentCode);
        }
    }
}