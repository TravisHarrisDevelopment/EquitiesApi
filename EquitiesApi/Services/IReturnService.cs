using EquitiesApi.Models;

namespace EquitiesApi.Services
{
    public interface IReturnsService
    {
        Task<IEnumerable<Return>> GetReturnsBySymbol(string symbol, string from, string to);
    }
}
