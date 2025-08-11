namespace BusinessLogicLayer.ProductService.Dtos;

public class ProductCardDto
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string Sku { get; set; }
    public long LikeCount { get; set; }
    public double score { get; set; }
    public string cover { get; set; }
    public long Price { get; set; }
    public long Count { get; set; }
}