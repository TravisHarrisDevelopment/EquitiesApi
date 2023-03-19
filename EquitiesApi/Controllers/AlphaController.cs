using Microsoft.AspNetCore.Mvc;
using EquitiesApi.Services;
using EquitiesApi.Services.OutboundModels;

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

        /// <summary>
        /// Gets Alpha (1 number) for the stock symbol provided measured against the benchmark symbol provided for 
        /// the date ranges provided.  Maximum date span for retrieval is 366 days.  
        /// If no dates are supplied default values for year-to-date are used.
        /// </summary>
        /// <param name="symbol">Stock symbol for equity of interest.</param>
        /// <param name="benchmarkSymbol">Stock symbol for benchmark.</param>
        /// <param name="from">The start date for which you're pulling data.<br />Use date format yyyy-MM-dd.</param>
        /// <param name="to">The end date for which you're pulling data.<br />Use date format yyyy-MM-dd.</param>
        /// <returns>Returns a JSON representation of Alpha (1 number)for requested date range</returns>
        /// <response code="200">Returns JSON representation of Alpha (1 number) for requested date range</response>
        [ProducesResponseType(typeof(Alpha), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("/{symbol}/{benchmarkSymbol}/")]
        public async Task<IActionResult> GetAlpha(
            [FromRoute] string symbol,
            [FromRoute] string benchmarkSymbol,
            [FromQuery] string? from = null,
            [FromQuery] string? to = null)
        {
            try
            {
                var dates = Validation.ValidateDates(from, to);
                var info = await _alphaService.GetAlpha(symbol, benchmarkSymbol,
                    dates.Item1.ToString("yyyy-MM-dd"),
                    dates.Item2.ToString("yyyy-MM-dd"));

                return Ok(info);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                var error = new ObjectResult(ex.Message);
                error.StatusCode = StatusCodes.Status500InternalServerError;

                return error;
            }
        }


    }
}
