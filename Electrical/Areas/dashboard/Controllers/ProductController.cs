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
        private readonly UploadFile uploadImage;
        private readonly ProductPhotoLogic productPhotoLogic;
        public ProductController(CategoryLogic categoryLogic, ProductLogic productLogic,SubCategoryLogic SubCategoryLogic, UploadFile uploadImage, ProductPhotoLogic productPhotoLogic)
        {
            this.productLogic = productLogic;
            this.categoryLogic = categoryLogic;
            subcategoryLogic = SubCategoryLogic;
            this.uploadImage = uploadImage;
            this.productPhotoLogic = productPhotoLogic;

        }
        public IActionResult Index()
        {
            var model = new ProductListViewModel()
            {
                products = productLogic.ProductList(),
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            var model = new AddEditProductViewModel()
            {
                Categories = (List<Category>)categoryLogic.CategoryList(),
            };
            ViewBag.categories = new SelectList(model.Categories, "Id", "Name");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddEditProductViewModel model)
        {
            //bind subcategory class to product model by use SubCategoryDetail method 
            model.Product.subCategory = subcategoryLogic.SubCategoryDetail(model.Product.SubCategory_Id);
            //add product to db
            var resault = await productLogic.AddProduct(model.Product);
            if (resault)
            {
                var limitsize = 0;
                var destination = "Image\\Product\\" + model.Product.subCategory.Parent + "\\" + model.Product.subCategory.IdentifierName + "\\" + model.Product.Name;
                //foreach on images files
                foreach (var file in model.File)
                {
                    //make uniqe name
                    var ImageName = Guid.NewGuid().ToString();
                    //upload image on server;
                    var UploadFileResault = await uploadImage.UploadImage(ImageName, destination, limitsize, "", file);
                    //if upload successfull 
                    if ((bool)UploadFileResault.First() == true)
                    {
                        //convert bytes to kilobytes
                        var ImageSize = file.Length / 1024;
                        ProductPhoto productPhoto = new ProductPhoto()
                        {
                            Name = ImageName + UploadFileResault.Last().ToString(),
                            Size = (int)ImageSize,
                            Product_Id = model.Product.Id,
                            format = UploadFileResault.Last().ToString(),
                        };
                        //add photo to productphoto model
                        await productPhotoLogic.AddProductPhoto(productPhoto);

                    }
                    else
                    {
                        ModelState.AddModelError("", UploadFileResault.Last().ToString());
                        return View(model);
                    }

                }
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
            var category = categoryLogic.CategoryDetail(category_Id);
            var subcategories = subcategoryLogic.SubCategoryList().Where(s => s.Parent == category.IdentifierName).ToList();
            return Json(new SelectList(subcategories, "Id", "Name"));

        }
        public IActionResult ProductDetails()
        {
            return View();
        }
        public IActionResult AddOptions()
        {
            return View();
        }
    }
}
