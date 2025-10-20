namespace AppForSEII2526.API.DTOs.ProductDTOs
{
    public class ProductForPurchasingDTO
    {
        public ProductForPurchasingDTO(int id, string name, string brandName)
        {
            Id = id;
            Name = name;
            Brand = brandName;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name can be neither longer than 50 characters nor shorter than 10. ", MinimumLength = 10)]
        public string Name { get; set; }

        public string Brand { get; set; }
    }
}


