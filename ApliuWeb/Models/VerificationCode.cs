﻿using ApliuTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApliuWeb
{
    public class CodeCase
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code = string.Empty;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime = TimeHelper.DataTimeNow;
        /// <summary>
        /// 有效期 秒
        /// </summary>
        public long Timeout = 0L;
        /// <summary>
        /// 场景类型
        /// </summary>
        public CodeType Type = CodeType.Null;
        /// <summary>
        /// 使用对象
        /// </summary>
        public string User = string.Empty;
    }

    [Serializable]
    public enum CodeType
    {
        /// <summary>
        /// 默认类型 非法
        /// </summary>
        Null = -1,
        /// <summary>
        /// 测试类型
        /// </summary>
        Test = 0,
        /// <summary>
        /// 注册验证码
        /// </summary>
        Register = 1,
        /// <summary>
        /// 登录验证码
        /// </summary>
        Login = 2,
        /// <summary>
        /// 修改密码验证码
        /// </summary>
        ChangePassword = 3
    }


    public class VerificationCode
    {
        /// <summary>
        /// 发送指定类型的验证码，并记录到Session
        /// </summary>
        /// <param name="Mobile"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public static bool SendSMS(string Mobile, CodeType codeType, out string SendMsg)
        {
            string smsContent = string.Empty;
            string smsCode = getVerifyCode(codeSerialNum, 6);
            long timeOut = 0L;
            SendMsg = "短信发送失败";
            switch (codeType)
            {
                case CodeType.Null: return false;
                    break;
                case CodeType.Test: timeOut = 120L;
                    smsContent = TimeHelper.DataTimeNow.ToString("yyyy年MM月dd日 HH:mm:ss") + ", 测试短信. --Apliu";
                    break;
                case CodeType.Register: timeOut = 120L;
                    smsContent = "您好, 您的注册验证码是: " + smsCode + ", 有效期" + timeOut / 60 + "分钟, 如非本人发送, 请忽略该短信。";
                    break;
                case CodeType.Login: timeOut = 120L;
                    smsContent = "您好, 您的登录动态验证码是: " + smsCode + ", 有效期" + timeOut / 60 + "分钟, 如非本人发送, 请忽略该短信。";
                    break;
                case CodeType.ChangePassword: timeOut = 120L;
                    smsContent = "您好, 您的修改密码验证码是: " + smsCode + ", 有效期" + timeOut / 60 + "分钟, 如非本人发送, 请忽略该短信。";
                    break;
                default: return false;
                    break;
            }
            bool result = SendSMS(Mobile, smsContent, out SendMsg);
            if (result)
            {
                CodeCase cc = new CodeCase();
                cc.Code = smsCode;
                cc.Timeout = timeOut;
                cc.Type = codeType;
                cc.User = Mobile;
                SessionHelper.SetSessionValue(codeType.ToString(), cc);
            }
            return result;
        }

        /// <summary>
        /// 向指定手机发送短信
        /// </summary>
        /// <param name="Mobile"></param>
        /// <param name="SMSContent"></param>
        /// <returns></returns>
        private static bool SendSMS(string Mobile, string SMSContent, out string SendMsg)
        {
            string SMSAppId = SiteConfig.GetConfigNodeValue("SMSAppId");
            string SMSAppKey = SiteConfig.GetConfigNodeValue("SMSAppKey");
            SMSMessage sms = new TencentSMS();
            bool resutl = sms.SendSMS(Mobile, SMSContent, out SendMsg, SMSAppId, SMSAppKey);
            return resutl;
        }

        /// <summary>
        /// 自定义随机码字符串序列(使用逗号分隔) 由于0/o，i/l难区分，所以去掉
        /// </summary>
        public const string codeSerialAbc = "2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";

        public const string codeSerialNum = "0,1,2,3,4,5,6,7,8,9";

        /// <summary>
        /// 生成随机字符码
        /// </summary>
        /// <param name="codeLen"></param>
        /// <returns></returns>
        public static string getVerifyCode(string codeSerial, int codeLen)
        {
            string[] arr = codeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }
    }
}
