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

        [HttpGet("/{symbol}/{benchmarkSymbol}/")]
        public async Task<IActionResult> GetAlpha(
            [FromRoute] string symbol,
            [FromRoute] string benchmarkSymbol,
            [FromQuery] string? from = "2023-03-06",
            [FromQuery] string? to = "2023-03-10")
        {
            var info = await _alphaService.GetAlpha(symbol, benchmarkSymbol, from, to);
            return Ok(new { info });
        }


    }
}
