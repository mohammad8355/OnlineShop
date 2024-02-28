using BusinessLogicLayer.CommentService;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentLogic commentLogic;
        private readonly UserManager<ApplicationUser> userManager;
        public CommentController(CommentLogic commentLogic, UserManager<ApplicationUser> userManager)
        {
            this.commentLogic = commentLogic;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> AddComment(string description, int product_Id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (!string.IsNullOrEmpty(description))
            {
                var comment = new Commnet()
                {
                    Title = "",
                    Product_Id = product_Id,
                    User_Id = user.Id,
                    Description = description,
                    LastUpdate = DateTime.Now,
                };
                var result = await commentLogic.AddComment(comment);
                if (result)
                {
                    return Json(new { message = "نظر شما با موفقیت ثبت شد" });
                }
                else
                {
                    return Json(new { message = "خطایی  رخ داده لطفا بعدا تلاش کنید !" });
                }
            }
            else
            {
                return Json(new { message = "لطفا پیما خود را بنویسید" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddComment(string description,string title,int ticket_Id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (!string.IsNullOrEmpty(description))
            {
                var comment = new Commnet()
                {
                    Title = title,
                    User_Id = user.Id,
                    Description = description.ToString(),
                    LastUpdate = DateTime.Now,
                    Ticket_Id = ticket_Id,
                };
                var result = await commentLogic.AddComment(comment);
                if (result)
                {
                    return Json(new { message = "کامنت شما با موفقیت ثبت شد" });
                }
                else
                {
                    return Json(new { message = "خطایی  رخ داده لطفا بعدا تلاش کنید !" });
                }
            }
            else
            {
                return Json(new { message = "لطفا پیام خود را بنویسید" });
            }
        }
    }
}
