using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ApliuWeb
{
    public class ServerHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendMsg(string message)
        {
            //调用所有客户端的sendMessage方法
            Clients.All.sendMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Threading.Thread.CurrentThread.ManagedThreadId,
                ApliuTools.Web.HYRequest.GetIP(), message);
        }
    }
}