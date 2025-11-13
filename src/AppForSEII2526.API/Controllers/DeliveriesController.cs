using AppForSEII2526.API.DTOs.DeliveryDriverDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeliveriesController> _logger;

        public DeliveriesController(ApplicationDbContext context, ILogger<DeliveriesController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<OrderForSchedulingDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAvailableOrders(
             [FromQuery] string? postalCode,
             [FromQuery] decimal? minTotalPrice)
        {
            // alt flow 2: validation check
            if (minTotalPrice < 0)
            {
                ModelState.AddModelError(nameof(minTotalPrice), "Minimum total price cannot be negative.");
                _logger.LogError("Error: Minimum total price was negative.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // request filter
            var query = _context.PurchaseOrders
                .Where(o => o.State == PurchaseState.Request);

            // alt flow 2 applied filters
            if (!string.IsNullOrEmpty(postalCode))
            {
                query = query.Where(o => o.PostalCode.Contains(postalCode));
            }

            if (minTotalPrice.HasValue)
            {
                query = query.Where(o => o.TotalPrice >= minTotalPrice.Value);
            }

            // to dto
            var orders = await query
                .OrderBy(o => o.Date)
                .Select(o => new OrderForSchedulingDTO(
                    o.Id,
                    o.Street,
                    o.City,
                    o.PostalCode,
                    o.Date,
                    o.TotalPrice,
                    o.NameSurname)) 
                .ToListAsync();

            // alt flow 1: no orders found
            if (orders == null || !orders.Any())
            {
                _logger.LogInformation("No purchase orders found with state 'Request'.");
                return NotFound("No purchase orders available for scheduling.");
            }

            return Ok(orders);
        }
    }
}
