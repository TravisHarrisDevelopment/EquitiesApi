using EquitiesApi.Models;
using System.Text.Json;

namespace EquitiesApi.Services
{
    public class AlphaService : IAlphaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AlphaService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Alpha> GetAlpha(string symbol, string benchmarkSymbol, string from, string to)
        {
            string token = _configuration.GetValue<string>("ApiToken");

            var httpClient = _httpClientFactory.CreateClient("Iex");

            var response = await httpClient.GetAsync(
                $"{symbol}?token={token}&from={from}&to={to}");
            var responseBenchmark = await httpClient.GetAsync(
                $"{benchmarkSymbol}?token={token}&from={from}&to={to}");
            response.EnsureSuccessStatusCode();
            responseBenchmark.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var benchmarkJson = await response.Content.ReadAsStringAsync();

            var returns = JsonSerializer.Deserialize<List<Return>>(json);
            var benchmarkReturns = JsonSerializer.Deserialize<List<Return>>(benchmarkJson);

            

            foreach (var r in returns)
            {
                r.DailyReturn = ((r.Close - r.Open) / r.Open) * 100;
            }
            foreach (var b in benchmarkReturns)
            {
                b.DailyReturn = ((b.Close - b.Open)/b.Open)*100;
            }

            var alpha = CalculateAlpha(returns, benchmarkReturns);

            return alpha;
        }
        private Alpha CalculateAlpha(IEnumerable<Return> returns, IEnumerable<Return> benchmark)
        {
            /*
             * putting together a quick swag at risk free rate of return
             * long term average interest rate on 1 Year T-Bill:  2.87%
             * current annual inflation rate: 6.04%
             * T-Bill rate - inflation rate = 2.87% - 6.04% = -3.17%
             */

            var riskFreeRate = - 0.0317;
            return new Alpha { AlphaValue= (int)riskFreeRate };
            
        }

        //private double CalculateCovariance(IEnumerable<Return> returns, IEnumerable<Return> benchmark)

        /*
         * Alpha = Return - RiskFreeReturn - (beta * (benchmarkReturn-RiskFreeReturn))
         * 
         * Beta = covariance(returns, benchmark)/variance (benchmark)
         * */
    }

    
}
    /*Get Alpha 
- Similar to the above, the URL should include the ticker and date range, with the
addition of another ticker for the “benchmark” 
- Again, leverage IEX to get the ticker and benchmark’s historical prices 
- Calculate the alpha (one number) of the ticker vs. the benchmark over the time period 
requested 
- Respond to the GET with the alpha number as JSON
    */