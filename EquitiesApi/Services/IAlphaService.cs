using EquitiesApi.Models;

namespace EquitiesApi.Services
{
    public interface IAlphaService
    {
        Task<Alpha> GetAlpha(string symbol, string benchmarkSymbol, string from, string to);

        
    }
}
