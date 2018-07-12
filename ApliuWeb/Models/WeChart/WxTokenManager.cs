﻿using ApliuTools;
using ApliuTools.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ApliuWeb.WeChart
{
    public class WxTokenManager
    {
        /// <summary>
        /// 获取access_token的地址
        /// </summary>
        public static string requestAccessTokenUri
        {
            get
            {
                return "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + WeChartBase.WxAppId + "&secret=" + WeChartBase.WxAppSecret;
            }
        }

        private static string _accessToken;
        /// <summary>
        /// 公众号的全局唯一接口调用凭据
        /// </summary>
        public static string AccessToken
        {
            get { return _accessToken; }
        }

        /// <summary>
        /// AccessToken有效时间 秒，同时也一起刷新jsApiTicket
        /// </summary>
        private static String expires_in;

        /// <summary>
        /// AccessToken过期计时器
        /// </summary>
        private static System.Timers.Timer timer = null;

        /// <summary>
        /// 获取jsapi_ticket的地址
        /// </summary>
        public static string requestJsTicketUri
        {
            get
            {
                return "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + _accessToken + "&type=jsapi";
            }
        }

        private static string _jsApiTicket;
        /// <summary>
        /// jsapi_ticket是公众号用于调用微信JS接口的临时票据
        /// </summary>
        public static string jsApiTicket
        {
            get { return _jsApiTicket; }
        }

        /// <summary>
        /// 重新获取AccessToken
        /// </summary>
        private static void GetAccessToken()
        {
            try
            {
                //AccessToken
                HttpResponseMessage respAccessToken = HttpRequestHelper.HttpGetAsync(requestAccessTokenUri).Result;
                respAccessToken.EnsureSuccessStatusCode();
                String accessTokenContent = respAccessToken.Content.ReadAsStringAsync().Result;
                JObject jObjAccessToken = Newtonsoft.Json.JsonConvert.DeserializeObject(accessTokenContent) as JObject;
                if (jObjAccessToken["access_token"] == null)
                {
                    throw new Exception(accessTokenContent);
                }
                _accessToken = jObjAccessToken["access_token"].ToString();
                expires_in = jObjAccessToken["expires_in"].ToString();
                if (timer == null) Logger.WriteLog("初始化Access_Token完成，Access_Token：" + AccessToken);

                //JsApiTicket
                HttpResponseMessage respJsTicket = HttpRequestHelper.HttpGetAsync(requestJsTicketUri).Result;
                respJsTicket.EnsureSuccessStatusCode();
                String jsTicketContent = respJsTicket.Content.ReadAsStringAsync().Result;
                JObject jObjJsTicket = Newtonsoft.Json.JsonConvert.DeserializeObject(jsTicketContent) as JObject;
                if (jObjJsTicket["ticket"] == null)
                {
                    throw new Exception(jsTicketContent);
                }
                _jsApiTicket = jObjJsTicket["ticket"].ToString();
                if (timer == null) Logger.WriteLog("初始化JsApiTicket完成，JsApiTicket：" + _jsApiTicket);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("初始化access_token失败，详情：" + ex.Message);
            }
        }

        /// <summary>
        /// 启动access_token管理任务 自动刷新
        /// </summary>
        public static void TokenTaskStart()
        {
            GetAccessToken();
            Thread tokenThread = new Thread(new ThreadStart(RefreshAccessToken));
            tokenThread.Start();
        }

        private static void RefreshAccessToken()
        {
            //第一次运行创建计时器
            if (timer == null)
            {
                int delaySecond = 0;
                if (!Int32.TryParse(expires_in, out delaySecond))
                {
                    delaySecond = 7200;
                }
                timer = new System.Timers.Timer(delaySecond * 1000);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();

                Logger.WriteLog("启动access_token管理任务完成");
            }
        }

        /// <summary>
        /// 计时器计时有效期到期后，执行刷新AccessToken
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetAccessToken();
            Logger.WriteLog("access_token管理任务自动刷新，access_token：" + AccessToken);
        }
    }
}