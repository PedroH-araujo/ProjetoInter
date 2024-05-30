using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoInter.Models
{
    public class MarketCarModel
    {
        public Guid Id { get; set; } // Se preferir usar Guid em vez de string
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Valor padrão: data e hora atual
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        [NotMapped]
        public ProductModel Product { get; set; } // Propriedade de navegação para o produto associado
        [NotMapped]
        public UserModel User { get; set; } // Propriedade de navegação para o usuário proprietário
    }
}
