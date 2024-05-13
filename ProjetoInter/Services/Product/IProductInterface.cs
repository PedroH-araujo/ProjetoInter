using ProjetoInter.Models;

namespace ProjetoInter.Services.Produto
{
    public interface IProductInterface
    {
        Task<ProductModel> CreateProduct(ProductModel product, IFormFile image);
        Task<List<ProductModel>> GetProducts();
        Task<List<ProductModel>> GetMyProducts();
        Task<ProductModel> GetProductById(Guid id);
        Task<ProductModel> UpdateProduct(ProductModel product, IFormFile image);
        Task<ProductModel> DeleteProduct(Guid id);
        Task<List<ProductModel>> GetFilteredProducts(string? filter);
    }
}
