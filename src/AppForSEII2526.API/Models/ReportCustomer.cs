namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BanReportId),
    nameof(CustomerId))]
    public class ReportCustomer
    {
        
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be between 1 and 999999")]
        [Key]
        public int CustomerId { get; set; } // PK

        [Range(1, 999999, ErrorMessage = "BanReportId must be between 1 and 999999")]
        public int BanReportId { get; set; } // FK: it reffers to the Id of BanReport class

        [StringLength(500, ErrorMessage = "Message cannot be longer than 500 characters.")]
        public string Message { get; set; } 

        public ReportState State { get; set; } // Enum: InProgress, Rejected

        //Relationships
        public BanReport BanReport { get; set; } // Navigation property to BanReport

        //public Customer Customers { get; set; } // Navigation property to Customer. I DON'T KNOW WHAT TO INTERPRET WITH 'CUSTOMER'

      

    }
}
