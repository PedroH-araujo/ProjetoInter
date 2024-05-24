using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace ProjetoInter.Models
{
    public enum UserRole
    {
        seller,
        buyer
    }

    public class UserModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Digite o nome")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Digite o email")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Digite a senha")]
        public required string Password { get; set; }
        public UserRole Role { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Valor padrão: data e hora atual
        public string? Phone { get; set; }
        [NotMapped]
        public int? MarketCarProductsCount { get; set; } // valor da bolinha vermelha
        [NotMapped]
        public float? MarketCarProductsTotalValue { get; set; } // valor total dos produtos no carrinho
        [NotMapped]
        public ICollection<ProductModel>? ProductsSold { get; set; }
        [NotMapped]
        public ICollection<MarketCarModel>? MarketCars { get; set; }

        public bool ValidPassword(string providedPassword)
        {
            bool isMatchPassword = CheckIfMatchPassword(providedPassword, Password);
            return isMatchPassword;
        }

        static string CalcularHash(string password, string key)
        {
            string passwordWithHashKey = password + key;
            byte[] hashBytes;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordWithHashKey));
            }
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        static bool CheckIfMatchPassword(string text, string bdHash)
        {
            string hashCalculado = CalcularHash(text, "AD5DC23AF55662306");
            return hashCalculado == bdHash;
        }


    }
}
