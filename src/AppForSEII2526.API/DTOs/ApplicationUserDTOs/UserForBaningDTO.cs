
namespace AppForSEII2526.API.DTOs.ApplicationUserDTOs
{
    public class UserForBaningDTO
    {
        public UserForBaningDTO(string name, string surname, DateTime accountCreationDate, IList<ComplaintDTO> complaints, string message)
        {
            Name = name;
            Surname = surname;
            AccountCreationDate = accountCreationDate;
            Complaints = complaints;
            Message = message;

        }

        public string Name { get; set; }
        public string Surname { get; set; }
        
        public DateTime AccountCreationDate { get; set; }
        public IList <ComplaintDTO> Complaints{ get; set; }
        public string? Message { get; set; }
        public override bool Equals(object? obj)
        {
            return obj is UserForBaningDTO dTO &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname &&
                   AccountCreationDate == dTO.AccountCreationDate &&
                   EqualityComparer<IList<ComplaintDTO>>.Default.Equals(Complaints, dTO.Complaints);
        }
    }
}
