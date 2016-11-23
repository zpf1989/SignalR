using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace SignalRChat.Chat
{
    public class BroadcastHub : Hub
    {
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="msg"></param>
        public void Send(string msg)
        {
            MsgModel model = new MsgModel
            {
                Type = MsgType.Broadcast,
                Msg = msg,
                SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            Clients.AllExcept(Context.ConnectionId).broadcastMessage(model);
        }

        public override Task OnConnected()
        {
            //客户端上线，截取用户信息
            //通知已连接用户Send()

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //客户端下线，截取用户信息
            //通知已连接用户Send()
            return base.OnDisconnected(stopCalled);
        }
    }
}