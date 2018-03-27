﻿using ApliuTools.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApliuTools
{
    public class QCloundTranslate
    {
        public static string Translate(string SecretId, string sourceText, string source, string target)
        {
            string json = getSendJson(SecretId, sourceText, source, target);
            string result = HttpRequestHelper.HttpGet(json);
            return "";
        }
        private static string sendurl = @"https://tmt.api.qcloud.com/v2/index.php?Action={0}&Nonce={1}&Region={2}&SecretId={3}&Timestamp={4}&sourceText={5}&source={6}&target={7}&Signature={8}";

        private static string sendJson = @"{
                                'Action' : '{0}',
                                'Nonce' : {1},
                                'Region' : '{2}',
                                'SecretId' : '{3}',
                                'Timestamp' : {4},
                                'sourceText': '{5}',
                                'source': '{6}',
                                'target': '{7}'
                            }";

        private static string getSendJson(string SecretId, string sourceText, string source, string target)
        {
            int rand = new Random().Next(1000, 9999);
            return getSendJson("TextTranslate", rand.ToString(), "gz", SecretId, TimeHelper.getCurrentUnixTime().ToString(), sourceText, SecurityHelper.UrlEncode(source, Encoding.UTF8), target);
        }

        /// <summary>
        /// 获取发送参数Json
        /// </summary>
        /// <param name="Action">方法名</param>
        /// <param name="Nonce">随机正整数</param>
        /// <param name="Region"></param>
        /// <param name="SecretId">SecretId</param>
        /// <param name="Timestamp">当前时间戳</param>
        /// <param name="sourceText">待翻译的文本</param>
        /// <param name="source">源语言</param>
        /// <param name="target">目标语言</param>
        /// <returns></returns>
        private static string getSendJson(string Action, string Nonce, string Region, string SecretId, string Timestamp, string sourceText, string source, string target)
        {
            string json = string.Format(sendJson, Action, Nonce, Region, SecretId, Timestamp, sourceText, source, target);
            return json;
        }
    }
}
