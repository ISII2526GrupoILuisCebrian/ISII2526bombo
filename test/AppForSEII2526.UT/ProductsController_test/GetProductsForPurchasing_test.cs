using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.ProductsController_test
{
    public class GetProductsForPurchasing_test : AppForSEII25264SqliteUT
    {
        public GetProductsForPurchasing_test()
        {
            // Crear marcas
            var brands = new List<Brand>
            {
                new Brand(){ Id=1, Name="AlmightyGNZ Protein", Location="Albacete"},
                new Brand(){ Id=2, Name="AlmightyGNZ Creatine", Location="Albacete"}
            };

            // Crear productos
            var products = new List<Product>
            {
                new Product(1, "Whey Protein Isolate ", "High quality whey protein isolate.", 29.99m, "Brown", true, 100, brands[0]),
                new Product(2, "Creatine Monohydrate", "Pure creatine monohydrate powder.", 19.99m, "White", true, 50, brands[1]),
                new Product(3, "Mass Gainer", "High-calorie mass gainer shake.", 39.99m, "Chocolate", true, 0, brands[0])
            };

            _context.AddRange(brands);
            _context.AddRange(products);
            _context.SaveChanges();
        }

        private ProductsController CreateControllerWithLogger(out Mock<ILogger<ProductsController>> mockLogger)
        {
            mockLogger = new Mock<ILogger<ProductsController>>();
            return new ProductsController(_context, mockLogger.Object);
        }

        [Fact]
        public async Task GetProductsForPurchasing_NoFilters_ReturnsAllProducts()
        {
            // Arrange
            var expectedProducts = new List<ProductForPurchasingDTO>
    {
        new ProductForPurchasingDTO(1, "Whey Protein Isolate", "AlmightyGNZ Protein", "High quality whey protein isolate.", 29.99m, 100),
        new ProductForPurchasingDTO(2, "Creatine Monohydrate", "AlmightyGNZ Creatine", "Pure creatine monohydrate powder.", 19.99m, 50)
    };

            var mockLogger = new Mock<ILogger<ProductsController>>();
            var controller = new ProductsController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetProductsForPurchasing(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualProducts = Assert.IsType<List<ProductForPurchasingDTO>>(okResult.Value);

            // Ordenar listas para evitar problemas de orden
            var expected = expectedProducts.OrderBy(p => p.Id).ToList();
            var actual = actualProducts.OrderBy(p => p.Id).ToList();

            Assert.Equal(expected.Count, actual.Count);

            // Comparación propiedad por propiedad
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
                Assert.Equal(expected[i].Name.Trim(), actual[i].Name.Trim());
                Assert.Equal(expected[i].Brand, actual[i].Brand);
                Assert.Equal(expected[i].Description, actual[i].Description);
                Assert.Equal(expected[i].PurchasePrice, actual[i].PurchasePrice);
                Assert.Equal(expected[i].Quantity, actual[i].Quantity);
            }
        }


        [Fact]
        public async Task GetProductsForPurchasing_FilterByName_ReturnsMatchingProducts()
        {
            // Arrange
            var expectedProducts = new List<ProductForPurchasingDTO>
            {
                new ProductForPurchasingDTO(1, "Whey Protein Isolate", "AlmightyGNZ Protein", "High quality whey protein isolate.", 29.99m, 100)
            };

            var controller = CreateControllerWithLogger(out _);

            // Act
            var result = await controller.GetProductsForPurchasing("Whey", null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualProducts = Assert.IsType<List<ProductForPurchasingDTO>>(okResult.Value);

            var expected = expectedProducts.OrderBy(p => p.Id).ToList();
            var actual = actualProducts.OrderBy(p => p.Id).ToList();

            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
                Assert.Equal(expected[i].Name.Trim(), actual[i].Name.Trim());
                Assert.Equal(expected[i].Brand, actual[i].Brand);
                Assert.Equal(expected[i].Description, actual[i].Description);
                Assert.Equal(expected[i].PurchasePrice, actual[i].PurchasePrice);
                Assert.Equal(expected[i].Quantity, actual[i].Quantity);
            }
        }

        [Fact]
        public async Task GetProductsForPurchasing_FilterByColour_ReturnsMatchingProducts()
        {
            // Arrange
            var expectedProducts = new List<ProductForPurchasingDTO>
            {
                new ProductForPurchasingDTO(2, "Creatine Monohydrate", "AlmightyGNZ Creatine", "Pure creatine monohydrate powder.", 19.99m, 50)
            };

            var controller = CreateControllerWithLogger(out _);

            // Act
            var result = await controller.GetProductsForPurchasing(null, "White");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualProducts = Assert.IsType<List<ProductForPurchasingDTO>>(okResult.Value);

            var expected = expectedProducts.OrderBy(p => p.Id).ToList();
            var actual = actualProducts.OrderBy(p => p.Id).ToList();

            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
                Assert.Equal(expected[i].Name.Trim(), actual[i].Name.Trim());
                Assert.Equal(expected[i].Brand, actual[i].Brand);
                Assert.Equal(expected[i].Description, actual[i].Description);
                Assert.Equal(expected[i].PurchasePrice, actual[i].PurchasePrice);
                Assert.Equal(expected[i].Quantity, actual[i].Quantity);
            }
        }

        [Fact]
        public async Task GetProductsForPurchasing_FilterByNameAndColour_ReturnsMatchingProducts()
        {
            // Arrange
            var expectedProducts = new List<ProductForPurchasingDTO>
            {
                new ProductForPurchasingDTO(2, "Creatine Monohydrate", "AlmightyGNZ Creatine", "Pure creatine monohydrate powder.", 19.99m, 50)
            };

            var controller = CreateControllerWithLogger(out _);

            // Act
            var result = await controller.GetProductsForPurchasing("Creatine", "White");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualProducts = Assert.IsType<List<ProductForPurchasingDTO>>(okResult.Value);

            var expected = expectedProducts.OrderBy(p => p.Id).ToList();
            var actual = actualProducts.OrderBy(p => p.Id).ToList();

            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
                Assert.Equal(expected[i].Name.Trim(), actual[i].Name.Trim());
                Assert.Equal(expected[i].Brand, actual[i].Brand);
                Assert.Equal(expected[i].Description, actual[i].Description);
                Assert.Equal(expected[i].PurchasePrice, actual[i].PurchasePrice);
                Assert.Equal(expected[i].Quantity, actual[i].Quantity);
            }
        }

        [Fact]
        public async Task GetProductsForPurchasing_NoMatchingProducts_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateControllerWithLogger(out _);

            // Act
            var result = await controller.GetProductsForPurchasing("NonExistentProduct", "Purple");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("There are no products available to purchase.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetProductsForPurchasing_StockZeroProductsAreExcluded()
        {
            // Arrange
            var controller = CreateControllerWithLogger(out _);

            // Act
            var result = await controller.GetProductsForPurchasing(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualProducts = Assert.IsType<List<ProductForPurchasingDTO>>(okResult.Value);

            // Verificar que ningún producto tiene stock 0
            Assert.DoesNotContain(actualProducts, p => p.Quantity == 0);
        }
    }
}
