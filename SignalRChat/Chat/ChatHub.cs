using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRChat.Chat
{
    public class ChatHub : Hub
    {
        public void Send(string name, string msg)
        {
            // Call the broadcastMessage method to update clients.
            //Clients.All.broadcastMessage(name, msg);
            Clients.AllExcept(Context.ConnectionId).broadcastMessage(name, msg);
        }

        public override Task OnConnected()
        {
            //var clientId = (Connection)((HubConnectionContextBase)Clients);
            Clients.All.broadcastMessage(Context.ConnectionId, "上线了");

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.All.broadcastMessage(Context.ConnectionId, "下线了");

            return base.OnDisconnected(stopCalled);
        }
    }
}