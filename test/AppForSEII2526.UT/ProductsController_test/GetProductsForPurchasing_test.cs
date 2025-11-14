using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ProductsController_test
{
    public class GetProductsForPurchasing_test : AppForSEII25264SqliteUT
    {
        public GetProductsForPurchasing_test()
        {
            var brands = new List<Brand>()
            {
                new Brand(){ Id=1, Name="AlmightyGNZ Protein", Location="Albacete"},
                new Brand(){ Id=2, Name="AlmightyGNZ Creatine", Location= "Albacete"},
            };

            var products = new List<Product>()
            {
                new Product(1, "Whey Protein Isolate", "High quality whey protein isolate.", 29.99m, "Brown", true, 100, brands[0]),
                new Product(2, "Creatine Monohydrate", "Pure creatine monohydrate powder.", 19.99m, "White", true, 50, brands[1]),
                //This product has quantity = 0 so it should,'t appear in the results
                new Product(3, "Mass Gainer", "High-calorie mass gainer shake.", 39.99m, "Chocolate", true, 0, brands[0]),
            };

            _context.AddRange(brands);
            _context.AddRange(products);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetProductsForPurchasing_ReturnsAllProducts_WhenNoFiltersProvided()
        {
            // Arrange
            List<ProductForPurchasingDTO> expectedProducts = new List<ProductForPurchasingDTO>()
    {
        new ProductForPurchasingDTO(1, "Whey Protein Isolate", "AlmightyGNZ Protein", "High quality whey protein isolate.", 29.99m, 100),
        new ProductForPurchasingDTO(2, "Creatine Monohydrate", "AlmightyGNZ Creatine", "Pure creatine monohydrate powder.", 19.99m, 50),
    };

            var mock = new Mock<ILogger<ProductsController>>();
            ILogger<ProductsController> logger = mock.Object;
            ProductsController controller = new ProductsController(_context, logger);

            // Act
            var result = await controller.GetProductsForPurchasing(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualProducts = Assert.IsType<List<ProductForPurchasingDTO>>(okResult.Value);

            // Ordenar por Id para evitar problemas de orden
            var expected = expectedProducts.OrderBy(p => p.Id).ToList();
            var actual = actualProducts.OrderBy(p => p.Id).ToList();

            // Comprobar cantidad de elementos
            Assert.Equal(expected.Count, actual.Count);

            // Comparar propiedad por propiedad
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
                Assert.Equal(expected[i].Name.Trim(), actual[i].Name.Trim()); // elimina espacios extra
                Assert.Equal(expected[i].Brand, actual[i].Brand);
                Assert.Equal(expected[i].Description, actual[i].Description);
                Assert.Equal(expected[i].PurchasePrice, actual[i].PurchasePrice);
                Assert.Equal(expected[i].Quantity, actual[i].Quantity);
            }
        }


    }
}
