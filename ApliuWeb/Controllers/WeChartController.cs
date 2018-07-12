using ApliuWeb.WeChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApliuWeb.Controllers
{
    public class WeChartController : Controller
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

            ViewData["AppId"] = WeChartBase.WxAppId;
            ViewData["Timestamp"] = timestamp;
            ViewData["NonceStr"] = nonceStr;
            ViewData["Signature"] = signature;

            return View();
        }

        //
        // GET: /WeChart/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /WeChart/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /WeChart/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /WeChart/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /WeChart/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /WeChart/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /WeChart/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /WeChart/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
