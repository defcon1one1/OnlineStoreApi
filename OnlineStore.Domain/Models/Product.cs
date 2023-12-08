using OnlineStore.Domain.Products.Commands.UpdateProduct;

namespace OnlineStore.Domain.Models;
public class Product(Guid id, string name, string? description, decimal price)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public decimal Price { get; private set; } = price;

    public void Update(UpdateProductData updateProductData)
    {
        if (updateProductData is null) throw new ArgumentException(null, nameof(updateProductData));
        Name = updateProductData.Name;
        Description = updateProductData.Description;
        Price = updateProductData.Price;
    }
}
