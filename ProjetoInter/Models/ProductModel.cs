namespace ProjetoInter.Models
{
    public enum ProductStatus
    {
        sligthly_damaged,
        perfect,
        heavily_damaged
    }
    public class ProductModel
    {
        public Guid Id { get; set; } // Se preferir usar Guid em vez de string
        public string Title { get; set; }
        public string Description { get; set; }
        public ProductStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Valor padrão: data e hora atual
        public string ImageUrl { get; set; }
        public string SellerId { get; set; }
        public string MarketCartId { get; set; }
    }
}
