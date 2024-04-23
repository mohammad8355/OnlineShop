namespace PresentationLayer.NotificationSystem
{
    public class NotificationManager
    {
        private readonly Dictionary<string,INotificationProvider> _Providers;
        public NotificationManager(Dictionary<string, INotificationProvider> Providers)
        {
            _Providers = Providers;
        }
        public void SendNotification(string providerName,string message,string type,string source,string user_Id,string title = "")
        {
            if (_Providers.ContainsKey(providerName))
            {
                _Providers[providerName].SendNotification(message, type, source, user_Id, title);
            }
            else
            {
                _Providers["push"].SendNotification(message, type, source, user_Id, title);
            }
        }
    }
}
