namespace AppForSEII2526.API.DTOs.BanReportDTOs
{
    public class BanReportForDetailDTO 
    {
        public string Id { get; set; }

        public string ReportReason { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string? Message { get; set; }
    }
}
