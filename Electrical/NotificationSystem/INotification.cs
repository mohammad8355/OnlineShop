using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.NotificationSystem
{
    public interface INotification
    {
        public void SendNotification(string message,string title,string type,string source,string user_Id);
        
    }
}
