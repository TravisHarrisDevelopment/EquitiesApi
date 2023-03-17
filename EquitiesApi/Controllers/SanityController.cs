using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EquitiesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanityController : ControllerBase
    {
        public SanityController()
        { }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            
            return Ok(new { V = Environment.GetEnvironmentVariable("ApiToken") });
        }
    }
}
