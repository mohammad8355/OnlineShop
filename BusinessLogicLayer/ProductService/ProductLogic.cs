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

        public async Task<Product> AddProduct(Product model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.ProductCode))
            {
                return new Product();
            }
            else
            {
                var product = await ProductRepository.AddItem(model);
                return product;
            }
        }

        public async Task<List<ProductCardDto>> GetProductByCategoryIdList(int categoryId)
        {
            return await ProductRepository.Get(
                    p => p.CategoryToProducts
                        .Any(cp => cp.Category_Id == categoryId || cp.Category.ParentId == categoryId),
                    p => p.ProductPhotos
                )
                .Select(p => new ProductCardDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = (long)p.Price,
                    Count = p.QuantityInStock,
                    cover = p.ProductPhotos.FirstOrDefault().Name ?? "",
                    score = 4,
                    Sku = p.ProductCode
                })
                .ToListAsync();
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
        public async Task<Product?> ProductDetail(int id)
        {
            return await ProductRepository.Get(p => p.Id == id)
                .Include(p => p.brand)
                .Include(c => c.commnets)
                .Include(c => c.ProductPhotos)
                .Include(p => p.keyToProducts)
                .ThenInclude(k => k.adjKey)
                .ThenInclude(a => a.adjValues)
                .Include(p => p.discountToProducts)
                .Include(p => p.ProductPhotos)
                .Include(p => p.valueToProducts)
                .Include(p => p.favoriteProducts)
                .Include(p => p.CategoryToProducts)
                .FirstOrDefaultAsync();
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

        public async Task<List<SelectListItemDto>> ProductSelectList()
        {
            return await ProductRepository.Get()
                .Select(c => new SelectListItemDto(c.Name, c.Id.ToString()))
                .ToListAsync();
        }
        public async Task<List<Product>> ProductList()
        {
            return await ProductRepository.Get()
                .Include(p => p.ProductPhotos)
                .Select(c => new Product()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price,
                    QuantityInStock = c.QuantityInStock,
                    ProductPhotos = c.ProductPhotos,
                })
                .ToListAsync();
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
        public async Task<List<Product>> Search(string searchInput = "", int categoryId = 0)
        {
            var query = ProductRepository.Get(
                p => p.Name.Contains(searchInput),
                p => p.CategoryToProducts
            ).Include(c => c.ProductPhotos).AsQueryable();

            if (categoryId != 0)
                query = query.Where(p => p.CategoryToProducts.Any(c => c.Category_Id == categoryId));

            return await query.ToListAsync();
        }
        public async Task<List<Product>> SearchFilter(
            int categoryId, bool isExit, List<int> values,
            decimal minPrice = 0, decimal maxPrice = 0, string sortby = "")
        {
            var query = ProductRepository.Get(
                p => true,
                p => p.valueToProducts,
                p => p.CategoryToProducts
            ).Include(c => c.ProductPhotos).AsQueryable();

            if (categoryId != 0)
                query = query.Where(p => p.CategoryToProducts.Any(cp => cp.Category_Id == categoryId));

            if (isExit)
                query = query.Where(p => p.QuantityInStock > 0);

            if (values.Any())
                query = query.Where(p => p.valueToProducts.Any(v => values.Contains(v.Value_Id)));

            if (minPrice > 0)
                query = query.Where(p => p.Price >= minPrice);

            if (maxPrice > 0)
                query = query.Where(p => p.Price <= maxPrice);

            // Sorting
            query = sortby switch
            {
                "expensive" => query.OrderByDescending(p => p.Price),
                "cheap" => query.OrderBy(p => p.Price),
                "bestsell" => query.OrderByDescending(p => p.Like),
                _ => query.OrderBy(p => p.Id)
            };

            return await query.ToListAsync();
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
