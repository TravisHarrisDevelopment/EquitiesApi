namespace EquitiesApi.Services
{
    public static class ExtensionMethods
    {
        public static IEnumerable<OutboundModels.Return> Translate(this IEnumerable<Private.ReturnDTO> input)
        {
            var ordered = input.OrderBy(s => s.Date).ToList();
            var returns = new List<OutboundModels.Return>();
            foreach (var item in ordered)
            {
                var ret = new OutboundModels.Return()
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

