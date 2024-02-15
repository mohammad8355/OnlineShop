using BusinessLogicLayer.CommentService;
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly CommentLogic commentLogic;
        public TicketController(TicketLogic ticketLogic, UserManager<ApplicationUser> userManager, CommentLogic commentLogic)
        {
            this.ticketLogic = ticketLogic;
            this.userManager = userManager;
            this.commentLogic = commentLogic;
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
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                var ticket = new Ticket()
                {
                    Title = model.Title,
                    Description = model.Description,
                    User_Id = user.Id,
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
        public async Task<IActionResult> EditTicket(int Id)
        {
            var ticket = ticketLogic.TicketDetail(Id);
            var user = await userManager.FindByIdAsync(ticket.User_Id);
            var model = new AddEditTicketViewModel()
            {
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                UserName = user.UserName,
            };
            return View("AddTicket",model);
        }
        [HttpPost]
        public IActionResult EditTicket(AddEditTicketViewModel model)
        {
            model.LastUpdate = DateTime.Now;
            if (ModelState.IsValid)
            {
                var ticket = ticketLogic.TicketDetail(model.Id);
                ticket.Title = model.Title;
                ticket.Description = model.Description;
                ticket.Status = model.Status;
                ticket.LastUpdate = model.LastUpdate;
                if (ticketLogic.EditTicket(ticket))
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "خطا در افزودن تیکت");
                return View("AddTicket",model);
            }
            return View("AddTicket", model);
        }
        [HttpGet]
        public IActionResult CloseTicket(int Id)
        {
            var ticket = ticketLogic.TicketDetail(Id);
            if(ticket.Status == "open")
            {
                ticket.Status = "close";
            }
            else if(ticket.Status == "close")
            {
                ticket.Status = "open";
            }
            var result = ticketLogic.EditTicket(ticket);
            return Json(new { result = result, name = ticket.Title });
        }
        [HttpGet]
        public IActionResult AnswerTicket(int Id)
        {
            var ticket = ticketLogic.TicketDetail(Id);

            var model = new AnswerTicketViewModel()
            {
                TicketDescription = ticket.Description,
                Username = User.Identity.Name,
                TicketId = Id,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AnswerTicket(AnswerTicketViewModel model)
        {
            ModelState.Remove("TicketDescription");
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Username);
                var comment = new Commnet()
                {
                    Title = "",
                    Description = model.AnswerDescription,
                    User_Id = user.Id,
                    Ticket_Id = model.TicketId,
                    LastUpdate = DateTime.Now,

                };
                if (await commentLogic.AddComment(comment))
                {
                    var ticket = ticketLogic.TicketDetail(model.TicketId);
                    ticket.Status = "responed";
                    ticketLogic.EditTicket(ticket);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "error");
                return View(model);
            }
          
            return View(model);
        }
        public IActionResult TicketDetail(int Id)
        {
            var ticket = ticketLogic.TicketDetail(Id);
            return View(ticket);
        }
    }
}
