using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DeliveryDTOs;

namespace AppForSEII2526.UT.DeliveriesController_test
{
    public class ScheduleDelivery_test : AppForSEII25264SqliteUT
    {
        // Reusable constants
        private const int _existingDriverId = 1;
        private const int _otherDriverId = 2;

        private const int _orderRequest1 = 1;
        private const int _orderRequest2 = 2;
        private const int _orderAlreadyAssigned = 3;

        public ScheduleDelivery_test()
        {
            //data seeding

            //customer seeding
            var customer = new ApplicationUser
            {
                Id = "user1",
                UserName = "user1@test.com",
                Address = "Long Address 12345",
                Name = "CustomerName",
                Surname = "CustomerSurname",
                AccountCreationDate = DateTime.Today
            };
            _context.Add(customer);

            // payment method seeding
            var payment = new PayPal
            {
                Email = "customer@test.com",
                User = customer
            };
            _context.Add(payment);

            // driver seeding
            var drivers = new List<DeliveryDriver>
            {
                new DeliveryDriver
                {
                    Id = _existingDriverId,
                    Name = "Antonio Driver",
                    Available = true
                },
                new DeliveryDriver
                {
                    Id = _otherDriverId,
                    Name = "Beatriz Driver",
                    Available = true
                }
            };
            _context.AddRange(drivers);

            // purchase order seeding
            var orders = new List<PurchaseOrder>
            {
                new PurchaseOrder
                {
                    Id = _orderRequest1,
                    City = "Albacete",
                    Street = "C/ Mayor 1",
                    PostalCode = "02001",
                    Date = DateTime.Today.AddDays(-1),
                    NameSurname = "Customer One",
                    Rating = 5,
                    TotalPrice = 50.00m,
                    State = PurchaseState.Request,
                    Customer = customer,
                    PaymentMethod = payment
                },
                new PurchaseOrder
                {
                    Id = _orderRequest2,
                    City = "Albacete",
                    Street = "C/ Menor 2",
                    PostalCode = "02002",
                    Date = DateTime.Today,
                    NameSurname = "Customer Two",
                    Rating = 4,
                    TotalPrice = 30.00m,
                    State = PurchaseState.Request,
                    Customer = customer,
                    PaymentMethod = payment
                },
                new PurchaseOrder
                {
                    Id = _orderAlreadyAssigned,
                    City = "Albacete",
                    Street = "C/ Ocupada 3",
                    PostalCode = "02003",
                    Date = DateTime.Today,
                    NameSurname = "Customer Three",
                    Rating = 3,
                    TotalPrice = 10.00m,
                    State = PurchaseState.Delivery,
                    Customer = customer,
                    PaymentMethod = payment
                }
            };

            _context.AddRange(orders);
            _context.SaveChanges();
        }

        //not found use case

        public static IEnumerable<object[]> TestCasesFor_ScheduleDelivery_NotFound()
        {
            // 1) driver does not exist
            var driverNotFound = new DeliveryAssignmentCreateDTO
            {
                DeliveryDriverId = 999,
                ExtraReward = 5.0m,
                PersonalMessage = "Handle carefully",
                Deadline = DateTime.Today.AddDays(2),
                OrdersToAssign = new List<OrderPriorityDTO>
                {
                    new OrderPriorityDTO
                    {
                        PurchaseOrderId = _orderRequest1,
                        Priority = PriorityType.High
                    }
                }
            };

            // 2) invalid order states
            var invalidOrders = new DeliveryAssignmentCreateDTO
            {
                DeliveryDriverId = _existingDriverId,
                ExtraReward = 0.0m,
                PersonalMessage = null,
                Deadline = DateTime.Today.AddDays(2),
                OrdersToAssign = new List<OrderPriorityDTO>
                {
                    new OrderPriorityDTO
                    {
                        PurchaseOrderId = _orderRequest1,
                        Priority = PriorityType.High
                    },
                    new OrderPriorityDTO
                    {
                        PurchaseOrderId = _orderAlreadyAssigned,
                        Priority = PriorityType.Low
                    }
                }
            };

            return new List<object[]>
            {
                new object[] { driverNotFound, "Selected delivery driver does not exist" },
                new object[] { invalidOrders, "Some orders were invalid" }
            };
        }

        //theory of not found
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_ScheduleDelivery_NotFound))]
        public async Task ScheduleDelivery_NotFoundErrors_test(
            DeliveryAssignmentCreateDTO dto,
            string expectedErrorStart)
        {
            // arrange
            var mock = new Mock<ILogger<DeliveriesController>>();
            var controller = new DeliveriesController(_context, mock.Object);

            // act
            var result = await controller.ScheduleDelivery(dto);

            // assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(notFound.Value);

            string actualError = details.Errors.First().Value[0];
            Assert.StartsWith(expectedErrorStart, actualError);
        }

        //model state invalid bad request test
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task ScheduleDelivery_BadRequest_ModelStateInvalid_test()
        {
            // arrange
            var mock = new Mock<ILogger<DeliveriesController>>();
            var controller = new DeliveriesController(_context, mock.Object);

            controller.ModelState.AddModelError(
                nameof(DeliveryAssignmentCreateDTO.OrdersToAssign),
                "At least one purchase order must be selected.");

            var dto = new DeliveryAssignmentCreateDTO
            {
                DeliveryDriverId = _existingDriverId,
                ExtraReward = 0.0m,
                PersonalMessage = "msg",
                Deadline = DateTime.Today.AddDays(1),
                OrdersToAssign = new List<OrderPriorityDTO>() // empty
            };

            // act
            var result = await controller.ScheduleDelivery(dto);

            // assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.Equal("At least one purchase order must be selected.",
                         details.Errors.First().Value[0]);
        }

        // success test fact
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task ScheduleDelivery_Success_test()
        {
            // arrange
            var mock = new Mock<ILogger<DeliveriesController>>();
            var controller = new DeliveriesController(_context, mock.Object);

            DateTime deadline = DateTime.Today.AddDays(2);

            var dto = new DeliveryAssignmentCreateDTO
            {
                DeliveryDriverId = _existingDriverId,
                ExtraReward = 10.00m,
                PersonalMessage = "Please deliver before lunch",
                Deadline = deadline,
                OrdersToAssign = new List<OrderPriorityDTO>
                {
                    new OrderPriorityDTO
                    {
                        PurchaseOrderId = _orderRequest1,
                        Priority = PriorityType.High
                    },
                    new OrderPriorityDTO
                    {
                        PurchaseOrderId = _orderRequest2,
                        Priority = PriorityType.Medium
                    }
                }
            };

            // act
            var result = await controller.ScheduleDelivery(dto);

            // assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(DeliveriesController.GetSchedulingDetails), created.ActionName);

            var detailDto = Assert.IsType<DeliveryAssignmentDetailDTO>(created.Value);

            // validate DTO contents
            Assert.Equal("Antonio Driver", detailDto.DriverName);
            Assert.Equal(deadline, detailDto.Deadline);
            Assert.Equal(dto.PersonalMessage, detailDto.PersonalMessage);
            Assert.Equal(dto.ExtraReward, detailDto.ExtraReward);

            Assert.Equal(2, detailDto.AssignedOrders.Count);

            var assignedIds = detailDto.AssignedOrders.Select(a => a.Id).OrderBy(i => i);
            Assert.Equal(new[] { _orderRequest1, _orderRequest2 }, assignedIds);

            // check priorities
            Assert.Equal(PriorityType.High,
                detailDto.AssignedOrders.Single(o => o.Id == _orderRequest1).Priority);

            Assert.Equal(PriorityType.Medium,
                detailDto.AssignedOrders.Single(o => o.Id == _orderRequest2).Priority);
        }
    }
}
