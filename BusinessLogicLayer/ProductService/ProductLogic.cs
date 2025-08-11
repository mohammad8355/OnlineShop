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
using BusinessLogicLayer.ProductService.Dtos;

namespace BusinessLogicLayer.ProductService
{
    public class ProductLogic
    {
        private readonly MainRepository<Product> ProductRepository;
        private readonly MainRepository<CategoryToProduct> CategoryToProductRepository;
        private readonly MainRepository<Category> CategoryRepository;
        private readonly MainRepository<KeyToProduct> KeyToProductRepository;
        private readonly MainRepository<DiscountToProduct> DiscountToProductRepository;
        private readonly MainRepository<ProductPhoto> productphotoRepository;
        private readonly CategoryToProductLogic categoryToProductLogic;
        private readonly MainRepository<ValueToProduct> ValueToProductRepository;
        private readonly MainRepository<FavoriteProduct> favoriteProductRepository;
        private readonly CommentLogic commentLogic;
        private readonly MainRepository<AdjValue> adjValueLogic;
        public ProductLogic(CommentLogic commentLogic,
            MainRepository<ValueToProduct> ValueToProductRepository,
            
            MainRepository<FavoriteProduct> favoriteProductRepository,
            MainRepository<AdjValue> adjValueLogic, CategoryToProductLogic categoryToProductLogic,
            MainRepository<Category> categoryRepository,
            MainRepository<Product> ProductRepository, MainRepository<CategoryToProduct> SubRepository, 
            MainRepository<KeyToProduct> KeyToProductRepository, MainRepository<DiscountToProduct> discountToProductRepository,
            MainRepository<ProductPhoto> productphotoRepository)
        {
            this.ProductRepository = ProductRepository;
            this.commentLogic = commentLogic;
            CategoryToProductRepository = SubRepository;
            this.KeyToProductRepository = KeyToProductRepository;
            DiscountToProductRepository = discountToProductRepository;
            this.productphotoRepository = productphotoRepository;
            CategoryRepository = categoryRepository;
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

        public async Task<List<ProductCardDto>> GetProductByCategoryIdList(int categoryId )
        {
            var childIds = await CategoryRepository.Get(c => c.ParentId == categoryId).Select(c => c.Id).ToListAsync(); 
            var productIdList = await CategoryToProductRepository.Get(c => childIds.Contains(c.Category_Id))
                .Select(c => c.Product_Id).ToListAsync();
            var products = await ProductRepository.Get(p => productIdList.Contains(p.Id)).Select(c =>
                new ProductCardDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Price = (long)c.Price,
                    Count = c.QuantityInStock,
                    cover = "",
                    score = 4,
                    Sku = c.Name
                }).ToListAsync();
            return products;
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

            if (productphotoRepository.Get(pp => pp.Product_Id == Id).Any())
                foreach (var photo in productphotoRepository.Get(pp => pp.Product_Id == Id).ToList())
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
            if (ProductRepository.Get(p => p.Id == Id).Any())
            {
                var model = ProductRepository.Get(p => p.Id == Id, b => b.brand).FirstOrDefault();
                model.keyToProducts = KeyToProductRepository.Get(k => k.Product_Id == model.Id, v => v.adjKey).ToList();
                foreach (var key in model.keyToProducts)
                {
                    var values = adjValueLogic.Get(v => v.adjkey_Id == key.Key_Id).ToList();
                    key.adjKey.adjValues = values;
                }
                model.discountToProducts = DiscountToProductRepository.Get(d => d.Product_Id == model.Id).ToList();
                model.ProductPhotos = productphotoRepository.Get(p => p.Product_Id == model.Id).ToList();
                model.valueToProducts = ValueToProductRepository.Get(v => v.Product_Id == Id).ToList();
                model.commnets = commentLogic.CommentsOfProduct(Id);
                model.favoriteProducts = favoriteProductRepository.Get(f => f.Product_Id == Id).ToList();
                model.CategoryToProducts = categoryToProductLogic.CategoryToProductList().Where(cp => cp.Product_Id == model.Id).ToList();
                return model;
            }
            else
            {
                return new Product();
            }
        }

        private async Task<List<ProductCardDto>> GetProductCardListByOrder()
        {
            return await ProductRepository.Get().Select(c => new ProductCardDto()
            {
                Id = c.Id,
                Name = c.Name,
                Price = (long)c.Price,
                LikeCount = c.Like,
                Count = c.QuantityInStock,
                cover = c.ProductPhotos.FirstOrDefault().Name,
                score = 4,
                Sku = c.Name
            }).ToListAsync();
            
        }
        public ICollection<Product> ProductList()
        {
            ICollection<Product> products = new List<Product>();
            foreach (var item in ProductRepository.Get().ToList())
            {
                item.keyToProducts = KeyToProductRepository.Get(k => k.Product_Id == item.Id, k => k.adjKey).ToList();
                foreach (var key in item.keyToProducts)
                {
                    var values = adjValueLogic.Get(v => v.adjkey_Id == key.Key_Id).ToList();
                    key.adjKey.adjValues = values;
                }
                item.valueToProducts = ValueToProductRepository.Get(v => v.Product_Id == item.Id).ToList();
                item.favoriteProducts = favoriteProductRepository.Get(f => f.Product_Id == item.Id).ToList();
                item.discountToProducts = DiscountToProductRepository.Get(d => d.Product_Id == item.Id, v => v.discount).ToList();
                item.ProductPhotos = productphotoRepository.Get(p => p.Product_Id == item.Id).ToList();
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
        public async Task<IEnumerable<ProductCardDto>> PopularProduct()
        {
            var productList = await GetProductCardListByOrder();
            var Popularproduct = productList.OrderBy(p => p.LikeCount).ToList();
            return Popularproduct;
        }
        public async Task<IEnumerable<ProductCardDto>> MostExpensiveProduct()
        {
            var productList = await GetProductCardListByOrder();
            var MostExpensiveProduct = productList.OrderBy(p => p.Price);
            return MostExpensiveProduct;
        }
        public async Task<IEnumerable<ProductCardDto>> CheapestProduct()
        {
            var productList = await GetProductCardListByOrder();
            var MostExpensiveProduct = productList.OrderByDescending(p => p.Price);
            return MostExpensiveProduct;
        }
        public async Task<IEnumerable<ProductCardDto>> BestSellingProduct()
        {
            var productList = await GetProductCardListByOrder();
            var BestSellingProduct = productList;
            return BestSellingProduct;
        }
        // public async Task<IEnumerable<ProductCardDto>> OffProducts()
        // {
        //     var productList = await GetProductCardListByOrder();
        //     var offProducts = productList.Where(p => p.Discount > 0).OrderBy(p => p.Discount);
        //     return offProducts;
        // }
        // public async Task<IEnumerable<ProductCardDto>> HotDiscountProduct(float redLine)
        // {
        //     if (redLine > 0)
        //     {
        //         var productList = await GetProductCardListByOrder();
        //         var HotDiscountProduct = productList.Where(p => p.Discount >= redLine).OrderBy(p => p.Discount);
        //         return HotDiscountProduct;
        //     }
        //     return new List<Product>();
        // }
        public async Task<IEnumerable<ProductCardDto>> NewsetProduct()
        {
            var productList = await GetProductCardListByOrder();
            var newsetProduct = productList.OrderByDescending(p => p.Id);
            return newsetProduct;
        }
        public List<Product> Search(string SearchInput = "", int Category_Id = 0)
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
        public async Task<List<Product>> SearchFilter(int Category_Id, bool isExit, List<int> values, decimal minPrice = 0, decimal maxPrice = 0,string sortby = "")
        {
            var productList = new List<Product>();
            if (Category_Id != 0)
            {
                var tempList = ProductList();
                productList = tempList.Where(p => p.CategoryToProducts.Select(cp => cp.Category_Id).Contains(Category_Id)).ToList();
            }
            else
            {
                var tempList = ProductList();                productList = tempList.ToList();
            }
            if (isExit)
            {
                productList = productList.Where(p => p.QuantityInStock > 0).ToList();
            }
            if (values.Count() > 0)
            {
                var valuesList = new List<AdjValue>();
                foreach (var id in values)
                {
                    valuesList.Add(adjValueLogic.Get(av => av.Id == id).First());
                }
                var GroupedValuesList = valuesList.GroupBy(v => v.adjkey_Id).ToList();
                foreach (var group in GroupedValuesList)
                {
                    var groupvalue = new List<Product>();
                    foreach (var value in group)
                    {
                        groupvalue.AddRange(productList.Where(p => p.valueToProducts.Select(vp => vp.Value_Id).Contains(value.Id)).ToList());
                    }
                    productList = groupvalue;

                }
            }
            if (minPrice != 0 && maxPrice != 0)
            {
                productList = productList.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
            }
            if (minPrice != 0 && maxPrice == 0)
            {
                productList = productList.Where(p => p.Price >= minPrice).ToList();
            }
            if (minPrice == 0 && maxPrice != 0)
            {
                productList = productList.Where(p => p.Price <= maxPrice).ToList();
            }
            return productList.ToList();
        }
        public async Task<IEnumerable<ProductCardDto>> SortBy(string sortby = "")
        {
            switch (sortby)
            {
                case "expensive":
                    return await MostExpensiveProduct();
                case " cheap":
                    return await CheapestProduct();
                case "bestsell":
                    return await BestSellingProduct();
                default:
                    return await GetProductCardListByOrder();
                // case "new":
                //     return NewsetProduct();
                // case "popular":
                //     return await PopularProduct();
                // default:
                //     return ProductList();
            }
        }
    }
}
