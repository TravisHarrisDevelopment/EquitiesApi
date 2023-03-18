using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EquitiesApi.Services;

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

        [HttpGet("/{symbol}/")]
        //[Route("GetReturns/{symbol:string}/{from:DateOnly?}/{to:DateOnly?}")]
        public async Task<IActionResult> GetReturns (
            [FromRoute] string symbol,
            [FromQuery] string? from = "2023-03-06", 
            [FromQuery] string? to = "2023-03-10")
        {
            DateTime fromDate, toDate;
            var fromResult = DateTime.TryParse(from, out fromDate);
            var toResult = DateTime.TryParse(to, out toDate);
            if (! fromResult && toResult)
            {
                return BadRequest("cannot parse dates - use format \"yyyy-MM-DD\"");
            }
            if (! ValidateDates(fromDate, toDate))
            {
                return BadRequest("Dates may not span more than 1 year and from date must occur before to date.");
            }

            var info = await _returnsService.GetReturnsBySymbol(symbol, fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"));
            //return Ok(new { info });
            return Ok(info);


        }

        private bool ValidateDates(DateTime fromDate, DateTime toDate)
        {
            //validate that fromDate < toDate
            var rangeValid = fromDate < toDate ? true : false;

            //validate that fromDate thru toDate is less than 1 year
            var span = toDate - fromDate;
            var spanValid = span.Days <= 366 ? true : false;

            return rangeValid && spanValid;

        }
    }
}

/*
- The URL for the call should include the stock ticker symbol to get returns for, and
should accept parameters for the “from date” and “to date”
- Check to make sure the date range is not too large
- If no dates are passed in, assume the time period is YTD
- Leverage IEX to get the ticker’s historical prices (or mock a similar endpoint):
o https://iexcloud.io/docs/api-basics
o https://iexcloud.io/docs/core
- Calculate daily returns for the days specified
- Respond to the GET with the returns as JSON
*/