using ApliuWeb.WeChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApliuWeb.Controllers
{
    public class WeChatController : Controller
    {
        public ActionResult JSSDK()
        {
            var jssdk = new WxJSSDKHelper();
            //获取时间戳
            var timestamp = WxJSSDKHelper.GetTimestamp();
            //获取随机码
            var nonceStr = WxJSSDKHelper.GetNoncestr();
            //获取票证
            var jsticket = WxTokenManager.jsApiTicket;
            //获取签名
            var signature = jssdk.GetSignature(jsticket, nonceStr, timestamp, HttpContext.Request.Url.AbsoluteUri);

            ViewData["AppId"] = WeChatBase.WxAppId;
            ViewData["Timestamp"] = timestamp;
            ViewData["NonceStr"] = nonceStr;
            ViewData["Signature"] = signature;

            return View();
        }

        //
        // GET: /WeChat/

        public ActionResult Index()
        {
            return View();
        }
    }
}
