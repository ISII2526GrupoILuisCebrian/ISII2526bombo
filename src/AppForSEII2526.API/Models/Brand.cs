namespace AppForSEII2526.API.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot be longer than 100 characters.")]
        public string Location { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
