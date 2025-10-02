namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BanReportId),
    nameof(CustomerId))]
    public class ReportCustomer
    {
        
        public int BanReportId { get; set; }        
        public int CustomerId { get; set; }
        public string Message { get; set; }


    }
}
