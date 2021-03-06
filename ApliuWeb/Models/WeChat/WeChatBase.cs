using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using ApliuTools;
using System.Collections;
using System.Security.Cryptography;

namespace ApliuWeb.WeChat
{
    public class WeChatBase
    {
        /// <summary>
        /// 微信服务器绑定域名  格式 https://www.xxxxxx.com
        /// </summary>
        public static String WxDomain
        {
            get
            {
                String wxDomain = SiteConfig.GetConfigAppSettingsValue("WxDomain").Trim();
                if (wxDomain.EndsWith("/")) wxDomain = wxDomain.Substring(0, wxDomain.Length - 1);
                return wxDomain;
            }
        }

        /// <summary>
        /// 开发者ID
        /// </summary>
        public static String WxAppId = SiteConfig.GetConfigAppSettingsValue("WxAppId").Trim();
        /// <summary>
        /// 开发者密码
        /// </summary>
        public static String WxAppSecret = SiteConfig.GetConfigAppSettingsValue("WxAppSecret").Trim();
        /// <summary>
        /// 用户设置的令牌(Token)
        /// </summary>
        public static String WxToken = SiteConfig.GetConfigAppSettingsValue("WxToken").Trim();
        /// <summary>
        /// 消息加解密密钥
        /// </summary>
        public static String WxEncodingAESKey = SiteConfig.GetConfigAppSettingsValue("WxEncodingAESKey").Trim();

        /// <summary>
        /// 微信公众号解析统一编码
        /// </summary>
        public static Encoding WxEncoding = Encoding.UTF8;

        /// <summary>
        /// 微信公众号消息模式是否是安全模式（加密消息）
        /// 更改设置由公众号配置决定是否加密
        /// </summary>
        [Obsolete]
        public static Boolean IsSecurity = SiteConfig.GetConfigAppSettingsValue("IsSecurity").ToBoolean();

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
        /// 微信公众号JS-SDK使用权限签名算法
        /// </summary>
        /// <param name="nonceStr"></param>
        /// <param name="jsApiTicket"></param>
        /// <param name="timesamp"></param>
        /// <param name="reqUrl"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static int GenarateSinature(string nonceStr, string jsApiTicket, string timesamp, string reqUrl, String signature)
        {
            ArrayList AL = new ArrayList();
            AL.Add(nonceStr);
            AL.Add(jsApiTicket);
            AL.Add(timesamp);
            AL.Add(reqUrl);
            AL.Sort(new Tencent.WXBizMsgCrypt.DictionarySort());
            string raw = "";
            for (int i = 0; i < AL.Count; ++i)
            {
                raw += AL[i];
            }

            SHA1 sha;
            ASCIIEncoding enc;
            string hash = "";
            try
            {
                sha = new SHA1CryptoServiceProvider();
                enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(raw);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
            }
            catch (Exception)
            {
                return 40001;
            }
            signature = hash;
            return 0;
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