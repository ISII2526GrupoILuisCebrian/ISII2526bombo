using AppForSEII2526.API.DTOs.ApplicationUserDTOs;

namespace AppForSEII2526.API.DTOs.BanReportDTOs
{
    public class BanReportForCreateDTO
    {
        public BanReportForCreateDTO(string reportReason, string description, DateTime startDate, DateTime endDate, string? message, IList<UserForBaningDTO> usersForBaning)
        {
            ReportReason = reportReason;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Message = message;
            UsersForBaning = usersForBaning;
        }

        public string ReportReason { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string? Message { get; set; }

        public IList<UserForBaningDTO> UsersForBaning { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BanReportForCreateDTO dTO &&
                   ReportReason == dTO.ReportReason &&
                   Description == dTO.Description &&
                   StartDate == dTO.StartDate &&
                   EndDate == dTO.EndDate &&
                   Message == dTO.Message &&
                   UsersForBaning.SequenceEqual(dTO.UsersForBaning);
        }
    }
}
