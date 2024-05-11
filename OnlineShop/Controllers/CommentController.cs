using BusinessLogicLayer.CommentService;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize]
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
                    return Json(new { message = "نظر شما با موفقیت ثبت شد" ,type = "success"});
                }
                else
                {
                    return Json(new { message = "خطایی  رخ داده لطفا بعدا تلاش کنید !" , type = "error"});
                }
            }
            else
            {
                return Json(new { message = "لطفا پیما خود را بنویسید" , type = "info" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddCommentTicket(string description,string title,int ticket_Id)
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
                    return Json(new { message = "کامنت شما با موفقیت ثبت شد",type = "success" });
                }
                else
                {
                    return Json(new { message = "خطایی  رخ داده لطفا بعدا تلاش کنید !" , type = "error"});
                }
            }
            else
            {
                return Json(new { message = "لطفا پیام خود را بنویسید", type = "warning" });
            }
        }

        public async Task<IActionResult> AddCommentPost(string description,int post_Id,int? reply_Id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (!string.IsNullOrEmpty(description))
            {
                var comment = new Commnet()
                {
                    Title = "",
                    User_Id = user.Id,
                    Description = description.ToString(),
                    LastUpdate = DateTime.Now,
                    BlogPost_Id = post_Id,
                    Reply_Id = reply_Id,
                };
                var result = await commentLogic.AddComment(comment);
                if (result)
                {
                    return Json(new { message = "کامنت شما با موفقیت ثبت شد" , type = "success"});
                }
                else
                {
                    return Json(new { message = "خطایی  رخ داده لطفا بعدا تلاش کنید !" ,type = "error"});
                }
            }
            else
            {
                return Json(new { message = "لطفا پیام خود را بنویسید" , type  = "warning"});
            }
        }
        public async Task<IActionResult> EditCommentPost(string description, int post_Id, int comment_Id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (!string.IsNullOrEmpty(description))
            {
                var comment = commentLogic.CommentDetail(comment_Id);
                comment.Title = "";
                comment.User_Id = user.Id;
                comment.Description = description.ToString();
                comment.LastUpdate = DateTime.Now;
                comment.BlogPost_Id = post_Id;
                var result = commentLogic.EditComment(comment);
                if (result)
                {
                    return Json(new { message = "کامنت شما با موفقیت ثبت شد" , type = "success"});
                }
                else
                {
                    return Json(new { message = "خطایی  رخ داده لطفا بعدا تلاش کنید !" , type = "error"});
                }
            }
            else
            {
                return Json(new { message = "لطفا پیام خود را بنویسید" , type = "info"});
            }
        }
        public async Task<IActionResult> DeleteComment(int Id)
        {
            var comment = commentLogic.CommentDetail(Id);
            if (comment != null)
            {
                comment.IsHide = true;
                var result = commentLogic.EditComment(comment);
                if (result)
                {
                    return Json(new { message = "حذف با موفقیت انجام شد" , type = "success" });
                }
                else
                {
                    return Json(new { message = "خطا در حذف کامنت" , type = "error"});
                }
            }
            else
            {
                return Json(new { message = "کامنتی یافت نشد" , type = "info" });
            }
        }
    }
}
