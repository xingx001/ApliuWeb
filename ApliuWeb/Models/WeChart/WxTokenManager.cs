using ApliuTools;
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
        public static string requestUri
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
        /// AccessToken有效时间 秒
        /// </summary>
        private static String expires_in;

        /// <summary>
        /// AccessToken过期计时器
        /// </summary>
        private static System.Timers.Timer timer = null;

        #region jsapiTicket 暂时未用到
        [Obsolete]
        public static string requestJsTicketUri
        {
            get
            {
                return "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + _accessToken + "&type=jsapi";
            }
        }

        [Obsolete]
        private static string jsapiTicket;

        [Obsolete]
        public static string JSapiTicket
        {
            get { return jsapiTicket; }
            set { jsapiTicket = value; }
        }
        #endregion

        /// <summary>
        /// 重新获取AccessToken
        /// </summary>
        private static void GetAccessToken()
        {
            HttpClient http = new HttpClient();
            try
            {
                //AccessToken
                Task<HttpResponseMessage> taskResponse = http.GetAsync(requestUri);
                HttpResponseMessage response = taskResponse.Result;
                response.EnsureSuccessStatusCode();
                Task<String> taskContent = response.Content.ReadAsStringAsync();
                String content = taskContent.Result;
                JObject jObj = Newtonsoft.Json.JsonConvert.DeserializeObject(content) as JObject;
                if (jObj["access_token"] == null)
                {
                    throw new Exception(content);
                }
                _accessToken = jObj["access_token"].ToString();
                expires_in = jObj["expires_in"].ToString();

                Logger.WriteLog("初始化access_token完成，access_token：" + AccessToken);
                //jsTicket
                //response = await http.GetAsync(requestJsTicketUri);
                //response.EnsureSuccessStatusCode();
                //content = await response.Content.ReadAsStringAsync();
                //jObj = Newtonsoft.Json.JsonConvert.DeserializeObject(content) as JObject;
                //jsapiTicket = jObj["ticket"].ToString();
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

                Logger.WriteLog("启动access_token管理任务完成，access_token：" + AccessToken);
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