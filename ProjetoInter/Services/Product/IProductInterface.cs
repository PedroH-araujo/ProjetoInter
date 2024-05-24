using ProjetoInter.Models;

namespace ProjetoInter.Services.Produto
{
    public interface IProductInterface
    {
        Task<ProductModel> CreateProduct(ProductModel product, IFormFile image);
        Task<List<ProductModel>> GetProducts();
        Task<List<ProductModel>> GetMyProducts(bool? active);
        Task<ProductModel> GetProductById(Guid id);
        Task<ProductModel> UpdateProduct(ProductModel product, IFormFile image);
        Task<ProductModel> InactivateProduct(Guid id);
        Task<List<ProductModel>> GetFilteredProducts(string? filter);
        Task<ProductModel> DeleteProduct(Guid id);
    }
}
