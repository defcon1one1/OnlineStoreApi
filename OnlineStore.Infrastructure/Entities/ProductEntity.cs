using OnlineStore.Domain.Models;

namespace OnlineStore.Infrastructure.Entities;
public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Product ToProduct()
    {
        return new Product(Id, Name, Description, Price);
    }
    public static ProductEntity FromProduct(Product product)
    {
        return new ProductEntity
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };
    }
}
