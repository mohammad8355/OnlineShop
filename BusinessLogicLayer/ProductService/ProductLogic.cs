using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;
using BusinessLogicLayer.CategoryToProductService;
using BusinessLogicLayer.ProductPhotoService;
using BusinessLogicLayer.AdjValueService;
using BusinessLogicLayer.favoriteProductService;
using BusinessLogicLayer.CommentService;

namespace BusinessLogicLayer.ProductService
{
    public class ProductLogic
    {
        private readonly MainRepository<Product> ProductRepository;
        private readonly MainRepository<Category> SubCategoryRepository;
        private readonly MainRepository<KeyToProduct> KeyToProductRepository;
        private readonly MainRepository<DiscountToProduct> DiscountToProductRepository;
        private readonly MainRepository<ProductPhoto> productphotoRepository;
        private readonly CategoryToProductLogic categoryToProductLogic;
        private readonly MainRepository<ValueToProduct> ValueToProductRepository;
        private readonly MainRepository<FavoriteProduct> favoriteProductRepository;
        private readonly CommentLogic commentLogic;
        private readonly MainRepository<AdjValue> adjValueLogic;
        public ProductLogic(CommentLogic commentLogic, MainRepository<ValueToProduct> ValueToProductRepository, MainRepository<FavoriteProduct> favoriteProductRepository, MainRepository<AdjValue> adjValueLogic, CategoryToProductLogic categoryToProductLogic, MainRepository<Product> ProductRepository, MainRepository<Category> SubRepository, MainRepository<KeyToProduct> KeyToProductRepository, MainRepository<DiscountToProduct> discountToProductRepository, MainRepository<ProductPhoto> productphotoRepository)
        {
            this.ProductRepository = ProductRepository;
            this.commentLogic = commentLogic;
            SubCategoryRepository = SubRepository;
            this.KeyToProductRepository = KeyToProductRepository;
            DiscountToProductRepository = discountToProductRepository;
            this.productphotoRepository = productphotoRepository;
            this.ValueToProductRepository = ValueToProductRepository;
            this.favoriteProductRepository = favoriteProductRepository;
            this.categoryToProductLogic = categoryToProductLogic;
            this.adjValueLogic = adjValueLogic;

        }

        public async Task<bool> AddProduct(Product model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.ProductCode))
            {
                return false;
            }
            else
            {
                await ProductRepository.AddItem(model);
                return true;
            }
        }
        public async Task<bool> UpdateProduct(Product model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.ProductCode))
            {
                return false;
            }
            else
            {
                ProductRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteProduct(int Id)
        {
            if (categoryToProductLogic.CategoryToProductList().Where(cp => cp.Product_Id == Id).Any())
                foreach (var catpro in categoryToProductLogic.CategoryToProductList().Where(cp => cp.Product_Id == Id).ToList())
                {
                    await categoryToProductLogic.DeleteCategoryToProduct(catpro.Category_Id, catpro.Product_Id);
                };

            if (productphotoRepository.Get(pp => pp.Product_Id == Id).Result.Any())
                foreach (var photo in productphotoRepository.Get(pp => pp.Product_Id == Id).Result.ToList())
                {
                    await productphotoRepository.DeleteItem(photo.Id);
                };
            if (await ProductRepository.DeleteItem(Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<Product> ProductDetail(int Id)
        {
            if (ProductRepository.Get(p => p.Id == Id).Result.Any())
            {
                var model = ProductRepository.Get(p => p.Id == Id, b => b.brand).Result.FirstOrDefault();
                model.keyToProducts = KeyToProductRepository.Get(k => k.Product_Id == model.Id, v => v.adjKey).Result.ToList();
                foreach (var key in model.keyToProducts)
                {
                    var values = adjValueLogic.Get(v => v.adjkey_Id == key.Key_Id).Result.ToList();
                    key.adjKey.adjValues = values;
                }
                model.discountToProducts = DiscountToProductRepository.Get(d => d.Product_Id == model.Id).Result.ToList();
                model.ProductPhotos = productphotoRepository.Get(p => p.Product_Id == model.Id).Result.ToList();
                model.valueToProducts = ValueToProductRepository.Get(v => v.Product_Id == Id).Result.ToList();
                model.commnets = commentLogic.CommentsOfProduct(Id);
                model.favoriteProducts = favoriteProductRepository.Get(f => f.Product_Id == Id).Result.ToList();
                model.CategoryToProducts = categoryToProductLogic.CategoryToProductList().Where(cp => cp.Product_Id == model.Id).ToList();
                return model;
            }
            else
            {
                return new Product();
            }
        }
        public ICollection<Product> ProductList()
        {
            ICollection<Product> products = new List<Product>();
            foreach (var item in ProductRepository.Get().Result.ToList())
            {
                item.keyToProducts = KeyToProductRepository.Get(k => k.Product_Id == item.Id, k => k.adjKey).Result.ToList();
                foreach (var key in item.keyToProducts)
                {
                    var values = adjValueLogic.Get(v => v.adjkey_Id == key.Key_Id).Result.ToList();
                    key.adjKey.adjValues = values;
                }
                item.valueToProducts = ValueToProductRepository.Get(v => v.Product_Id == item.Id).Result.ToList();
                item.favoriteProducts = favoriteProductRepository.Get(f => f.Product_Id == item.Id).Result.ToList();
                item.discountToProducts = DiscountToProductRepository.Get(d => d.Product_Id == item.Id, v => v.discount).Result.ToList();
                item.ProductPhotos = productphotoRepository.Get(p => p.Product_Id == item.Id).Result.ToList();
                item.commnets = commentLogic.CommentsOfProduct(item.Id);
                item.CategoryToProducts = categoryToProductLogic.CategoryToProductList().Where(cp => cp.Product_Id == item.Id).ToList();
                products.Add(item);
            }
            return products;
        }
        public List<Product> RelatedProduct(List<int> categoryIds)
        {
            var relatedProduct = new List<Product>();
            foreach (var cp in categoryIds)
            {
                relatedProduct.AddRange(categoryToProductLogic.CategoryToProductList().Where(c => c.Category_Id == cp).Select(c => c.Product).ToList());
            }
            return relatedProduct;
        }
        public async Task<IEnumerable<Product>> PopularProduct()
        {
            var productList = ProductList();
            var Popularproduct = productList.OrderBy(p => p.Like);
            return Popularproduct;
        }
        public async Task<IEnumerable<Product>> MostExpensiveProduct()
        {
            var productList = ProductList();
            var MostExpensiveProduct = productList.OrderBy(p => p.Price);
            return MostExpensiveProduct;
        }
        public async Task<IEnumerable<Product>> CheapestProduct()
        {
            var productList = ProductList();
            var MostExpensiveProduct = productList.OrderByDescending(p => p.Price);
            return MostExpensiveProduct;
        }
        public async Task<IEnumerable<Product>> BestSellingProduct()
        {
            var productList = ProductList();
            var BestSellingProduct = productList.OrderBy(p => p.OrderDetails.Where(o => o.order.IsFinally == true).Count());
            return BestSellingProduct;
        }
        public async Task<IEnumerable<Product>> OffProducts()
        {
            var productList = ProductList();
            var offProducts = productList.Where(p => p.Discount > 0).OrderBy(p => p.Discount);
            return offProducts;
        }
        public async Task<IEnumerable<Product>> HotDiscountProduct(float redLine)
        {
            if (redLine > 0)
            {
                var productList = ProductList();
                var HotDiscountProduct = productList.Where(p => p.Discount >= redLine).OrderBy(p => p.Discount);
                return HotDiscountProduct;
            }
            return new List<Product>();
        }
        public IEnumerable<Product> NewsetProduct()
        {
            var productList = ProductList();
            var newsetProduct = productList.OrderByDescending(p => p.Id);
            return newsetProduct;
        }
        public List<Product> Search(string SearchInput="",int Category_Id=0)
        {
            var productList = ProductList().ToList();
            if (Category_Id != 0)
            {
               productList = productList.Where(p => p.Name.Contains(SearchInput) && p.CategoryToProducts.Select(c => c.Category_Id).Contains(Category_Id)).ToList();

            }
            else
            {
               productList = productList.Where(p => p.Name.Contains(SearchInput)).ToList();
            }
            return productList;
        }
        public List<Product> SearchFilter(bool isExit, List<int> values, decimal minPrice = 0, decimal maxPrice = 0)
        {
            var productList = ProductList();
            if (isExit)
            {
                productList = productList.Where(p => p.QuantityInStock > 0).ToList();
            }
            if (values.Count() > 0)
            {
                var valuesList = new List<AdjValue>();
                foreach(var id in values)
                {
                    valuesList.Add(adjValueLogic.Get(av => av.Id == id).Result.First());
                }
                var GroupedValuesList = valuesList.GroupBy(v => v.adjkey_Id).ToList();
                foreach(var group in GroupedValuesList)
                {
                    var groupvalue = new List<Product>();
                   foreach(var value in group)
                    {
                        groupvalue.AddRange(productList.Where(p => p.valueToProducts.Select(vp => vp.Value_Id).Contains(value.Id)).ToList());
                    }
                   if(groupvalue.Count() > 0)
                    {
                        productList = groupvalue;
                    }
                }
            }
            if (minPrice != 0 && minPrice != 0)
            {
                productList = productList.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
            }
            if(minPrice !=0 && maxPrice == 0)
            {
                productList = productList.Where(p => p.Price >= minPrice).ToList();
            }
            if (minPrice == 0 && maxPrice != 0)
            {
                productList = productList.Where(p => p.Price <= maxPrice).ToList();
            }
            return productList.ToList();
        }

    }
}
