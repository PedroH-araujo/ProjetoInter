using ProjetoInter.Models;

namespace ProjetoInter.Services.User
{
    public interface IUserInterface
    {

        UserModel SearchByUsername(string username);
        UserModel CreateUser(UserModel user);
    }
}
