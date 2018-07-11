using ApliuTools;
using ApliuWeb.WeChart;
using System;
using System.IO;
using System.Net.Http;
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
                            string postString = WeChartBase.WxEncoding.GetString(postBytes);
                            if (!string.IsNullOrEmpty(postString))
                            {
                                if (WeChartBase.IsSecurity)
                                {
                                    Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(WeChartBase.WxToken, WeChartBase.WxEncodingAESKey, WeChartBase.WxAppId);
                                    string reqData = String.Empty;  //解析之后的明文
                                    int reqRet = wxcpt.DecryptMsg(signature, timestamp, nonce, postString, ref reqData);
                                    if (reqRet == 0)
                                    {
                                        WxMessageHelp wxMsgHelp = new WxMessageHelp();
                                        String retMessage = wxMsgHelp.ReturnMessage(reqData);
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
                                    String retMessage = wxMsgHelp.ReturnMessage(postString);
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
            MsgCryptTest.Sample.Main(null);
            return "OK";
        }
    }
}
