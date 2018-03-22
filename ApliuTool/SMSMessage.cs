using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApliuTools
{
    class SMSMessage
    {
    }

    public class TencentSMS
    {
        /// <summary>
        /// 短信接口发送地址
        /// </summary>
        private readonly string SendUrl = "https://yun.tim.qq.com/v5/tlssmssvr/sendsms?sdkappid={0}&random={1}";
        /// <summary>
        /// 请求Json格式
        /// </summary>
        private readonly string RequestJson = @"{
                    ""ext"": ""{0}"",
                    ""extend"": ""{1}"",
                    ""msg"": ""{2}"",
                    ""sig"": ""{3}"",
                    ""tel"": {
                        ""mobile"": ""{4}"",
                        ""nationcode"": ""{5}""
                    },
                    ""time"": {6},
                    ""type"": {7}
                }";
        /// <summary>
        /// 返回Json格式
        /// </summary>
        private readonly string ResponseJson = @"{
                    ""result"": 0,
                    ""errmsg"": ""OK"",
                    ""ext"": """",
                    ""fee"": 1,
                    ""sid"": ""xxxxxxx""
                }";

        /// <summary>
        /// 获取发送短信的Json
        /// </summary>
        /// <param name="ext">用户的 session 内容，腾讯 server 回包中会原样返回，可选字段，不需要就填空</param>
        /// <param name="extend">短信码号扩展号，格式为纯数字串，其他格式无效，默认没有开通</param>
        /// <param name="msg">短信消息，utf8 编码，需要匹配审核通过的模板内容</param>
        /// <param name="mobile">电话号码</param>
        /// <param name="nationcode">国家码 中国86</param>
        /// <param name="time">请求发起时间，unix 时间戳（单位：秒），如果和系统时间相差超过 10 分钟则会返回失败</param>
        /// <param name="type">短信类型，Enum{0: 普通短信, 1: 营销短信}（注意：要按需填值，不然会影响到业务的正常使用）</param>
        ///<param name="random">随机数 10位，与发送URL的random一致</param>
        /// <returns></returns>
        private string GetSendJson(string ext, string extend, string msg, string mobile, string nationcode, long time, string type, string random)
        {
            //"sig" 字段根据公式 sha256（appkey=$appkey&random=$random&time=$time&mobile=$mobile）生成
            string sig = string.Format(@"appkey={0}&random={1}&time={2}&mobile={3}", ConfigurationManager.AppSettings["SMSAppKey"], random, time, mobile);
            sig = SecurityHelper.SHA256Encrypt(sig, Encoding.UTF8);
            string json = string.Format(RequestJson, ext, extend, msg, sig, mobile, nationcode, time, type);
            return json;
        }

        /// <summary>
        /// 获取发送短信的Json
        /// </summary>
        /// <param name="Mobile">手机号码</param>
        /// <param name="Message">短信内容</param>
        /// <param name="random">随机数 10位，与发送URL的random一致</param>
        /// <returns></returns>
        private string GetSendJson(string Mobile, string Message, string Random)
        {
            string json = GetSendJson("", "", Message, Mobile, "86", TimeHelper.getCurrentUnixTime(), "0", Random);
            return json;
        }

    }
    /*
    public class SmsSingleSender : SmsBase
    {
        private string url = "https://yun.tim.qq.com/v5/tlssmssvr/sendsms";

        public SmsSingleSender(int appid, string appkey)
            : base(appid, appkey, new DefaultHTTPClient())
        { }

        public SmsSingleSender(int appid, string appkey, IHTTPClient httpclient)
            : base(appid, appkey, httpclient)
        { }

        /// <summary>
        /// Send single SMS message.
        /// </summary>
        /// <param name="type">SMS message type, Enum{0: normal SMS, 1: marketing SMS}</param>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="phoneNumber">phone number</param>
        /// <param name="msg">SMS message content< /param>
        /// <param name="extend">extend field, default is empty string</param>
        /// <param name="ext">ext field, content will be returned by server as it is</param>
        /// <returns>SmsSingleSenderResult</returns>
        public SmsSingleSenderResult send(int type, string nationCode, string phoneNumber,
            string msg, string extend, string ext)
        {
            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();
            JSONObjectBuilder body = new JSONObjectBuilder()
                .Put("tel", (new JSONObjectBuilder()).Put("nationcode", nationCode).Put("mobile", phoneNumber).Build())
                .Put("type", type)
                .Put("msg", msg)
                .Put("sig", SmsSenderUtil.calculateSignature(this.appkey, random, now, phoneNumber))
                .Put("time", now)
                .Put("extend", !String.IsNullOrEmpty(extend) ? extend : "")
                .Put("ext", !String.IsNullOrEmpty(ext) ? ext : "");

            HTTPRequest req = new HTTPRequest(HTTPMethod.POST, this.url)
                .addHeader("Conetent-Type", "application/json")
                .addQueryParameter("sdkappid", this.appid)
                .addQueryParameter("random", random)
                .setConnectionTimeout(60 * 1000)
                .setRequestTimeout(60 * 1000)
                .setBody(body.Build().ToString());

            // May throw HttpRequestException
            HTTPResponse res = httpclient.fetch(req);

            // May throw HTTPException
            handleError(res);

            SmsSingleSenderResult result = new SmsSingleSenderResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }

        /// <summary>
        /// Send single SMS message with template paramters.
        /// </summary>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="phoneNumber">phone number</param>
        /// <param name="templateId">template id</param>
        /// <param name="parameters">template parameters</param>
        /// <param name="sign">Sms user sign</param>
        /// <param name="extend">extend field, default is empty string</param>
        /// <param name="ext">ext field, content will be returned by server as it is</param>
        /// <returns>SmsSingleSenderResult</returns>
        public SmsSingleSenderResult sendWithParam(string nationCode, string phoneNumber, int templateId,
            string[] parameters, string sign, string extend, string ext)
        {

            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();

            JSONObjectBuilder body = new JSONObjectBuilder()
                .Put("tel", (new JSONObjectBuilder()).Put("nationcode", nationCode).Put("mobile", phoneNumber).Build())
                .Put("sig", SmsSenderUtil.calculateSignature(appkey, random, now, phoneNumber))
                .Put("tpl_id", templateId)
                .PutArray("params", parameters)
                .Put("sign", !String.IsNullOrEmpty(sign) ? sign : "")
                .Put("time", now)
                .Put("extend", !String.IsNullOrEmpty(extend) ? extend : "")
                .Put("ext", !String.IsNullOrEmpty(ext) ? ext : "");

            HTTPRequest req = new HTTPRequest(HTTPMethod.POST, this.url)
                .addHeader("Conetent-Type", "application/json")
                .addQueryParameter("sdkappid", this.appid)
                .addQueryParameter("random", random)
                .setConnectionTimeout(60 * 1000)
                .setRequestTimeout(60 * 1000)
                .setBody(body.Build().ToString());

            // May throw HttpRequestException
            HTTPResponse res = httpclient.fetch(req);

            // May throw HTTPException
            handleError(res);

            SmsSingleSenderResult result = new SmsSingleSenderResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }
    }
     * */
}

