using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using EquitiesApi.Models;
using EquitiesApi.Models.DTO;
using EquitiesApi.Helpers;
using EquitiesApi.Outbound.Models;

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
            
            var dto = JsonSerializer.Deserialize<List<ReturnDTO>>(json);
            
            var returns = Mapper.Map(dto);
            
            return returns;
        }
    }
}
