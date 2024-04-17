using DataAccessLayer.Models;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.NotificationLogic
{
    public class NotificationLogic
    {
        private readonly MainRepository<Notification> _notifRepos;
        public NotificationLogic(MainRepository<Notification> notifRepos)
        {
            _notifRepos = notifRepos;
        }
        public async Task<bool> AddNotification(Notification model)
        {
            if(string.IsNullOrEmpty(model.message))
            {
                return false;
            }
            else
            {
                await _notifRepos.AddItem(model);
                return true;
            }
        }
        public async Task<List<Notification>> AllNotif()
        {
            var notifs = await _notifRepos.Get(null,n => n.User);
            return notifs.ToList();
        }
        public async Task<Notification> NotificationDetail(int Id)
        {
            var notification = await AllNotif();
            return notification.Where(n => n.Id == Id).FirstOrDefault();
        }
        public async Task<List<Notification>> UserNotifications(string user_Id)
        {
            var notifications = await AllNotif();
            return notifications.Where(n => n.User_Id == user_Id).ToList();

        }
    }
}
