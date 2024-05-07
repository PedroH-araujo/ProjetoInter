using System.ComponentModel.DataAnnotations;

namespace ProjetoInter.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Digite o login")]
        public required string Login { get; set; }
        [Required(ErrorMessage = "Digite a senha")]
        public required string Password { get; set; }
    }
}
