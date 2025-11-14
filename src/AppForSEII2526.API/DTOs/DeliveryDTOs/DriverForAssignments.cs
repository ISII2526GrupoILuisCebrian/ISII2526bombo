namespace AppForSEII2526.API.DTOs.DeliveryDTOs
{
    public class DriverForAssignmentDTO
    {
        public DriverForAssignmentDTO(int id, string name, bool available)
        {
            Id = id;
            Name = name;
            Available = available;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }
    }
}