﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Digite o título do produto")]
        public required string Title { get; set; }
        [Required(ErrorMessage = "Digite a descrição do produto")]
        public required string Description { get; set; }
        [Required(ErrorMessage = "Digite o valor do produto")]
        public required string Value { get; set; }
        public ProductStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Valor padrão: data e hora atual
        public string? ImageUrl { get; set; }
        public Guid? SellerId { get; set; }
        [NotMapped]
        public UserModel? Seller { get; set; } // Propriedade de navegação
        public Guid[]? MarketCartId { get; set; }
        public bool IsActive { get; set; } = true;
        [NotMapped]
        public bool IsProductInMyMarketCart { get; set; }
        [NotMapped]
        public ICollection<MarketCarModel>? MarketCars { get; set; }


    }
}
