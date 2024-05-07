namespace ProjetoInter.Models
{
    public class MarketCarModel
    {
        public Guid Id { get; set; } // Se preferir usar Guid em vez de string
        public string ProductId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Valor padrão: data e hora atual
        
    }
}
