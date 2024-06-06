using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoInter.Data;
using ProjetoInter.Models;
using ProjetoInter.Services.Shared;
using System;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetoInter.Services.User
{
    public class UserService : IUserInterface
    {
        private readonly AppDbContext _dbContext;
        private readonly ISharedMethodsInterface _sharedMethods;

        public UserService(AppDbContext dbContext, ISharedMethodsInterface sharedMethods)
        {
            this._dbContext = dbContext;
            _sharedMethods = sharedMethods;
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
                if (user.Password != dbUser.Password)
                {
                    string passwordHash = HashPassword(user.Password);
                    dbUser.Password = passwordHash;
                }

                _dbContext.Update(dbUser);
                await _dbContext.SaveChangesAsync();

                return dbUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public UserModel GetUserById()
        {
            return _dbContext.Users.FirstOrDefault(user => user.Id == _sharedMethods.GetUser().Id);
        }

        public UserModel GetUserMarketCarCount(UserModel user)
        {
            int count = _dbContext.MarketCars.Where(mc => mc.UserId == user.Id && mc.IsActive == true).Count();
            user.MarketCarProductsCount = count; //bolinha vermelha
            return user;
        }

        public UserModel GetUserMarketCarTotalValue(UserModel user, List<ProductModel> products)
        {
            float totalValue = 0;
            for (int i = 0; i < products.Count(); i++)
            {
                var product = products[i];
                totalValue += product.Value;
            }

            user.MarketCarProductsTotalValue = totalValue;
            return user;
        }
    }
}
