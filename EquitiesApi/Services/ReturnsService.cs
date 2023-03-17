using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using EquitiesApi.Models;

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
            string testURL = $"https://cloud.iexapis.com/stable/time-series/HISTORICAL_PRICES/{symbol}?token={token}&from=2023-03-01&to=2023-03-01";
            var response = await httpClient.GetAsync(
                $"{symbol}?token={token}&from={from}&to={to}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            
            var returns = JsonSerializer.Deserialize<List<Return>>(json);

            foreach(var r in returns)
            {
                r.DailyReturn = r.Close - r.Open;
            }
            return returns;
        }

        //Daily return is closing price - opening price


    }
}
