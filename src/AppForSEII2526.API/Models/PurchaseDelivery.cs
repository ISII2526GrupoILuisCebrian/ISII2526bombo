namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(DeliveryAssignmentId), nameof(PurchaseOrderId))]
    public class PurchaseDelivery
    {
        public DeliveryAssignment DeliveryAssignment { get; set; }
        public int DeliveryAssignmentId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }

        public PriorityType Priority { get; set; }

    }
}
