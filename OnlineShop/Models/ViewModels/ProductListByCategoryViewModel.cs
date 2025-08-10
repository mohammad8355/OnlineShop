using BusinessLogicLayer.CategoryService.Dtos;
using BusinessLogicLayer.ProductService.Dtos;

namespace PresentationLayer.Models.ViewModels;

public class ProductListByCategoryViewModel
{
    public List<CategoryNameAndIdListDto> _categories { get; set; }
    public int? SelectedCategoryId { get; set; }
    public List<ProductCardDto> Products { get; set; }
}