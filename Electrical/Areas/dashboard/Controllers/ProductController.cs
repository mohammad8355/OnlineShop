using BusinessEntity;
using BusinessLogicLayer.CategoryService;
using BusinessLogicLayer.ProductService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.uploadfile;
using PresentationLayer.Models.ViewModels;
using System.Drawing;
using BusinessLogicLayer.ProductPhotoService;
using BusinessLogicLayer.CategoryToProductService;
using Microsoft.Build.Framework;
using BusinessLogicLayer.AdjKeyService;
using BusinessLogicLayer.KeyToProductService;
using BusinessLogicLayer.ValueToProductService;
using System.Text.Json;
using BusinessLogicLayer.AdjValueService;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class ProductController : Controller
    {
        private readonly ProductLogic productLogic;
        private readonly CategoryLogic categoryLogic;
        private readonly UploadFile fileManager;
        private readonly ProductPhotoLogic productPhotoLogic;
        private readonly CategoryToProductLogic categoryToProductLogic;
        private readonly AdjKeyLogic adjKeyLogic;
        private readonly AdjValueLogic adjvaluelogic;
        private readonly ValueToProductLogic valueToProductLogic;
        private readonly KeyToProductLogic keyToProductLogic;
        public ProductController(AdjValueLogic adjvaluelogic,ValueToProductLogic valueToProductLogic, KeyToProductLogic keyToProductLogic,AdjKeyLogic adjKeyLogic,CategoryToProductLogic categoryToProductLogic, CategoryLogic categoryLogic, ProductLogic productLogic, UploadFile fileManager, ProductPhotoLogic productPhotoLogic)
        {
            this.productLogic = productLogic;
            this.fileManager = fileManager;
            this.productPhotoLogic = productPhotoLogic;
            this.categoryLogic = categoryLogic;
            this.categoryToProductLogic = categoryToProductLogic;
            this.adjKeyLogic = adjKeyLogic;
            this.keyToProductLogic = keyToProductLogic;
            this.valueToProductLogic = valueToProductLogic;
            this.adjvaluelogic = adjvaluelogic;
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
            return View();
        }
        //get model and add product
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddEditProductViewModel model)
        {
            if (ModelState.IsValid)
            {                //in future this formats list would be dynamic
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
                };
                #endregion
                var resault = await productLogic.AddProduct(ProductModel);
                if (resault)
                {
                    var product = productLogic.ProductList().Where(p => p.Name == model.Name).First();
                    foreach (var cat in model.SelectList)
                    {
                        var categoryToProduct = new CategoryToProduct()
                        {
                            Category_Id = cat,
                            Product_Id = product.Id,
                        };
                        var addResult = await categoryToProductLogic.AddCategoryToProduct(categoryToProduct);
                        if (!addResult)
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"can not add relation between category with id {cat} with product {product.Name} with Id {product.Id}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    #region set requried parameters such as limitsize and destination of files
                    var limitsize = 0;
                    var destination = "Image\\Product\\" + model.Name;
                    #endregion

                    #region upload image files 
                    foreach (var file in model.File)
                    {
                        //make uniqe name
                        var ImageName = Guid.NewGuid().ToString();
                        //upload image on server;

                        var UploadFileResault = await fileManager.Upload(ImageName, destination, limitsize, "", file);
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
                                Product_Id = product.Id,
                                format = UploadFileResault.Last().ToString(),
                            };
                            #endregion
                            //add photo to productphoto model
                            var result = await productPhotoLogic.AddProductPhoto(productPhoto);
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
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> ProductDetails(int Id)
        {
            var model = new ProductDetailViewModel()
            {
                product = await productLogic.ProductDetail(Id),
                categories = categoryLogic.CategoryList(),
                keyToProducts = keyToProductLogic.KeyToProductList().Where(kp => kp.Product_Id == Id).ToList(),
                valueToProducts = valueToProductLogic.ValueToProductList().Where(vp => vp.Product_Id == Id).ToList(),

            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ProductEdit(int Id)
        {
            var ProductModel = await productLogic.ProductDetail(Id);
            var categorytoproduct = categoryToProductLogic.CategoryToProductList().Where(cp => cp.Product_Id == ProductModel.Id).Select(c => c.Category_Id).ToList();

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
                SelectList = categorytoproduct,
            };
            #endregion
            return View("AddProduct", model);
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
            var oldName = ProductModel.Name;
            ProductModel.Name = model.Name;
            ProductModel.Price = model.Price;
            ProductModel.QuantityInStock = model.QuantityInStock;
            ProductModel.Description = model.Description;
            ProductModel.Width = model.Width;
            ProductModel.height = model.height;
            ProductModel.Weight = model.Weight;
            ProductModel.length = model.length;
            var resault = await productLogic.UpdateProduct(ProductModel);
            if (resault)
            {

                var newpath = "Image\\Product\\" + model.Name + "\\";
                var oldpath = "Image\\Product\\" + oldName + "\\";
                    fileManager.ChangeDirFile(oldpath, newpath);
                   //fileManager.DeleteDire("Image\\Product\\" + oldName);
                var categorytoproductlist = categoryToProductLogic.CategoryToProductList().Where(cp => cp.Product_Id == model.Id).ToList();
                foreach (var catToPro in categorytoproductlist)
                {
                    var deleteResult = await categoryToProductLogic.DeleteCategoryToProduct(catToPro.Category_Id, catToPro.Product_Id);
                    if (!deleteResult)
                     Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"can not delete relation between category with id {catToPro} with product {model.Name} with Id {model.Id}");
                   Console.ForegroundColor = ConsoleColor.White;
                }
                foreach (var cat in model.SelectList)
                {
                    var categoryToProduct = new CategoryToProduct()
                    {
                        Category_Id = cat,
                        Product_Id = model.Id,
                    };
                    var addResult = await categoryToProductLogic.AddCategoryToProduct(categoryToProduct);
                    if (!addResult)
                     Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"can not add relation between category with id {cat} with product {model.Name} with Id {model.Id}");
                    Console.ForegroundColor = ConsoleColor.White;
                }

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
                    var destination = "Image\\Product\\" + model.Name;
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
                                return View("AddProduct", model);
                            }
                            #endregion
                        }
                        else
                        {
                            ModelState.AddModelError("", isSuccess.Last().ToString());
                            return View("AddProduct", model);
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
                        var UploadFileResault = await fileManager.Upload(ImageName, destination, limitsize, "", file);
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
                            return View("AddProduct", model);
                        }

                    }
                    #endregion

                }
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "عملیات ویرایش با شکست مواجه شد");
                return View("AddProduct", model);
            }
        }
        [HttpGet("Product/DeleteProduct/Id")]
        public async Task<IActionResult> DeleteProduct(string Id)
        {
            #region delete product
            int PId = int.Parse(Id);
            var product =  await productLogic.ProductDetail(PId);
            foreach (var photo in product.ProductPhotos)
            {
                var destination = "Image\\Product\\" + product.Name + "\\" + photo.Name;
                await fileManager.DeleteFile(destination);
            }
            fileManager.DeleteDire("Image\\Product\\" + product.Name);
            var resualt = await productLogic.DeleteProduct(PId);
            #endregion
            return Json(new { name = product.Name, Resualt = resualt });
        }
        [HttpGet("Product/DeleteKeyValue/Id")]
        public async Task<IActionResult> DeleteKeyValue(string key_Id,string product_Id)
        {
            #region delete product
            int keyId = int.Parse(key_Id);
            int productId = int.Parse(product_Id);
            var Result = await  keyToProductLogic.DeleteKeyToProduct(keyId, productId);
            var values = valueToProductLogic.ValueToProductList().Where(v => v.Product_Id == productId && v.Value.adjkey_Id == keyId).ToList();
            foreach(var value in values)
            {
                  Result = await valueToProductLogic.DeleteValueToProduct(value.Value_Id,value.Product_Id);
            }
            return Json(new { result = Result, name = " " });
            #endregion
        }
        [HttpGet]
        public IActionResult AddOptions()
        {
            ViewBag.keys = new SelectList(adjKeyLogic.AdjKeyList(),"Id","Name");
            ViewBag.Products = new SelectList(productLogic.ProductList(), "Id", "Name");
            var model = new AddKeyValueToProduct();
            return View(model);
        }
 

        [HttpPost] 
        public async Task<IActionResult> AddOptions(AddKeyValueToProduct model)
        {
            if (ModelState.IsValid)
            {
                foreach(var product in model.ProductIds)
                {
                    var keytoproduct = new KeyToProduct()
                    {
                        Key_Id = model.KeyId,
                        Product_Id = product,
                    };
                   await keyToProductLogic.AddKeyToProduct(keytoproduct);
                    foreach(var value in model.ValueIds)
                    {
                        var valuetoproduct = new ValueToProduct()
                        {
                            Value_Id = value,
                            Product_Id = product,
                        };
                       await valueToProductLogic.AddValueToProduct(valuetoproduct);
                    }  
                }
                ViewBag.Success = "عملیات افزودن کلید و مقدار به محصولات با موفقیت انجام شد";
                return View();
            }
            return View(model);

        }
    }

}
