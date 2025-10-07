namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ProductId), nameof(PurchaseOrderId))]
    public class PurchaseProduct
    {
        
        [Precision(7, 2)]
        public decimal Price { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public int PurchaseOrderId { get; set; }

        [ForeignKey(nameof(PurchaseOrderId))]
        public PurchaseOrder PurchaseOrder { get; set; }

        [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10")]
        public int Quantity { get; set; }

       

        
    }
}
