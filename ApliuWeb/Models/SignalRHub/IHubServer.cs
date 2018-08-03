using ApliuTools.WebTools;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApliuWeb.SignalRHub
{
    /// <summary>
    /// 服务端方法
    /// </summary>
    public abstract class IHubServer : Hub<IHubClient>
    {
        public static void AddUser(String connectionId, UserSession userSession)
        {
            if (userSession != null) HttpRuntimeCache.Add(connectionId, userSession);
        }

        public static UserSession GetUser(String connectionId)
        {
            return HttpRuntimeCache.Get(connectionId) as UserSession;
        }

        public static void RemoveUser(String connectionId)
        {
            HttpRuntimeCache.Remove(connectionId);
        }

        public static void RemoveAll()
        {
            HttpRuntimeCache.RemoveAll();
        }

        /// <summary>
        /// 向所有客户端发送消息
        /// </summary>
        /// <param name="Message"></param>
        public abstract void SendAllMessage(String Message);

        /// <summary>
        /// 向除自己之外的客户端发送消息
        /// </summary>
        /// <param name="Message"></param>
        public abstract void SendOthersMessage(String Message);


        /// <summary>
        /// 离线通知
        /// </summary>
        public override Task OnDisconnected(bool stopCalled)
        {
            UserSession userSession = IHubServer.GetUser(Context.ConnectionId);
            if (userSession != null)
            {
                Clients.All.OnDisconnected(userSession.UserName);   //调用客户端用户离线通知
                IHubServer.RemoveUser(Context.ConnectionId);
            }
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 上线通知
        /// </summary>
        public override Task OnConnected()
        {
            UserSession userSession = IHubServer.GetUser(Context.ConnectionId);
            if (userSession != null)
            {
                Clients.All.OnConnected(userSession.UserName);   //调用客户端用户上线线通知
            }
            return base.OnConnected();
        }

        /// <summary>
        /// 重连通知
        /// </summary>
        public override Task OnReconnected()
        {
            UserSession userSession = IHubServer.GetUser(Context.ConnectionId);
            if (userSession != null)
            {
                Clients.All.OnReconnected(userSession.UserName);   //调用客户端用户重连通知
            }
            return base.OnReconnected();
        }
    }
}
