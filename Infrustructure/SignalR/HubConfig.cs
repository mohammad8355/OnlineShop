using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.SignalR
{
    public class HubConfig : Hub
    {
        public void SendMessage(string message,DateTime date,string source,string type,string title = "")
        {
            Clients.All.SendAsync("receiveMessage",message,date,source,type,title);
        }
        //public void sendMessageToClient(string clientId, string message)
        //{
        //    Clients.Client(clientId).SendAsync("receiveMessage", message);
        //}
        //public void SendMessageToGroup(string groupName, string message)
        //{
        //    Clients.Group(groupName).SendAsync("receiveMessage", message);
        //}
        //public void AddToGroup(string groupName)
        //{
        //    Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //}
        //public void RemoveFromGroup(string groupName)
        //{
        //    Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        //}
        //public override Task OnConnectedAsync()
        //{
        //    return base.OnConnectedAsync();
        //}
        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    return base.OnDisconnectedAsync(exception);
        //}
    }
}
