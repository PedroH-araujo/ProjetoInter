using ProjetoInter.Models;

namespace ProjetoInter.Services.MarketCar
{
    public interface IMarketCarInterface
    {
        Task<MarketCarModel> AddToMarketCar(Guid productId);
        Task<MarketCarModel> RemoveFromMarketCar(Guid productId);
        Task<MarketCarModel> GetMyMarketCars(Guid userId);
    }
}
