using ProjetoInter.Models;

namespace ProjetoInter.Services.MarketCar
{
    public interface IMarketCarInterface
    {
        Task<MarketCarModel> AddToMarketCar(Guid productId);
        Task<MarketCarModel> RemoveFromMarketCar(Guid productId);
        Task<List<MarketCarModel>> GetMyMarketCars(Guid userId);
        Task<List<ProductModel>> GetProductsInMyMarketCar();
    }
}
