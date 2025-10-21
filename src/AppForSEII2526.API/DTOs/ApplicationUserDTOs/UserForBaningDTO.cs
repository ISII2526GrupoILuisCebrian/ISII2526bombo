namespace AppForSEII2526.API.DTOs.ApplicationUserDTOs
{
    public class UserForBaningDTO
    {
        public UserForBaningDTO(string name, string surname, DateTime accountCreationDate, IList<ComplaintDTO> complaints)
        {
            Name = name;
            Surname = surname;
            AccountCreationDate = accountCreationDate;
            Complaints = complaints;
            //Complaint = complaint;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        //public string Complaints { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public IList <ComplaintDTO> Complaints{ get; set; }
        



    }
}
