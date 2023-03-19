using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquitiesApi.Services;
using EquitiesApi.Services.OutboundModels;

namespace EquitiesApiTests
{
    public class AlphaServiceTests
    {
        List<Return> returns = new List<Return> {
            new Return { Date = "2023-03-14", DailyReturn = 1.2, Close = 5.2, Open = 4.0 },
            new Return { Date = "2023-03-15", DailyReturn = 1.8, Close = 7.0, Open = 5.2},
            new Return { Date = "2023-03-16", DailyReturn = 2.2, Close = 9.2, Open = 7.0},
            new Return { Date = "2023-03-17", DailyReturn = 1.5, Close = 10.7, Open = 9.2}};
        List<Return> benchmark = new List<Return> {
            new Return { Date = "2023-03-14", DailyReturn = 3.1, Close = 9.1, Open = 6.0 },
            new Return { Date = "2023-03-15", DailyReturn = 4.2, Close = 13.3, Open = 9.1},
            new Return { Date = "2023-03-16", DailyReturn = 5.0, Close = 18.3, Open = 13.3},
            new Return { Date = "2023-03-17", DailyReturn = 4.2, Close = 22.5, Open = 18.3}};

        public void CalculateVariance_ReturnsCorrectCalculationValue()
        {
            //Arrange

            //Act

            //Assert
        }

    }
}
