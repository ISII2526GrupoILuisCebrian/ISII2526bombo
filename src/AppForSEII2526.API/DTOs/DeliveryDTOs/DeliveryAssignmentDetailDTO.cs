using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.DTOs.DeliveryDTOs
{
    public class AssignedOrderDTO
    {
        public AssignedOrderDTO(int id, string street, string city, string postalCode,
                                DateTime date, decimal totalPrice, PriorityType priority)
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
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        public PriorityType Priority { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AssignedOrderDTO dto &&
                   Id == dto.Id &&
                   Street == dto.Street &&
                   City == dto.City &&
                   PostalCode == dto.PostalCode &&
                   Date == dto.Date &&
                   TotalPrice == dto.TotalPrice &&
                   Priority == dto.Priority;
        }
    }

    public class DeliveryAssignmentDetailDTO
    {
        public DeliveryAssignmentDetailDTO(int id, string driverName, DateTime deadline,
                                           string? personalMessage, decimal? extraReward,
                                           IList<AssignedOrderDTO> assignedOrders)
        {
            Id = id;
            DriverName = driverName;
            Deadline = deadline;
            PersonalMessage = personalMessage;
            ExtraReward = extraReward;
            AssignedOrders = assignedOrders;
        }

        public int Id { get; set; }
        public string DriverName { get; set; } = string.Empty;

        public DateTime Deadline { get; set; }

        [Precision(10, 2)]
        public decimal? ExtraReward { get; set; }

        public string? PersonalMessage { get; set; }

        public IList<AssignedOrderDTO> AssignedOrders { get; set; } = new List<AssignedOrderDTO>();

        public override bool Equals(object? obj)
        {
            return obj is DeliveryAssignmentDetailDTO dto &&
                   Id == dto.Id &&
                   DriverName == dto.DriverName &&
                   Deadline == dto.Deadline &&
                   ExtraReward == dto.ExtraReward &&
                   PersonalMessage == dto.PersonalMessage &&
                   AssignedOrders.SequenceEqual(dto.AssignedOrders);
        }
    }
}
