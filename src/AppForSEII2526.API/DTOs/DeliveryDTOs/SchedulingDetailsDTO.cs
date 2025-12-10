using AppForSEII2526.API.DTOs.DeliveryDriverDTOs;

namespace AppForSEII2526.API.DTOs.DeliveryDTOs
{
    public class SchedulingDetailsDTO
    {
        public SchedulingDetailsDTO(IList<OrderForSchedulingDTO> selectedOrders,
                                    IList<DriverForAssignmentDTO> availableDrivers)
        {
            SelectedOrders = selectedOrders;
            AvailableDrivers = availableDrivers;
        }

        public IList<OrderForSchedulingDTO> SelectedOrders { get; set; } = new List<OrderForSchedulingDTO>();
        public IList<DriverForAssignmentDTO> AvailableDrivers { get; set; } = new List<DriverForAssignmentDTO>();

        public override bool Equals(object? obj)
        {
            return obj is SchedulingDetailsDTO dto &&
                   SelectedOrders.SequenceEqual(dto.SelectedOrders) &&
                   AvailableDrivers.SequenceEqual(dto.AvailableDrivers);
        }
    }
}
