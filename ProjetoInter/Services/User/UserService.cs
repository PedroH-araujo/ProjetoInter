using ProjetoInter.Data;
using ProjetoInter.Models;

namespace ProjetoInter.Services.User
{
    public class UserService : IUserInterface
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public UserModel CreateUser(UserModel user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public UserModel SearchByUsername(string username)
        {
            return _dbContext.Users.FirstOrDefault(user => user.Email.ToLower().Equals(username.ToLower()));
        }
    }
}
