using AppForSEII2526.API.DTOs.DeliveryDTOs;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DeliveryAssignmentDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> ScheduleDelivery([FromBody] DeliveryAssignmentCreateDTO assignmentDTO)
        {
           //validate values from dto
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error: Mandatory information missing for delivery scheduling.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            //check if driver exists
            var driver = await _context.DeliveryDrivers.FindAsync(assignmentDTO.DeliveryDriverId);
            if (driver == null)
            {
                ModelState.AddModelError(nameof(assignmentDTO.DeliveryDriverId), "Selected delivery driver does not exist.");
                _logger.LogError($"Error: Driver with ID {assignmentDTO.DeliveryDriverId} not found.");
                return NotFound(new ValidationProblemDetails(ModelState));
            }

            //gets a list of drivers
            var orderIds = assignmentDTO.OrdersToAssign.Select(o => o.PurchaseOrderId).ToList();

            // checks if all orders exist AND are in the request state
            var ordersToSchedule = await _context.PurchaseOrders
                .Where(o => orderIds.Contains(o.Id) && o.State == PurchaseState.Request)
                .ToListAsync();

            if (ordersToSchedule.Count != orderIds.Count)
            {
                //checks invalid IDs
                var invalidIds = orderIds.Except(ordersToSchedule.Select(o => o.Id)).ToList();
                ModelState.AddModelError(nameof(assignmentDTO.OrdersToAssign), $"Some orders were invalid (not found or already assigned): {string.Join(", ", invalidIds)}");
                return NotFound(new ValidationProblemDetails(ModelState));
            }

            //entry of data
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //create delivery assignment
                var newAssignment = new DeliveryAssignment
                {
                    DeliveryDriverId = assignmentDTO.DeliveryDriverId,
                    ExtraReward = (decimal)assignmentDTO.ExtraReward,
                    PersonalMessage = assignmentDTO.PersonalMessage,
                    DeliveryAssignmentDone = assignmentDTO.Deadline, 

                };
                _context.DeliveryAssignments.Add(newAssignment);
                await _context.SaveChangesAsync(); // Get the newAssignment.Id from the DB

               //create purchasedelivery
                foreach (var orderDto in assignmentDTO.OrdersToAssign)
                {
                    var orderToUpdate = ordersToSchedule.First(o => o.Id == orderDto.PurchaseOrderId);

                    // create the record
                    var purchaseDelivery = new PurchaseDelivery
                    {
                        DeliveryAssignmentId = newAssignment.Id,
                        PurchaseOrderId = orderDto.PurchaseOrderId,
                        Date = DateTime.Now,
                        Priority = orderDto.Priority
                    };
                    _context.PurchaseDeliveries.Add(purchaseDelivery);

                    //update the state 
                    orderToUpdate.State = PurchaseState.Delivery;
                }

                await _context.SaveChangesAsync(); 
                await transaction.CommitAsync();


                // get full structure to display confirmation details
                var confirmationDetails = await _context.DeliveryAssignments
                    .Where(da => da.Id == newAssignment.Id)
                    .Include(da => da.DeliveryDriver)
                    .Include(da => da.PurchaseDeliveries!)
                        .ThenInclude(pd => pd.PurchaseOrder)
                    .FirstOrDefaultAsync();


                // mapping for DTO structure
                var assignedOrdersDto = confirmationDetails!.PurchaseDeliveries!
                    .Select(pd => new AssignedOrderDTO(
                        pd.PurchaseOrder!.Id,
                        pd.PurchaseOrder.Street,
                        pd.PurchaseOrder.City,
                        pd.PurchaseOrder.PostalCode,
                        pd.PurchaseOrder.Date,
                        pd.PurchaseOrder.TotalPrice,
                        pd.Priority
                    )).ToList();

                var responseDto = new DeliveryAssignmentDetailDTO(
                    confirmationDetails.Id,
                    confirmationDetails.DeliveryDriver!.Name,
                    confirmationDetails.DeliveryAssignmentDone, 
                    confirmationDetails.PersonalMessage,
                    confirmationDetails.ExtraReward,
                    assignedOrdersDto
                );

                // return 201 upon successful creation
                return CreatedAtAction(
                    nameof(GetSchedulingDetails),
                    new { orderIds = orderIds },
                    responseDto
                );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Failed to schedule delivery.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An unexpected error occurred during delivery scheduling (try dropping IX_PurchaseDeliveries_PurchaseOrderId)");
            }
        }
    }
}
