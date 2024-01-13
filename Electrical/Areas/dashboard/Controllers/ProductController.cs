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

        private readonly HeadCategoryLogic headCategoryLogic;
        private readonly ProductLogic productLogic;
        private readonly CategoryLogic categoryLogic;
        private readonly SubCategoryLogic subcategoryLogic;
        private readonly UploadFile fileManager;
        private readonly ProductPhotoLogic productPhotoLogic;
        public ProductController(CategoryLogic categoryLogic,HeadCategoryLogic headCategoryLogic, ProductLogic productLogic, SubCategoryLogic SubCategoryLogic, UploadFile fileManager, ProductPhotoLogic productPhotoLogic)
        {
            this.productLogic = productLogic;
            this.headCategoryLogic = headCategoryLogic;
            subcategoryLogic = SubCategoryLogic;
            this.fileManager = fileManager;
            this.productPhotoLogic = productPhotoLogic;
            this.categoryLogic = categoryLogic;

        }
        //list of product
        public IActionResult Index()
        {
            #region bind view model
            var model = new ProductListViewModel()
            {
                products = productLogic.ProductList(),
                categories = categoryLogic.CategoryList(),
            };
            #endregion
            return View(model);
        }
        //add product  view 
        [HttpGet]
        public IActionResult AddProduct()
        { 
            var headcategories = (List<HeadCategory>)headCategoryLogic.HeadCategoryList();
            #region create select list of categories
            ViewBag.categories = new SelectList(headcategories, "Id", "Name");
            #endregion
            return View();
        }
        //get model and add product
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddEditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var subCategory = subcategoryLogic.SubCategoryDetail(model.SubCategory_Id);
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
                #region assign values to model
                var ProductModel = new Product()
                {
                    Name = model.Name,
                    Price = model.Price,
                    QuantityInStock = model.QuantityInStock,
                    Description = model.Description,
                    Width = model.Width,
                    height = model.height,
                    Weight = model.Weight,
                    length = model.length,
                    SubCategory_Id = model.SubCategory_Id,
                    subCategory = subCategory,
                };
                #endregion
                var resault = await productLogic.AddProduct(ProductModel);
                if (resault)
                {
                    #region set requried parameters such as limitsize and destination of files
                    var limitsize = 0;
                    var category = categoryLogic.CategoryDetail(subCategory.category_Id);
                    var destination = "Image\\Product\\" + category.Parent + "\\" + subCategory.Parent + "\\" + subCategory.IdentifierName + "\\" + model.Name;
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
                            var product = productLogic.ProductList().Where(p => p.Name == model.Name && p.SubCategory_Id == model.SubCategory_Id).First();
                            ProductPhoto productPhoto = new ProductPhoto()
                            {
                                Name = ImageName + UploadFileResault.Last().ToString(),
                                Size = (int)ImageSize,
                                Product_Id = product.Id,
                                format = UploadFileResault.Last().ToString(),
                            };
                            #endregion
                            //add photo to productphoto model
                           var result =  await productPhotoLogic.AddProductPhoto(productPhoto);
                            if (!result)
                            {
                                ModelState.AddModelError("", "افزودن ابا مشکل مواجه شد");
                                return View(model);
                            }

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
            var headcategories = (List<HeadCategory>)headCategoryLogic.HeadCategoryList();
            #region create select list of categories
            ViewBag.categories = new SelectList(headcategories, "Id", "Name");
            #endregion
            return View(model);

        }
        [HttpGet("Product/SubCategoryDropDown/Id")]
        public JsonResult SubCategoryDropDown(int category_Id)
        {
            if(category_Id != 0)
            {
                #region make list of categories and subcategories for create select list
                var category = categoryLogic.CategoryDetail(category_Id);
                var subcategories = subcategoryLogic.SubCategoryList().Where(s => s.category_Id == category.Id).ToList();
                #endregion
                return Json(new SelectList(subcategories, "Id", "Name"));
            }
            return Json(new SelectList(new List<SubCategory>(), "Id", "Name"));
        }
        [HttpGet("Product/CategoryDropDown/Id")]
        public JsonResult CategoryDropDown(int category_Id)
        {
            #region make list of categories and subcategories for create select list
            if(category_Id != 0)
            {
                var headcategory = headCategoryLogic.HeadCategoryDetail(category_Id);
                var categories = categoryLogic.CategoryList().Where(s => s.headCategory_Id == headcategory.Id).ToList();
                return Json(new SelectList(categories, "Id", "Name"));
            }
            else
            {
                return Json(new SelectList(new List<Category>(), "Id", "Name"));
            }
            #endregion

        }
        [HttpGet]
        public async Task<IActionResult> ProductDetails(int Id)
        {
            var model = new ProductDetailViewModel()
            {
                product = await productLogic.ProductDetail(Id),
                categories = categoryLogic.CategoryList(),
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ProductEdit(int Id)
        {
            var ProductModel = await productLogic.ProductDetail(Id);
            #region bind add edit view model 
            var model = new AddEditProductViewModel()
            {
                Name = ProductModel.Name,
                Price = ProductModel.Price,
                QuantityInStock = ProductModel.QuantityInStock,
                Description = ProductModel.Description,
                Width = ProductModel.Width,
                height = ProductModel.height,
                Weight = ProductModel.Weight,
                length = ProductModel.length,
                SubCategory_Id = ProductModel.SubCategory_Id,

            };
            #endregion
            #region create select list of categories
            var headCategories = (List<HeadCategory>)headCategoryLogic.HeadCategoryList();
            var categories = categoryLogic.CategoryDetail(ProductModel.subCategory.category_Id);
            ViewBag.categories = new SelectList(headCategories, "Id", "Name",categories.headCategory_Id);
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
            var ProductModel = await productLogic.ProductDetail(model.Id);
            ProductModel.Name = model.Name;
            ProductModel.Price = model.Price;
            ProductModel.QuantityInStock = model.QuantityInStock;
            ProductModel.Description = model.Description;
            ProductModel.Width = model.Width;
            ProductModel.height = model.height;
            ProductModel.Weight = model.Weight;
            ProductModel.length = model.length;
            ProductModel.SubCategory_Id = model.SubCategory_Id;
            var resault = await productLogic.UpdateProduct(ProductModel);
            if (resault)
            {
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
                    var subcategory = subcategoryLogic.SubCategoryDetail(model.SubCategory_Id);
                    var category = categoryLogic.CategoryDetail(subcategory.category_Id);
                    var destination = "Image\\Product\\" + category.Parent + "\\" + subcategory.Parent + "\\" + subcategory.IdentifierName + "\\" + model.Name;
                    #region delete last files 
                    foreach (var pp in productPhotoLogic.ProductPhotoList().Where(pp => pp.Product_Id == model.Id).ToList())
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
                                Product_Id = model.Id,
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
