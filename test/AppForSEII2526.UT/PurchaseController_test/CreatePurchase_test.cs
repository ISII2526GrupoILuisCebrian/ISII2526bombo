using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
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
    public class CreatePurchase_test : AppForSEII25264SqliteUT
    {
        public CreatePurchase_test()
        {
            // --- Crear marcas y productos ---
            var brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "AlmightyGNZ Protein", Location = "Albacete" },
                new Brand { Id = 2, Name = "AlmightyGNZ Creatine", Location = "Albacete" }
            };

            var products = new List<Product>
            {
                new Product(1, "Whey Protein Isolate", "High quality whey protein isolate.", 29.99m, "Brown", true, 100, brands[0]),
                new Product(2, "Creatine Monohydrate", "Pure creatine monohydrate powder.", 19.99m, "White", true, 50, brands[1])
            };

            // --- Crear cliente y método de pago concreto ---
            var customer = new ApplicationUser
            {
                Id = "1",
                UserName = "testuser",
                Name = "Juan",
                Surname = "Pérez",
                Address = "Calle Falsa 123",
                AccountCreationDate = DateTime.Now
            };

            var paymentMethod = new PaymentMethodMock
            {
                Id = 1,
                User = customer
            };

            _context.AddRange(brands);
            _context.AddRange(products);
            _context.Add(customer);
            _context.Add(paymentMethod);
            _context.SaveChanges();
        }

        private class PaymentMethodMock : PaymentMethod { }

        private PurchaseController CreateController(out Mock<ILogger<PurchaseController>> mockLogger)
        {
            mockLogger = new Mock<ILogger<PurchaseController>>();
            return new PurchaseController(_context, mockLogger.Object);
        }

        // -----------------------------
        // 1️⃣ Compra válida
        // -----------------------------
        [Fact]
        public async Task CreatePurchase_ValidPurchase_ReturnsCreated()
        {
            var controller = CreateController(out _);

            var dtoIn = new PurchaseForCreateDTO(
                street: "Calle Falsa 123",
                city: "Albacete",
                postalCode: "02001",
                nameCustomer: "Juan",
                surnameCustomer: "Pérez",
                paymentMethodId: 1,
                rating: 5,
                purchasedProducts: new List<PurchaseProductDTO>
                {
                    new PurchaseProductDTO(1,"Whey Protein Isolate","AlmightyGNZ Protein","Brown",2,29.99m),
                    new PurchaseProductDTO(2,"Creatine Monohydrate","AlmightyGNZ Creatine","White",1,19.99m)
                }
            );

            var result = await controller.CreatePurchase(dtoIn);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var dtoOut = Assert.IsType<PurchaseForDetailDTO>(created.Value);

            Assert.Equal("Juan", dtoOut.NameSurname);
            Assert.Equal(2, dtoOut.Products.Count);

            // Usando Equals() de PurchaseProductDTO
            var expectedProducts = dtoIn.PurchasedProducts;
            foreach (var p in expectedProducts)
            {
                Assert.Contains(p, dtoOut.Products);
            }

            Assert.Equal(29.99m * 2 + 19.99m * 1, dtoOut.TotalPrice);
        }

        // -----------------------------
        // 2️⃣ Sin productos
        // -----------------------------
        [Fact]
        public async Task CreatePurchase_WithoutProducts_ReturnsBadRequest()
        {
            var controller = CreateController(out _);

            var dtoIn = new PurchaseForCreateDTO(
                "Calle Falsa 123", "Albacete", "02001", "Juan", "Pérez", 1, 5, new List<PurchaseProductDTO>()
            );

            var result = await controller.CreatePurchase(dtoIn);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(bad.Value);
            Assert.Contains("Items", details.Errors.Keys);
        }

        // -----------------------------
        // 3️⃣ Cantidad inválida
        // -----------------------------
        [Fact]
        public async Task CreatePurchase_InvalidQuantity_ReturnsBadRequest()
        {
            var controller = CreateController(out _);

            var dtoIn = new PurchaseForCreateDTO(
                "Calle Falsa 123", "Albacete", "02001", "Juan", "Pérez", 1, 5,
                new List<PurchaseProductDTO>
                {
                    new PurchaseProductDTO(1,"Whey Protein Isolate","AlmightyGNZ Protein","Brown",0,29.99m)
                }
            );

            var result = await controller.CreatePurchase(dtoIn);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(bad.Value);
            Assert.Contains("Items", details.Errors.Keys);
        }

        // -----------------------------
        // 4️⃣ Producto inexistente
        // -----------------------------
        [Fact]
        public async Task CreatePurchase_ProductNotFound_ReturnsBadRequest()
        {
            var controller = CreateController(out _);

            var dtoIn = new PurchaseForCreateDTO(
                "Calle Falsa 123", "Albacete", "02001", "Juan", "Pérez", 1, 5,
                new List<PurchaseProductDTO>
                {
                    new PurchaseProductDTO(999,"Unknown","Unknown","Black",1,10m)
                }
            );

            var result = await controller.CreatePurchase(dtoIn);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(bad.Value);
            Assert.Contains("Items", details.Errors.Keys);
        }

        // -----------------------------
        // 5️⃣ Método de pago inexistente
        // -----------------------------
        [Fact]
        public async Task CreatePurchase_PaymentMethodNotFound_ReturnsBadRequest()
        {
            var controller = CreateController(out _);

            var dtoIn = new PurchaseForCreateDTO(
                "Calle Falsa 123", "Albacete", "02001", "Juan", "Pérez", 999, 5,
                new List<PurchaseProductDTO>
                {
                    new PurchaseProductDTO(1,"Whey Protein Isolate","AlmightyGNZ Protein","Brown",1,29.99m)
                }
            );

            var result = await controller.CreatePurchase(dtoIn);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(bad.Value);
            Assert.Contains("PaymentMethodId", details.Errors.Keys);
        }

        // -----------------------------
        // 6️⃣ Stock insuficiente
        // -----------------------------
        [Fact]
        public async Task CreatePurchase_InsufficientStock_ReturnsBadRequest()
        {
            var controller = CreateController(out _);

            var dtoIn = new PurchaseForCreateDTO(
                "Calle Falsa 123", "Albacete", "02001", "Juan", "Pérez", 1, 5,
                new List<PurchaseProductDTO>
                {
                    new PurchaseProductDTO(1,"Whey Protein Isolate","AlmightyGNZ Protein","Brown",500,29.99m)
                }
            );

            var result = await controller.CreatePurchase(dtoIn);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(bad.Value);
            Assert.Contains("Stock", details.Errors.Keys);
        }
    }
}
