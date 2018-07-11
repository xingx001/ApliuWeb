using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using ApliuTools;

namespace ApliuWeb.WeChart
{
    public class WeChartBase
    {
        /// <summary>
        /// 绑定的服务器域名
        /// </summary>
        public static String WxDomain = SiteConfig.GetConfigNodeValue("WxDomain");
        /// <summary>
        /// 开发者ID
        /// </summary>
        public static String WxAppId = SiteConfig.GetConfigNodeValue("WxAppId");
        /// <summary>
        /// 开发者密码
        /// </summary>
        public static String WxAppSecret = SiteConfig.GetConfigNodeValue("WxAppSecret");
        /// <summary>
        /// 用户设置的令牌(Token)
        /// </summary>
        public static String WxToken = SiteConfig.GetConfigNodeValue("WxToken");
        /// <summary>
        /// 消息加解密密钥
        /// </summary>
        public static String WxEncodingAESKey = SiteConfig.GetConfigNodeValue("WxEncodingAESKey");

        /// <summary>
        /// 微信公众号解析统一编码
        /// </summary>
        public static Encoding WxEncoding = Encoding.UTF8;

        /// <summary>
        /// 微信公众号消息模式是否是安全模式（加密消息）
        /// 更改设置由公众号配置决定是否加密
        /// </summary>
        [Obsolete]
        public static Boolean IsSecurity = SiteConfig.GetConfigNodeValue("IsSecurity").ToBoolean();

        /// <summary>
        /// 验证消息是否来自微信服务器
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        /// <param name="token">公众号基本配置中填写的Token</param>
        /// <returns></returns>
        public static bool CheckSignature(string signature, string timestamp, string nonce)
        {
            if (String.IsNullOrEmpty(signature) || String.IsNullOrEmpty(timestamp) || String.IsNullOrEmpty(nonce))
            {
                return false;
            }

            List<string> sortList = new List<string>();
            sortList.Add(timestamp);
            sortList.Add(nonce);
            sortList.Add(WxToken);
            sortList.Sort();

            byte[] data = WxEncoding.GetBytes(string.Join("", sortList));
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] result = sha1.ComputeHash(data);//得到哈希值
            string srt = System.BitConverter.ToString(result).Replace("-", "").ToLower(); //转换成为字符串的显示

            if (srt == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 返回HTTP响应消息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HttpResponseMessage encapResult(string content)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    content,
                    WxEncoding,
                    "text/html"
                )
            };
        }
    }
}