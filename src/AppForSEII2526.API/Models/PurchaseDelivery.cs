namespace AppForSEII2526.API.Models
{
    [Index(nameof(PurchaseOrderInt), IsUnique = true)]
    public class PurchaseDelivery
    {
        [Key]
        public int DeliveryAssignmentId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        public int PurchaseOrderInt { get; set; }


        enum PriorityType
        {
            Low,
            Medium,
            High
        }
    }
}
