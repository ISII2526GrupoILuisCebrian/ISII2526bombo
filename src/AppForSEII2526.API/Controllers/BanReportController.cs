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
        public async Task<ActionResult> GetBanReport(int id) // QUE PARAMETROS PASO??
        {
            IList<BanReportForDetailDTO> banReportForDetailDTOs = await _context.BanReports
                .Include(banReport => )
        }
}
