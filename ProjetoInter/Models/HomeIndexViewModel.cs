namespace ProjetoInter.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<ProductModel> Products { get; set; }
        public UserModel User { get; set; }
        public ProductSearch? Search { get; set; }
    }
}
