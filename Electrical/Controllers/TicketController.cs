using BusinessLogicLayer.TicketService;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Controllers
{
    public class TicketController : Controller
    {
        private readonly TicketLogic ticketLogic;
        private readonly UserManager<ApplicationUser> userManager;
        public TicketController(TicketLogic ticketLogic, UserManager<ApplicationUser> userManager)
        {
            this.ticketLogic = ticketLogic;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var model = ticketLogic.TicketListOfUser(user.Id);
            return View(model);
        }
        [HttpGet]
        public IActionResult AddTicket()
        {
            return View();
        }
        public async Task<IActionResult> AddTicket(string description,string title)
        {
            var message = "";
            if (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(title))
            {
                message = "لطفا فیلد ها را به درستی پر کنید ";
            }
            else
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                var ticket = new Ticket()
                {
                    Title = title,
                    Status = "open",
                    Description = description.ToString(),
                    LastUpdate = DateTime.Now,
                    User_Id = user.Id,
                };
                if (await ticketLogic.AddTicket(ticket))
                {
                    message = "تیکت شما با موفقیت ارسال شد,به روزدی توس پشتیبانی بررسی خواهد شد";
                }
                else
                {
                    message = "متاسفانه مشکلی رخ داده است ";
                }
            }
              
  
            return Json(message);
        }
        public IActionResult TicketDetail(int Id)
        {
            var model = ticketLogic.TicketDetail(Id);
            return View(model);
        }
    }
}
