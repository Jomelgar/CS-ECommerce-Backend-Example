using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedData;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _appDb;

        public HealthController(AppDbContext appDb)
        {
            _appDb = appDb;
        }
        [HttpGet]

        public async Task<IActionResult> Get()
        {
            try
            {
                if (_appDb.Database.CanConnect())
                {
                    return Ok(new { status = "Healthy", message = "Database is connected" });
                }
                else { return StatusCode(500, new { status = "Unhealthy", message = "Database is not connected" }); }
            }
            catch(Exception e)
            {
                return StatusCode(501, new { status = "Dead", message="Database can't be connected"});
            }
        }
    }
}
