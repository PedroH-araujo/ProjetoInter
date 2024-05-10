using ProjetoInter.Models;

namespace ProjetoInter.Services.Produto
{
    public interface IProductInterface
    {
        ProductModel CreateProduct(ProductModel product, IFormFile image);
        Task<List<ProductModel>> GetProducts();
    }
}
