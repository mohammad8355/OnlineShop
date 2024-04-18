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
    public class PushNotification:INotification
    {
        private readonly HubConfig _hubConfig;
        private readonly NotificationLogic _notifLogic;
        private readonly ILogger _logger;
        public PushNotification(ILogger<PushNotification> logger,NotificationLogic notifLogic,HubConfig hubConfig)
        {
            _hubConfig = hubConfig;
            _notifLogic = notifLogic;
            _logger = logger;
        }
        public async void SendNotification(string message,string type, string source, string user_Id,string title = "")
        {
            var model = new Notification()
            {
                Title = title,
                message = message,
                Type = type,
                Source = source,
                User_Id = user_Id,
                Date = DateTime.Now,
                Status = false,
            };
            var result = await _notifLogic.AddNotification(model);
            if (result)
            {
                _hubConfig.SendMessage(model.message,model.Date,model.Source, model.Type,model.Title);
            }
        }
    }
}
