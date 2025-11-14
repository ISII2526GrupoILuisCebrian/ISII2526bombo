using AppForSEII2526.API.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        //{

        //    if (op2 == 0)
        //    {
        //        string error = "Op2 cannot be 0 to compute a division";
        //        _logger.LogError(DateTime.Now + " Error:" + error);
        //        return BadRequest(error);
        //    }
        //    decimal result = op1 / op2;
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ProductForPurchasingDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelError), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetProductsForPurchasing(string? productName, string? colour)
        {
            if (string.IsNullOrWhiteSpace(productName) && string.IsNullOrWhiteSpace(colour))
            {
                _logger.LogWarning($"{DateTime.Now} Warning: No filters were provided for product search.");
            }

            var productsQuery = _context.Products
                .Include(p => p.Brand) // incluir marca para poder sacar su nombre
                .Include(p => p.PurchaseProducts) // incluir compras
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(productName))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(productName));
            }

            if (!string.IsNullOrWhiteSpace(colour))
            {
                productsQuery = productsQuery.Where(p => p.Colour.Contains(colour));
            }

            IList<ProductForPurchasingDTO> productsDTOS = await _context.Products
                .Include(product => product.Brand)
                .Include(product => product.PurchaseProducts)
                .Where(product =>
                 (productName == null || product.Name.Contains(productName)) &&
                 (colour == null || product.Colour.Contains(colour)) &&
                 product.Stock > 0)
                .OrderBy(product => product.Name)
                 .Select(product => new ProductForPurchasingDTO(
                    product.Id,
                    product.Name,
                    product.Brand.Name,
                    product.Description,
                    product.Price,
                    product.Stock
                  ))
                .ToListAsync();
            if (!productsDTOS.Any())
            {
                return BadRequest("There are no products available to purchase.");
            }

            return Ok(productsDTOS);

        }
    }
}
