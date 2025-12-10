using AppForSEII2526.API.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.DTOs.DeliveryDTOs
{
    public class OrderPriorityDTO
    {
        [Required]
        public int PurchaseOrderId { get; set; }

        [Required]
        public PriorityType Priority { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is OrderPriorityDTO dto &&
                   PurchaseOrderId == dto.PurchaseOrderId &&
                   Priority == dto.Priority;
        }
    }

    public class DeliveryAssignmentCreateDTO
    {
        [Required]
        public int DeliveryDriverId { get; set; }

        [Precision(10, 2)]
        [Range(0.00, 100.00, ErrorMessage = "Extra reward must be between 0.00 and 100.00 EUR.")]
        public decimal? ExtraReward { get; set; }

        [StringLength(200, ErrorMessage = "Personal message cannot exceed 200 characters.")]
        public string? PersonalMessage { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one purchase order must be selected.")]
        public IList<OrderPriorityDTO> OrdersToAssign { get; set; } = new List<OrderPriorityDTO>();

        public override bool Equals(object? obj)
        {
            return obj is DeliveryAssignmentCreateDTO dto &&
                   DeliveryDriverId == dto.DeliveryDriverId &&
                   ExtraReward == dto.ExtraReward &&
                   PersonalMessage == dto.PersonalMessage &&
                   Deadline == dto.Deadline &&
                   OrdersToAssign.SequenceEqual(dto.OrdersToAssign);
        }
    }
}
