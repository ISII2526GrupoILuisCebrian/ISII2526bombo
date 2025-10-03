namespace AppForSEII2526.API.Models
{
    public class PurchaseDelivery
    {
        [Key]
        public int DeliveryAssignmentId { get; set; }
        public DateTime Date { get; set; }
        public int PurchaseOrderInt { get; set; }


        enum PriorityType
        {
            Low,
            Medium,
            High
        }
    }
}
