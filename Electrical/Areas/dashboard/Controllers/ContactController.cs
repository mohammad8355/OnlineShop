using DataAccessLayer.Models;
using BusinessLogicLayer.ContactService;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("Dashboard")]
    public class ContactController : Controller
    {
        private readonly ContactsLogic contactsLogic;
        public ContactController(ContactsLogic contactsLogic)
        {
            this.contactsLogic = contactsLogic;
        } 
        public IActionResult Index()
        {
            var model = contactsLogic.ContactList();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddContact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddContact(Contact model)
        {
            if(ModelState.IsValid)
            {
                var result = await contactsLogic.AddContact(model);
                if (result)
                {
                    ViewBag.success = "افزودن راه ارتباطی با موفقیت انجام شد";
                    return View();
                }
                ModelState.AddModelError("", "خطا در افزودن راه ارتباطی");
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult EditContact(int Id)
        {
            var contact = contactsLogic.ContactDetail(Id);
            return View("AddContact",contact);
        }
        [HttpPost]
        public IActionResult EditContact(Contact model)
        {
            if (ModelState.IsValid)
            {
                var contact = contactsLogic.ContactDetail(model.Id);
                contact.Name = model.Name;
                contact.Content = model.Content;
                contact.IsSocialMedia = model.IsSocialMedia;
                var result = contactsLogic.EditContact(contact);
                if (result)
                {
                    ViewBag.success = "ویرایش راه ارتباطی با موفقیت انجام شد";
                    return View("AddContact");
                }
                ModelState.AddModelError("", "خطا در ویرایش راه ارتباطی");
                return View("AddContact", model);
            }
            return View("AddContact",model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteContact(int Id)
        {
            var Contact = contactsLogic.ContactDetail(Id);
            var result = await contactsLogic.DeleteContact(Id);
            return Json(new { name = Contact.Name, result = result });
        }
    }
}
