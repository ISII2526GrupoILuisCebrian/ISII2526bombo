using AppForSEII2526.API.DTOs.DeliveryDriverDTOs;

namespace AppForSEII2526.API.DTOs.DeliveryDTOs
{
    public class SchedulingDetailsDTO
    {
        public SchedulingDetailsDTO(IList<OrderForSchedulingDTO> selectedOrders, IList<DriverForAssignmentDTO> availableDrivers)
        {
            SelectedOrders = selectedOrders;
            AvailableDrivers = availableDrivers;
        }

        public IList<OrderForSchedulingDTO> SelectedOrders { get; set; }

        public IList<DriverForAssignmentDTO> AvailableDrivers { get; set; }
    }
}
