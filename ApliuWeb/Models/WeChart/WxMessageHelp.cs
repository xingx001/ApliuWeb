using ApliuTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace ApliuWeb.WeChart
{
    public class WxMessageHelp
    {
        public string ReturnMessage(string postStr)
        {
            string responseContent = "";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(new System.IO.MemoryStream(WeChartBase.WxEncoding.GetBytes(postStr)));
            XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
            if (MsgType != null)
            {
                switch (MsgType.InnerText)
                {
                    case "event":
                        responseContent = EventHandle(xmldoc);//事件处理
                        break;
                    case "text":
                        responseContent = TextHandle(xmldoc);//接受文本消息处理
                        break;
                }
            }
            return responseContent;
        }

        //事件
        public string EventHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode Event = xmldoc.SelectSingleNode("/xml/Event");
            XmlNode EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            if (Event != null)
            {
                //菜单单击事件
                if (Event.InnerText.Equals("CLICK"))
                {
                    //Helper.GetUserDetail(Helper.IsExistAccess_Token(), FromUserName.InnerText);//获取用户基本信息
                    if (EventKey.InnerText.Equals("12"))
                    {
                        responseContent = string.Format(ReplyType.Message_Text,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "欢迎查看ApliuTools");
                    }
                }
                else if (Event.InnerText.Equals("subscribe"))//关注公众号时推送消息
                {
                    //Helper.GetUserDetail(Helper.IsExistAccess_Token(), FromUserName.InnerText);//获取用户基本信息
                    responseContent = string.Format(ReplyType.Message_Text,
                        FromUserName.InnerText,
                        ToUserName.InnerText,
                        DateTime.Now.Ticks,
                        "欢迎关注ApliuTools");
                }
            }
            Logger.WriteLog("接收事件日志，用户OpenId：" + ToUserName.InnerText + "，开发者微信号：" + FromUserName.InnerText + "，事件内容：" + Event.InnerText + "，EventKey：" + EventKey.InnerText);
            return responseContent;
        }

        //接受文本消息
        public string TextHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");//接收方帐号（收到的OpenID）
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");//开发者微信号
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            if (Content != null)
            {
                responseContent = string.Format(ReplyType.Message_Text,
                   FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    "欢迎使用ApliuTools微信公众号，功能主页：https://apliu.xyz");
            }
            Logger.WriteLog("接收文本消息日志，用户OpenId：" + ToUserName.InnerText + "，开发者微信号：" + FromUserName.InnerText + "，消息内容：" + Content.InnerText);
            return responseContent;
        }
    }

    public class ReplyType
    {
        /// <summary>
        /// 普通文本消息
        /// </summary>
        public static string Message_Text
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>";
            }
        }
    }
}