using Microsoft.EntityFrameworkCore;
using ProjetoInter.Data;
using ProjetoInter.Models;

namespace ProjetoInter.Services.Produto
{
    public class ProductService : IProductInterface
    {
        private readonly AppDbContext _dbContext;
        private readonly string _system;
        public ProductService(AppDbContext dbContext, IWebHostEnvironment system)
        {
            this._dbContext = dbContext;
            this._system = system.WebRootPath;
        }
        public ProductModel CreateProduct(ProductModel product, IFormFile image)
        {
            var imageUrl = GetImageUrl(image);
            product.ImageUrl = imageUrl;

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

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

        public async Task<List<ProductModel>> GetProducts()
        {
            try
            {
                return await _dbContext.Products.ToListAsync();
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
