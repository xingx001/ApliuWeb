using ApliuTools;
using ApliuWeb.WeChart;
using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace ApliuWeb.Controllers
{
    public class WxController : ApiController
    {
        /// <summary>
        /// 微信公众号获取Token Api 消息验证、接收微信服务器推送的消息
        /// /api/wx
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage Main()
        {
            String respContent = "Error: 非法请求";
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
            string echostr = HttpContext.Current.Request.QueryString["echostr"];
            string openid = HttpContext.Current.Request.QueryString["openid"];
            //用于验证加密内容的签名
            string msg_signature = HttpContext.Current.Request.QueryString["msg_signature"];
            string encrypt_type = HttpContext.Current.Request.QueryString["encrypt_type"];

            Logger.WriteLog("请求方式：" + HttpContext.Current.Request.HttpMethod.ToUpper() + "，请求原地址：" + HttpContext.Current.Request.Url.ToString());

            //验证消息是否来自微信服务器
            if (WeChartBase.CheckSignature(signature, timestamp, nonce))
            {
                if (HttpContext.Current.Request.HttpMethod.ToUpper() == "GET")
                {
                    if (!string.IsNullOrEmpty(echostr))
                    {
                        respContent = echostr;
                    }
                }
                else if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
                {
                    respContent = "Error: 处理失败";
                    try
                    {
                        using (Stream stream = HttpContext.Current.Request.InputStream)
                        {
                            Byte[] postBytes = new Byte[stream.Length];
                            stream.Read(postBytes, 0, (Int32)stream.Length);
                            string beforeReqData = WeChartBase.WxEncoding.GetString(postBytes);
                            if (!string.IsNullOrEmpty(beforeReqData))
                            {
                                if ("aes".Equals(encrypt_type))//(WeChartBase.IsSecurity)
                                {
                                    Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(WeChartBase.WxToken, WeChartBase.WxEncodingAESKey, WeChartBase.WxAppId);
                                    string afterReqData = String.Empty;  //解析之后的明文
                                    int reqRet = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, beforeReqData, ref afterReqData);
                                    if (reqRet == 0)
                                    {
                                        WxMessageHelp wxMsgHelp = new WxMessageHelp();
                                        String retMessage = wxMsgHelp.MessageHandle(afterReqData);
                                        string respData = String.Empty; //xml格式的密文
                                        int resqRet = wxcpt.EncryptMsg(retMessage, timestamp, nonce, ref respData);
                                        if (resqRet == 0)
                                        {
                                            respContent = respData;
                                        }
                                        else
                                        {
                                            Logger.WriteLog("Error：接收微信服务器推送的消息，加密报文失败，ret: " + resqRet);
                                        }
                                    }
                                    else
                                    {
                                        Logger.WriteLog("Error：接收微信服务器推送的消息，解密报文失败，ret: " + reqRet);
                                    }
                                }
                                else
                                {
                                    WxMessageHelp wxMsgHelp = new WxMessageHelp();
                                    String retMessage = wxMsgHelp.MessageHandle(beforeReqData);
                                    respContent = retMessage;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog("Error：接收微信服务器推送的消息，处理失败，详情: " + ex.Message);
                    }
                }
            }

            return WeChartBase.encapResult(respContent);
        }

        /// <summary>
        /// 默认是Post
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public String Test()
        {
            //SELECT * FROM sysprocesses where loginame='apliuweb'
            //select * from test;waitfor delay '00:00:05';
            //如果加上(nolock)标记，则是以ReadUncommitted执行该表的查询事务
            String respContent = "ERROR";
            try
            {
                respContent = "测试";
                return respContent;
                //DataAccess.Instance.TestCeshi();

                DataAccess.Instance.BeginTransaction(10);

                string sql01 = "insert into Test values('" + Guid.NewGuid().ToString().ToLower() + "','BeginTransaction001','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
                bool p1 = DataAccess.Instance.PostData(sql01);

                string sql02 = "insert into Test values('" + Guid.NewGuid().ToString().ToLower() + "','BeginTransaction002','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
                bool p2 = DataAccess.Instance.PostData(sql02);


                DataSet ds = DataAccess.Instance.GetData("select * from Test where CreateTime like '%" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:s") + "%';waitfor delay '00:00:01';");
                if (ds != null && ds.Tables.Count > 0) respContent = JsonHelper.JsonSerialize(ds.Tables[0]);
                Thread.Sleep(5000);

                DataAccess.Instance.Rollback();
            }
            catch (Exception ex)
            {
                respContent = ex.Message;
            }

            //MsgCryptTest.Sample.Main(null);
            //Logger.WriteLog("Api Wx Test");
            return respContent;
        }
    }
}
