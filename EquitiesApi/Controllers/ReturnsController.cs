using Microsoft.AspNetCore.Mvc;
using EquitiesApi.Services;
using EquitiesApi.Services.OutboundModels;

namespace EquitiesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnsController : ControllerBase
    {
        private readonly IReturnsService _returnsService;

        public ReturnsController(IReturnsService returnsService) {
            _returnsService = returnsService;
        }

        /// <summary>
        /// Gets daily returns as percentage for the stock symbol provided for the date ranges provided.  Maximum date 
        /// span for retrieval is 366 days.  
        /// If no dates are supplied default values for year-to-date are used.
        /// </summary>
        /// <param name="symbol">Stock symbol for which you're requesting returns.</param>
        /// <param name="from">The start date for which you're pulling data.<br />Use date format yyyy-MM-dd.</param>
        /// <param name="to">The end date for which you're pulling data.<br />Use date format yyyy-MM-dd.</param>
        /// <returns>Returns a JSON array of daily returns as percentage for requested date range</returns>
        /// <response code="200">Returns JSON Array of daily returns as percentage for the requested date range</response>
        [ProducesResponseType(typeof(Return), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("/{symbol}/")]
        public async Task<IActionResult> GetReturns (
            [FromRoute] string symbol,
            [FromQuery] string? from = null, 
            [FromQuery] string? to = null)
        {
            try
            {
                var dates = Validation.ValidateDates(from, to);
                var info = await _returnsService.GetReturnsBySymbol(symbol, 
                    dates.Item1.ToString("yyyy-MM-dd"), 
                    dates.Item2.ToString("yyyy-MM-dd"));

                return Ok(info);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                var error = new ObjectResult(ex.Message);
                error.StatusCode = StatusCodes.Status500InternalServerError;

                return error;
            }
        }
    }
}