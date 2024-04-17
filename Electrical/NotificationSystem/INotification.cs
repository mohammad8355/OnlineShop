﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.NotificationSystem
{
    public interface INotification
    {
        public void SendNotification(string message, string receiver_Id);
        
    }
}
