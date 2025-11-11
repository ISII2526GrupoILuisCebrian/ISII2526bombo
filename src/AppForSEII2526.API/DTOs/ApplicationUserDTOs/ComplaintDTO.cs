namespace AppForSEII2526.API.DTOs.ApplicationUserDTOs
{
    public class ComplaintDTO
    {
        public ComplaintDTO(string complaintName, int numComplaints)
        {
            ComplaintName = complaintName;
            NumComplaints = numComplaints;
        }

        public string ComplaintName { get; set; }
        public int NumComplaints { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ComplaintDTO dTO &&
                   ComplaintName == dTO.ComplaintName &&
                   NumComplaints == dTO.NumComplaints;
        }
    }
}
