using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using SubSonic;
using Suite.API;

namespace Suite.Common
{
    public class SuiteAuth
    {
        public int VerifyURL(string mToken,string mEncodingAesKey, string msgSignature, string timeStamp,string nonce,string encryptStr,ref string suiteKey)
        {
            var ret = 0;
            List<SuiteKeyInfo> suikKeyList = new Select().From(SuiteKeyInfo.Schema).ExecuteTypedList<SuiteKeyInfo>();
            foreach (SuiteKeyInfo suiteKeyInfo in suikKeyList)
            {
                DingTalkCrypt dingTalk = new DingTalkCrypt(mToken, mEncodingAesKey, suiteKeyInfo.SuiteKey);
                string sEchoStr = "";
                ret = dingTalk.VerifyURL(msgSignature, timeStamp, nonce, encryptStr, ref sEchoStr);
                if (ret == 0)
                {
                    IsvReceive isvReceive = new IsvReceive
                    {
                        Signature = msgSignature,
                        Timestamp = timeStamp,
                        Nonce = nonce,
                        Encrypt = encryptStr,
                        EchoString = sEchoStr,
                        CreateTime = DateTime.Now
                    };
                    isvReceive.Save();
                    suiteKey = suiteKeyInfo.SuiteKey;
                    return ret;
                }
            }
            return ret;
        }

        /// <summary>
        /// 保存服务器定时推送过来的Ticket
        /// </summary>
        /// <param name="tb">解析后hashTable</param>
        public void SaveSuiteTicket(Hashtable tb)
        {
            try
            {
                SuiteKeyInfo suiteKeyInfo = new SuiteKeyInfo(tb["SuiteKey"]);

                suiteKeyInfo.SuiteTicket = tb["SuiteTicket"].ToString();
                suiteKeyInfo.TimeStamp = tb["TimeStamp"].ToString();

                DingApiDispatch dingApi = new DingApiDispatch(suiteKeyInfo.SuiteKey.Trim(), suiteKeyInfo.SuiteSecret.Trim(), suiteKeyInfo.SuiteTicket.Trim());
                //获取套件访问Token
                SuiteAccessToken suiteAccessToken = dingApi.GetSuiteAccessToken();
                if (suiteAccessToken != null)
                {
                    suiteKeyInfo.SuiteToken = suiteAccessToken.suite_access_token;
                    suiteKeyInfo.SuiteTokenExpires = DateTime.Now.AddSeconds(suiteAccessToken.expires_in);

                    suiteKeyInfo.Save();
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog("Err:" + ex.Message);
            }
        }

        /// <summary>
        /// 保存服务器推送临时授权码
        /// </summary>
        /// <param name="tb"></param>
        public void SaveTmpAuthCode(Hashtable tb)
        {
            SuiteKeyInfo suiteKeyInfo = new SuiteKeyInfo(tb["SuiteKey"]);
            DingApiDispatch dingApi = new DingApiDispatch(suiteKeyInfo.SuiteKey, suiteKeyInfo.SuiteSecret,
                suiteKeyInfo.SuiteTicket);

            //用临时授权码获得永久授权码
            PermanentCode permanentCode = dingApi.GetPermanentCode(suiteKeyInfo.SuiteToken, tb["AuthCode"].ToString());

            SuiteCorpInfo suiteCorp = new SuiteCorpInfo(SuiteCorpInfo.Columns.Corpid,
                permanentCode.auth_corp_info.corpid);
            suiteCorp.SuiteKey = suiteKeyInfo.SuiteKey;
            suiteCorp.TmpAuthCode = tb["AuthCode"].ToString();
            suiteCorp.PermanentCode = permanentCode.permanent_code;
            suiteCorp.Corpid = permanentCode.auth_corp_info.corpid;
            suiteCorp.CorpName = permanentCode.auth_corp_info.corp_name;

            //获取企业授权的access_token
            CorpAccessToken corpAccessToken = dingApi.GetCorpToken(suiteKeyInfo.SuiteToken, suiteCorp.Corpid,
                suiteCorp.PermanentCode);

            suiteCorp.AccessToken = corpAccessToken.access_token;
            suiteCorp.TokenExpires = DateTime.Now.AddSeconds(corpAccessToken.expires_in);

            //获取企业授权的授权数据
            AuthCorp authCorp = dingApi.GetAuthInfo(suiteKeyInfo.SuiteKey, suiteKeyInfo.SuiteToken, suiteCorp.Corpid,
                suiteCorp.PermanentCode);

            suiteCorp.CorpLogoUrl = authCorp.auth_corp_info.corp_logo_url;
            suiteCorp.Mobile = authCorp.auth_user_info.mobile;
            suiteCorp.Name = authCorp.auth_user_info.name;

            suiteCorp.Save();

            try
            {
                foreach (AgentInfo angInfo in authCorp.auth_info.agent)
                {
                    SuiteCorpAgent suiteCorpAgent = new Select().From(SuiteCorpAgent.Schema)
                        .Where(SuiteCorpAgent.Columns.CorpId).IsEqualTo(suiteCorp.Corpid)
                        .And(SuiteCorpAgent.Columns.AgentId).IsEqualTo(angInfo.agentid)
                        .ExecuteSingle<SuiteCorpAgent>() ?? new SuiteCorpAgent();
                    suiteCorpAgent.SuiteKey = suiteKeyInfo.SuiteKey;
                    suiteCorpAgent.CorpId = suiteCorp.Corpid;
                    suiteCorpAgent.AgentId = angInfo.agentid;
                    suiteCorpAgent.AgentName = angInfo.agent_name;
                    suiteCorpAgent.LogoUrl = angInfo.logo_url;
                    suiteCorpAgent.Appid = angInfo.appid;

                    dingApi.ActivateSuite(suiteKeyInfo.SuiteKey, suiteKeyInfo.SuiteToken, suiteCorp.Corpid,
                    suiteCorp.PermanentCode);
                    Agent agent = dingApi.GetAgent(suiteKeyInfo.SuiteKey, suiteKeyInfo.SuiteToken, suiteCorp.Corpid,
                        suiteCorp.PermanentCode, suiteCorpAgent.AgentId);
                    suiteCorpAgent.Description = agent.description;
                    suiteCorpAgent.IsClose = agent.close;

                    suiteCorpAgent.Save();
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog("Err:" + ex.Message);
            }
        }
    }
}
