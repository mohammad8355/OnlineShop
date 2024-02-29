using BusinessLogicLayer.TicketService;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Controllers
{
    [Authorize]
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
        [HttpPost]
        public async Task<IActionResult> AddTicket(AddEditTicketViewModel model)
        {
            ModelState.Remove("Status");
            ModelState.Remove("UserName");
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "لطفا فیلد ها را به درستی پر کنید ");
                return View(model);
            }
            else
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                var ticket = new Ticket()
                {
                    Title = model.Title,
                    Status = "open",
                    Description = model.Description.ToString(),
                    LastUpdate = DateTime.Now,
                    User_Id = user.Id,
                };
                if (await ticketLogic.AddTicket(ticket))
                {
                    ViewBag.success = "تیکت شما با موفقیت ارسال شد,به روزدی توس پشتیبانی بررسی خواهد شد";
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "متاسفانه مشکلی رخ داده است ");
                    return View(model);
                }
            }
        }
        public IActionResult TicketDetail(int Id)
        {
            var model = ticketLogic.TicketDetail(Id);
            return View(model);
        }
    }
}
