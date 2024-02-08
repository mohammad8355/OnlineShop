using BusinessLogicLayer.TicketService;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class TicketController : Controller
    {
        private readonly TicketLogic ticketLogic;
        private UserManager<ApplicationUser> userManager;
        public TicketController(TicketLogic ticketLogic, UserManager<ApplicationUser> userManager)
        {
            this.ticketLogic = ticketLogic;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var model = ticketLogic.TicketList();
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
            model.LastUpdate = DateTime.Now;
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            model.User_Id = user.Id;
            if (ModelState.IsValid)
            {
                var ticket = new Ticket()
                {
                    Title = model.Title,
                    Description = model.Description,
                    User_Id = model.User_Id,
                    Status = model.Status,
                    LastUpdate = model.LastUpdate,
                };
                if(await ticketLogic.AddTicket(ticket))
                {
                    ViewBag.success = "افزودن تیکت با موفقیت انجام شد";
                    return View();
                }
                ModelState.AddModelError("", "خطا در افزودن تیکت");
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult CloseTicket(int Id)
        {
            var ticket = ticketLogic.TicketDetail(Id);
            ticket.Status = "close";
            var result = ticketLogic.EditTicket(ticket);
            return Json(new { result = result, name = ticket.Title });
        }
    }
}
