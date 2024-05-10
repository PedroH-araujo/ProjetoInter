using ProjetoInter.Data;
using ProjetoInter.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ProjetoInter.Services.User
{
    public class UserService : IUserInterface
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private string hashKey = "AD5DC23AF55662306";

        public UserModel CreateUser(UserModel user)
        {

            // Crie o hash da senha
            string passwordHash = HashPassword(user.Password);

            // Atualize o objeto do usuário com o hash da senha
            user.Password = passwordHash;

            // Salve o usuário no banco de dados
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }

        private string HashPassword(string password)
        {
            // Crie um objeto SHA256
            string passwordWithHashKey = password + this.hashKey;
            byte[] hashBytes;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordWithHashKey));
            }

            // Converte o hash para uma representação em string (opcional)
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hashString;
        }

        public UserModel SearchByUsername(string username)
        {
            return _dbContext.Users.FirstOrDefault(user => user.Email.ToLower().Equals(username.ToLower()));
        }
    }
}
