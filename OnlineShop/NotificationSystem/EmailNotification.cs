using DataAccessLayer.Models;
using Infrustructure.MessageSender;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.NotificationSystem
{
    public class EmailNotification : INotificationProvider
    {
        private readonly EmailSender _sender;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmailNotification(EmailSender sender,UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _sender = sender;
        }
        public async void SendNotification(string message, string type, string source, string user_Id, string title = "")
        {
         
            if (!string.IsNullOrEmpty(message))
            {
                var user = await _userManager.FindByIdAsync(user_Id);
                var email = user.Email;
                await _sender.SendEmailAsync(email, title, message);
            }
        }
    }
}
