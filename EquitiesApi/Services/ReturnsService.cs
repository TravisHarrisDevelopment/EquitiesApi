using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using EquitiesApi.Models;
using EquitiesApi.Models.Outbound;

namespace EquitiesApi.Services
{
    public class ReturnsService : IReturnsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ReturnsService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Return>> GetReturnsBySymbol(string symbol, string from, string to)
        {
            string token = _configuration.GetValue<string>("ApiToken");

            var httpClient = _httpClientFactory.CreateClient("Iex");
            
            var response = await httpClient.GetAsync($"{symbol}?token={token}&from={from}&to={to}");
            
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            
            var dto = JsonSerializer.Deserialize<List<Private.ReturnDTO>>(json);

            return dto.Translate();
        }
    }
}
/* Assignment for Get Return
 * 
 * Get Return 
 * - The URL for the call should include the stock ticker symbol to get returns for, and
 *   should accept parameters for the “from date” and “to date” 
 * - Check to make sure the date range is not too large 
 * - If no dates are passed in, assume the time period is YTD 
 * - Leverage IEX to get the ticker’s historical prices (or mock a similar endpoint): 
 *      o https://iexcloud.io/docs/api-basics 
 *      o https://iexcloud.io/docs/core 
 * - Calculate daily returns for the days specified 
 * - Respond to the GET with the returns as JSON 
 */

