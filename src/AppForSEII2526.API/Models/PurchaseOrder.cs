namespace AppForSEII2526.API.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public IList<PurchaseDelivery> PurchaseDeliveries { get; set; }

        public PurchaseState PurchaseState { get; set; }

    }
}
