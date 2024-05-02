using BusinessLogicLayer.GeneralService;
using DataAccessLayer.Models;
using Infrustructure.uploadfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;
using System.Diagnostics;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin")]
    public class GeneralController : Controller
    {
        private readonly GeneralLogic generalLogic;
        private readonly UploadFile FileManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        public GeneralController(GeneralLogic generalLogic, UploadFile FileManager, IWebHostEnvironment webHostEnvironment)
        {
            this.generalLogic = generalLogic;
            this.FileManager = FileManager;
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = generalLogic.GeneralList();
            return View(model.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> DeleteGeneral(int Id)
        {
            var res = await generalLogic.DeleteGeneral(Id);
            return Json(res);
        }
        [HttpGet]
        public async Task<IActionResult> Aboutus()
        {
            var aboutus = await generalLogic.ReturnByLabel("aboutus");
            var model = new AddEditAboutusViewModel()
            {
                Name = aboutus.First().Name,
                Description = aboutus.First().Description,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Aboutus(AddEditAboutusViewModel model)
        {
            ModelState.Remove("File");
            if (ModelState.IsValid)
            {
                var aboutus = await generalLogic.ReturnByLabel("aboutus");
                if (aboutus == null)
                {
                    var aboutusModel = new General()
                    {
                        Name = model.Name,
                        label = "aboutus",
                        Description = model.Description,
                    };
                    var res = await generalLogic.AddGeneral(aboutusModel);
                    if (!res)
                    {
                        ViewBag.error = "لطفا در وارد نمودن فیلد ها دقت کنید";
                    }
                    if (model.File != null)
                    {
                        var destination = "Image/icons/aboutus";
                        var webroot = webHostEnvironment.WebRootPath;
                        var result = await FileManager.Upload(model.Name, Path.Combine(webroot, destination), null, null, model.File);
                        if (!result.result)
                        {
                            ViewBag.error = result.message;
                        }
                        else
                        {
                            aboutusModel.ImageName = model.Name + result.message.ToString();
                            await generalLogic.UpdateGeneral(aboutusModel);
                        }
                    }
                }
                else
                {
                    aboutus.First().Name = model.Name;
                    aboutus.First().Description = model.Description;
                    if (model.File != null)
                    {
                        var destination = "Image/icons/aboutus";
                        var webroot = webHostEnvironment.WebRootPath;
                        var result = await FileManager.Upload(model.Name, Path.Combine(webroot, destination), null, null, model.File);
                        if (!result.result)
                        {
                            ViewBag.error = result.message;
                        }
                        else
                        {
                            aboutus.First().ImageName = model.Name + result.message.ToString();
                            var res = await generalLogic.UpdateGeneral(aboutus.First());
                            if (!res)
                            {
                                ViewBag.error = "لطفا در وارد نمودن فیلد ها دقت کنید";
                            }
                        }
                    }
                }
            }

            return View(model);
        }
        public IActionResult ContactList()
        {
            return View();
        }
        public IActionResult AddContact()
        {
            return View();
        }
        public async Task<IActionResult> SliderList()
        {
            var slides = await generalLogic.ReturnByLabel("slider");
            return View(slides);
        }
        [HttpGet]
        public IActionResult AddSlider()
        {
            return View();
        }
        public async Task<IActionResult> AddSlider(AddEditSliderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var slide = new General()
                {
                    Name = model.Name,
                    label = "slider",

                };
                if (await generalLogic.AddGeneral(slide))
                {
                    if (model.File != null)
                    {
                        var destination = "Image/slider/";
                        var webroot = webHostEnvironment.WebRootPath;
                        var res = await FileManager.Upload(model.Name, Path.Combine(webroot, destination), null, null, model.File);
                        if (res.result)
                        {
                            slide.ImageName = model.Name + res.message.ToString();
                            await generalLogic.UpdateGeneral(slide);
                            ViewBag.success = "با موفقیت اسلاید افزوده شد";
                            return View();
                        }
                    }
                    ModelState.AddModelError("", "لطفا یک عکس انتخاب کنید");
                };
                ModelState.AddModelError("", "لطفا نام اسلاید را وارد کنید");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditSlide(int Id)
        {
            var slide = await generalLogic.GeneralDetail(Id);
            var model = new AddEditSliderViewModel()
            {
                Name = slide.Name,
            };
            return View("AddSlider", model);
        }
        [HttpPost]
        public async Task<IActionResult> EditSlide(AddEditSliderViewModel model)
        {
            ModelState.Remove("File");
            if (ModelState.IsValid)
            {
                var slide = await generalLogic.GeneralDetail(model.Id);
                slide.Name = model.Name;
                if (model.File != null)
                {
                    var destination = "Image/slider/";
                    var webroot = webHostEnvironment.WebRootPath;
                    var res = await FileManager.Upload(model.Name, Path.Combine(webroot, destination), null, null, model.File);
                    if (res.result)
                    {
                        slide.ImageName = model.Name + res.message.ToString();
                    }
                }
                await generalLogic.UpdateGeneral(slide);
                return RedirectToAction("SliderList");
            }
            return View("AddSlider", model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteSlide(int Id)
        {
            var slide = await generalLogic.GeneralDetail(Id);
            var message = "Error Not Found !!";
            var FinResult = false;
            if (slide != null)
            {
                var webroot = webHostEnvironment.WebRootPath;
                var destination = "Image/slider/" + slide.ImageName;
                var path = Path.Combine(webroot, destination);
                var result = await FileManager.DeleteFile(path);
                if (result.result)
                {
                    var deleteRes = await generalLogic.DeleteGeneral(slide.Id);
                    FinResult = deleteRes;
                    if (deleteRes)
                    {
                        message = "با موفقیت انجام شد";
                    }
                    else
                    {
                        message = "خطایی رخ داده است ";
                    }
                }
                else
                {
                    FinResult = result.result;
                    message = result.message;
                }
            }
            return Json(new { result = FinResult, message = message });
        }

    }
}

