namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class DeliveryDriver
    {
        public int Id { get; set; }
        [Required]
        public bool Available { get; set; }
        [StringLength(50, ErrorMessage = "Name can be neither longer than 50 characters nor shorter than 10.", MinimumLength=10)]
        [Required]
        public string Name { get; set; }
        public List<DeliveryAssignment> Assignments { get; set; }

    }
}
