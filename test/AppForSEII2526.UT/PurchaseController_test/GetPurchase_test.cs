using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.PurchaseController_test
{
    public class GetPurchase_test : AppForSEII25264SqliteUT
    {
        public GetPurchase_test()
        {
            // --- Crear marcas ---
            var brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "AlmightyGNZ Protein", Location = "Albacete" },
                new Brand { Id = 2, Name = "AlmightyGNZ Creatine", Location = "Albacete" }
            };

            // --- Crear productos ---
            var products = new List<Product>
            {
                new Product(1, "Whey Protein Isolate", "High quality whey protein isolate.", 29.99m, "Brown", true, 100, brands[0]),
                new Product(2, "Creatine Monohydrate", "Pure creatine monohydrate powder.", 19.99m, "White", true, 50, brands[1])
            };

            // --- Crear cliente ---
            var customer = new ApplicationUser
            {
                Id = "1",
                UserName = "testuser",
                Name = "Juan",
                Surname = "Pérez",
                Address = "Calle Falsa 123",
                AccountCreationDate = DateTime.Now
            };

            // --- Crear método de pago concreto (ya que PaymentMethod es abstracto) ---
            var paymentMethod = new PaymentMethodMock
            {
                Id = 1
            };

            // --- Crear compras ---
            var purchase1 = new PurchaseOrder
            {
                Id = 1,
                Customer = customer,
                PaymentMethod = paymentMethod,
                TotalPrice = 49.98m,
                Date = DateTime.Now,
                Street = "Calle Falsa 123",
                City = "Albacete",
                PostalCode = "02001",
                NameSurname = "Juan Pérez",
                State = PurchaseState.Done,
                PurchaseProducts = new List<PurchaseProduct>
                {
                    new PurchaseProduct { Product = products[0], ProductId = products[0].Id, Quantity = 1, Price = products[0].Price },
                    new PurchaseProduct { Product = products[1], ProductId = products[1].Id, Quantity = 1, Price = products[1].Price }
                }
            };

            var purchase2 = new PurchaseOrder
            {
                Id = 2,
                Customer = customer,
                PaymentMethod = paymentMethod,
                TotalPrice = 29.99m,
                Date = DateTime.Now,
                Street = "Calle Segunda 45",
                City = "Albacete",
                PostalCode = "02002",
                NameSurname = "Juan Pérez",
                State = PurchaseState.Processing,
                PurchaseProducts = new List<PurchaseProduct>
                {
                    new PurchaseProduct { Product = products[0], ProductId = products[0].Id, Quantity = 1, Price = products[0].Price }
                }
            };

            _context.AddRange(brands);
            _context.AddRange(products);
            _context.Add(customer);
            _context.Add(paymentMethod);
            _context.AddRange(new[] { purchase1, purchase2 });
            _context.SaveChanges();
        }

        // Clase concreta para instanciar PaymentMethod en tests
        private class PaymentMethodMock : PaymentMethod { }

        private PurchaseController CreateController(out Mock<ILogger<PurchaseController>> mockLogger)
        {
            mockLogger = new Mock<ILogger<PurchaseController>>();
            return new PurchaseController(_context, mockLogger.Object);
        }

        [Fact]
        public async Task GetPurchase_ExistingId_ReturnsCorrectPurchase()
        {
            var controller = CreateController(out _);

            var result = await controller.GetPurchase(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var purchase = Assert.IsType<PurchaseForDetailDTO>(okResult.Value);

            Assert.Equal(1, purchase.Id);
            Assert.Equal("Juan Pérez", purchase.NameSurname);
            Assert.Equal("testuser", purchase.CustomerUserName);
            Assert.Equal("PaymentMethodMock", purchase.PaymentMethod); // tu controller devuelve tipo del PaymentMethod
            Assert.Equal(2, purchase.Products.Count);

            var productNames = purchase.Products.Select(pp => pp.ProductName.Trim()).ToList();
            Assert.Contains("Whey Protein Isolate", productNames);
            Assert.Contains("Creatine Monohydrate", productNames);

            foreach (var pp in purchase.Products)
            {
                Assert.True(pp.Quantity > 0);
                Assert.True(pp.UnitPrice > 0);
                Assert.False(string.IsNullOrWhiteSpace(pp.BrandName));
            }
        }

        [Fact]
        public async Task GetPurchase_NonExistingId_ReturnsNotFound()
        {
            var controller = CreateController(out _);

            var result = await controller.GetPurchase(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetPurchase_MultiplePurchases_ReturnsCorrectDetails()
        {
            var controller = CreateController(out _);

            var result1 = await controller.GetPurchase(1);
            var result2 = await controller.GetPurchase(2);

            var p1 = Assert.IsType<PurchaseForDetailDTO>(((OkObjectResult)result1).Value);
            var p2 = Assert.IsType<PurchaseForDetailDTO>(((OkObjectResult)result2).Value);

            Assert.Equal(2, p1.Products.Count);
            Assert.Single(p2.Products);
            Assert.Equal("Juan Pérez", p1.NameSurname);
            Assert.Equal("Juan Pérez", p2.NameSurname);
        }

        [Fact]
        public async Task GetPurchase_VerifyProductDTOProperties()
        {
            var controller = CreateController(out _);

            var result = await controller.GetPurchase(1);
            var purchase = Assert.IsType<PurchaseForDetailDTO>(((OkObjectResult)result).Value);

            foreach (var pp in purchase.Products)
            {
                Assert.False(string.IsNullOrWhiteSpace(pp.ProductName));
                Assert.False(string.IsNullOrWhiteSpace(pp.BrandName));
                Assert.False(string.IsNullOrWhiteSpace(pp.Colour));
                Assert.True(pp.Quantity > 0);
                Assert.True(pp.UnitPrice > 0);
            }
        }
    }
}
