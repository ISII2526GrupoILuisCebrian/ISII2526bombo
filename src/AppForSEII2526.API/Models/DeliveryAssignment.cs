namespace AppForSEII2526.API.Models
{
    public class DeliveryAssignment
    {
        public int Id { get; set; }
        [Range(1,1000, ErrorMessage = "Minimum 1, maximum 1000.")]
        [Precision(5,2)]
        public decimal ExtraReward { get; set; }
        [Required]
        public DateTime DeliveryAssignmentDone { get; set; }
        [StringLength(500, ErrorMessage = "Maximum 500 characters allowed.")]
        public string PersonalMessage { get; set; }
    }
}
