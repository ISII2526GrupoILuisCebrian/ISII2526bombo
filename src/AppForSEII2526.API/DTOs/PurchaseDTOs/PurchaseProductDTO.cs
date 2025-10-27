namespace AppForSEII2526.API.DTOs.PurchaseDTOs;
using DataType = System.ComponentModel.DataAnnotations.DataType;

public class PurchaseProductDTO
{
    public PurchaseProductDTO(
        int productId,
        string productName,
        string brandName,
        int quantity,
        decimal price,
        string? description)
    {
        ProductId = productId;
        ProductName = productName;
        BrandName = brandName;
        Quantity = quantity;
        Price = price;
        Description = description;
    }

    public int ProductId { get; set; }

    [StringLength(50)]
    public string ProductName { get; set; }

    [StringLength(50)]
    public string BrandName { get; set; }

    [Range(1, 100)]
    public int Quantity { get; set; }

    [DataType(DataType.Currency)]
    [Precision(10, 2)]
    public decimal Price { get; set; }

    public string? Description { get; set; }


}



