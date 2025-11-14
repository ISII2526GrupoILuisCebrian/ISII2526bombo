namespace AppForSEII2526.API.DTOs.PurchaseDTOs;
using DataType = System.ComponentModel.DataAnnotations.DataType;

public class PurchaseProductDTO
{
    public PurchaseProductDTO(
        int productId,
        string productName,
        string brandName,
<<<<<<< HEAD
        string? colour,
        int quantity,
        decimal unitPrice)
=======
        int quantity,
        decimal price,
        string? description)
>>>>>>> origin/development
    {
        ProductId = productId;
        ProductName = productName;
        BrandName = brandName;
<<<<<<< HEAD
        Colour = colour;
        Quantity = quantity;
        UnitPrice = unitPrice;
=======
        Quantity = quantity;
        Price = price;
        Description = description;
>>>>>>> origin/development
    }

    public int ProductId { get; set; }

<<<<<<< HEAD
    [Required, StringLength(100, MinimumLength = 3)]
    public string ProductName { get; set; }

    [Required, StringLength(100)]
    public string BrandName { get; set; }

    [StringLength(30)]
    public string? Colour { get; set; }

=======
    [StringLength(50)]
    public string ProductName { get; set; }

    [StringLength(50)]
    public string BrandName { get; set; }

>>>>>>> origin/development
    [Range(1, 100)]
    public int Quantity { get; set; }

    [DataType(DataType.Currency)]
    [Precision(10, 2)]
<<<<<<< HEAD
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
=======
    public decimal Price { get; set; }

    public string? Description { get; set; }
>>>>>>> origin/development


}



