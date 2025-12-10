using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DeliveryDriverDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII2526.UT.DeliveriesController_test
{
    public class GetAvailableOrders_test : AppForSEII25264SqliteUT
    {
        private const int _requestOrder1 = 1;
        private const int _requestOrder2 = 2;
        private const int _deliveryOrder = 3;

        public GetAvailableOrders_test()
        {
            // Seed customer
            var customer = new ApplicationUser
            {
                Id = "cust1",
                UserName = "customer@test.com",
                Address = "Long Address 12345",
                Name = "CustomerName",
                Surname = "CustomerSurname",
                AccountCreationDate = DateTime.Today
            };

            // Seed payment
            var payment = new PayPal
            {
                Email = "customer@test.com",
                User = customer
            };

            _context.Add(customer);
            _context.Add(payment);

            // Seed purchase orders
            _context.AddRange(new List<PurchaseOrder>
            {
                new PurchaseOrder
                {
                    Id = _requestOrder1,
                    City = "Albacete",
                    Street = "Street A",
                    PostalCode = "02001",
                    Date = DateTime.Today.AddDays(-1),
                    NameSurname = "Name A",
                    TotalPrice = 40.00m,
                    State = PurchaseState.Request,
                    Customer = customer,
                    PaymentMethod = payment
                },
                new PurchaseOrder
                {
                    Id = _requestOrder2,
                    City = "Albacete",
                    Street = "Street B",
                    PostalCode = "02002",
                    Date = DateTime.Today,
                    NameSurname = "Name B",
                    TotalPrice = 70.00m,
                    State = PurchaseState.Request,
                    Customer = customer,
                    PaymentMethod = payment
                },
                new PurchaseOrder
                {
                    Id = _deliveryOrder,
                    City = "Albacete",
                    Street = "Street C",
                    PostalCode = "02003",
                    Date = DateTime.Today,
                    NameSurname = "Name C",
                    TotalPrice = 5.00m,
                    State = PurchaseState.Delivery,
                    Customer = customer,
                    PaymentMethod = payment
                }
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAvailableOrders_BadRequest_WhenMinTotalPriceNegative_test()
        {
            var controller = new DeliveriesController(_context, new Mock<ILogger<DeliveriesController>>().Object);

            var result = await controller.GetAvailableOrders(null, -1);

            var badReq = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(badReq.Value);

            Assert.StartsWith("Minimum total price cannot be negative", details.Errors.First().Value[0]);
        }

        public static IEnumerable<object[]> TestCases()
        {
            return new List<object[]>
            {
                new object[]
                {
                    null,
                    null,
                    new List<OrderForSchedulingDTO>
                    {
                        new(_requestOrder1, "Street A", "Albacete", "02001",
                            DateTime.Today.AddDays(-1), 40.00m, "Name A"),

                        new(_requestOrder2, "Street B", "Albacete", "02002",
                            DateTime.Today, 70.00m, "Name B")
                    }
                },
                new object[]
                {
                    "02001",
                    null,
                    new List<OrderForSchedulingDTO>
                    {
                        new(_requestOrder1, "Street A", "Albacete", "02001",
                            DateTime.Today.AddDays(-1), 40.00m, "Name A")
                    }
                },
                new object[]
                {
                    null,
                    60.00m,
                    new List<OrderForSchedulingDTO>
                    {
                        new(_requestOrder2, "Street B", "Albacete", "02002",
                            DateTime.Today, 70.00m, "Name B")
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task GetAvailableOrders_OK_test(string? postal, decimal? minPrice, List<OrderForSchedulingDTO> expected)
        {
            var controller = new DeliveriesController(_context, new Mock<ILogger<DeliveriesController>>().Object);

            var result = await controller.GetAvailableOrders(postal, minPrice);

            var ok = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<OrderForSchedulingDTO>>(ok.Value);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAvailableOrders_NotFound_test()
        {
            var controller = new DeliveriesController(_context, new Mock<ILogger<DeliveriesController>>().Object);

            var result = await controller.GetAvailableOrders("NOMATCH", 9999);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No purchase orders available for scheduling.", notFound.Value);
        }
    }
}
