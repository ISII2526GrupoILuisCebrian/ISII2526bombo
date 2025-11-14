namespace AppForSEII2526.API.DTOs.DeliveryDTOs
{
    public class AssignedOrderDTO
    {
        public AssignedOrderDTO(int id, string street, string city, string postalCode, DateTime date, decimal totalPrice, PriorityType priority)
        {
            Id = id;
            Street = street;
            City = city;
            PostalCode = postalCode;
            Date = date;
            TotalPrice = totalPrice;
            Priority = priority;
        }

        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public DateTime Date { get; set; }

        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }
        public PriorityType Priority { get; set; }
    }
    public class DeliveryAssignmentDetailDTO
    {
        public DeliveryAssignmentDetailDTO(int id, string driverName, DateTime deadline, string? personalMessage, decimal? extraReward, IList<AssignedOrderDTO> assignedOrders)
        {
            Id = id;
            DriverName = driverName;
            Deadline = deadline;
            PersonalMessage = personalMessage;
            ExtraReward = extraReward;
            AssignedOrders = assignedOrders;
        }

        public int Id { get; set; }
        public string DriverName { get; set; }
        public DateTime Deadline { get; set; }

        [Precision(10, 2)]
        public decimal? ExtraReward { get; set; }

        public string? PersonalMessage { get; set; }

        public IList<AssignedOrderDTO> AssignedOrders { get; set; }
    }
}
