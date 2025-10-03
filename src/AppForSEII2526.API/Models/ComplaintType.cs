namespace AppForSEII2526.API.Models
{
    public class ComplaintType
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }


        //Relationships
        public List<Complaint> Complaints { get; set; } // Navigation property to Complaint
    }
}
