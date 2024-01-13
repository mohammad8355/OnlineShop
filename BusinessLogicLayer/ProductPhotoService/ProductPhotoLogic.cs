 
using BusinessEntity;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ProductPhotoService
{
    public class ProductPhotoLogic
    {
        private readonly MainRepository<ProductPhoto> ProductPhotoRepository;
        public ProductPhotoLogic(MainRepository<ProductPhoto> ProductPhotoRepository)
        {
            this.ProductPhotoRepository = ProductPhotoRepository;
        }

        public async Task<bool> AddProductPhoto(ProductPhoto model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.format) || model.Size == 0 || model.Product_Id == 0)
            {
                return false;
            }
            else
            {
                await ProductPhotoRepository.AddItem(model);
                return true;
            }
        }
        public async Task<bool> UpdateProductPhoto(ProductPhoto model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.format) || model.Size == 0 || model.Product_Id == 0)
            {
                return false;
            }
            else
            {
                 ProductPhotoRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteProductPhoto(int Id)
        {
            if (await ProductPhotoRepository.DeleteItem(Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<ProductPhoto> ProductPhotoDetail(int Id)
        {
            if (ProductPhotoRepository.Get(p => p.Id == Id).Result.Any())
            {
                var model = ProductPhotoRepository.Get(p => p.Id == Id).Result.FirstOrDefault();
                return model;
            }
            else
            {
                return new ProductPhoto();
            }
        }
        public ICollection<ProductPhoto> ProductPhotoList()
        {
            ICollection<ProductPhoto> ProductPhotos = new List<ProductPhoto>();
            foreach (var item in ProductPhotoRepository.Get().Result.ToList())
            {
              
                ProductPhotos.Add(item);
            }
            return ProductPhotos;
        }
    }

}
