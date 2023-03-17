using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public async Task<string> GetReturnsBySymbol(string symbol, string from, string to)
        {
            string token = _configuration.GetValue<string>("ApiToken");

            var httpClient = _httpClientFactory.CreateClient("Iex");
            string testURL = $"https://cloud.iexapis.com/stable/time-series/HISTORICAL_PRICES/{symbol}?token={token}&from=2023-03-01&to=2023-03-01";
            var response = await httpClient.GetAsync(
                $"{symbol}?token={token}&from={from}&to={to}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            dynamic quote = JsonConvert.DeserializeObject(content);
            return content;
        }
    }
}
