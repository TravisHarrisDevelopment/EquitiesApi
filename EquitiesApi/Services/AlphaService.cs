using EquitiesApi.Helpers;
using EquitiesApi.Models.DTO;
using EquitiesApi.Outbound.Models;
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

            var response = await httpClient.GetAsync($"{symbol}?token={token}&from={from}&to={to}");
            var responseBenchmark = await httpClient.GetAsync($"{benchmarkSymbol}?token={token}&from={from}&to={to}");
            response.EnsureSuccessStatusCode();
            responseBenchmark.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var benchmarkJson = await responseBenchmark.Content.ReadAsStringAsync();

            var returnsDTO = JsonSerializer.Deserialize<List<ReturnDTO>>(json);
            var benchmarkReturnsDTO = JsonSerializer.Deserialize<List<ReturnDTO>>(benchmarkJson);

            var returns = Mapper.Map(returnsDTO);
            var benchmarkReturns = Mapper.Map(benchmarkReturnsDTO);

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
             * 
             * Alpha is the mean return of the investment - risk free rate of return - (beta * (benchmark mean return - riskfree rate of return)
             * Alpha = R-R[f]-(beta * (R[m]-R[f])
             */

            var riskFreeRate = -0.0317;

            var covariance = CalculateCovariance(returns, benchmark);
            var variance = CalculateVariance(benchmark);
            var beta = covariance / variance;

            var rSum = returns.Sum(r => r.DailyReturn);
            var rAvg = rSum / returns.Count();

            var bSum = benchmark.Sum(b => b.DailyReturn);
            var bAvg = bSum / benchmark.Count();

            var alpha = rAvg - riskFreeRate - (beta * (bAvg - riskFreeRate));

            return new Alpha { AlphaValue= (int)alpha };
            
        }

        private double CalculateBeta(IEnumerable<Return>returns, IEnumerable<Return> benchmark)
        {
            return 2.0;
        }

        private double CalculateCovariance(IEnumerable<Return> returns, IEnumerable<Return> benchmark)
        {
            if (! (returns.Count() == benchmark.Count()) )
            {
                throw new Exception("Investment and Benchmark have a differing number of returns for this date range.");
            }
            var count = returns.Count();
            var rSum = returns.Sum(r => r.DailyReturn);
            var rAvg = rSum / count;
            var bSum = benchmark.Sum(b => b.DailyReturn);
            var bAvg = bSum / count;
            var days = new List<double>();
            for (int i = 0;i< count;i++)
            {
                days.Add((returns.ElementAt(i).DailyReturn - rAvg) * (benchmark.ElementAt(i).DailyReturn - bAvg));
            }
            var daysSum = days.Sum(d => d);
            
            return daysSum / (count - 1);
        }

        private double CalculateVariance(IEnumerable<Return> benchmark)
        {
            //Variance is the difference between return and mean return squared and dividing the sum of the squares by number of returns in the set
          
            var bSum = benchmark.Sum(b => b.DailyReturn);
            var bAvg = bSum / benchmark.Count();

            var days = new List<double>();
            for(int i =0;i< benchmark.Count(); i++)
            {
                var difference = benchmark.ElementAt(i).DailyReturn - bAvg;
                days.Add(difference * difference);
            }
            var sumofSquares = days.Sum(d => d);
            return sumofSquares / benchmark.Count();
        }

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