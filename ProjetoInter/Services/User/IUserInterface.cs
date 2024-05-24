using ProjetoInter.Models;

namespace ProjetoInter.Services.User
{
    public interface IUserInterface
    {

        UserModel SearchByUsername(string username);
        UserModel GetUserById();
        UserModel CreateUser(UserModel user);
        Task<UserModel> UpdateUser(UserModel user);
        UserModel GetUserMarketCarCount(UserModel user);
         public UserModel GetUserMarketCarTotalValue(UserModel user, List<ProductModel> products);
    }
}
