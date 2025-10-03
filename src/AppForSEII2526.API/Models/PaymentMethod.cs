namespace AppForSEII2526.API.Models
{
    public abstract class PaymentMethod
    {
        public int Id { get; set; }

        //Relations
        public PurchaseOrder PurchaseOrder { get; set; } // 1 to 1 with PurchaseOrder
    }
}
