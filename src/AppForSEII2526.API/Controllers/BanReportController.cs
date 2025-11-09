using AppForSEII2526.API.DTOs.ApplicationUserDTOs;
using AppForSEII2526.API.DTOs.BanReportDTOs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanReportController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ApplicationUsersController> _logger;

        public BanReportController(ApplicationDbContext context, ILogger<ApplicationUsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(BanReportForDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBanReport(int id)
        {
            BanReportForDetailDTO? report = await _context.BanReports
                .Where(br => br.Id == id)

                .Include(br => br.ReportCustomers) // join con ReportCustomer
                   .ThenInclude(rc => rc.Customer) // join con ApplicationUser
                   
                        .ThenInclude(appUser => appUser.Complaints) // join con Complaint

                .Select(br => new BanReportForDetailDTO(br.Id, br.Reason, br.DetailedDescription, br.StartDate, br.EndDate, 
                br.ReportCustomers
                    .Select(rc => new UserForBaningDTO(rc.Customer.Name, rc.Customer.Surname, rc.Customer.AccountCreationDate, new List<ComplaintDTO>(), rc.Message)).ToList()

                )).FirstOrDefaultAsync();

            if(report == null)
            {
                _logger.LogError($"Error: Report with id {id} does not exist");
                return NotFound();
            }

                return Ok(report);
        }
    }
}
