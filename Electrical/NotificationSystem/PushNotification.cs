using BusinessLogicLayer.NotificationLogic;
using DataAccessLayer.Models;
using Infrustructure.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.NotificationSystem
{
    public class PushNotification 
    {
        private readonly HubConfig _hubConfig;
        private readonly NotificationLogic _notifLogic;
        private readonly ILogger _logger;
        public PushNotification(Logger<PushNotification> logger,NotificationLogic notifLogic,HubConfig hubConfig)
        {
            _hubConfig = hubConfig;
            _notifLogic = notifLogic;
            _logger = logger;
        }
        //public async void SendNotification(string message, string receiver_Id)
        //{
        //    //_hubConfig.sendMessageToClient(receiver_Id, message);
        //    //var usertonotif = new List<UserToNotification>();
        //    //var model = new Notification()
        //    //{
        //    //    message = message,
        //    //    Date = DateTime.Now,
        //    //};
        //    //usertonotif.Add(new UserToNotification() { User_Id = receiver_Id, Notification_Id = model.Id });
        //    //model.userToNotifications = usertonotif;
        //    //var result = await _notifLogic.AddNotification(model);
        //    //if (result)
        //    //{
        //    //    _logger.LogInformation("send notification to user with Id '{receiver_Id}'", receiver_Id);
        //    //}
        //    //else
        //    //{
        //    //    _logger.LogWarning("failed to send notification to user with Id '{receiver_Id}'", receiver_Id);
        //    //}

        //}
    }
}
