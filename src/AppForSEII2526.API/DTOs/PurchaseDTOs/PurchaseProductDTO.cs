namespace AppForSEII2526.API.DTOs.PurchaseDTOs;
using DataType = System.ComponentModel.DataAnnotations.DataType;

public class PurchaseProductDTO
{
    public PurchaseProductDTO(
        int productId,
        string productName,
        string brandName,
        string? colour,
        int quantity,
        decimal unitPrice)

    {
        ProductId = productId;
        ProductName = productName;
        BrandName = brandName;
        Colour = colour;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public int ProductId { get; set; }


    [Required, StringLength(100, MinimumLength = 3)]
    public string ProductName { get; set; }

    [Required, StringLength(100)]
    public string BrandName { get; set; }

    [StringLength(30)]
    public string? Colour { get; set; }

    [Range(1, 100)]
    public int Quantity { get; set; }

    [DataType(DataType.Currency)]
    [Precision(10, 2)]

    public decimal UnitPrice { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is PurchaseProductDTO dTO &&
               ProductId == dTO.ProductId &&
               ProductName == dTO.ProductName &&
               BrandName == dTO.BrandName &&
               Colour == dTO.Colour &&
               UnitPrice == dTO.UnitPrice &&
               Quantity == dTO.Quantity;
    }

    
}



