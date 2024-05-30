using ProjetoInter.Models;

namespace ProjetoInter.Services.MarketCar
{
    public interface IMarketCarInterface
    {
        Task<MarketCarModel> AddToMarketCar(Guid productId);
        Task<MarketCarModel> RemoveFromMarketCar(Guid productId);
        Task<List<MarketCarModel>> GetMyMarketCars(Guid userId, bool active = true);
        Task<List<ProductModel>> GetProductsInMyMarketCar();
        Task<List<ProductModel>> BuyProducts();
        Task<List<ProductModel>> GetMyPurshasedProducts();
    }
}
