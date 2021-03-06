using ApliuTools;
using ApliuWeb.WeChat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ApliuWeb.WeChat
{
    public class WxJSSDKHelper
    {
        public WxJSSDKHelper()
        {
            Parameters = new Hashtable();
        }

        protected Hashtable Parameters;

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        private void SetParameter(string parameter, string parameterValue)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                if (Parameters.Contains(parameter))
                {
                    Parameters.Remove(parameter);
                }

                Parameters.Add(parameter, parameterValue);
            }
        }

        private void ClearParameter()
        {
            Parameters.Clear();
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GetNoncestr()
        {
            Random random = new Random();
            return SecurityHelper.MD5Encrypt(random.Next(1000).ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// sha1加密
        /// </summary>
        /// <returns></returns>
        private string CreateSha1()
        {
            StringBuilder sb = new StringBuilder();
            ArrayList akeys = new ArrayList(Parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)Parameters[k];

                if (sb.Length == 0)
                {
                    sb.Append(k + "=" + v);
                }
                else
                {
                    sb.Append("&" + k + "=" + v);
                }
            }
            return SecurityHelper.SHA1Encrypt(sb.ToString(), WeChatBase.WxEncoding).ToString().ToLower();
        }

        /// <summary>
        /// 生成cardSign的加密方法
        /// </summary>
        /// <returns></returns>
        private string CreateCardSha1()
        {
            StringBuilder sb = new StringBuilder();
            ArrayList akeys = new ArrayList(Parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)Parameters[k];

                sb.Append(v);
            }
            return SecurityHelper.SHA1Encrypt(sb.ToString(), WeChatBase.WxEncoding).ToString().ToLower();
        }

        /// <summary>
        /// 获取JS-SDK权限验证的签名Signature
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="noncestr"></param>
        /// <param name="timestamp"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetSignature(string ticket, string noncestr, string timestamp, string url)
        {
            //清空Parameters
            ClearParameter();

            SetParameter("jsapi_ticket", ticket);
            SetParameter("noncestr", noncestr);
            SetParameter("timestamp", timestamp);
            SetParameter("url", url);

            return CreateSha1();
        }

        /// <summary>
        /// 获取位置签名AddrSign
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="noncestr"></param>
        /// <param name="timestamp"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetAddrSign(string appId, string appSecret, string noncestr, string timestamp, string url)
        {
            //清空Parameters
            ClearParameter();

            var accessToken = WxTokenManager.AccessToken;
            SetParameter("appId", appId);
            SetParameter("noncestr", noncestr);
            SetParameter("timestamp", timestamp);
            SetParameter("url", url);
            SetParameter("accesstoken", accessToken);

            return CreateSha1();
        }

        /// <summary>
        /// 获取卡券签名CardSign
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="locationId"></param>
        /// <param name="noncestr"></param>
        /// <param name="timestamp"></param>
        /// <param name="cardId"></param>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public string GetCardSign(string appId, string appSecret, string locationId, string noncestr, string timestamp, string cardId, string cardType)
        {
            //清空Parameters
            ClearParameter();

            SetParameter("appId", appId);
            SetParameter("appsecret", appSecret);
            SetParameter("location_id", locationId);
            SetParameter("nonce_str", noncestr);
            SetParameter("times_tamp", timestamp);
            SetParameter("card_id", cardId);
            SetParameter("card_type", cardType);

            return CreateCardSha1();
        }
    }
}