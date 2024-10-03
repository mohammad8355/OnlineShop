﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.NotificationSystem
{
    public interface INotificationProvider
    {
        public void SendNotification(string message, string type, string source, string user_Id, string title = "");
        
    }
}
