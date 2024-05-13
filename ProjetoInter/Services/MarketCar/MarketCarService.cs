using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoInter.Data;
using ProjetoInter.Models;

namespace ProjetoInter.Services.MarketCar
{
    public class MarketCarService : IMarketCarInterface
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;

        public MarketCarService(AppDbContext dbContext, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        public async Task<MarketCarModel> AddToMarketCar(Guid productId)
        {
            try
            {
                MarketCarModel productInMarketCar = new MarketCarModel();
                productInMarketCar.ProductId = productId.ToString();
                productInMarketCar.UserId = GetUser().Id.ToString();

                _dbContext.Add(productInMarketCar);
                await _dbContext.SaveChangesAsync();

                await UpdateProduct(productInMarketCar.Id, productId);
                return productInMarketCar;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateProduct(Guid productMarketCarId, Guid productId)
        {
            try
            {
                ProductModel product = await GetProductById(productId);
                product.MarketCartId = productMarketCarId.ToString();

                _dbContext.Update(product);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductModel> GetProductById(Guid id)
        {
            try
            {
                return await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MarketCarModel> RemoveFromMarketCar(Guid productId)
        {
            MarketCarModel productInMarketCar = new MarketCarModel();
            string productIdString = productId.ToString();
            string userId = GetUser().Id.ToString();
            productInMarketCar = await _dbContext.MarketCars.FirstOrDefaultAsync(marketCar => marketCar.UserId == userId && marketCar.ProductId == productIdString);

            _dbContext.Remove(productInMarketCar);
            await _dbContext.SaveChangesAsync();
            return productInMarketCar;
        }

        private UserModel GetUser()
        {
            string userSession = _httpContext.HttpContext.Session.GetString("sessionUserLogged");
            UserModel user = JsonConvert.DeserializeObject<UserModel>(userSession);

            return user;
        }

        public async Task<MarketCarModel> GetMyMarketCars(Guid userId)
        {
            try
            {
                return await _dbContext.MarketCars.FirstOrDefaultAsync(marketCar => marketCar.UserId == userId.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
