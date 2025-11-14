using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DeliveryDriverDTOs;

namespace AppForSEII2526.UT.DeliveriesController_test
{
    public class GetAvailableOrders_test : AppForSEII25264SqliteUT
    {
        // reusable values
        private const int _requestOrder1 = 1;
        private const int _requestOrder2 = 2;
        private const int _deliveryOrder = 3;

        public GetAvailableOrders_test()
        {
            //filling the database with test data

            var customer = new ApplicationUser
            {
                Id = "cust1",
                UserName = "customer@test.com",
                Address = "Long Address 12345",
                Name = "CustomerName",
                Surname = "CustomerSurname",
                AccountCreationDate = DateTime.Today
            };
            _context.Add(customer);

            var payment = new PayPal
            {
                Email = "customer@test.com",
                User = customer
            };
            _context.Add(payment);

            // request-state orders
            var orders = new List<PurchaseOrder>
            {
                new PurchaseOrder
                {
                    Id = _requestOrder1,
                    City = "Albacete",
                    Street = "Street A",
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
                    Id = _requestOrder2,
                    City = "Albacete",
                    Street = "Street B",
                    PostalCode = "02002",
                    Date = DateTime.Today,
                    NameSurname = "Name B",
                    Rating = 4,
                    TotalPrice = 70.00m,
                    State = PurchaseState.Request,
                    Customer = customer,
                    PaymentMethod = payment
                },

                // delivery-state order (should not be retrieved)
                new PurchaseOrder
                {
                    Id = _deliveryOrder,
                    City = "Albacete",
                    Street = "Street C",
                    PostalCode = "02003",
                    Date = DateTime.Today,
                    NameSurname = "Name C",
                    Rating = 3,
                    TotalPrice = 5.00m,
                    State = PurchaseState.Delivery,
                    Customer = customer,
                    PaymentMethod = payment
                }
            };

            _context.AddRange(orders);
            _context.SaveChanges();
        }


        //bad request test case
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetAvailableOrders_BadRequest_WhenMinTotalPriceNegative_test()
        {
            // arrange
            var mock = new Mock<ILogger<DeliveriesController>>();
            var controller = new DeliveriesController(_context, mock.Object);

            // act
            var result = await controller.GetAvailableOrders(null, -1);

            // assert
            var badReq = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(badReq.Value);

            Assert.StartsWith("Minimum total price cannot be negative",
                              details.Errors.First().Value[0]);
        }


        //success test cases with different filters
        public static IEnumerable<object[]> TestCasesFor_GetAvailableOrders_OK()
        {
            // DTOs for expected lists (same DTO shape as controller)
            var order1 = new OrderForSchedulingDTO(
                _requestOrder1, "Street A", "Albacete", "02001",
                DateTime.Today.AddDays(-1), 40.00m, "Name A");

            var order2 = new OrderForSchedulingDTO(
                _requestOrder2, "Street B", "Albacete", "02002",
                DateTime.Today, 70.00m, "Name B");

            var bothOrders = new List<OrderForSchedulingDTO> { order1, order2 };
            var only1 = new List<OrderForSchedulingDTO> { order1 };
            var only2 = new List<OrderForSchedulingDTO> { order2 };

            return new List<object[]>
            {
                // no filters
                new object[] { null, null, bothOrders },

                // postalCode filter matches only order1
                new object[] { "02001", null, only1 },

                // minTotalPrice filter picks only order2
                new object[] { null, 60.00m, only2 },

                // combined: postalCode=02002 AND price>=70 to only order2
                new object[] { "02002", 70.00m, only2 },
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetAvailableOrders_OK))]
        public async Task GetAvailableOrders_OK_FilterTests(
            string? postalCode,
            decimal? minTotalPrice,
            List<OrderForSchedulingDTO> expectedOrders)
        {
            // arrange
            var mock = new Mock<ILogger<DeliveriesController>>();
            var controller = new DeliveriesController(_context, mock.Object);

            // act
            var result = await controller.GetAvailableOrders(postalCode, minTotalPrice);

            // assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<OrderForSchedulingDTO>>(ok.Value);

            Assert.Equal(expectedOrders.Select(x => x.Date.Date),
             actual.Select(x => x.Date.Date));

        }


        //not found test case
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetAvailableOrders_NotFound_WhenNoOrdersMatch_test()
        {
            // arrange
            var mock = new Mock<ILogger<DeliveriesController>>();
            var controller = new DeliveriesController(_context, mock.Object);

            // act
            // strict filter to ensure no matches
            var result = await controller.GetAvailableOrders("NOPOSTAL", 9999);

            // assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var message = Assert.IsType<string>(notFound.Value);

            Assert.Equal("No purchase orders available for scheduling.", message);
        }
    }
}
