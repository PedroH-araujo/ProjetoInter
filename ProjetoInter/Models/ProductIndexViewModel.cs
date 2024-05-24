namespace ProjetoInter.Models
{
    public class ProductIndexViewModel
    {
        public IEnumerable<ProductModel> Products { get; set; }
        public bool? ActiveProducts { get; set; }
    }
}
