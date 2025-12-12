using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DeliveryDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII2526.UT.DeliveriesController_test
{
    public class ScheduleDelivery_test : AppForSEII25264SqliteUT
    {
        private const int _driverId = 1;
        private const int _wrongDriver = 999;

        private const int _order1 = 1;
        private const int _order2 = 2;
        private const int _assignedOrder = 3;

        public ScheduleDelivery_test()
        {
            var customer = new ApplicationUser
            {
                Id = "cust",
                UserName = "cust@test.com",
                Name = "CustomerName",
                Surname = "CustomerSurname",
                Address = "Long Address"
            };

            var payment = new PayPal { Email = "cust@test.com", User = customer };

            _context.Add(customer);
            _context.Add(payment);

            _context.Add(new DeliveryDriver
            {
                Id = _driverId,
                Name = "Antonio Driver",
                Available = true
            });

            _context.AddRange(new List<PurchaseOrder>
            {
                new PurchaseOrder
                {
                    Id = _order1,
                    Street = "C/ Mayor 1",
                    City = "Albacete",
                    PostalCode = "02001",
                    Date = DateTime.Today.AddDays(-1),
                    NameSurname = "Customer One",
                    TotalPrice = 50m,
                    State = PurchaseState.Request,
                    Customer = customer,
                    PaymentMethod = payment
                },
                new PurchaseOrder
                {
                    Id = _order2,
                    Street = "C/ Menor 2",
                    City = "Albacete",
                    PostalCode = "02002",
                    Date = DateTime.Today,
                    NameSurname = "Customer Two",
                    TotalPrice = 30m,
                    State = PurchaseState.Request,
                    Customer = customer,
                    PaymentMethod = payment
                },
                new PurchaseOrder
                {
                    Id = _assignedOrder,
                    Street = "C/ Ocupada 3",
                    City = "Albacete",
                    PostalCode = "02003",
                    Date = DateTime.Today,
                    NameSurname = "Customer Three",
                    TotalPrice = 10m,
                    State = PurchaseState.Delivery,
                    Customer = customer,
                    PaymentMethod = payment
                }
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task ScheduleDelivery_NotFound_Driver()
        {
            var controller = new DeliveriesController(_context, new Mock<ILogger<DeliveriesController>>().Object);

            var dto = new DeliveryAssignmentCreateDTO
            {
                DeliveryDriverId = _wrongDriver,
                Deadline = DateTime.Today.AddDays(2),
                OrdersToAssign = new List<OrderPriorityDTO>
                {
                    new() { PurchaseOrderId = _order1, Priority = PriorityType.High }
                }
            };

            var result = await controller.ScheduleDelivery(dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(notFound.Value);

            Assert.StartsWith("Selected delivery driver does not exist", details.Errors.First().Value[0]);
        }

        [Fact]
        public async Task ScheduleDelivery_NotFound_InvalidOrders()
        {
            var controller = new DeliveriesController(_context, new Mock<ILogger<DeliveriesController>>().Object);

            var dto = new DeliveryAssignmentCreateDTO
            {
                DeliveryDriverId = _driverId,
                Deadline = DateTime.Today.AddDays(2),
                OrdersToAssign =
                new List<OrderPriorityDTO>
                {
                    new() { PurchaseOrderId = _order1, Priority = PriorityType.High },
                    new() { PurchaseOrderId = _assignedOrder, Priority = PriorityType.Low }
                }
            };

            var result = await controller.ScheduleDelivery(dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(notFound.Value);

            Assert.StartsWith("Some orders were invalid", details.Errors.First().Value[0]);
        }

        [Fact]
        public async Task ScheduleDelivery_Success_test()
        {
            var controller = new DeliveriesController(_context, new Mock<ILogger<DeliveriesController>>().Object);

            var deadline = DateTime.Today.AddDays(2);

            var dto = new DeliveryAssignmentCreateDTO
            {
                DeliveryDriverId = _driverId,
                ExtraReward = 10.00m,
                PersonalMessage = "Please deliver before lunch",
                Deadline = deadline,
                OrdersToAssign = new List<OrderPriorityDTO>
                {
                    new() { PurchaseOrderId = _order1, Priority = PriorityType.High },
                    new() { PurchaseOrderId = _order2, Priority = PriorityType.Medium }
                }
            };

            var result = await controller.ScheduleDelivery(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var detailDto = Assert.IsType<DeliveryAssignmentDetailDTO>(created.Value);

            var expected = new DeliveryAssignmentDetailDTO(
                detailDto.Id, // auto-generated ID
                "Antonio Driver",
                deadline,
                dto.PersonalMessage,
                dto.ExtraReward,
                new List<AssignedOrderDTO>
                {
                    new(_order1, "C/ Mayor 1", "Albacete", "02001",
                        DateTime.Today.AddDays(-1), 50m, PriorityType.High),

                    new(_order2, "C/ Menor 2", "Albacete", "02002",
                        DateTime.Today, 30m, PriorityType.Medium)
                }
            );

            Assert.Equal(expected, detailDto);
        }
    }
}
