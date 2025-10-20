using AppForSEII2526.API.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> GetProductsForPurchasing(string? productName)
        {
            IList<ProductForPurchasingDTO> productsDTOS = await _context.Products
                .Include(product=>product.Brand)
                .Where(product=>product.Name.Contains(productName)
                    || (productName == null))
                .OrderBy(product=>product.Name)
                .Select(product => new ProductForPurchasingDTO(product.Id, product.Name, product.Brand.Name))
                .ToListAsync();
            return Ok(productsDTOS);
        }
    }
}
