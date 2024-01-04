using BusinessEntity;
using BusinessLogicLayer.CategoryService;
using BusinessLogicLayer.ProductService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.uploadfile;
using PresentationLayer.Models.ViewModels;
using System.Drawing;
using BusinessLogicLayer.ProductPhotoService;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class ProductController : Controller
    {
        private readonly CategoryLogic categoryLogic;
        private readonly ProductLogic productLogic;
        private readonly SubCategoryLogic subcategoryLogic;
        private readonly UploadFile fileManager;
        private readonly ProductPhotoLogic productPhotoLogic;
        public ProductController(CategoryLogic categoryLogic, ProductLogic productLogic, SubCategoryLogic SubCategoryLogic, UploadFile fileManager, ProductPhotoLogic productPhotoLogic)
        {
            this.productLogic = productLogic;
            this.categoryLogic = categoryLogic;
            subcategoryLogic = SubCategoryLogic;
            this.fileManager = fileManager;
            this.productPhotoLogic = productPhotoLogic;

        }
        //list of product
        public IActionResult Index()
        {
            #region bind view model
            var model = new ProductListViewModel()
            {
                products = productLogic.ProductList(),
            };
            #endregion
            return View(model);
        }
        //add product  view 
        [HttpGet]
        public IActionResult AddProduct()
        {
            #region bind addedit View Model
            var model = new AddEditProductViewModel()
            {
                Categories = (List<Category>)categoryLogic.CategoryList(),
            };
            #endregion

            #region create select list of categories
            ViewBag.categories = new SelectList(model.Categories, "Id", "Name");
            #endregion
            return View(model);
        }
        //get model and add product
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddEditProductViewModel model)
        {
            //in future this formats list would be dynamic
            #region regular image formats list 
            var formats = new List<string>()
            {
                ".jpg",
                ".jpeg",
                ".jfif",
                ".png",
            };
            #endregion

            #region check recevied format files
            foreach (var file in model.File)
            {
                var formatFileCheck = fileManager.FormatChecker(file, formats);
                if (!formatFileCheck)
                {
                    ModelState.AddModelError("", $"فایل با نام {file.FileName} فرمت های محاز را ندارد");
                    return View(model);
                }
            }
            #endregion
            //add product to db
            var resault = await productLogic.AddProduct(model.Product);
            if (resault)
            {
                #region set requried parameters such as limitsize and destination of files
                var limitsize = 0;
                var destination = "Image\\Product\\" + model.Product.subCategory.Parent + "\\" + model.Product.subCategory.IdentifierName + "\\" + model.Product.Name;
                #endregion

                #region upload image files 
                foreach (var file in model.File)
                {
                    //make uniqe name
                    var ImageName = Guid.NewGuid().ToString();
                    //upload image on server;

                    var UploadFileResault = await fileManager.fileManager(ImageName, destination, limitsize, "", file);
                    //if upload successfull 
                    if ((bool)UploadFileResault.First() == true)
                    {
                        //convert bytes to kilobytes
                        var ImageSize = file.Length / 1024;
                        #region bind productphoto model 
                        ProductPhoto productPhoto = new ProductPhoto()
                        {
                            Name = ImageName + UploadFileResault.Last().ToString(),
                            Size = (int)ImageSize,
                            Product_Id = model.Product.Id,
                            format = UploadFileResault.Last().ToString(),
                        };
                        #endregion
                        //add photo to productphoto model
                        await productPhotoLogic.AddProductPhoto(productPhoto);

                    }
                    else
                    {
                        ModelState.AddModelError("", UploadFileResault.Last().ToString());
                        return View(model);
                    }

                }
                #endregion
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "عملیات افزودن با شکست مواجه شد");
                return View(model);
            }


        }
        [HttpGet("Product/SubCategoryDropDown/Id")]
        public JsonResult SubCategoryDropDown(int category_Id)
        {
            #region make list of categories and subcategories for create select list
            var category = categoryLogic.CategoryDetail(category_Id);
            var subcategories = subcategoryLogic.SubCategoryList().Where(s => s.Parent == category.IdentifierName).ToList();
            #endregion
            return Json(new SelectList(subcategories, "Id", "Name"));
        }
        [HttpGet]
        public async Task<IActionResult> ProductDetails(int Id)
        {
            var model = await productLogic.ProductDetail(Id);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ProductEdit(int Id)
        {
            #region bind add edit view model 
            var model = new AddEditProductViewModel()
            {
                Product = await productLogic.ProductDetail(Id),
                Categories = (List<Category>)categoryLogic.CategoryList(),
            };
            #endregion
            #region create select list of categories
            ViewBag.categories = new SelectList(model.Categories, "Id", "Name", model.Product.subCategory.category_Id);
            #endregion
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(AddEditProductViewModel model)
        {
            //in future this list must be dynamic thta admin can control formats
            #region regular imagefiles formats
            var formats = new List<string>()
            {
                ".jpg",
                ".jpeg",
                ".jfif",
                ".png",
            };
            #endregion

            if (model.File != null)
            {
                #region check format of image files
                foreach (var file in model.File)
                {
                    var formatFileCheck = fileManager.FormatChecker(file, formats);
                    if (!formatFileCheck)
                    {
                        ModelState.AddModelError("", $"فایل با نام {file.FileName} فرمت های محاز را ندارد");
                        return View(model);
                    }
                }
                #endregion
                var limitsize = 0;
                var destination = "Image\\Product\\" + model.Product.subCategory.Parent + "\\" + model.Product.subCategory.IdentifierName + "\\" + model.Product.Name;
                var resault = await productLogic.UpdateProduct(model.Product);
                if (resault)
                {
                    #region delete last files 
                    foreach (var pp in productPhotoLogic.ProductPhotoList().Where(pp => pp.Product_Id == model.Product.Id).ToList())
                    {
                        //make complete path for delete file
                        var path = destination + "\\" + pp.Name;
                        //delete file from server
                        #region delete last images from server
                        var isSuccess = await fileManager.DeleteFile(path);
                        if ((bool)isSuccess.First())
                        {
                            #region delete last images from db
                            //delete photo from db
                            var isDelete = await productPhotoLogic.DeleteProductPhoto(pp.Id);
                            if (!isDelete)
                            {
                                ModelState.AddModelError("", "خطایی رخ داده است");
                                return View(model);
                            }
                            #endregion
                        }
                        else
                        {
                            ModelState.AddModelError("", isSuccess.Last().ToString());
                            return View(model);
                        }
                        #endregion
                    }
                    #endregion

                    #region upload new image files
                    foreach (var file in model.File)
                    {
                        //make uniqe name
                        var ImageName = Guid.NewGuid().ToString();
                        //upload image on server;
                        var UploadFileResault = await fileManager.fileManager(ImageName, destination, limitsize, "", file);
                        //if upload successfull 
                        if ((bool)UploadFileResault.First() == true)
                        {
                            //convert bytes to kilobytes
                            var ImageSize = file.Length / 1024;
                            #region bind productphoto model
                            ProductPhoto productPhoto = new ProductPhoto()
                            {
                                Name = ImageName + UploadFileResault.Last().ToString(),
                                Size = (int)ImageSize,
                                Product_Id = model.Product.Id,
                                format = UploadFileResault.Last().ToString(),
                            };
                            #endregion
                            //add photo to productphoto model
                            await productPhotoLogic.AddProductPhoto(productPhoto);

                        }
                        else
                        {
                            ModelState.AddModelError("", UploadFileResault.Last().ToString());
                            return View(model);
                        }

                    }
                    #endregion
                }

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "عملیات ویرایش با شکست مواجه شد");
                return View(model);
            }
        }
        [HttpGet("Product/DeleteProduct/Id")]
        public async Task<IActionResult> DeleteProduct(string Id)
        {
            #region delete product
            int PId = int.Parse(Id);
            var product = productLogic.ProductDetail(PId);
            var resualt = await productLogic.DeleteProduct(PId);
            #endregion
            return Json(new { name = product.Result.Name, Resualt = resualt });
        }
        public IActionResult AddOptions()
        {
            return View();
        }
    }
}
