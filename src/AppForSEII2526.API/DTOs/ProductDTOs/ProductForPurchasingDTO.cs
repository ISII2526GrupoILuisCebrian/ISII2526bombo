using DataType = System.ComponentModel.DataAnnotations.DataType;
namespace AppForSEII2526.API.DTOs.ProductDTOs
{
    public class ProductForPurchasingDTO
    {
        public ProductForPurchasingDTO(int id, string name, string brandName, string description, decimal purchasePrice, int quantity)
        {
            Id = id;
            Name = name;
            Brand = brandName;
            Description = description;
            PurchasePrice = purchasePrice;
            Quantity = quantity;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name can be neither longer than 50 characters nor shorter than 10. ", MinimumLength = 10)]
        public string Name { get; set; }

        public string Brand { get; set; }

        [StringLength(100, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Quantity Available For Purchase")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity available must be 0 or more.")]
        public int Quantity { get; set; }



    }
}




