using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DeliveryDriverDTOs;
using AppForSEII2526.API.DTOs.DeliveryDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII2526.UT.DeliveriesController_test
{
    public class GetSchedulingDetails_test : AppForSEII25264SqliteUT
    {
        private const int _order1 = 1;
        private const int _order2 = 2;
        private const int _orderMissing = 99;

        private const int _driver1 = 10;
        private const int _driver2 = 11;

        public GetSchedulingDetails_test()
        {

            //seeding data for customer
            var customer = new ApplicationUser
            {
                Id = "cust1",
                UserName = "cust@test.com",
                Address = "Some Long Address 123",
                Name = "CustomerName",
                Surname = "CustomerSurname",
                AccountCreationDate = DateTime.Today
            };
            _context.Add(customer);

            //payment method data seeding
            var payment = new PayPal
            {
                Email = "cust@test.com",
                User = customer
            };
            _context.Add(payment);

            //purchase orders data seeding
            var orders = new List<PurchaseOrder>
            {
                new PurchaseOrder
                {
                    Id = _order1,
                    City = "Albacete",
                    Street = "C/ A",
                    PostalCode = "02001",
                    Date = DateTime.Today.AddDays(-1),
                    NameSurname = "Name A",
                    Rating = 5,
                    TotalPrice = 40.00m,
                    State = PurchaseState.Request,
                    Customer = customer,
                    PaymentMethod = payment
                },
                new PurchaseOrder
                {
                    Id = _order2,
                    City = "Albacete",
                    Street = "C/ B",
                    PostalCode = "02002",
                    Date = DateTime.Today,
                    NameSurname = "Name B",
                    Rating = 4,
                    TotalPrice = 80.00m,
                    State = PurchaseState.Request,
                    Customer = customer,
                    PaymentMethod = payment
                }
            };

            _context.AddRange(orders);

            //creation of delivery drivers
            var drivers = new List<DeliveryDriver>
            {
                new DeliveryDriver { Id = _driver1, Name = "Driver One", Available = true },
                new DeliveryDriver { Id = _driver2, Name = "Driver Two", Available = false }
            };

            _context.AddRange(drivers);
            _context.SaveChanges();
        }

        //creation of a bad request test case
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetSchedulingDetails_BadRequest_EmptyList_test()
        {
            // arrange
            var mock = new Mock<ILogger<DeliveriesController>>();
            var controller = new DeliveriesController(_context, mock.Object);

            List<int> emptyList = new List<int>();  // simulate empty

            // act
            var result = await controller.GetSchedulingDetails(emptyList);

            // assert
            var badReq = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No purchase orders selected for scheduling.", badReq.Value);
        }


        //not found test case
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetSchedulingDetails_NotFound_Missing_test()
        {
            // arrange
            var mock = new Mock<ILogger<DeliveriesController>>();
            var controller = new DeliveriesController(_context, mock.Object);

            var ids = new List<int> { _order1, _orderMissing }; // _orderMissing not in DB

            // act
            var result = await controller.GetSchedulingDetails(ids);

            // assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("The following order IDs were not found", notFound.Value!.ToString());
            Assert.Contains(_orderMissing.ToString(), notFound.Value!.ToString());
        }

        //success test case
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetSchedulingDetails_Success_test()
        {
            // arrange
            var mock = new Mock<ILogger<DeliveriesController>>();
            var controller = new DeliveriesController(_context, mock.Object);

            var ids = new List<int> { _order1, _order2 };

            var expectedOrders = new List<OrderForSchedulingDTO>
            {
                new OrderForSchedulingDTO(_order1, "C/ A", "Albacete", "02001",
                    DateTime.Today.AddDays(-1), 40.00m, "Name A"),

                new OrderForSchedulingDTO(_order2, "C/ B", "Albacete", "02002",
                    DateTime.Today, 80.00m, "Name B")
            };

            // act
            var result = await controller.GetSchedulingDetails(ids);

            // assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<SchedulingDetailsDTO>(ok.Value);

            var actualOrders = dto.SelectedOrders;

            // normalize dates to avoid timezone issues
            foreach (var a in actualOrders)
                a.Date = a.Date.Date;

            foreach (var e in expectedOrders)
                e.Date = e.Date.Date;

            // ONLY for comparing calendar date
            Assert.Equal(expectedOrders.Select(x => x.Date.Date),
                         actualOrders.Select(x => x.Date.Date));


            // check returned drivers
            Assert.Equal(2, dto.AvailableDrivers.Count);

            var names = dto.AvailableDrivers.Select(d => d.Name).OrderBy(x => x).ToList();
            Assert.Equal(new[] { "Driver One", "Driver Two" }, names);
        }
    }
}
