using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DeliveryDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using AppForSEII2526.API.DTOs.PurchaseDTOs;

namespace AppForSEII2526.UT.DeliveriesController_test
{
    public class GetSchedulingDetails_test : AppForSEII25264SqliteUT
    {
        private const int _order1 = 1;
        private const int _order2 = 2;
        private const int _missing = 99;

        private const int _driver1 = 10;
        private const int _driver2 = 11;
        public GetSchedulingDetails_test()
        {
            var customer = new ApplicationUser
            {
                Id = "cust",
                UserName = "cust@test.com",
                Name = "Name",
                Surname = "Surname",
                Address = "Long Address"
            };

            var payment = new PayPal { Email = "cust@test.com", User = customer };

            _context.Add(customer);
            _context.Add(payment);

            _context.AddRange(new List<PurchaseOrder>
            {
                new PurchaseOrder
                {
                    Id = _order1,
                    Street = "C/ A",
                    City = "Albacete",
                    PostalCode = "02001",
                    Date = DateTime.Today.AddDays(-1),
                    TotalPrice = 40m,
                    NameSurname = "Name A",
                    State = PurchaseState.Request,
                    Customer = customer,
                    PaymentMethod = payment
                },
                new PurchaseOrder
                {
                    Id = _order2,
                    Street = "C/ B",
                    City = "Albacete",
                    PostalCode = "02002",
                    Date = DateTime.Today,
                    TotalPrice = 80m,
                    NameSurname = "Name B",
                    State = PurchaseState.Request,
                    Customer = customer,
                    PaymentMethod = payment
                }
            });

            _context.AddRange(new List<DeliveryDriver>
            {
                new DeliveryDriver { Id = _driver1, Name = "Driver One", Available = true },
                new DeliveryDriver { Id = _driver2, Name = "Driver Two", Available = false }
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetSchedulingDetails_BadRequest_Empty()
        {
            var controller = new DeliveriesController(_context, new Mock<ILogger<DeliveriesController>>().Object);

            var result = await controller.GetSchedulingDetails(new List<int>());

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No purchase orders selected for scheduling.", bad.Value);
        }

        [Fact]
        public async Task GetSchedulingDetails_NotFound_MissingId()
        {
            var controller = new DeliveriesController(_context, new Mock<ILogger<DeliveriesController>>().Object);

            var ids = new List<int> { _order1, _missing };

            var result = await controller.GetSchedulingDetails(ids);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Contains(_missing.ToString(), notFound.Value!.ToString());
        }

        [Fact]
        public async Task GetSchedulingDetails_Success()
        {
            var controller = new DeliveriesController(_context, new Mock<ILogger<DeliveriesController>>().Object);

            var ids = new List<int> { _order1, _order2 };

            var result = await controller.GetSchedulingDetails(ids);

            var ok = Assert.IsType<OkObjectResult>(result);

            var dto = Assert.IsType<SchedulingDetailsDTO>(ok.Value);

            var expectedOrders = new List<OrderForSchedulingDTO>
            {
                new(_order1, "C/ A", "Albacete", "02001",
                    DateTime.Today.AddDays(-1), 40m, "Name A"),

                new(_order2, "C/ B", "Albacete", "02002",
                    DateTime.Today, 80m, "Name B")
            };

            Assert.Equal(expectedOrders, dto.SelectedOrders);

            var driverNames = dto.AvailableDrivers.Select(x => x.Name).OrderBy(x => x).ToList();
            Assert.Equal(new[] { "Driver One", "Driver Two" }, driverNames);
        }
    }
}
