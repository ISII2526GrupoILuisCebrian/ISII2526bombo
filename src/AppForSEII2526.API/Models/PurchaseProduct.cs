namespace AppForSEII2526.API.Models
{
    public class PurchaseProduct
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int Quantity { get; set; }
    }
}
