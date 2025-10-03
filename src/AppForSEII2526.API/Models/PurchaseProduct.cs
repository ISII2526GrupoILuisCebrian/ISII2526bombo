namespace AppForSEII2526.API.Models
{
    public class PurchaseProduct
    {
        public int Id { get; set; }

        [Precision(7, 2)]
        public decimal Price { get; set; }

        public int ProductId { get; set; }
        public int PurchaseOrderId { get; set; }

        [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10")]
        public int Quantity { get; set; }

        public Product Product { get; set; }

    }
}
