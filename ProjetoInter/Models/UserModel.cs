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
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public UserRole Role { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Valor padrão: data e hora atual
        public string? Phone { get; set; }
        public string? MarketCartId { get; set; }

        public bool ValidPassword(string password)
        {
            return Password == password;
        }
    }
}
