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
            String respContent = String.Empty;
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "GET")
            {
                respContent = "Error: 非法请求";
                string signature = HttpContext.Current.Request.QueryString["signature"];
                string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
                string nonce = HttpContext.Current.Request.QueryString["nonce"];
                string echostr = HttpContext.Current.Request.QueryString["echostr"];

                if (WeChartBase.CheckSignature(signature, timestamp, nonce, echostr))
                {
                    if (!string.IsNullOrEmpty(echostr))
                    {
                        respContent = echostr;
                    }
                }
            }
            else if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                respContent = "Error: 处理失败";
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    string postString = WeChartBase.WxEncoding.GetString(postBytes);
                    if (!string.IsNullOrEmpty(postString))
                    {
                        WxMessageHelp wxMsgHelp = new WxMessageHelp();
                        respContent = wxMsgHelp.ReturnMessage(postString);
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
            return "OK";
        }
    }
}
