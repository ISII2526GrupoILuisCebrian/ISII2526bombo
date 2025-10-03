namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Product
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name can be neither longer than 50 characters nor shorter than 10. ", MinimumLength =10)]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters.")]
        public string? Description { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Display(Name = "Selling Price")]
        [Precision(5, 2)]
        public decimal Price { get; set; }

        [StringLength(20, ErrorMessage = "Colour cannot be longer than 20 characters.")]
        public string Colour { get; set; }

        public bool IsReturnable { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }

        public Brand Branch { get; set; }

        public List<PurchaseProduct> PurchaseProducts { get; set; }
    }
}
