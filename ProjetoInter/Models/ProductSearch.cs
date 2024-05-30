namespace ProjetoInter.Models
{
    public class ProductSearch
    {
        public string Name { get; set; }
        public ProductStatus? Status { get; set; }
        public float MaximumValue { get; set; }
        public float MinimumValue { get; set; }
    }
}
