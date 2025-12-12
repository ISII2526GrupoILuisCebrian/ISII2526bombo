using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class DeliveryStateContainer
    {

        //shceduling state
        public DriverForAssignmentDTO? SelectedDriver { get; private set; }

        public DateTime Deadline { get; private set; } = DateTime.Today.AddDays(1);

        public string? PersonalMessage { get; private set; }

        public decimal? ExtraReward { get; private set; }

        private readonly List<DeliveryOrderSelection> _selectedOrders = new();

        public IReadOnlyList<DeliveryOrderSelection> SelectedOrders => _selectedOrders;

        public int SelectedOrderCount => _selectedOrders.Count;

        public decimal TotalOrderPrice =>
            (decimal)_selectedOrders.Sum(o => o.Order.TotalPrice);


        //result state

        public DeliveryAssignmentDetailDTO? CreatedAssignment { get; private set; }


      // state notif
        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();



        //order selection and priority
        public void AddOrder(OrderForSchedulingDTO order)
        {
            if (!_selectedOrders.Any(o => o.Order.Id == order.Id))
            {
                _selectedOrders.Add(new DeliveryOrderSelection
                {
                    Order = order,
                    Priority = PriorityType.Medium
                });
                NotifyStateChanged();
            }
        }

        public void RemoveOrder(int orderId)
        {
            var item = _selectedOrders.FirstOrDefault(o => o.Order.Id == orderId);
            if (item != null)
            {
                _selectedOrders.Remove(item);
                NotifyStateChanged();
            }
        }

        public void ClearOrders()
        {
            _selectedOrders.Clear();
            NotifyStateChanged();
        }

        public void SetPriority(int orderId, PriorityType priority)
        {
            var item = _selectedOrders.FirstOrDefault(o => o.Order.Id == orderId);
            if (item != null)
            {
                item.Priority = priority;
                NotifyStateChanged();
            }
        }



        //driver deadline message reward
        public void SetDriver(DriverForAssignmentDTO driver)
        {
            SelectedDriver = driver;
            NotifyStateChanged();
        }

        public void DeliveryProcessed()
        {
            SelectedDriver = null;
            Deadline = DateTime.Today.AddDays(1);
            PersonalMessage = null;
            ExtraReward = null;

            _selectedOrders.Clear();

            NotifyStateChanged();
        }


        public void SetDeadline(DateTime deadline)
        {
            Deadline = deadline;
            NotifyStateChanged();
        }

        public void SetPersonalMessage(string? message)
        {
            PersonalMessage = message;
            NotifyStateChanged();
        }

        public void SetExtraReward(decimal? reward)
        {
            ExtraReward = reward;
            NotifyStateChanged();
        }


       //post dto
        public DeliveryAssignmentCreateDTO BuildRequestDTO()
        {
            return new DeliveryAssignmentCreateDTO
            {
                DeliveryDriverId = SelectedDriver?.Id ?? 0,
                Deadline = Deadline,
                PersonalMessage = PersonalMessage,
                ExtraReward = (double?)ExtraReward,

                OrdersToAssign = _selectedOrders
                    .Select(o => new OrderPriorityDTO
                    {
                        PurchaseOrderId = o.Order.Id,
                        Priority = o.Priority
                    })
                    .ToList()
            };
        }


        //storing result after fto
        public void SetCreatedAssignment(DeliveryAssignmentDetailDTO assignment)
        {
            CreatedAssignment = assignment;
            NotifyStateChanged();
        }


        //reset scheduling data
        public void ResetSchedulingData()
        {
            SelectedDriver = null;
            Deadline = DateTime.Today.AddDays(1);
            PersonalMessage = null;
            ExtraReward = null;
            _selectedOrders.Clear();

            NotifyStateChanged();
        }

       
        public void ClearAll()
        {
            ResetSchedulingData();
            CreatedAssignment = null;
            NotifyStateChanged();
        }
    }


 
    public class DeliveryOrderSelection
    {
        public OrderForSchedulingDTO Order { get; set; } = default!;
        public PriorityType Priority { get; set; }
    }


}


