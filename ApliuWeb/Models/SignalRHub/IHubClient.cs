using ApliuTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApliuWeb.SignalRHub
{
    /// <summary>
    /// 消息体
    /// </summary>
    public class MessageModel
    {
        public String userName;
        public String Message;
        public String datetimeNow = TimeHelper.DataTimeNow.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// 客户端方法
    /// </summary>
    public interface IHubClient
    {
        /// <summary>
        /// 客户端接收消息
        /// </summary>
        /// <param name="messageModel"></param>
        void ReceiveMessage(MessageModel messageModel);

        /// <summary>
        /// 用户掉线/离线通知
        /// </summary>
        /// <param name="userName"></param>
        void OnDisconnected(String userName);

        /// <summary>
        /// 用户上线通知
        /// </summary>
        /// <param name="userName"></param>
        void OnConnected(String userName);

        /// <summary>
        /// 用户重连通知
        /// </summary>
        /// <param name="userName"></param>
        void OnReconnected(String userName);
    }
}
