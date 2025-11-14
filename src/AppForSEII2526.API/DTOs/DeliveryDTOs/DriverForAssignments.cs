namespace AppForSEII2526.API.DTOs.DeliveryDTOs
{
    public class DriverForAssignmentDTO
    {
        private bool available;

        public DriverForAssignmentDTO(int id, string name, string? phoneNumber)
        {
            Id = id;
            Name = name;
        }

        public DriverForAssignmentDTO(int id, string name, bool available)
        {
            Id = id;
            Name = name;
            this.available = available;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}