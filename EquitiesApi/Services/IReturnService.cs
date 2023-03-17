namespace EquitiesApi.Services
{
    public interface IReturnsService
    {
        Task<string> GetReturnsbySymbol(string symbol);
    }
}
