using ApliuTools.Web;
using ApliuTools.WebTools;
using ApliuWeb.SignalRHub;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;

namespace ApliuWeb.WeChat
{
    [HubName("weChatHub")]
    public class WeChatHub : IHubServer
    {
        [HubMethodName("sendAllMessage")]
        public override void SendAllMessage(string Message)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.userName = HttpRuntimeCache.Get(Context.ConnectionId).ToString();
            messageModel.Message = Message;
            Clients.All.ReceiveMessage(messageModel);

            //Clients.All.ReceiveMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + Message);
            //IHubContext _hubContext = GlobalHost.ConnectionManager.GetHubContext<WeChatHub>();
            //Context.ConnectionId
            //clientModel.LastUpdatedBy = Context.ConnectionId;
            // Update the shape model within our broadcaster
            //Clients.AllExcept(clientModel.LastUpdatedBy).updateShape(clientModel);
        }

        [HubMethodName("chatLogin")]
        public void ChatLogin(string userName, String password)
        {
            HttpRuntimeCache.Add(Context.ConnectionId, userName);

            //UserSession userSession = new UserSession();
            //userSession.UserName = userName;
            //userSession.hubConnectionId = Context.ConnectionId;
            //UserInfo.SetUserInfo(userSession);
        }

        [HubMethodName("sendOthersMessage")]
        public override void SendOthersMessage(string Message)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.userName = HttpRuntimeCache.Get(Context.ConnectionId).ToString();
            messageModel.Message = Message;
            String chatLogSql = String.Format(@"insert into ChatMessage (ChatMsgID,UserName,Message,SendTime,HubConnectionId,IP)
                values('{0}','{1}','{2}','{3}','{4}','{5}');", Guid.NewGuid().ToString(), messageModel.userName, messageModel.Message,
                messageModel.datetimeNow, Context.ConnectionId, HYRequest.GetIP());
            Boolean logResult = DataAccess.Instance.PostData(chatLogSql);
            Clients.Others.ReceiveMessage(messageModel);
        }
    }
}