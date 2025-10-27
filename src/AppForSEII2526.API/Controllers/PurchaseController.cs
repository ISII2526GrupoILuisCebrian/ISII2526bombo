using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PurchaseController> _logger;

        public PurchaseController(ApplicationDbContext context, ILogger<PurchaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseForDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchase(int id)
        {

            //perhaps it does not exist a Purchase with the id provided
            PurchaseForDetailDTO? purchase = await _context.PurchaseOrders
                .Where(p => p.Id == id)

                .Include(p => p.PurchaseProducts) //join table PurchaseProducts
                    .ThenInclude(pp => pp.Product) //then join table Products
                        .ThenInclude(product => product.Brand) //then join table Brand

                .Include(p => p.Customer) //join table ApplicationUser

                .Select(p => new PurchaseForDetailDTO(
                    p.Id,
                    p.Date,
                    p.Street,
                    p.PostalCode,
                    p.State,
                    p.TotalPrice,
                    p.NameSurname,
                    p.PaymentMethod,
                    p.PurchaseProducts

                        .Select(pp => new PurchaseProductDTO(
                            pp.Product.Id,
                            pp.Product.Name,
                            pp.Product.Brand.Name,
                            pp.Quantity,
                            pp.Product.Price,
                            pp.Product.Description
                        )).ToList())
                )

                
             //it obtains just the first Purchase that satisfies the where clause
             .FirstOrDefaultAsync();



            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }


            return Ok(purchase);
        }
    }
}
