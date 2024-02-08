using BusinessLogicLayer.CommentService;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models.ViewModels;
using System.Collections.Generic;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class CommentController : Controller
    {
        private readonly CommentLogic commentLogic;
        public CommentController(CommentLogic commentLogic)
        {
            this.commentLogic = commentLogic;
        }
        public IActionResult Index()
        {
            var model = commentLogic.CommentList();

            return View(model);
        }
        #region Add Comment code
        //[HttpGet]
        //public ActionResult AddComment()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> AddComment(AddEditCommentViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var comment = new Commnet()
        //        {
        //            Title = model.Title,
        //            Description = model.Description,
        //            User_Id = model.User_Id,
        //            Ticket_Id = model.Ticket_Id,
        //            LastUpdate = DateTime.Now,
        //            Reply_Id = model.reply_Id,
        //        };
        //        if (await commentLogic.AddComment(comment))
        //        {
        //            ViewBag.success = "کامنت با موفقیت ارسال شد";
        //            return View();
        //        }
        //        ModelState.AddModelError("", "لطفا به درستی فیلد ها را پر کنید");
        //        return View(model);
        //    }
        //    return View(model);

        //}
        #endregion
        #region Edit comment code 
        //[HttpGet]
        //public IActionResult EditComment(int Id)
        //{
        //    var comment = commentLogic.CommentDetail(Id);
        //    var model = new AddEditCommentViewModel()
        //    {
        //        Id = comment.Id,
        //        Title = comment.Title,
        //        Description = comment.Description,
        //        reply_Id = comment.Reply_Id,
        //        User_Id = comment.User_Id,

        //    };
        //    return View(model);
        //}
        //[HttpPost]
        //public IActionResult EditComment(AddEditCommentViewModel model)
        //{
        //    var comment = commentLogic.CommentDetail(model.Id);
        //    comment.LastUpdate = DateTime.Now;
        //    if (ModelState.IsValid)
        //    {
        //        comment.Title = model.Title;
        //        comment.Description = model.Description;
        //        comment.User_Id = model.User_Id;
        //        comment.Reply_Id = model.reply_Id;
        //        if (commentLogic.EditComment(comment))
        //        {
        //            RedirectToAction("Index");
        //        }
        //        ModelState.AddModelError("", "لطفا فیلد ها را به درستی پر کنید");
        //        return View(model);
        //    }
        //    return View(model);
        //}
        #endregion
        [HttpGet]
        public IActionResult HideShowComment(int Id)
        {
            var comment = commentLogic.CommentDetail(Id);
            if (comment.IsHide)
            {
                comment.IsHide = false;
            }
            else
            {
                comment.IsHide = true;
            };
            var result = commentLogic.EditComment(comment);
            return Json(new { res = result });
        }
        [HttpGet]
        public IActionResult DeleteComment(int Id)
        {
            var comment = commentLogic.CommentDetail(Id);
            var result = commentLogic.DeleteComment(Id);
            return Json(new { result = result, name = comment.Title });
        }
    }
}
