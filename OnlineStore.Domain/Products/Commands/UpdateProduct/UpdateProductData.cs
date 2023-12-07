namespace OnlineStore.Domain.Products.Commands.UpdateProduct;
public record UpdateProductData
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}
