using AppForSEII2526.API.DTOs.PurchaseDTOs;
<<<<<<< HEAD
using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
=======
>>>>>>> origin/development
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
=======
>>>>>>> origin/development

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
<<<<<<< HEAD
    public class PurchasesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PurchasesController> _logger;

        public PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger)
=======
    public class PurchaseController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PurchaseController> _logger;

        public PurchaseController(ApplicationDbContext context, ILogger<PurchaseController> logger)
>>>>>>> origin/development
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
<<<<<<< HEAD
            var purchase = await _context.PurchaseOrders
                .Where(po => po.Id == id)

                .Include(po => po.PurchaseProducts)
                    .ThenInclude(pp => pp.Product)
                        .ThenInclude(p => p.Brand)

                .Include(po => po.Customer)
                .Include(po => po.PaymentMethod)

                .Select(po => new PurchaseForDetailDTO(
                    po.Id,
                    po.TotalPrice,
                    po.Date,
                    po.Street,
                    po.City,
                    po.PostalCode,
                    po.NameSurname,
                    po.State.ToString(),
                    po.PaymentMethod.GetType().Name,
                    po.Customer.UserName!,
                    po.PurchaseProducts
                        .Select(pp => new PurchaseProductDTO(
                            pp.ProductId,
                            pp.Product.Name,
                            pp.Product.Brand.Name,
                            pp.Product.Colour,
                            pp.Quantity,
                            pp.Price
                            
                        )).ToList()
                ))
                .FirstOrDefaultAsync();
=======

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
                    p.Customer.Name,
                    p.Customer.Surname,
                    p.PaymentMethod,
                    p.Customer.UserName,
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


>>>>>>> origin/development

            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }

<<<<<<< HEAD
            return Ok(purchase);
        }

       

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseForDetailDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreatePurchase([FromBody] PurchaseForCreateDTO purchaseForCreate)
        {
            //Validaciones
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            if (purchaseForCreate.PurchasedProducts == null || purchaseForCreate.PurchasedProducts.Count == 0)
            {
                ModelState.AddModelError("Items", "Error! You must include at least one product to be purchased");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Validar cantidades y existencia de productos
            var invalidQty = purchaseForCreate.PurchasedProducts
                .Where(i => i.Quantity < 1)
                .Select(i => i.ProductId)
                .ToList();
            if (invalidQty.Any())
            {
                ModelState.AddModelError("Items", $"Error! Quantity must be >= 1 for products: {string.Join(", ", invalidQty)}");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var productIds = purchaseForCreate.PurchasedProducts.Select(i => i.ProductId).Distinct().ToList();
            var products = await _context.Products
                .Include(p => p.Brand)
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            var missing = productIds.Except(products.Select(p => p.Id)).ToList();
            if (missing.Any())
            {
                ModelState.AddModelError("Items", $"Error! Products not found: {string.Join(", ", missing)}");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            var requestedByProd = purchaseForCreate.PurchasedProducts
                .GroupBy(i => i.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));
            foreach (var p in products)
            {
                if (p.Stock < requestedByProd[p.Id])
                {
                    ModelState.AddModelError("Stock", $"Error! Not enough stock for product '{p.Name}'");
                }
            }
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // PaymentMethod y usuario (precondición: el cliente está logado)
            var paymentMethod = await _context.PaymentMethods
                .Include(pm => pm.User)
                .FirstOrDefaultAsync(pm => pm.Id == purchaseForCreate.PaymentMethodId);

            if (paymentMethod == null || paymentMethod.User == null)
            {
                ModelState.AddModelError("PaymentMethodId", "Error! Payment method not found or not associated to a user");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            
            // Cabecera del pedido
            var order = new PurchaseOrder
            {
                Street = purchaseForCreate.Street,
                PostalCode = purchaseForCreate.PostalCode,
                NameSurname = purchaseForCreate.NameCustomer,
                City = purchaseForCreate.City,
                Date = DateTime.Now,
                State = PurchaseState.Request,
                PaymentMethodId = paymentMethod.Id,
                Customer = paymentMethod.User,
                TotalPrice = 0m,
                Rating = purchaseForCreate.Rating
            };

            _context.PurchaseOrders.Add(order);
            await _context.SaveChangesAsync();
            foreach (var it in purchaseForCreate.PurchasedProducts)
            {
                var p = products.First(x => x.Id == it.ProductId);

                _context.PurchaseProducts.Add(new PurchaseProduct
                {
                    PurchaseOrderId = order.Id,
                    ProductId = p.Id,
                    Price = p.Price,
                    Quantity = it.Quantity
                });

                order.TotalPrice += p.Price * it.Quantity;
            }

            _context.PurchaseOrders.Update(order);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: {ex.Message}");
                return Conflict("Error " + ex.Message);
            }

            var dtoOut = await _context.PurchaseOrders
                .Where(po => po.Id == order.Id)
                .Include(po => po.PurchaseProducts)
                    .ThenInclude(pp => pp.Product)
                        .ThenInclude(p => p.Brand)
                .Include(po => po.Customer)
                .Include(po => po.PaymentMethod)
                .Select(po => new PurchaseForDetailDTO(
                    po.Id,
                    po.TotalPrice,
                    po.Date,
                    po.Street,
                    po.City,
                    po.PostalCode,
                    po.NameSurname,
                    po.State.ToString(),
                    po.PaymentMethod.GetType().Name,
                    po.Customer.UserName!,
                    po.PurchaseProducts
                        .Select(pp => new PurchaseProductDTO(
                            pp.ProductId,
                            pp.Product.Name,
                            pp.Product.Brand.Name,
                            pp.Product.Colour,
                            pp.Quantity,
                            pp.Price
                        )).ToList(),
                    po.Rating
                ))
                .FirstAsync();

            return CreatedAtAction(nameof(GetPurchase), new { id = order.Id }, dtoOut);
        }


    }

=======

            return Ok(purchase);
        }
    }
>>>>>>> origin/development
}

