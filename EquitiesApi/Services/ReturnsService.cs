using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EquitiesApi.Services
{
    public class ReturnsService : IReturnsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiToken;

        public ReturnsService(HttpClient httpClient, string apiToken)
        {
            _httpClient = httpClient;
            _apiToken = apiToken;
        }

        public async Task<string> GetReturnsbySymbol(string symbol)
        {
            string token = "";
            string url = $"https://cloud.iexapis.com/stable/stock/{symbol}/quote?token={token}";
            string url2 = "https://cloud.iexapis.com/stable/stock/aapl/quote?token=";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            dynamic quote = JsonConvert.DeserializeObject(content);
            return content;
        }
    }
}
