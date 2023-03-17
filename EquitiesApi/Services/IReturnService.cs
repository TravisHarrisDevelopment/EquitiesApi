namespace EquitiesApi.Services
{
    public interface IReturnsService
    {
        Task<string> GetReturnsBySymbol(string symbol, string from, string to);
    }
}
