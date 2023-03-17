using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EquitiesApi.Services;

namespace EquitiesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlphaController : ControllerBase
    {
        private readonly IAlphaService _alphaService;

        public AlphaController(IAlphaService alphaService)
        {
            _alphaService = alphaService;
        }

        [HttpGet(Name = "GetAlpha")]
        public async Task<IActionResult> Get()
        {
            var info = await _alphaService.GetAlphabySymbol("AAPL");
            return Ok(new { info });
        }
    }
}
