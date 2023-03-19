using EquitiesApi.Services.OutboundModels;

namespace EquitiesApi.Services
{
    public class AlphaService : IAlphaService
    {
        private readonly IReturnsService _returnsService;

        public AlphaService(IReturnsService returnsService)
        {
            _returnsService = returnsService;
        }

        public async Task<Alpha> GetAlpha(string symbol, string benchmarkSymbol, string from, string to)
        {
            var returns = await _returnsService.GetReturnsBySymbol(symbol, from, to);
            var benchmarkReturns = await _returnsService.GetReturnsBySymbol(benchmarkSymbol, from, to);

            var alpha = CalculateAlpha(returns, benchmarkReturns);

            return alpha;
        }

        private Alpha CalculateAlpha(IEnumerable<Return> returns, IEnumerable<Return> benchmark)
        {
            /*
             * a quick swag at risk free rate of return:
             * long term average interest rate on 1 Year T-Bill:  2.87%
             * current annual inflation rate: 6.04%
             * T-Bill rate - inflation rate = 2.87% - 6.04% = -3.17%
             * 
             * Beta = covariance(returns, benchmark)/variance (benchmark)
             * 
             * Alpha is the mean return of the investment - risk free rate of return - (beta * (benchmark mean return - riskfree rate of return)
             * Alpha = R-R[f]-(Beta * (R[m]-R[f])
             * 
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

        public double CalculateCovariance(IEnumerable<Return> returns, IEnumerable<Return> benchmark)
        {
            if (! (returns.Count() == benchmark.Count()) )
            {
                throw new Exception("Investment and Benchmark have a differing number of returns for this date range.");
            }

            // determine the sum and average of returns for ticker and benchmark
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

        public double CalculateVariance(IEnumerable<Return> benchmark)
        {
            //Variance is the difference between return and mean return squared and dividing the sum of the squares
            //by number of returns in the set
          
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
    }

    
}
/* Assignment for Get Alpha:
 * Get Alpha 
 * - Similar to the above, the URL should include the ticker and date range, with the
 *   addition of another ticker for the “benchmark” 
 * - Again, leverage IEX to get the ticker and benchmark’s historical prices 
 * - Calculate the alpha (one number) of the ticker vs. the benchmark over the time period 
 *   requested 
 * - Respond to the GET with the alpha number as JSON 
 */
