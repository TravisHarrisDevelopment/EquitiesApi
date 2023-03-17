namespace EquitiesApi.Services
{
    public interface IAlphaService
    {
        Task<string> GetAlphabySymbol(string symbol);
    }
}
