using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoInter.Data;
using ProjetoInter.Models;
using ProjetoInter.Services.Shared;

namespace ProjetoInter.Services.MarketCar
{
    public class MarketCarService : IMarketCarInterface
    {
        private readonly AppDbContext _dbContext;
        private readonly ISharedMethodsInterface _sharedMethods;

        public MarketCarService(AppDbContext dbContext, ISharedMethodsInterface sharedMethods)
        {
            _dbContext = dbContext;
            _sharedMethods = sharedMethods;
        }

        public async Task<MarketCarModel> AddToMarketCar(Guid productId)
        {
            try
            {
                MarketCarModel productInMarketCar = new MarketCarModel();
                productInMarketCar.ProductId = productId;
                productInMarketCar.UserId = _sharedMethods.GetUser().Id;

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
            Guid userId = _sharedMethods.GetUser().Id;
            productInMarketCar = await _dbContext.MarketCars.FirstOrDefaultAsync(marketCar => marketCar.UserId == userId && marketCar.ProductId == productId);
            await RemoveProductMarketCarId(productInMarketCar.Id, productId);

            _dbContext.Remove(productInMarketCar);
            await _dbContext.SaveChangesAsync();
            return productInMarketCar;
        }

       public async Task<List<MarketCarModel>> GetMyMarketCars(Guid userId, bool active = true)
        {
            try
            {
                return await _dbContext.MarketCars.Where(m => m.UserId == userId && m.IsActive == active).ToListAsync();
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
                var userId = _sharedMethods.GetUser().Id;
                var myMarketCars = await GetMyMarketCars(userId);
                var myMarketCarIds = myMarketCars.Select(mc => mc.Id).ToHashSet();

                // Obter todos os produtos do banco de dados
                var products = await _dbContext.Products.ToListAsync();

                // Filtrar os produtos que estão nos carrinhos de compras do usuário
                var productsInMyCart = products.Where(product =>
                    product.MarketCartId != null &&
                    product.MarketCartId.Any(id => myMarketCarIds.Contains(id)) &&
                    product.IsActive == true
                ).ToList();

                // Retornar a lista de produtos filtrados
                return productsInMyCart;

            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductModel>> BuyProducts()
        {
            try
            {
                List<ProductModel> products = await GetProductsInMyMarketCar();
                for (int i = 0; i < products.Count; i++)
                {
                    var product = products[i];
                    product.IsActive = false;

                    // Aguarde a conclusão da exclusão antes de continuar
                    await DeleteMarketCartFromSoldProduct(product.Id);

                    _dbContext.Update(product);
                }

                List<MarketCarModel> myMarketCars = await GetMyMarketCars(_sharedMethods.GetUser().Id);
                for (int i = 0; i < myMarketCars.Count; i++)
                {
                    var myMarketCar = myMarketCars[i];
                    myMarketCar.IsActive = false;
                    myMarketCar.UpdatedAt = DateTime.Now;

                    _dbContext.Update(myMarketCar);
                }

                await _dbContext.SaveChangesAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task DeleteMarketCartFromSoldProduct(Guid productId)
        {
            UserModel user = _sharedMethods.GetUser();
            try
            {
                var marketCars = await _dbContext.MarketCars
                    .Where(m => m.UserId != user.Id && m.IsActive && m.ProductId == productId)
                    .ToListAsync();

                for (int i = 0; i < marketCars.Count; i++)
                {
                    var marketCar = marketCars[i];

                    _dbContext.Remove(marketCar);
                }

                // Salvar as mudanças após a remoção
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductModel>> GetMyPurchasedProducts()
        {
            try
            {
                // Obter os IDs dos carrinhos de compras do usuário
                var userId = _sharedMethods.GetUser().Id;
                var myMarketCars = await GetMyMarketCars(userId, false);
                var myMarketCarIds = myMarketCars.Select(mc => mc.Id).ToHashSet();

                // Obter todos os produtos do banco de dados
                var products = await _dbContext.Products.ToListAsync();

                // Filtrar os produtos que estão nos carrinhos de compras do usuário
                var productsInMyCart = products.Where(product =>
                    product.MarketCartId != null &&
                    product.MarketCartId.Any(id => myMarketCarIds.Contains(id)) &&
                    product.IsActive == false
                ).ToList();

                // Retornar a lista de produtos filtrados
                return productsInMyCart;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MarketCarModel>> GetMarketCarsByProduct(Guid productId)
        {
            try
            {
                return await _dbContext.MarketCars.Where(m => m.ProductId == productId && m.IsActive == true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
