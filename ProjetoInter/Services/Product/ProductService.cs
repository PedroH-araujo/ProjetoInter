using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoInter.Data;
using ProjetoInter.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetoInter.Services.Produto
{
    public class ProductService : IProductInterface
    {
        private readonly AppDbContext _dbContext;
        private readonly string _system;
        private readonly IHttpContextAccessor _httpContext;
        public ProductService(AppDbContext dbContext, IWebHostEnvironment system, IHttpContextAccessor _httpContext)
        {
            this._dbContext = dbContext;
            this._system = system.WebRootPath;
            this._httpContext = _httpContext;
        }
        public async Task<ProductModel> CreateProduct(ProductModel product, IFormFile image)
        {
            var imageUrl = (image != null) ? GetImageUrl(image) : null;
            product.ImageUrl = imageUrl;

            UserModel user = GetUser();

            product.SellerId = user.Id;

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }

        public string GetImageUrl(IFormFile image)
        {
            var uniqueKey = Guid.NewGuid().ToString(); //caso seja gerado duas imagens de mesmo nome, nao vai dar conflito
            var ImageUrl = image.FileName.Replace(" ","").ToLower() + uniqueKey + ".png";

            var imagesPath = _system + "\\imagem\\";

            // criar pasta imagem dentro do wwwroot e salvar as imagens la dentro
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            using (var stream = File.Create(imagesPath + ImageUrl))
            {
                image.CopyToAsync(stream).Wait();
            }

            return ImageUrl;
        }

        private UserModel GetUser()
        {
            string userSession = _httpContext.HttpContext.Session.GetString("sessionUserLogged");
            UserModel user = JsonConvert.DeserializeObject<UserModel>(userSession);

            return user;
        }

        public async Task<List<ProductModel>> GetMyProducts()
        {
            UserModel user = GetUser();
            try
            {
                return await _dbContext.Products.Where(product => product.SellerId == user.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            try
            {
                List<ProductModel> products = await _dbContext.Products.ToListAsync();
                var formattedProducts = await SetIsProductInMyMarketCart(products);
                return formattedProducts;
            }
            catch(Exception ex) 
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

        private async Task<List<ProductModel>> SetIsProductInMyMarketCart(List<ProductModel> products)
        {
            var user = GetUser();
            var myMarketCar = await GetMyMarketCars(user.Id);

            foreach (var product in products)
            {
                if (product.MarketCartId.ToArray() == null || product.MarketCartId.ToArray().Length == 0) continue;

                product.IsProductInMyMarketCart = myMarketCar.Any(mc => product.MarketCartId.Contains(mc.Id));
            }

            return products;
        }

        public async Task<ProductModel> UpdateProduct(ProductModel product, IFormFile image)
        {
            try
            {
                var dbProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

                if (dbProduct == null)
                {
                    throw new Exception("Product not found.");
                }

                var newUrlImage = "";

                if (image != null)
                {
                    string urlImageActual = _system + "\\imagem\\" + dbProduct.ImageUrl;

                    if (File.Exists(urlImageActual))
                    {
                        File.Delete(urlImageActual);
                    }

                    newUrlImage = GetImageUrl(image);

                }

                if (newUrlImage != "")
                {
                    dbProduct.ImageUrl = newUrlImage;
                }

                dbProduct.Title = product.Title;
                dbProduct.Description = product.Description;
                dbProduct.Status = product.Status;
                dbProduct.Value = product.Value;

                _dbContext.Update(dbProduct);
                await _dbContext.SaveChangesAsync();

                return dbProduct;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductModel> DeleteProduct(Guid id)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (product.ImageUrl != null)
                {
                    string urlImageActual = _system + "\\imagem\\" + product.ImageUrl;

                    File.Delete(urlImageActual);
                }

                _dbContext.Remove(product);
                await _dbContext.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductModel>> GetFilteredProducts(string? filter)
        {
            try
            {
                var products = await _dbContext.Products.Where(p => p.Title.Contains(filter)).ToListAsync();
                var formattedProducts = await SetIsProductInMyMarketCart(products);
                return formattedProducts;

            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
    }
}
