using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoInter.Data;
using ProjetoInter.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetoInter.Services.User
{
    public class UserService : IUserInterface
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;

        public UserService(AppDbContext dbContext, IHttpContextAccessor httpContext)
        {
            this._dbContext = dbContext;
            _httpContext = httpContext;
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

        public async Task<UserModel> UpdateUser(UserModel user)
        {
            try
            {
                var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

                dbUser.Name = user.Name;
                dbUser.Email = user.Email;
                dbUser.Address = user.Address;
                dbUser.Phone = user.Phone;
                dbUser.Role = user.Role;

                _dbContext.Update(dbUser);
                await _dbContext.SaveChangesAsync();

                return dbUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private UserModel GetUser()
        {
            string userSession = _httpContext.HttpContext.Session.GetString("sessionUserLogged");
            UserModel user = JsonConvert.DeserializeObject<UserModel>(userSession);

            return user;
        }

        public UserModel GetUserById()
        {
            return _dbContext.Users.FirstOrDefault(user => user.Id == GetUser().Id);
        }

        public UserModel GetUserMarketCarCount(UserModel user)
        {
            int count = _dbContext.MarketCars.Where(mc => mc.UserId == user.Id).Count();
            user.MarketCarProductsCount = count;
            return user;
        }
    }
}
