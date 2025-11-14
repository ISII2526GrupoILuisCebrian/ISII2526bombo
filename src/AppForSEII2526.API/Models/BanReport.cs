namespace AppForSEII2526.API.Models
{
    [Index(nameof(DetailedDescription), IsUnique = true)]
    public class BanReport
    {
        [StringLength(1000, ErrorMessage = "Detailed Desciption can be neither longer than 1000 characters nor shorter than 10.", MinimumLength=10)]
        public string DetailedDescription { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Reason can be neither longer than 100 characters nor shorter than 10.", MinimumLength = 10)]
        public string Reason { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        // Relationships
        public List<ReportCustomer> ReportCustomers { get; set; } // Navigation property to ReportCustomer
    }
}
