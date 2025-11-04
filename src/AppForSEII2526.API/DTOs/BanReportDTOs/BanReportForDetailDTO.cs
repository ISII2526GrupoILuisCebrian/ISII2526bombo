using AppForSEII2526.API.DTOs.ApplicationUserDTOs;

namespace AppForSEII2526.API.DTOs.BanReportDTOs
{
    public class BanReportForDetailDTO : BanReportForCreateDTO
    {
        public int Id { get; set; }

        // Aqui tengo que marcar todas las complaints como 'processed' y el ReportState como 'InProgress'
        // Luego, mostrar mensaje de que "La operacion se ha completado con exito"

        // LO QUE SE AÑADE ES EL NAME Y SURNAME, que viene de UserForBaningDTO

        /* Hereda de BanReportForCreateDTO:
         * Message
         * ReportReason
         * Description
         * StartDate
         * EndDate
         */

        public BanReportForDetailDTO(int id, string reportReason, string description, DateTime startDate,
            DateTime endDate, string? message, IList<ApplicationUserDTOs.UserForBaningDTO> usersForBaning)
            : base(reportReason, description, startDate, endDate, message, usersForBaning)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            return obj is BanReportForDetailDTO dTO &&
                   base.Equals(obj) &&
                   ReportReason == dTO.ReportReason &&
                   Description == dTO.Description &&
                   StartDate == dTO.StartDate &&
                   EndDate == dTO.EndDate &&
                   Message == dTO.Message &&
                   UsersForBaning.SequenceEqual(dTO.UsersForBaning) &&
                   Id == dTO.Id;
        }
    }
}
