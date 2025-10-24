namespace AppForSEII2526.API.DTOs.ProductDTOs
{
    public class ProductForPurchasingDTO
    {
        public ProductForPurchasingDTO(int id, string name, string brandName, int quantity, string location)
        {
            Id = id;
            Name = name;
            Brand = brandName;
            Quantity = quantity;
            Location = location;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name can be neither longer than 50 characters nor shorter than 10. ", MinimumLength = 10)]
        public string Name { get; set; }

        public string Brand { get; set; }

        [Range(0, 10, ErrorMessage = "The quantity cannot be greater than 10.")]
        public int Quantity { get; set; }

        public string Location { get; set; }
    }
}


