using AppForSEII2526.API.DTOs.ApplicationUserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ApplicationUsersController> _logger;

        public ApplicationUsersController(ApplicationDbContext context, ILogger<ApplicationUsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /*
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(decimal), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ComputeDivision(decimal a, decimal b)
        {
            if(b == 0)
            {
                string error = "B cannot be 0 to compute a division";
                _logger.LogError(DateTime.Now + " Error:" + error);
                return BadRequest(error);
            }
            decimal result = a / b;
            return Ok(result);
        }
        */

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<UserForBaningDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SelectApplicationUsersForBaning(string? userSurname, string? typeComplaint)
        {
            IList<UserForBaningDTO> appUsersDTOS = await _context.Users
                .Include(user => user.Complaints)
                .Where(user => (
                    ((userSurname == null) || (user.Name.Contains(userSurname)))
                   //tipo complaint
                    && ((typeComplaint == null) || (user.
                        Complaints
                        .Any(complaint => complaint.Type.Name.Equals(typeComplaint) && complaint.Processed == false)
                        ) )
                    //complaint no processed
                    && (user.Complaints.Any(complaint => complaint.Processed == false))
                    ))
                .OrderBy(user=>user.Name)
                .Select(user=>new UserForBaningDTO(user.Name, user.Surname, user.AccountCreationDate,
                    user.Complaints
                    .GroupBy(complaint => complaint.Type.Name).Select(group => new ComplaintDTO(
                        group.Key,
                        group.Count()
                    )).ToList()
, TODO)) // CAMBIAR
                    // CAMBIAR
                .ToListAsync(); 
            return Ok(appUsersDTOS);
        }

    }
}
