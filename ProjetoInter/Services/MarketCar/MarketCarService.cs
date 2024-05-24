using System.Collections.Generic;
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
                productInMarketCar.ProductId = productId;
                productInMarketCar.UserId = GetUser().Id;

                _dbContext.Add(productInMarketCar);
                await _dbContext.SaveChangesAsync();

                await AddProductMarketCarId(productInMarketCar.Id, productId);
                return productInMarketCar;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task AddProductMarketCarId(Guid productMarketCarId, Guid productId)
        {
            try
            {
                ProductModel product = await GetProductById(productId);

                // Verifique se o MarketCartId já está inicializado, se não estiver, inicialize-o como um novo array
                if (product.MarketCartId == null)
                {
                    product.MarketCartId = new Guid[] { productMarketCarId };
                }
                else
                {
                    // Crie um novo array com um elemento extra e copie os valores do array original
                    Guid[] newMarketCartId = new Guid[product.MarketCartId.Length + 1]; // cria novo array com o tamanho do (product.MarketCartId) + 1
                    Array.Copy(product.MarketCartId, newMarketCartId, product.MarketCartId.Length); //copia os elementos de product.MarketCartId para newMarketCartId com o tamanho product.MarketCartId.Length
                    newMarketCartId[newMarketCartId.Length - 1] = productMarketCarId; //adiciona productMarketCarId ao final do array
                    product.MarketCartId = newMarketCartId;
                }

                _dbContext.Update(product);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task RemoveProductMarketCarId(Guid productMarketCarId, Guid productId)
        {
            try
            {
                ProductModel product = await GetProductById(productId);

                // Verifica se o MarketCartId está inicializado e contém o elemento que queremos remover
                if (product.MarketCartId != null && product.MarketCartId.Contains(productMarketCarId))
                {
                    // Converte o array para uma lista para facilitar a remoção do elemento
                    List<Guid> marketCartList = product.MarketCartId.ToList();

                    // Remove o elemento específico da lista
                    marketCartList.Remove(productMarketCarId);

                    // Atualiza o array no objeto product com a lista resultante
                    product.MarketCartId = marketCartList.ToArray();

                    _dbContext.Update(product);
                    await _dbContext.SaveChangesAsync();
                }
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
            Guid userId = GetUser().Id;
            productInMarketCar = await _dbContext.MarketCars.FirstOrDefaultAsync(marketCar => marketCar.UserId == userId && marketCar.ProductId == productId);
            await RemoveProductMarketCarId(productInMarketCar.Id, productId);

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

       public async Task<List<MarketCarModel>> GetMyMarketCars(Guid userId)
        {
            try
            {
                return await _dbContext.MarketCars.Where(m => m.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async  Task<List<ProductModel>> GetProductsInMyMarketCar()
        {
             try
            {
                // Obter os IDs dos carrinhos de compras do usuário
                var userId = GetUser().Id;
                var myMarketCars = await GetMyMarketCars(userId);
                var myMarketCarIds = myMarketCars.Select(mc => mc.Id).ToHashSet();

                // Obter todos os produtos do banco de dados
                var products = await _dbContext.Products.ToListAsync();

                // Filtrar os produtos que estão nos carrinhos de compras do usuário
                var productsInMyCart = products.Where(product =>
                    product.MarketCartId != null &&
                    product.MarketCartId.Any(id => myMarketCarIds.Contains(id))
                ).ToList();

                // Retornar a lista de produtos filtrados
                return productsInMyCart;

            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
