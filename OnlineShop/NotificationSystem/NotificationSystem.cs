namespace PresentationLayer.NotificationSystem
{
    public class NotificationManager
    {
        private readonly Dictionary<string,INotificationProvider> _Providers;
        public NotificationManager(Dictionary<string, INotificationProvider> Providers)
        {
            _Providers = Providers;
        }
        public void SendNotification(string[] providerName,string message,string type,string source,string user_Id,string title = "")
        {
            if(providerName != null)
            if(providerName.Count() > 0)
            foreach(var provider in providerName)
            {
                    _Providers[provider].SendNotification(message, type, source, user_Id, title);
            }
        }
    }
}
