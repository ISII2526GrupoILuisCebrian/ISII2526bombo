namespace AppForSEII2526.API.Models
{
    public class DeliveryAssignment
    {
        public int Id { get; set; }

        [Precision(5, 2)]
        public decimal ExtraReward { get; set; }

        [Required]
        public DateTime DeliveryAssignmentDone { get; set; }

        [StringLength(500, ErrorMessage = "Maximum 500 characters allowed.")]
        public string PersonalMessage { get; set; }

        public IList<PurchaseDelivery> PurchaseDeliveries { get; set; }

        public int DeliveryDriverId { get; set; }
        public DeliveryDriver DeliveryDriver { get; set; }

    }
}
