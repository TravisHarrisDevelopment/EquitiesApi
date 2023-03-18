using EquitiesApi.Models.DTO;
using EquitiesApi.Outbound.Models;

namespace EquitiesApi.Helpers
{
    public static class Mapper
    {
        public static IEnumerable<Return> Map(IEnumerable<ReturnDTO> dto)
        {
            var ordered = dto.OrderBy(s => s.Date).ToList();
            var returns = new List<Return>();
            foreach(var item in ordered)
            {
                var ret = new Return()
                {
                    Date = item.Date,
                    Open = item.Open,
                    Close = item.Close,
                    DailyReturn = ((item.Close - item.Open) / item.Open) * 100
                };
                returns.Add(ret);
            }
            return returns;
        }
    }
}
