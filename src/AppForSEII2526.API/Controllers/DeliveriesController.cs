using AppForSEII2526.API.DTOs.DeliveryDriverDTOs;
using AppForSEII2526.API.DTOs.DeliveryDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(SchedulingDetailsDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetSchedulingDetails([FromQuery] List<int> orderIds)
        {
            // alt flor 3: check if list is empty
            if (orderIds == null || !orderIds.Any())
            {
                return BadRequest("No purchase orders selected for scheduling.");
            }

            // put into dto
            var selectedOrders = await _context.PurchaseOrders
                .Where(o => orderIds.Contains(o.Id))
                .Select(o => new OrderForSchedulingDTO(
                    o.Id,
                    o.Street,
                    o.City,
                    o.PostalCode,
                    o.Date,
                    o.TotalPrice,
                    o.NameSurname))
                .ToListAsync();

            // show all orderds that were not found
            if (selectedOrders.Count != orderIds.Count)
            {
                var missingIds = orderIds.Except(selectedOrders.Select(o => o.Id)).ToList();
                return NotFound($"The following order IDs were not found: {string.Join(", ", missingIds)}");
            }

            // get available drivers
            var availableDrivers = await _context.DeliveryDrivers
                .Select(dd => new DriverForAssignmentDTO(
                    dd.Id,
                    dd.Name, 
                    dd.Available))
                .ToListAsync();

            var detailsDTO = new SchedulingDetailsDTO(selectedOrders, availableDrivers);

            return Ok(detailsDTO);
        }
    }
}
